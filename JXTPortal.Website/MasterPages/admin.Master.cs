﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JXTPortal.Website.BaseClasses;
using JXTPortal.Entities;
using System.Configuration;

namespace JXTPortal.Website.MasterPages
{
    public partial class admin : AdminPageBase
    {
        private SitesService _sitesService = null;

        public SitesService SitesService
        {
            get
            {
                if (_sitesService == null)
                {
                    _sitesService = new SitesService();
                }
                return _sitesService;
            }
        }

        protected string HostName = JXTPortal.Common.Utils.GetHostName();

        private void HideNavigation()
        {
            if (SessionData.AdminUser == null)
            {
                Repeater1.Visible = false;
            }
            else
            {
                if (!SessionData.AdminUser.isAdminUser)
                {
                    if (SessionData.AdminUser.AdminRoleId == (int)PortalEnums.Admin.AdminRole.ContentEditor)
                    {
                        SiteMapDataSource1.SiteMapProvider = "ContentEditorSitemapProvider";
                    }

                    if (SessionData.AdminUser.AdminRoleId == (int)PortalEnums.Admin.AdminRole.Developer)
                    {
                        SiteMapDataSource1.SiteMapProvider = "DeveloperSitemapProvider";
                    }

                    if (SessionData.AdminUser.AdminRoleId == (int)PortalEnums.Admin.AdminRole.Contributor)
                    {
                        SiteMapDataSource1.SiteMapProvider = "ContributorSitemapProvider";
                    }

                    if (SessionData.AdminUser.AdminRoleId == (int)PortalEnums.Admin.AdminRole.ContentEditor)
                    {
                        pnlAdminStyling.Visible = true;
                    }
                    else if (SessionData.AdminUser.AdminRoleId == (int)PortalEnums.Admin.AdminRole.Developer)
                    {
                        pnlDeveloperStyling.Visible = true;
                    }
                    else if (SessionData.AdminUser.AdminRoleId == (int)PortalEnums.Admin.AdminRole.Contributor)
                    {
                        pnlContributerStyling.Visible = true;
                    }
                }

                Repeater1.Visible = true;
                lnkLogout.Visible = true;
            }

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // Check if logged in, else don't show.
            HideNavigation();

            // CKeditor issue with IE browser .. 
            if (Request.RawUrl.ToLower().Contains("/campaignedit.aspx") ||
                Request.RawUrl.ToLower().Contains("/customwidgetedit.aspx") ||
                Request.RawUrl.ToLower().Contains("/emailtemplatesedit.aspx") ||
                Request.RawUrl.ToLower().Contains("/jobtemplatesedit.aspx") ||
                Request.RawUrl.ToLower().Contains("/jobsedit.aspx") ||
                Request.RawUrl.ToLower().Contains("/newsedit.aspx") ||
                Request.RawUrl.ToLower().Contains("/siteemailtemplatesedit.aspx") ||
                Request.RawUrl.ToLower().Contains("/sitewebpartsedit.aspx") ||
                Request.RawUrl.ToLower().Contains("/dynamicpagesedit.aspx") ||
                Request.RawUrl.ToLower().Contains("/widgetcontainersedit.aspx"))
            {
                ltCompatible.Text = "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=9\" />";
            }
            else
            {
                ltCompatible.Text = string.Empty;
            }

            // Show if there is an Image
            if (!Page.IsPostBack)
            {
                ltlTitle.Text = SessionData.Site.SiteName;
                hypSite.NavigateUrl = "/";
                hypSite.Text = String.Format("<span><strong>Visit Site &raquo;</strong></span>");

                int siteid = Convert.ToInt32(JXTPortal.Common.Utils.GetAppSettings("MasterSiteID"));

                if (SessionData.Site.HasAdminLogo)
                {
                    siteid = SessionData.Site.SiteId;
                    // imgAdminLogo.ImageUrl = "~/Admin/GetAdminLogo.aspx?SiteID=" + SessionData.Site.SiteId.ToString();                                        
                }

                using (Entities.Sites site = SitesService.GetBySiteId(SessionData.Site.SiteId))
                {
                    if (!string.IsNullOrWhiteSpace(site.SiteAdminLogoUrl))
                    {
                        imgAdminLogo.ImageUrl = string.Format("/media/{1}/{2}", SessionData.Site.SiteUrl, ConfigurationManager.AppSettings["SitesFolder"], site.SiteAdminLogoUrl);
                    }
                    else
                    {
                        imgAdminLogo.ImageUrl = "~/Admin/GetAdminLogo.aspx?SiteID=" + SessionData.Site.SiteId.ToString();
                    }
                }

                if (SessionData.AdminUser != null)
                {
                    ltlUser.Text = SessionData.AdminUser.FirstName;
                }

                GlobalSettingsService service = new GlobalSettingsService();
                GlobalSettings GlobalSettings = new GlobalSettings();
                GlobalSettings = service.GetBySiteId(SessionData.Site.SiteId).FirstOrDefault();
                hfIsJobBoard.Value = (GlobalSettings.SiteType == (int)PortalEnums.Admin.SiteType.JobBoard) ? "1" : string.Empty;
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            SessionService.RemoveAdminUser();

            SessionService.SessionAbandon();

            //TODO:why do we need this????
            //Session.Abandon();
            //Session.Clear();

            Response.Redirect("~/admin/login.aspx");

        }

        protected string GetUrl(object input)
        {
            if (string.IsNullOrEmpty(input.ToString()))
                return "#";
            return input.ToString();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SiteMapNode sitemap = e.Item.DataItem as SiteMapNode;
                GlobalSettingsService service = new GlobalSettingsService();
                GlobalSettings GlobalSettings = new GlobalSettings();
                GlobalSettings = service.GetBySiteId(SessionData.Site.SiteId).FirstOrDefault();

                Literal ltTitleIcon = e.Item.FindControl("ltTitleIcon") as Literal;

                if (!string.IsNullOrWhiteSpace(sitemap["iconLogo"]))
                {
                    ltTitleIcon.Text = string.Format("<span class=\"{0}\"></span>", sitemap["iconLogo"]);
                }

                if (sitemap.Title == "Invoice Report")
                {
                    if (GlobalSettings.SiteType == (int)PortalEnums.Admin.SiteType.Recruiter)
                    {
                        e.Item.Visible = false;
                    }
                }
            }
        }

        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SiteMapNode sitemap = e.Item.DataItem as SiteMapNode;

                GlobalSettingsService service = new GlobalSettingsService();
                GlobalSettings GlobalSettings = new GlobalSettings();
                GlobalSettings = service.GetBySiteId(SessionData.Site.SiteId).FirstOrDefault();

                if (sitemap.Title == "Invoice Report")
                {
                    if (GlobalSettings.SiteType == (int)PortalEnums.Admin.SiteType.Recruiter)
                    {
                        e.Item.Visible = false;
                    }
                }

                if (sitemap.Title == "Screening Questions")
                {
                    e.Item.Visible = GlobalSettings.EnableScreeningQuestions;
                }
            }
        }
        //private AdvertisersService _advertisersService;


    }

}
