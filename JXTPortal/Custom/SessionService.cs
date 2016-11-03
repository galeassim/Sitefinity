﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JXTPortal.Entities;
using System.Web;
using System.Configuration;
using JXTPortal.Common;
using System.Data;
using System.Security.Cryptography;

namespace JXTPortal
{
    public static class SessionService
    {
        public static string HASHKEY_COOKIE_NAME = "AuthToken";

        /// <summary>
        /// setting the session upon loading based on URL
        /// </summary>
        public static void SessionSetup()
        {
            SitesService siteService = new SitesService();

            // ToDO: Create Store proc for finding the url.
            DataSet dsSiteList = siteService.FindSite(null, GetCurrentWhitelabelUrl());

            if (dsSiteList != null && dsSiteList.Tables[0].Rows.Count > 0)
            {
                SetSite(dsSiteList);
            }
            else
            {
                // Not found - Show default site.
                SetSite(siteService.FindSite(Utils.GetAppSettingsInt("MasterSiteID"), string.Empty));
            }

            //dispose it properly
            siteService = null;
            dsSiteList.Dispose();

        }

        public static void SessionAbandon()
        {
            HttpContext.Current.Session.Abandon();
            HttpCookie thisCookie = HttpContext.Current.Request.Cookies["ASP.NET_SessionId"];
            if (thisCookie != null)
            {
                thisCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(thisCookie);
            }
        }

        /// <summary>
        /// verify the session since the user might navigate to the different whitelabel site
        /// within the same session. The issue found during accessing subdomain from parent domain
        /// the session will not be reloaded because its still active
        /// </summary>
        public static void SessionVerify()
        {
            //this is to make sure it's not being called to early when the session is still not available
            //we just need to verify the site url to decide whether to refresh the session or not
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[PortalConstants.Session.SessionSite] != null)
            {
                if (string.Compare(SessionData.Site.SiteUrl, GetCurrentWhitelabelUrl(), true) != 0)
                {
                    SessionSetup();
                }
            }
        }

        /// <summary>
        /// Setting the session site based on the whitelabel record
        /// </summary>
        /// <param name="dsSite"></param>
        public static void SetSite(DataSet dsSite)
        {
            SessionSite sessionSite = new SessionSite();
            sessionSite.DateFormat = "dd/MM/yyyy";
            int thisSiteID = int.Parse(dsSite.Tables[0].Rows[0]["SiteId"].ToString());

            sessionSite.SiteId = thisSiteID; 
            sessionSite.SiteName = dsSite.Tables[0].Rows[0]["SiteName"].ToString();
            sessionSite.SiteDescription = dsSite.Tables[0].Rows[0]["SiteDescription"].ToString();
            sessionSite.SiteUrl = dsSite.Tables[0].Rows[0]["SiteUrl"].ToString();
            sessionSite.HasAdminLogo = (dsSite.Tables[0].Rows[0]["SiteAdminLogo"] != null);
            sessionSite.IsLive = Convert.ToBoolean(dsSite.Tables[0].Rows[0]["Live"]);
            if (dsSite.Tables[0].Rows[0]["DefaultLanguageId"] != null)
                sessionSite.DefaultLanguageId = int.Parse(dsSite.Tables[0].Rows[0]["DefaultLanguageId"].ToString());
            else
                sessionSite.DefaultLanguageId = PortalConstants.DEFAULT_LANGUAGE_ID;

            System.Web.HttpContext.Current.Session[PortalConstants.Session.SessionSite] = sessionSite;

            GlobalSettingsService globalService = new GlobalSettingsService();
            TList<GlobalSettings> settings = globalService.GetBySiteId(sessionSite.SiteId);
            
            
            // Check if current site has a master site
            SiteMappingsService sitemappingsservice = new SiteMappingsService();
            using (TList<SiteMappings> sitemappingslist = sitemappingsservice.GetBySiteId(thisSiteID))
            {
                // Allocating MasterSiteId - use siteid if no record
                if (sitemappingslist.Count > 0)
                {
                    sessionSite.MasterSiteId = sitemappingslist[0].MasterSiteId;
                }
                else
                {
                    sessionSite.MasterSiteId = thisSiteID;
                }
            }

            sessionSite.WWWRedirect = settings[0].WwwRedirect;
            sessionSite.EnableSsl = settings[0].EnableSsl;
            sessionSite.DateFormat = settings[0].GlobalDateFormat;
            sessionSite.IsPrivateSite = (settings[0].IsPrivateSite.HasValue ? settings[0].IsPrivateSite.Value : false); //!string.IsNullOrWhiteSpace(settings[0].PrivateRedirectUrl) ? 
            //if (sessionSite.IsPrivateSite)
            //    sessionSite.PrivateRedirectUrl = settings[0].PrivateRedirectUrl;

            if (settings[0].MemberRegistrationNotification != null)
            {
                sessionSite.MemberRegistrationNotificationEmail = settings[0].MemberRegistrationNotification;
            }
            sessionSite.UseCustomProfessionRole = settings[0].UseCustomProfessionRole;
            sessionSite.IsJobBoard = (settings[0].SiteType == (int)PortalEnums.Admin.SiteType.JobBoard) ? true : false;
            sessionSite.AdvertiserApprovalProcess = (settings[0].AdvertiserApprovalProcess.HasValue) ? (PortalEnums.Admin.AdvertiserApproval)settings[0].AdvertiserApprovalProcess : PortalEnums.Admin.AdvertiserApproval.AutoApproved;
            // Set Language
            LanguagesService languageService = new LanguagesService();
            if (System.Web.HttpContext.Current.Session[PortalConstants.Session.SessionLanguage] == null)
            {
                SetLanguage(languageService.GetByLanguageId(sessionSite.DefaultLanguageId));
            }

            //Language available for this site
            List<SiteLanguages> siteLanguages;
            {
                SiteLanguagesService _sl = new SiteLanguagesService();
                siteLanguages = _sl.GetBySiteId(thisSiteID).ToList();
                _sl = null; //manaul dump
            } //this should GC
            sessionSite.SiteAvailableLanguage = (from lang in siteLanguages select (PortalEnums.Languages.URLLanguage)lang.LanguageId).Distinct().ToList();

            if (settings[0].DefaultEmailLanguageId.HasValue)
            {
                sessionSite.DefaultEmailLanguageId = settings[0].DefaultEmailLanguageId.Value;
            }
            else
            {
                sessionSite.DefaultEmailLanguageId = PortalConstants.DEFAULT_EMAIL_LANGUAGE_ID;
            }

            //Set AuthToken for the current Session
            sessionSite.AuthToken = GenerateAuthToken();

            
            // Dispose object
            languageService = null;
            globalService = null;
            sessionSite = null;

        }

        /// <summary>
        /// get current whitelabel url - replace the staging fix
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentWhitelabelUrl()
        {
            string siteURL = HttpContext.Current.Request.Url.Host.ToString();
            return siteURL.Replace("dev.", "").Replace("www.", "").Replace(ConfigurationManager.AppSettings[PortalConstants.WebConfigurationKeys.WEBSITEURLPOSTFIX], string.Empty);
        }

        public static void SetMember(Members member)
        {
            SessionMember sessionMember = new SessionMember();

            sessionMember.MemberId = member.MemberId;
            sessionMember.Username = member.Username;
            sessionMember.FirstName = member.FirstName;
            sessionMember.Surname = member.Surname;
            sessionMember.EmailAddress = member.EmailAddress;
            sessionMember.Phone = member.WorkPhone;
            sessionMember.Phone = member.MobilePhone;

            sessionMember.EmailFormat = member.EmailFormat;
            sessionMember.LinkedInAccessToken = member.LinkedInAccessToken;
            sessionMember.LastTermsAndConditionsDate = member.LastTermsAndConditionsDate;
            sessionMember.ReferringSiteID = member.ReferringSiteId;

            System.Web.HttpContext.Current.Session[PortalConstants.Session.SessionMember] = sessionMember;
            sessionMember = null;

        }

        public static void RemoveMemberAndAdvertiser()
        {
            System.Web.HttpContext.Current.Session.Remove(PortalConstants.Session.SessionMember);
            System.Web.HttpContext.Current.Session.Remove(PortalConstants.Session.SessionAdvertiserUser);

            // Remove stored advertiser remember cookie
            HttpCookie advertisercookie = System.Web.HttpContext.Current.Request.Cookies[PortalConstants.SiteCookie.AdvertiserUserCookie];
            if (advertisercookie != null)
            {
                advertisercookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(advertisercookie);
            }

            // Remove stored member remember cookie
            HttpCookie membercookie = System.Web.HttpContext.Current.Request.Cookies[PortalConstants.SiteCookie.MemberCookie];
            if (membercookie != null)
            {
                membercookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(membercookie);
            }

            // Remove the Custom Job Application Form data
            System.Web.HttpContext.Current.Session.Remove(PortalConstants.Session.SESSION_FORMDATA_KEY);
        }

        public static void RemoveMember()
        {
            System.Web.HttpContext.Current.Session.Remove(PortalConstants.Session.SessionMember);
            // Remove stored cookie
            HttpCookie membercookie = System.Web.HttpContext.Current.Request.Cookies["memberInfo"];
            if (membercookie != null)
            {
                membercookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(membercookie);
            }
        }

        public static void SetLanguage(Languages language)
        {
            System.Web.HttpContext.Current.Session[PortalConstants.Session.SessionLanguage] = language;
        }

        public static void SetAdminUser(AdminUsers adminUser)
        {
            SessionAdminUser sessionAdminUser = new SessionAdminUser();
            sessionAdminUser.AdminUserId = adminUser.AdminUserId;
            sessionAdminUser.AdminRoleId = adminUser.AdminRoleId;
            sessionAdminUser.FirstName = adminUser.FirstName;
            sessionAdminUser.SiteId = adminUser.SiteId;

            System.Web.HttpContext.Current.Session[PortalConstants.Session.SessionAdminUser] = sessionAdminUser;
            sessionAdminUser = null;
        }

        public static void RemoveAdminUser()
        {
            System.Web.HttpContext.Current.Session.Remove(PortalConstants.Session.SessionAdminUser);
        }

        public static void SetAdvertiserUser(AdvertiserUsers advertiserUser)
        {
            SessionAdvertiserUser sessionAdvertiserUser = new SessionAdvertiserUser();
            sessionAdvertiserUser.AdvertiserId = advertiserUser.AdvertiserId;
            sessionAdvertiserUser.AdvertiserUserId = advertiserUser.AdvertiserUserId;
            sessionAdvertiserUser.PrimaryAccount = advertiserUser.PrimaryAccount;
            sessionAdvertiserUser.Username = advertiserUser.UserName;
            sessionAdvertiserUser.FirstName = advertiserUser.FirstName;
            sessionAdvertiserUser.LastName = advertiserUser.Surname;
            sessionAdvertiserUser.AccountType = PortalEnums.Advertiser.AccountType.Account;
            sessionAdvertiserUser.LastTermsAndConditionsDate = advertiserUser.LastTermsAndConditionsDate;

            AdvertisersService service = new AdvertisersService();
            Advertisers adv = service.GetByAdvertiserId(advertiserUser.AdvertiserId);
            if (adv != null)
            {
                sessionAdvertiserUser.AccountType = (PortalEnums.Advertiser.AccountType)adv.AdvertiserAccountTypeId;
            }

            System.Web.HttpContext.Current.Session[PortalConstants.Session.SessionAdvertiserUser] = sessionAdvertiserUser;
            sessionAdvertiserUser = null;
        }

        public static void RemoveAdvertiserUser()
        {
            System.Web.HttpContext.Current.Session.Remove(PortalConstants.Session.SessionAdvertiserUser);
            // Remove stored cookie
            HttpCookie advertisercookie = System.Web.HttpContext.Current.Request.Cookies[PortalConstants.SiteCookie.AdvertiserUserCookie];
            if (advertisercookie != null)
            {
                advertisercookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(advertisercookie);
            }
        }

        public static string GenerateAuthToken()
        {
            //use sessionID as part of the key
            //throw exception if no session
            if (HttpContext.Current.Session == null)
                throw new Exception("Session is not available, can not generate hashed key.");

            StringBuilder myStr = new StringBuilder();
            //cannot include sessionID because of the remember me function
            //myStr.Append(HttpContext.Current.Session.SessionID);
            //myStr.Append(HttpContext.Current.Request.Browser.Browser);
            //myStr.Append(HttpContext.Current.Request.Browser.Platform);
            //myStr.Append(HttpContext.Current.Request.Browser.MajorVersion);
            //myStr.Append(HttpContext.Current.Request.Browser.MinorVersion);
            myStr.Append(HttpContext.Current.Request.UserHostAddress);
            //myStr.Append(HttpContext.Current.Request.UserAgent);

            //if (HttpContext.Current.Request.LogonUserIdentity != null && HttpContext.Current.Request.LogonUserIdentity.User != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Request.LogonUserIdentity.User.Value))
            //    myStr.Append(HttpContext.Current.Request.LogonUserIdentity.User.Value); //S-1-5-21-2902012959-710356124-767786954-1357

            //SHA1 sha = new SHA1CryptoServiceProvider();
            //byte[] hashdata = sha.ComputeHash(Encoding.UTF8.GetBytes(myStr.ToString()));
            //return Convert.ToBase64String(hashdata);

            return myStr.ToString();
        }

        public static bool IsAuthTokenValid()
        {
            if (SessionData.Site != null)
                return SessionData.Site.AuthToken == GenerateAuthToken();

            return false;

        }


        #region Mobile Setup
        /// <summary>
        /// setting the session upon loading based on URL
        /// </summary>
        public static void SessionMobileSetup()
        {

            SitesService siteService = new SitesService();

            // ToDO: Create Store proc for finding the url.
            Sites dsSiteList = siteService.GetByMobileUrl(GetCurrentWhitelabelUrl());

            if (dsSiteList != null)
            {
                SetMobileSite(dsSiteList);
                dsSiteList.Dispose();
            }
            else
            {
                // Not found - Show default site.
                SetMobileSite(siteService.GetBySiteId(Utils.GetAppSettingsInt("MasterSiteID")));
            }

            //dispose it properly
            siteService = null;
        }

        /// <summary>
        /// Setting the session site based on the whitelabel record
        /// </summary>
        /// <param name="Sites"></param>
        public static void SetMobileSite(Sites site)
        {
            SessionSite sessionSite = new SessionSite();
            GlobalSettingsService globalSettingsService = new GlobalSettingsService();
            GlobalSettings GlobalSettings = new GlobalSettings();

            if (site != null)
            {
                GlobalSettings = globalSettingsService.GetBySiteId(site.SiteId).FirstOrDefault();

                sessionSite.SiteId = site.SiteId;
                sessionSite.SiteName = site.SiteName;
                sessionSite.SiteDescription = site.SiteDescription;
                sessionSite.SiteUrl = site.SiteUrl;
                sessionSite.HasAdminLogo = (site.SiteAdminLogo != null);
                sessionSite.IsLive = site.Live.HasValue ? site.Live.Value : false;


                if (GlobalSettings != null)
                {
                    sessionSite.DefaultLanguageId = GlobalSettings.DefaultLanguageId;
                    sessionSite.WWWRedirect = GlobalSettings.WwwRedirect;
                }
                else
                    sessionSite.DefaultLanguageId = PortalConstants.DEFAULT_LANGUAGE_ID;
            }

            System.Web.HttpContext.Current.Session[PortalConstants.Session.SessionSite] = sessionSite;

            if (GlobalSettings.MemberRegistrationNotification != null)
            {
                sessionSite.MemberRegistrationNotificationEmail = GlobalSettings.MemberRegistrationNotification;
            }

            // Set Language
            LanguagesService languageService = new LanguagesService();
            SetLanguage(languageService.GetByLanguageId(sessionSite.DefaultLanguageId));

            // Dispose object
            languageService = null;
            globalSettingsService = null;
            sessionSite = null;

        }
        #endregion
    }
}