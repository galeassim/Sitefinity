﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Net;
using System.IO;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using JXTPortal.Common;
using JXTPortal.Entities;

using JXTPortal.Client.Salesforce;
using JXTPortal.Entities.Models;
using SocialMedia;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NotesFor.HtmlToOpenXml;

namespace JXTPortal.Website
{
    public partial class NewApplyPage : System.Web.UI.Page
    {
        #region Properties
        private int _jobid = 0;
        private string _profession = "";

        private GlobalSettingsService _globalSettingsService = null;
        private GlobalSettingsService GlobalSettingsService
        {
            get
            {
                if (_globalSettingsService == null)
                {
                    _globalSettingsService = new GlobalSettingsService();
                }
                return _globalSettingsService;
            }
        }

        private MembersService _membersService = null;

        private MembersService MembersService
        {
            get
            {
                if (_membersService == null)
                {
                    _membersService = new MembersService();
                }
                return _membersService;
            }
        }

        private JobsService _jobsService = null;

        private JobsService JobsService
        {
            get
            {
                if (_jobsService == null)
                {
                    _jobsService = new JobsService();
                }
                return _jobsService;
            }
        }

        private JobAlertsService _jobalertService = null;

        private JobAlertsService JobAlertsService
        {
            get
            {
                if (_jobalertService == null)
                {
                    _jobalertService = new JobAlertsService();
                }
                return _jobalertService;
            }
        }

        private JobApplicationService _jobapplicationService = null;

        private JobApplicationService JobApplicationService
        {
            get
            {
                if (_jobapplicationService == null)
                {
                    _jobapplicationService = new JobApplicationService();
                }
                return _jobapplicationService;
            }
        }

        private IntegrationsService _integrationsService;
        private IntegrationsService IntegrationsService
        {
            get
            {
                if (_integrationsService == null)
                {
                    _integrationsService = new IntegrationsService();
                }

                return _integrationsService;
            }
        }

        private DynamicPagesService _dynamicpagesService;
        private DynamicPagesService DynamicPagesService
        {
            get
            {
                if (_dynamicpagesService == null)
                {
                    _dynamicpagesService = new DynamicPagesService();
                }

                return _dynamicpagesService;
            }
        }

        private MemberFilesService _memberfilesService;
        private MemberFilesService MemberFilesService
        {
            get
            {
                if (_memberfilesService == null)
                {
                    _memberfilesService = new MemberFilesService();
                }

                return _memberfilesService;
            }
        }

        private ViewJobsService _viewjobsService;
        private ViewJobsService ViewJobsService
        {
            get
            {
                if (_viewjobsService == null)
                {
                    _viewjobsService = new ViewJobsService();
                }

                return _viewjobsService;
            }
        }

        private JobViewsService _jobviewsService;
        private JobViewsService JobViewsService
        {
            get
            {
                if (_jobviewsService == null)
                {
                    _jobviewsService = new JobViewsService();
                }

                return _jobviewsService;
            }
        }

        private SiteProfessionService _siteprofessionService;
        private SiteProfessionService SiteProfessionService
        {
            get
            {
                if (_siteprofessionService == null)
                {
                    _siteprofessionService = new SiteProfessionService();
                }

                return _siteprofessionService;
            }
        }

        protected int JobID
        {
            get
            {
                if ((Request.QueryString["JobID"] != null))
                {
                    if (int.TryParse((Request.QueryString["JobID"].Trim()), out _jobid))
                    {
                        _jobid = Convert.ToInt32(Request.QueryString["JobID"]);
                    }
                    return _jobid;
                }

                return _jobid;
            }
        }

        protected string Profession
        {
            get
            {
                return Request.Params["profession"];
            }
        }

        protected string JobName
        {
            get
            {
                return Request.Params["jobname"];
            }
        }

        private string _googledeveloperkey;
        protected string GoogleDeveloperKey
        {
            get
            {
                return _googledeveloperkey;
            }
            set
            {
                _googledeveloperkey = value;
            }
        }

        private string _googleclientid;
        protected string GoogleClientID
        {
            get
            {
                return _googleclientid;
            }
            set
            {
                _googleclientid = value;
            }
        }

        private string _dropboxkey;
        protected string DropboxKey
        {
            get
            {
                return _dropboxkey;
            }
            set
            {
                _dropboxkey = value;
            }
        }

        private string _usernameerror = "Username is required";
        protected string UsernameError
        {
            get { return _usernameerror; }
            set { _usernameerror = value; }
        }

        private string _passworderror = "Password is required";
        protected string PasswordError
        {
            get { return _passworderror; }
            set { _passworderror = value; }
        }

        private string _firstnameerror = "First name is required";
        protected string FirstNameError
        {
            get { return _firstnameerror; }
            set { _firstnameerror = value; }
        }

        private string _lastnameerror = "Last name is required";
        protected string LastNameError
        {
            get { return _lastnameerror; }
            set { _lastnameerror = value; }
        }

        private string _phoneerror = "Phone is incorrect format";
        protected string PhoneError
        {
            get { return _phoneerror; }
            set { _phoneerror = value; }
        }

        private string _emailerror = "Email is required";
        protected string EmailError
        {
            get { return _emailerror; }
            set { _emailerror = value; }
        }

        private string _confirmemailerror = "Confirm email is required";
        protected string ConfirmEmailError
        {
            get { return _confirmemailerror; }
            set { _confirmemailerror = value; }
        }

        private string _newpassworderror = "Password is required";
        protected string NewPasswordError
        {
            get { return _newpassworderror; }
            set { _newpassworderror = value; }
        }

        private string _landlineerror = "Land Line is required";
        protected string LandLineError
        {
            get { return _landlineerror; }
            set { _landlineerror = value; }
        }

        private string _residencystatuserror = "Residency Status is required";
        protected string ResidencyStatusError
        {
            get { return _residencystatuserror; }
            set { _residencystatuserror = value; }
        }

        private string _stateerror = "State is required";
        protected string StateError
        {
            get { return _stateerror; }
            set { _stateerror = value; }
        }

        private string _addresserror = "Address is required";
        protected string AddressError
        {
            get { return _addresserror; }
            set { _addresserror = value; }
        }

        private string _contactnumbererror = "Contact Number is required";
        protected string ContactNumberError
        {
            get { return _contactnumbererror; }
            set { _contactnumbererror = value; }
        }

        private string _countryerror = "Country is required";
        protected string CountryError
        {
            get { return _countryerror; }
            set { _countryerror = value; }
        }

        private string _postcodeerror = "Postcode is required";
        protected string PostcodeError
        {
            get { return _postcodeerror; }
            set { _postcodeerror = value; }
        }

        private string _willingtorelocateerror = "Willing To Relocate is required";
        protected string WillingToRelocateError
        {
            get { return _willingtorelocateerror; }
            set { _willingtorelocateerror = value; }
        }

        private string _preferredrmploymenterrror = "Preferred Employment is required";
        protected string PreferredEmploymentError
        {
            get { return _preferredrmploymenterrror; }
            set { _preferredrmploymenterrror = value; }
        }

        private string _salaryerrror = "Salary is required";
        protected string SalaryErrror
        {
            get { return _salaryerrror; }
            set { _salaryerrror = value; }
        }

        private string _confirmemailnotmatch = "Confirm email not match";
        protected string ConfirmEmailNotMatch
        {
            get { return _confirmemailnotmatch; }
            set { _confirmemailnotmatch = value; }
        }

        private string _accessdenied = string.Empty;
        protected string AccessDenied
        {
            get { return _accessdenied; }
            set { _accessdenied = value; }
        }

        private string _indeedapitoken = string.Empty;
        protected string IndeedAPIToken
        {
            get { return _indeedapitoken; }
            set { _indeedapitoken = value; }
        }

        private string _joblocation = string.Empty;
        protected string JobLocation
        {
            get { return _joblocation; }
            set { _joblocation = value; }
        }

        private string _companyname = string.Empty;
        protected string CompanyName
        {
            get { return _companyname; }
            set { _companyname = value; }
        }

        private string _jobtitle = string.Empty;
        protected string JobTitle
        {
            get { return _jobtitle; }
            set { _jobtitle = value; }
        }

        private string _joburl = string.Empty;
        protected string JobURL
        {
            get { return _joburl; }
            set { _joburl = value; }
        }

        private string _posturl = string.Empty;
        protected string PostURL
        {
            get { return _posturl; }
            set { _posturl = value; }
        }

        #endregion

        private void SetDisplay()
        {
            UsernameError = CommonFunction.GetResourceValue("LabelUsernameRequired");
            PasswordError = CommonFunction.GetResourceValue("labellPasswordRequired");
            FirstNameError = CommonFunction.GetResourceValue("LabelFirstnameRequired");
            LastNameError = CommonFunction.GetResourceValue("LabelSurnameRequired");
            PhoneError = "Phone is incorrect format";
            EmailError = CommonFunction.GetResourceValue("LabelEmailRequired");
            ConfirmEmailError = CommonFunction.GetResourceValue("LabelConfirmEmailRequired");
            NewPasswordError = CommonFunction.GetResourceValue("labellPasswordRequired");
            LandLineError = "Land Line is required";
            ResidencyStatusError = "Residency Status is required";
            StateError = "State is required";
            ConfirmEmailNotMatch = CommonFunction.GetResourceValue("LabelConfirmEmailNotMatch");
            AccessDenied = CommonFunction.GetResourceValue("LabelAccessDenied");

            //set text displays
            tbUserName.Attributes.Add("placeholder", CommonFunction.GetResourceValue("LabelUsername"));
            tbPassword.Attributes.Add("placeholder", CommonFunction.GetResourceValue("LabelPassword"));
            btnSignIn.Text = CommonFunction.GetResourceValue("LabelLogin");
            tbFirstName.Attributes.Add("placeholder", CommonFunction.GetResourceValue("LabelEnterFirstName"));
            tbLastName.Attributes.Add("placeholder", CommonFunction.GetResourceValue("LabelEnterLastName"));
            tbPhone.Attributes.Add("placeholder", CommonFunction.GetResourceValue("LabelEnterPhoneNumber"));
            tbEmail.Attributes.Add("placeholder", CommonFunction.GetResourceValue("LabelEnterEmail"));
            tbConfirmEmail.Attributes.Add("placeholder", CommonFunction.GetResourceValue("LabelConfirmEmailPage"));
            tbNewPassword.Attributes.Add("placeholder", CommonFunction.GetResourceValue("LabelEnterPassword"));
            txtCoverLetterText.Attributes.Add("placeholder", CommonFunction.GetResourceValue("LabelWriteOwnCoverLetter"));
            apply.Text = CommonFunction.GetResourceValue("LabelApply");
            cancel.Text = CommonFunction.GetResourceValue("LabelBackToJob");

            spEmailError.InnerText = CommonFunction.GetResourceValue("LabelEmailInvalid");
            spConfEmailError.InnerText = CommonFunction.GetResourceValue("LabelConfirmEmailRequired");
        }

        private void LoadResume()
        {
            ddlResume.Items.Clear();
            if (SessionData.Member != null)
            {
                using (TList<JXTPortal.Entities.MemberFiles> MemberFiles = MemberFilesService.GetByMemberId(SessionData.Member.MemberId))
                {
                    ddlResume.DataSource = MemberFiles;
                    MemberFiles.Filter = "DocumentTypeID = 2";
                    ddlResume.DataBind();
                }
            }

            ddlResume.Items.Insert(0, new System.Web.UI.WebControls.ListItem(CommonFunction.GetResourceValue("LabelChooseResume"), "0"));

            if (ddlResume.Items.Count > 1)
            {
                cvUseExiting.Visible = true;
            }
        }

        private void LoadCoverLetter()
        {
            ddlCoverLetter.Items.Clear();

            if (SessionData.Member != null)
            {
                using (TList<JXTPortal.Entities.MemberFiles> MemberFiles = MemberFilesService.GetByMemberId(SessionData.Member.MemberId))
                {
                    ddlCoverLetter.DataSource = MemberFiles;
                    MemberFiles.Filter = "DocumentTypeID = 1";
                    ddlCoverLetter.DataBind();
                }
            }

            ddlCoverLetter.Items.Insert(0, new System.Web.UI.WebControls.ListItem(CommonFunction.GetResourceValue("LabelChooseCover"), "0"));

            if (ddlCoverLetter.Items.Count > 1)
            {
                clUseExisting.Visible = true;
            }
        }

        private void CheckMemberApplied(int jobid, int memberid = 0, bool justapply = false)
        {
            if (SessionData.Member != null || memberid > 0)
            {
                if (SessionData.Member != null)
                {
                    memberid = SessionData.Member.MemberId;
                }

                using (TList<JobApplication> jobApplicationList = JobApplicationService.CustomGetByJobIdMemberId(jobid, memberid))
                {
                    if (jobApplicationList != null && jobApplicationList.Count > 0 &&
                            memberid == jobApplicationList[0].MemberId &&
                            jobApplicationList[0].Draft == false)
                    {
                        if (justapply)
                        {
                            phError.Visible = true;
                            litErrorMessage.Text = String.Format("{0} {1}", CommonFunction.GetResourceValue("LabelJobApplied"),
                                                ((DateTime)jobApplicationList[0].ApplicationDate).ToString(SessionData.Site.DateFormat));
                        }
                        else
                        {
                            Response.Redirect("~/" + Utils.GetJobUrl(JobID, JobName, Profession));
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Action = Request.RawUrl;
            phSuccess.Visible = false;
            phError.Visible = false;
            divEmailRequired.Style["display"] = "none";
            pResumeRequired.Visible = false;
            divCoverLetterFormatNotValid.Visible = false;
            phLoginError.Visible = false;

            SetSocialMediaLoginButtons();
            SetDisplay();

            if (!string.IsNullOrEmpty(hfOAuthError.Value))
            {
                if (hfOAuthError.Value == "permission")
                {
                    phError.Visible = true;
                    litErrorMessage.Text = CommonFunction.GetResourceValue("LabelFullPermissionRequired");
                }
            }

            try
            {
                // Apply with seek checking
                HttpCookie seekCookie = new HttpCookie("Seek");
                string authorizationCode = string.Empty;
                if (Request.Cookies["Seek"] != null)
                {
                    seekCookie = Request.Cookies["Seek"];
                    authorizationCode = seekCookie.Value;

                    seekCookie.Expires = DateTime.Now.AddDays(-10);
                    Response.Cookies.Add(seekCookie);
                }

                oAuthSeek _oauth = new oAuthSeek();

                string clientId = string.Empty;
                string clientSecret = string.Empty;
                string errormsg = string.Empty;

                string seekauthorizationToken = string.Empty;
                // Read the cookie information and display it.
                if (!string.IsNullOrEmpty(authorizationCode))
                {
                    AdminIntegrations.Integrations integrations = IntegrationsService.AdminIntegrationsForSiteGet(SessionData.Site.SiteId);
                    if (integrations.Seek != null && integrations.Seek.Valid)
                    {
                        clientId = integrations.Seek.ClientID;
                        clientSecret = integrations.Seek.ClientSecret;
                    }
                    seekauthorizationToken = _oauth.oAuth2GetAuthorizeToken((ConfigurationManager.AppSettings["IsSeekLive"] == "1"), Request.Url.Host, clientId, clientSecret, authorizationCode, out errormsg);

                    if (!string.IsNullOrEmpty(errormsg))
                    {
                        phError.Visible = true;
                        litErrorMessage.Text = errormsg;
                    }
                    else
                    {
                        HttpCookie seekAuthCookie = new HttpCookie("SeekAuth");
                        seekAuthCookie.Value = seekauthorizationToken;
                        seekAuthCookie.Expires = DateTime.Now.AddMinutes(20);
                        Response.Cookies.Add(seekAuthCookie);
                    }
                }


                if (SessionData.Member != null)
                {
                    phLoginPanel.Visible = false;

                    if (Request.UrlReferrer != null && Request.UrlReferrer.OriginalString.ToLower().Contains("oauthcallback.aspx"))
                    {
                        phSocialLoggedIn.Visible = true;
                        ltFirstName.Text = SessionData.Member.FirstName;
                    }
                    else
                    {
                        phBoardyLoggedIn.Visible = true;
                        ltBoardyFirstName.Text = SessionData.Member.FirstName;
                    }

                    phJustApplyTop.Visible = false;
                    phJustApplyTopSection.Visible = false;
                    phJustApplyTopLeft.Visible = false;
                    phJustApplyLeft.Visible = false;
                    phJustApplyBottomLeft.Visible = false;
                    phJustApplyTopRight.Visible = false;
                    phJustApplyRight.Visible = false;
                    phJustApplyBottomRight.Visible = false;
                    phJustApplyBottomSection.Visible = false;
                    phJustApplyBottom.Visible = false;
                    cvUseProfile.Visible = true;

                    if (!Page.IsPostBack)
                    {
                        LoadCoverLetter();
                        LoadResume();
                    }

                }
                else
                {
                    clUseExisting.Visible = false;
                    cvUseProfile.Visible = false;
                    cvUseExiting.Visible = false;
                }

                // Custom Form - Currently this is hardcoded in future will change to dynamic form.
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["PeopleBankWLs"]) && ConfigurationManager.AppSettings["PeopleBankWLs"].Contains(" " + SessionData.Site.SiteId + " "))
                {
                    phJustApplyTop.Visible = false;
                    phJustApplyBottom.Visible = false;

                    phJustApplyTopSection.Visible = true;
                    phJustApplyTopLeft.Visible = true;
                    phJustApplyBottomLeft.Visible = true;
                    phJustApplyTopRight.Visible = true;
                    phJustApplyBottomRight.Visible = true;
                    phJustApplyBottomSection.Visible = true;

                    pnlPeopleBankLeft.Visible = true;
                    pnlPeopleBankRight.Visible = true;
                }
                else
                {
                    pnlPeopleBankLeft.Visible = false;
                    pnlPeopleBankRight.Visible = false;
                }
                // Safe Search
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["SafeSearchWLs"]) && ConfigurationManager.AppSettings["SafeSearchWLs"].Contains(" " + SessionData.Site.SiteId + " "))
                {
                    phJustApplyTop.Visible = false;
                    phJustApplyBottom.Visible = false;

                    phJustApplyTopSection.Visible = true;
                    phJustApplyTopLeft.Visible = true;
                    phJustApplyBottomLeft.Visible = true;
                    phJustApplyTopRight.Visible = true;
                    phJustApplyBottomRight.Visible = true;
                    phJustApplyBottomSection.Visible = true;

                    pnlSafeSearchLeft.Visible = true;
                    pnlSafeSearchRight.Visible = true;

                    divPhone.Visible = false;
                }
                else
                {
                    pnlSafeSearchLeft.Visible = false;
                    pnlSafeSearchRight.Visible = false;
                }

                if (JobID > 0)
                {
                    using (VList<JXTPortal.Entities.ViewJobs> jobs = ViewJobsService.GetByID(JobID, SessionData.Site.SiteId))
                    {
                        if (jobs.Count > 0)
                        {
                            //Get Integration Details
                            AdminIntegrations.Integrations integrations = IntegrationsService.AdminIntegrationsForSiteGet(SessionData.Site.SiteId);
                            if (integrations != null && integrations.GoogleDrive != null)
                            {
                                AdminIntegrations.GoogleDrive googledrive = integrations.GoogleDrive;
                                if (googledrive.Valid)
                                {
                                    GoogleDeveloperKey = googledrive.ApplicationSecret;
                                    GoogleClientID = googledrive.ApplicationID;

                                    phCLGoogleDrive.Visible = true;
                                    phResumeGoogleDrive.Visible = true;
                                }
                            }

                            if (integrations != null && integrations.Dropbox != null)
                            {
                                AdminIntegrations.Dropbox dropboxdrive = integrations.Dropbox;
                                if (dropboxdrive.Valid)
                                {
                                    DropboxKey = dropboxdrive.ApplicationID;

                                    phCLDropbox.Visible = true;
                                    phResumeDropbox.Visible = true;
                                }
                            }

                            if (integrations != null && integrations.Indeed != null)
                            {
                                AdminIntegrations.Indeed indeed = integrations.Indeed;
                                if (indeed.Valid)
                                {
                                    IndeedAPIToken = indeed.APIToken;
                                    JobLocation = jobs[0].SiteAreaName + ", " + jobs[0].SiteLocationName;
                                    CompanyName = jobs[0].CompanyName;
                                    JobTitle = jobs[0].JobName.Replace("\"", "'");
                                    JobURL = string.Format("{0}://{1}{2}", (Request.IsSecureConnection) ? "https" : "http", Request.Url.Host, JobsService.GetJobUrl(Profession, JobName, jobs[0].JobId));
                                    PostURL = string.Format("{0}://{1}{2}", (Request.IsSecureConnection) ? "https" : "http", Request.Url.Host,
                                        "/oauthcallback.aspx?cbtype=indeed&cbaction=apply&jobid=" + JobID.ToString() +
                                        "&profession=" + Profession + "&jobname=" + JobName
                                        );
                                    //protected string PostURL
                                    phApplyWithIndeed.Visible = true;
                                }
                            }

                            if (integrations != null && integrations.Seek != null)
                            {
                                AdminIntegrations.Seek seek = integrations.Seek;
                                if (seek.Valid)
                                {
                                    JobURL = string.Format("{0}://{1}{2}", (Request.IsSecureConnection) ? "https" : "http", Request.Url.Host, JobsService.GetJobUrl(Profession, JobName, jobs[0].JobId));

                                    hlApplyWithSeek.Text = CommonFunction.GetResourceValue("LabelApplyWith") + "<span class=\"seek-apply-btn__image\" aria-label=\"SEEK\"></span>";
                                    //hlApplyWithSeek.NavigateUrl = string.Format("{0}/api/iam/oauth2/authorize?client_id={1}&redirect_uri={2}oauthcallback.aspx%3Fcbtype%3Dseek%26cbaction%3Dapply&scope=r_profile_apply&response_type=code&state={3}",
                                    //                                seekurl, seek.ClientID, Server.UrlEncode("https://" + Request.Url.Authority + "/"), Server.UrlEncode(string.Format("{0}|{1}", jobs[0].JobId, JobURL)));
                                    phApplyWithSeek.Visible = true;
                                }
                            }

                            if (phApplyWithFacebook.Visible == false && phApplyWithLinkedIn.Visible == false && phApplyWithIndeed.Visible == false && phApplyWithSeek.Visible == false)
                            {
                                phApplyWith.Visible = false;
                            }

                            //        <%--<a class="seek-apply-btn" href="#">
                            //    <JXTControl:ucLanguageLiteral ID="UcLanguageLiteral12" runat="server" SetLanguageCode="LabelApplyWith" /><span class="seek-apply-btn__image" aria-label="SEEK"></span>
                            //</a>--%>


                            // Update Job View for the Job.
                            if (Utils.GetCookieDomain(Request.Cookies["JobsViewed"], JobID) == string.Empty)
                            {
                                bool needupdate = false;
                                string domain = Utils.GetUrlDomain();
                                Utils.UpdateJobsViewedCookie(JobID, domain, out needupdate);
                                if (needupdate)
                                {
                                    JobViewsService.UpdateCounter(JobID, SessionData.Site.SiteId, domain, DateTime.Now.Date);
                                }
                            }

                            JXTPortal.Entities.ViewJobs job = null;
                            foreach (JXTPortal.Entities.ViewJobs j in jobs)
                            {
                                using (DataSet ds = SiteProfessionService.GetBySiteIdFriendlyUrl(SessionData.Site.SiteId, Profession))
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow dr = ds.Tables[0].Rows[0];
                                        if (j.ProfessionId == Convert.ToInt32(dr["ProfessionID"]))
                                        {
                                            job = j;

                                            CheckMemberApplied(j.JobId);

                                            break;
                                        }
                                    }
                                }
                            }

                            if (job == null)
                            {
                                Response.Redirect("~/advancedsearch.aspx");
                            }


                            // Retrieving Seek Data
                            if (!string.IsNullOrEmpty(seekauthorizationToken))
                            {
                                string filename = string.Empty;
                                string applicationid = string.Empty;
                                string firstname = string.Empty;
                                string lastname = string.Empty;
                                string email = string.Empty;
                                string phone = string.Empty;
                                string url = string.Format("{0}://{1}{2}", (Request.IsSecureConnection) ? "https" : "http", Request.Url.Host, JobsService.GetJobUrl(Profession, JobName, job.JobId));

                                string result = _oauth.oAuth2GetPrefilled((ConfigurationManager.AppSettings["IsSeekLive"] == "1"), clientId, clientSecret, seekauthorizationToken, integrations.Seek.AdvertiserID, job.JobName, url, "", "", out applicationid, out firstname, out lastname, out email, out phone, out filename, out errormsg);
                                if (string.IsNullOrEmpty(errormsg))
                                {
                                    if (SessionData.Member == null)
                                    {
                                        tbFirstName.Text = firstname;
                                        tbLastName.Text = lastname;
                                        tbEmail.Text = email;
                                        tbConfirmEmail.Text = email;
                                        tbPhone.Text = phone;
                                    }

                                    hfSeekApplicationID.Value = applicationid;
                                    if (!string.IsNullOrEmpty(filename))
                                    {
                                        hfSeekResumeURL.Value = result;
                                        ltSeekFileName.Text = HttpUtility.HtmlEncode(filename);
                                    }
                                    else
                                    {
                                        ViewState["SeekJson"] = result;
                                        hfSeekResumeURL.Value = "SeekJson";
                                        ltSeekFileName.Text = "Seek Resume.docx";
                                    }

                                    divResumeFile.Style.Clear();
                                }
                                else
                                {
                                    phError.Visible = true;
                                    litErrorMessage.Text = errormsg;
                                }

                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(hfSeekResumeURL.Value))
                                {
                                    divResumeFile.Style.Clear();
                                }
                            }


                            DateTime timenow = DateTime.Now;
                            ltJobName.Text = job.JobName;
                            // Check if the job is expired if yes then redirect to the job detail page.
                            if (job.Expired.HasValue && (job.Expired.Value == (int)PortalEnums.Jobs.JobStatus.Expired) || DateTime.Compare(job.ExpiryDate, new DateTime(timenow.Year, timenow.Month, timenow.Day, timenow.Hour, timenow.Minute, timenow.Second)) <= 0)
                            {
                                Response.Redirect(Utils.GetJobUrl(job.JobId, job.JobFriendlyName));
                            }

                            // You can only apply for live jobs.
                            if (job.Expired.HasValue && (job.Expired.Value != (int)PortalEnums.Jobs.JobStatus.Live))
                                Response.Redirect("~/advancedsearch.aspx");


                            // SEO - Job Name on the Browser Title
                            using (Entities.GlobalSettings globalSettings = GlobalSettingsService.GetGlobalSettingBySiteID(SessionData.Site.SiteId))
                            {
                                if (globalSettings != null)
                                    CommonPage.SetBrowserPageTitle(Page, string.Format("{2} {0} {1}", job.JobName, globalSettings.PageTitleSuffix, CommonFunction.GetResourceValue("LabelApplyFor")));
                                else
                                    CommonPage.SetBrowserPageTitle(Page, CommonFunction.GetResourceValue("LabelApplyFor") + " " + job.JobName);
                            }

                            // SEO - Set the Browser Description - Description or Bulletpoints
                            string strJobDescription = string.Empty;
                            if ((!string.IsNullOrEmpty(job.BulletPoint1) && job.BulletPoint1.Trim().Length > 0) ||
                                    (!string.IsNullOrEmpty(job.BulletPoint2) && job.BulletPoint2.Trim().Length > 0) ||
                                    (!string.IsNullOrEmpty(job.BulletPoint3) && job.BulletPoint3.Trim().Length > 0))
                                if ((string.IsNullOrEmpty(job.BulletPoint3)) && (string.IsNullOrEmpty(job.BulletPoint2)))
                                    strJobDescription = job.BulletPoint1;
                                else if (string.IsNullOrEmpty(job.BulletPoint3))
                                    strJobDescription = String.Format(@"{0}, {1}",
                                                            job.BulletPoint1,
                                                            job.BulletPoint2);
                                else
                                    strJobDescription = String.Format(@"{0}, {1}, {2}",
                                                                job.BulletPoint1,
                                                                job.BulletPoint2,
                                                                job.BulletPoint3);
                            else
                            {
                                strJobDescription = job.Description;

                                if (strJobDescription.Length > 150)
                                {
                                    strJobDescription = (new System.Text.StringBuilder(strJobDescription, 0, 150, 150)).ToString();
                                }
                            }
                            CommonPage.SetBrowserPageDescription(Page, strJobDescription);
                        }
                        else
                        {
                            Response.Redirect("~/advancedsearch.aspx");
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/advancedsearch.aspx");
                }
            }
            catch (Exception ex)
            {
                phError.Visible = true;
                litErrorMessage.Text = ex.Message;
            }

        }

        protected void hlApplyWithSeek_Click(object sender, EventArgs e)
        {
            string seekurl = (ConfigurationManager.AppSettings["IsSeekLive"].ToString() == "1") ? "https://www.seek.com.au" : "https://www.seek.com.au.sandbox-qa";

            if (JobID > 0)
            {
                using (VList<JXTPortal.Entities.ViewJobs> jobs = ViewJobsService.GetByID(JobID, SessionData.Site.SiteId))
                {
                    if (jobs.Count > 0)
                    {
                        //Get Integration Details
                        AdminIntegrations.Integrations integrations = IntegrationsService.AdminIntegrationsForSiteGet(SessionData.Site.SiteId);
                        if (integrations != null && integrations.Seek != null)
                        {
                            AdminIntegrations.Seek seek = integrations.Seek;
                            if (seek.Valid)
                            {
                                JobURL = string.Format("{0}://{1}/applyjob{2}", (Request.IsSecureConnection) ? "https" : "http", Request.Url.Host, JobsService.GetJobUrl(Profession, JobName, jobs[0].JobId));

                                Response.Redirect(string.Format("{0}/api/iam/oauth2/authorize?client_id={1}&redirect_uri={2}oauthcallback.aspx%3Fcbtype%3Dseek%26cbaction%3Dapply&scope=r_profile_apply&response_type=code&state={3}",
                                                                seekurl, seek.ClientID, Server.UrlEncode(((Request.IsSecureConnection) ? "https://" : "http://") + Request.Url.Host + "/"), Server.UrlEncode(string.Format("{0}|{1}", jobs[0].JobId, JobURL))));
                            }
                        }
                    }
                }
            }
        }

        protected void ibApplyWithLinkedIn_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oAuthLinkedIn _oauth = new oAuthLinkedIn();
                string LinkedInAPI = string.Empty;
                string urlsuffix = string.Empty;

                using (TList<JXTPortal.Entities.GlobalSettings> tgs = GlobalSettingsService.GetBySiteId(SessionData.Site.SiteId))
                {
                    if (tgs.Count > 0)
                    {
                        JXTPortal.Entities.GlobalSettings gs = tgs[0];
                        if (!string.IsNullOrEmpty(gs.LinkedInApi))
                        {
                            LinkedInAPI = gs.LinkedInApi;
                            string http = "http://";
                            if (Request.IsSecureConnection)
                                http = "https://";
                            urlsuffix = http + HttpContext.Current.Request.Url.Host;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(LinkedInAPI))
                    Response.Redirect(_oauth.RequestToken(LinkedInAPI, urlsuffix, JobID.ToString(), HttpContext.Current.Request.RawUrl, Utils.GetUrlReferrerDomain(), new List<string> { "cbtype=linkedin", "cbaction=Apply" }));
            }
            catch (Exception ex)
            {
                phError.Visible = true;
                litErrorMessage.Text = ex.Message;
            }
        }

        private void SetFocus(string clientid)
        {
            string js = "$(document).ready(function() { var el = document.getElementById(\"" + clientid + "\"); el.scrollIntoView(false); })";

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "focusJS", js, true);
        }

        protected void apply_Click(object sender, EventArgs e)
        {
            if (SessionData.Member == null && phJustApplyTopLeft.Visible == false)
            {
                Response.Redirect(Request.RawUrl);
            }

            try
            {
                // Error Checking
                bool hasError = false;
                // Email Address
                string memberemail = tbEmail.Text;
                if (SessionData.Member != null)
                {
                    memberemail = SessionData.Member.EmailAddress;
                }

                bool validEmail = Regex.IsMatch(memberemail, @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$");
                if (!validEmail)
                {
                    divEmailRequired.Style["display"] = "block";
                    hasError = true;
                }

                // Resume

                if (string.IsNullOrEmpty(hfSeekResumeURL.Value))
                {
                    if (rbUploadResume.Checked)
                    {
                        if (fileUploadResume.HasFile == false && hfDropBoxResumeFileURL.Value == string.Empty && hfGoogleResumeFileId.Value == string.Empty)
                        {
                            pResumeRequired.Visible = true;
                            hasError = true;
                        }
                        else
                        {
                            string filename = fileUploadResume.PostedFile.FileName;
                            if (!string.IsNullOrWhiteSpace(hfDropBoxResumeFileName.Value))
                            {
                                filename = hfDropBoxResumeFileName.Value;
                            }

                            if (!string.IsNullOrWhiteSpace(hfGoogleResumeFileName.Value))
                            {
                                filename = hfGoogleResumeFileName.Value;
                            }

                            if (!CommonFunction.CheckExtension(filename))
                            {
                                SetFocus(fileUploadResume.ClientID);
                                spResumeError.InnerText = CommonFunction.GetResourceValue("ErrorInvalidFileType");
                                pResumeRequired.Visible = true;
                                hasError = true;
                            }
                        }
                    }

                    if (rbExistingResume.Checked && ddlResume.SelectedValue == "0")
                    {
                        pResumeRequired.Visible = true;
                        hasError = true;
                    }
                }

                // Cover Letter
                if (rbWriteOneNow.Checked)
                {
                    if (string.IsNullOrEmpty(Utils.StripHTML(txtCoverLetterText.Text.Trim())))
                    {
                        SetFocus(txtCoverLetterText.ClientID);
                        spCoverLetterError.InnerText = "Cover Letter is empty";
                        divCoverLetterFormatNotValid.Visible = true;
                        hasError = true;
                    }
                }

                if (rbUploadCoverLetter.Checked)
                {
                    string filename = fileUploadCV.PostedFile.FileName;
                    if (!string.IsNullOrWhiteSpace(hfDropBoxCLFileName.Value))
                    {
                        filename = hfDropBoxCLFileName.Value;
                    }

                    if (!string.IsNullOrWhiteSpace(hfGoogleCLFileName.Value))
                    {
                        filename = hfGoogleCLFileName.Value;
                    }

                    if (!string.IsNullOrWhiteSpace(filename))
                    {
                        if (!CommonFunction.CheckExtension(filename))
                        {
                            SetFocus(fileUploadCV.ClientID);
                            spCoverLetterError.InnerText = CommonFunction.GetResourceValue("ErrorInvalidFileType");
                            divCoverLetterFormatNotValid.Visible = true;
                            hasError = true;
                        }
                    }

                }

                if (hasError) return;

                JXTPortal.Entities.Members member = null;

                if (SessionData.Member == null && phJustApplyTopLeft.Visible)
                {
                    // First checking email, then username
                    member = MembersService.GetBySiteIdEmailAddress(SessionData.Site.MasterSiteId, tbEmail.Text.Trim());
                    if (member != null)
                    {
                        //member.FirstName = tbFirstName.Text;
                        //member.Surname = tbLastName.Text;
                        //member.MobilePhone = tbPhone.Text;
                        //member.SearchField = String.Format("{0} {1} {2}",
                        //                           member.FirstName,
                        //                           member.Surname,
                        //                           member.SearchField);
                        litErrorMessage.Text = CommonFunction.GetResourceValue("LabelEmailAlreadyExist");
                        phError.Visible = true;
                        return;
                    }
                    else
                    {
                        member = MembersService.GetBySiteIdUsername(SessionData.Site.MasterSiteId, tbEmail.Text.Trim());
                        if (member != null)
                        {
                            litErrorMessage.Text = CommonFunction.GetResourceValue("LabelUsernameExists");
                            phError.Visible = true;
                            return;
                        }
                        else
                        {
                            // If Enworld then ignore checking Salesforce
                            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EnworldSiteID"]) && !(ConfigurationManager.AppSettings["EnworldSiteID"].Contains(" " + SessionData.Site.MasterSiteId + " ")))
                            {
                                // SALESFORCE - Check if the account is on Salesforce, grab the account.
                                if (GetFromSalesforceAndSave(tbEmail.Text.Trim(), string.Empty))
                                {
                                    litErrorMessage.Text = CommonFunction.GetResourceValue("LabelEmailAlreadyExist");
                                    phError.Visible = true;
                                    return;
                                }
                            }

                        }
                    }

                    if (member == null)
                    {
                        member = new JXTPortal.Entities.Members();
                        string newpassword = tbNewPassword.Text;
                        member.FirstName = CommonService.EncodeString(tbFirstName.Text.Trim());
                        member.Surname = CommonService.EncodeString(tbLastName.Text.Trim());
                        member.Username = CommonService.EncodeString(tbEmail.Text.Trim());
                        member.EmailAddress = CommonService.EncodeString(tbEmail.Text.Trim());
                        member.MobilePhone = CommonService.EncodeString(tbPhone.Text.Trim());
                        member.EmailFormat = 1;
                        member.CountryId = 1;
                        TList<GlobalSettings> service = GlobalSettingsService.GetBySiteId(SessionData.Site.SiteId);
                        if (service.Count > 0)
                        {
                            if (service[0].DefaultCountryId.HasValue)
                            {
                                member.CountryId = service[0].DefaultCountryId.Value;
                            }
                        }
                        member.SiteId = SessionData.Site.MasterSiteId;
                        member.ValidateGuid = Guid.NewGuid();
                        member.Password = CommonService.EncryptMD5(newpassword);
                        member.ValidateGuid = Guid.NewGuid();
                        member.MonthlyUpdate = true;
                        member.ReferringSiteId = SessionData.Site.SiteId;
                        member.SearchField = String.Format("{0} {1}",
                                                       member.FirstName,
                                                       member.Surname);

                        if (MembersService.Insert(member))
                        {
                            MailService.SendMemberRegistration(member, newpassword);
                        }
                        else
                        {
                            litErrorMessage.Text = CommonFunction.GetResourceValue("LabelFailedInsertMember");
                            phError.Visible = true;

                            return;
                        }
                    }
                }
                else
                {
                    member = MembersService.GetByMemberId(SessionData.Member.MemberId);
                }

                CheckMemberApplied(JobID, member.MemberId, phJustApplyTop.Visible);
                if (phError.Visible)
                {
                    return;
                }
                else
                {
                    MembersService.Update(member);
                }

                int jobappid = 0;
                string jobapplicationemail = string.Empty;
                // Check existing member

                if (member != null)
                {
                    string strUrlReferral = Utils.GetCookieDomain(Request.Cookies["JobsViewed"], JobID);

                    // [TODO] Member Apply for job sp
                    using (JXTPortal.Entities.JobApplication jobapp = new JXTPortal.Entities.JobApplication())
                    {
                        jobapp.ApplicationDate = DateTime.Now;
                        jobapp.JobId = JobID;
                        jobapp.MemberId = member.MemberId;

                        jobapp.JobAppValidateId = new Guid();
                        jobapp.SiteIdReferral = SessionData.Site.SiteId;
                        jobapp.UrlReferral = strUrlReferral;
                        jobapp.FirstName = member.FirstName;
                        jobapp.Surname = member.Surname;
                        jobapp.EmailAddress = member.EmailAddress;
                        jobapp.MobilePhone = member.MobilePhone;
                        jobapp.ApplicationStatus = (int)PortalEnums.JobApplications.ApplicationStatus.Applied;
                        jobapp.Draft = false;
                        JobApplicationService.Insert(jobapp);
                        jobappid = jobapp.JobApplicationId;

                        // Disable the Apply Button
                        apply.Enabled = false;

                        using (Entities.Jobs job = JobsService.GetByJobId(jobapp.JobId.Value))
                        {
                            if (job != null)
                            {
                                jobapplicationemail = job.ApplicationEmailAddress;
                                string domain = string.Empty;
                                ltReferrer.Text = domain;

                                // Retrieve value from JobsViewed Cookie, the format is {JobID}|{Domain},...
                                domain = Utils.GetCookieDomain(Request.Cookies["JobsViewed"], JobID);

                                // call JobClientTracking to retrieve job application email if matches criteria (for Broadbean atm) 
                                JobClientTracking tracking = new JobClientTracking(jobapplicationemail);
                                jobapplicationemail = tracking.RetrieveEmail(domain);

                            }
                        }

                    }

                    if (cbSubscribeJobAlert.Checked)
                    {
                        int jobAlertID = JobAlertsService.SaveMemberJobAlert(JobID, SessionData.Site.MasterSiteId, member.MemberId, member.EmailFormat, Profession);
                    }


                    //JobApplicationSyncWithSalesForce(member, jobappid);
                    using (JXTPortal.Entities.JobApplication jobapp = JobApplicationService.GetByJobApplicationId(jobappid))
                    {
                        if (jobapp != null)
                        {
                            int coverletterid = 0;
                            int resumeid = 0;

                            Entities.MemberFiles coverletter = null;
                            Entities.MemberFiles resume = null;

                            string errormessage = string.Empty;

                            bool useFTP = (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FTPJobApplyResumeUrl"]));
                            string ftpclpath = string.Empty;
                            string ftpresumepath = string.Empty;
                            string ftpusername = string.Empty;
                            string ftppassword = string.Empty;


                            Regex r = new Regex("(?:[^a-z0-9.]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                            FtpClient ftpclient = new FtpClient();
                            if (useFTP)
                            {
                                ftpclpath = ConfigurationManager.AppSettings["FTPJobApplyCoverLetterUrl"];
                                ftpresumepath = ConfigurationManager.AppSettings["FTPJobApplyResumeUrl"];
                                ftpusername = ConfigurationManager.AppSettings["FTPJobApplyUsername"];
                                ftppassword = ConfigurationManager.AppSettings["FTPJobApplyPassword"];
                            }

                            if (rbExistingCoverLetter.Checked && ddlCoverLetter.SelectedValue != "0")
                            {
                                coverletterid = Convert.ToInt32(ddlCoverLetter.SelectedValue);
                                coverletter = MemberFilesService.GetByMemberFileId(coverletterid);

                                jobapp.MemberCoverLetterFile = string.Format("{0}_Coverletter_{1}", jobappid, r.Replace(coverletter.MemberFileName, "_"));

                                if (useFTP)
                                {
                                    ftpclient.Host = ftpclpath;
                                    ftpclient.Username = ftpusername;
                                    ftpclient.Password = ftppassword;
                                    ftpclient.UploadFileFromStream(new MemoryStream(coverletter.MemberFileContent), ftpclpath + jobapp.MemberCoverLetterFile, out errormessage);
                                }
                                else
                                {
                                    string filepath = string.Format("{0}{1}_Coverletter_{2}", (ConfigurationManager.AppSettings["ApplicationUploadCoverLetterPaths"]), jobappid, coverletter.MemberFileName);


                                    Utils.EncryptFile(new MemoryStream(coverletter.MemberFileContent), filepath);
                                }
                            }

                            if (rbUploadCoverLetter.Checked)
                            {
                                if (fileUploadCV.PostedFile != null && fileUploadCV.PostedFile.ContentLength > 0)
                                {
                                    jobapp.MemberCoverLetterFile = string.Format("{0}_Coverletter_{1}", jobappid, r.Replace(fileUploadCV.PostedFile.FileName, "_"));
                                    if (useFTP)
                                    {
                                        ftpclient.Host = ftpclpath;
                                        ftpclient.Username = ftpusername;
                                        ftpclient.Password = ftppassword;
                                        ftpclient.UploadFileFromStream(fileUploadCV.PostedFile.InputStream, ftpclpath + jobapp.MemberCoverLetterFile, out errormessage);
                                    }
                                    else
                                    {
                                        string filepath = string.Format("{0}{1}_Coverletter_{2}", (ConfigurationManager.AppSettings["ApplicationUploadCoverLetterPaths"]), jobappid, r.Replace(fileUploadCV.PostedFile.FileName, "_"));
                                        fileUploadCV.PostedFile.SaveAs(filepath);
                                        //Utils.EncryptFile(fileUploadCV.PostedFile.InputStream, filepath);
                                    }
                                }
                                else
                                {
                                    string filename = string.Empty;
                                    Stream filestream = null;

                                    if (!string.IsNullOrEmpty(hfDropBoxCLFileURL.Value))
                                    {
                                        filename = hfDropBoxCLFileName.Value;
                                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hfDropBoxCLFileURL.Value);

                                        request.Method = "GET";

                                        System.Net.WebResponse response = request.GetResponse();
                                        System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)response;
                                        filestream = webresponse.GetResponseStream();

                                    }
                                    else if (!string.IsNullOrEmpty(hfGoogleCLFileId.Value))
                                    {
                                        filename = hfGoogleCLFileName.Value;
                                        System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v2/files/" + hfGoogleCLFileId.Value + "?alt=media");
                                        request.Method = "GET";
                                        request.Headers[HttpRequestHeader.Authorization] = "Bearer " + hfGoogleCLToken.Value;

                                        WebResponse response = request.GetResponse();
                                        HttpWebResponse webresponse = (HttpWebResponse)response;
                                        filestream = webresponse.GetResponseStream();
                                    }

                                    if (filestream != null)
                                    {
                                        jobapp.MemberCoverLetterFile = string.Format("{0}_Coverletter_{1}", jobappid, r.Replace(filename, "_"));

                                        using (MemoryStream writestream = new MemoryStream())
                                        {
                                            filestream.CopyTo(writestream);
                                            if (useFTP)
                                            {
                                                ftpclient.Host = ftpclpath;
                                                ftpclient.Username = ftpusername;
                                                ftpclient.Password = ftppassword;
                                                ftpclient.UploadFileFromStream(writestream, ftpclpath + jobapp.MemberCoverLetterFile, out errormessage);
                                            }
                                            else
                                            {
                                                string filepath = string.Format("{0}{1}_Coverletter_{2}", (ConfigurationManager.AppSettings["ApplicationUploadCoverLetterPaths"]), jobappid, r.Replace(filename, "_"));
                                                using (var fileStream = File.Create(filepath))
                                                {
                                                    writestream.Seek(0, SeekOrigin.Begin);
                                                    writestream.CopyTo(fileStream);
                                                }
                                            }
                                        }
                                    }
                                }
                            }


                            if (string.IsNullOrEmpty(hfSeekResumeURL.Value))
                            {
                                if (rbExistingResume.Checked && ddlResume.SelectedValue != "0")
                                {
                                    resumeid = Convert.ToInt32(ddlResume.SelectedValue);
                                    resume = MemberFilesService.GetByMemberFileId(resumeid);

                                    jobapp.MemberResumeFile = string.Format("{0}_Resume_{1}", jobappid, r.Replace(resume.MemberFileName, "_"));
                                    if (useFTP)
                                    {
                                        ftpclient.Host = ftpresumepath;
                                        ftpclient.Username = ftpusername;
                                        ftpclient.Password = ftppassword;
                                        ftpclient.UploadFileFromStream(new MemoryStream(resume.MemberFileContent), ftpresumepath + jobapp.MemberResumeFile, out errormessage);
                                    }
                                    else
                                    {
                                        string filepath = string.Format("{0}{1}_Resume_{2}", (ConfigurationManager.AppSettings["ApplicationUploadResumePaths"]), jobappid, resume.MemberFileName);

                                        Utils.EncryptFile(new MemoryStream(resume.MemberFileContent), filepath);
                                    }
                                }

                                if (rbUploadResume.Checked)
                                {
                                    if (fileUploadResume.PostedFile != null && fileUploadResume.PostedFile.ContentLength > 0)
                                    {
                                        jobapp.MemberResumeFile = string.Format("{0}_Resume_{1}", jobappid, r.Replace(fileUploadResume.PostedFile.FileName, "_"));

                                        if (useFTP)
                                        {
                                            ftpclient.Host = ftpresumepath;
                                            ftpclient.Username = ftpusername;
                                            ftpclient.Password = ftppassword;
                                            ftpclient.UploadFileFromStream(fileUploadResume.PostedFile.InputStream, ftpresumepath + jobapp.MemberResumeFile, out errormessage);
                                        }
                                        else
                                        {
                                            string filepath = string.Format("{0}{1}_Resume_{2}", (ConfigurationManager.AppSettings["ApplicationUploadResumePaths"]), jobappid, r.Replace(fileUploadResume.PostedFile.FileName, "_"));
                                            fileUploadResume.SaveAs(filepath);

                                            //Utils.EncryptFile(fileUploadResume.PostedFile.InputStream, filepath);
                                        }
                                    }
                                    else
                                    {
                                        string filename = string.Empty;
                                        Stream filestream = null;

                                        if (!string.IsNullOrEmpty(hfDropBoxResumeFileURL.Value))
                                        {
                                            filename = hfDropBoxResumeFileName.Value;
                                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hfDropBoxResumeFileURL.Value);
                                            request.Method = "GET";

                                            System.Net.WebResponse response = request.GetResponse();
                                            System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)response;
                                            filestream = webresponse.GetResponseStream();
                                        }
                                        else if (!string.IsNullOrEmpty(hfGoogleResumeFileId.Value))
                                        {
                                            filename = hfGoogleResumeFileName.Value;
                                            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v2/files/" + hfGoogleResumeFileId.Value + "?alt=media");
                                            request.Method = "GET";
                                            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + hfGoogleResumeToken.Value;

                                            WebResponse response = request.GetResponse();
                                            HttpWebResponse webresponse = (HttpWebResponse)response;
                                            filestream = webresponse.GetResponseStream();
                                        }

                                        if (filestream != null)
                                        {
                                            jobapp.MemberResumeFile = string.Format("{0}_Resume_{1}", jobappid, r.Replace(filename, "_"));

                                            using (MemoryStream writestream = new MemoryStream())
                                            {
                                                filestream.CopyTo(writestream);
                                                if (useFTP)
                                                {
                                                    ftpclient.Host = ftpclpath;
                                                    ftpclient.Username = ftpusername;
                                                    ftpclient.Password = ftppassword;
                                                    ftpclient.UploadFileFromStream(writestream, ftpresumepath + jobapp.MemberResumeFile, out errormessage);
                                                }
                                                else
                                                {
                                                    string filepath = string.Format("{0}{1}_Resume_{2}", (ConfigurationManager.AppSettings["ApplicationUploadResumePaths"]), jobappid, r.Replace(fileUploadResume.PostedFile.FileName, "_"));
                                                    using (var fileStream = File.Create(filepath))
                                                    {
                                                        writestream.Seek(0, SeekOrigin.Begin);
                                                        writestream.CopyTo(fileStream);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Seek Resume
                                HttpCookie seekAuthCookie = new HttpCookie("SeekAuth");
                                if (Request.Cookies["SeekAuth"] != null)
                                {
                                    seekAuthCookie = Request.Cookies["SeekAuth"];
                                    string authorizationCode = seekAuthCookie.Value;

                                    seekAuthCookie.Expires = DateTime.Now.AddDays(-10);
                                    Response.Cookies.Add(seekAuthCookie);

                                    string filename = string.Empty;
                                    string errormsg = string.Empty;
                                    oAuthSeek _oauth = new oAuthSeek();
                                    string result = string.Empty;
                                    MemoryStream stream = new MemoryStream();

                                    if (hfSeekResumeURL.Value == "SeekJson")
                                    {
                                        filename = "Seek.docx";
                                        string strUrl = Page.ResolveUrl("~/GetAdminLogo.aspx?SiteID=" + SessionData.Site.MasterSiteId.ToString());
                                        result = _oauth.oAuth2GetProfileHTML(ViewState["SeekJson"].ToString(), strUrl);

                                        using (WordprocessingDocument package = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
                                        {
                                            MainDocumentPart mainPart = package.MainDocumentPart;
                                            if (mainPart == null)
                                            {
                                                mainPart = package.AddMainDocumentPart();
                                                new Document(new Body()).Save(mainPart);
                                            }

                                            HtmlConverter converter = new HtmlConverter(mainPart);
                                            Body body = mainPart.Document.Body;

                                            var paragraphs = converter.Parse(result);
                                            for (int i = 0; i < paragraphs.Count; i++)
                                            {
                                                body.Append(paragraphs[i]);
                                            }

                                            mainPart.Document.Save();
                                        }
                                    }
                                    else
                                    {
                                        Stream inputstream = _oauth.oAuth2GetResume(authorizationCode, hfSeekResumeURL.Value, out filename, out errormsg);
                                        inputstream.CopyTo(stream);
                                        stream.Position = 0;

                                    }

                                    if (string.IsNullOrEmpty(errormsg))
                                    {
                                        jobapp.MemberResumeFile = string.Format("{0}_Resume_{1}", jobappid, r.Replace(filename, "_"));
                                        jobapp.AppliedWith = "Seek";
                                        ftpclient.Host = ftpresumepath;
                                        ftpclient.Username = ftpusername;
                                        ftpclient.Password = ftppassword;



                                        ftpclient.UploadFileFromStream(stream, ftpresumepath + jobapp.MemberResumeFile, out errormessage);
                                        if (string.IsNullOrEmpty(errormessage))
                                        {
                                            AdminIntegrations.Integrations integrations = IntegrationsService.AdminIntegrationsForSiteGet(SessionData.Site.SiteId);
                                            string clientId = string.Empty;
                                            string clientSecret = string.Empty;

                                            if (integrations.Seek != null && integrations.Seek.Valid)
                                            {
                                                clientId = integrations.Seek.ClientID;
                                                clientSecret = integrations.Seek.ClientSecret;
                                            }

                                            _oauth.oAuth2ApplyComplete((ConfigurationManager.AppSettings["IsSeekLive"] == "1"), clientId, clientSecret, hfSeekApplicationID.Value, out errormsg);
                                            if (!string.IsNullOrEmpty(errormsg))
                                            {
                                                phError.Visible = true;
                                                litErrorMessage.Text = errormsg;
                                                return;
                                            }
                                        }
                                    }

                                }
                            }


                            if (rbWriteOneNow.Checked)
                            {
                                byte[] byteArray = Encoding.ASCII.GetBytes(txtCoverLetterText.Text);
                                MemoryStream stream = new MemoryStream(byteArray);
                                jobapp.MemberCoverLetterFile = string.Format("{0}_Coverletter.txt", jobappid);

                                if (useFTP)
                                {
                                    ftpclient.Host = ftpclpath;
                                    ftpclient.Username = ftpusername;
                                    ftpclient.Password = ftppassword;
                                    ftpclient.UploadFileFromStream(stream, ftpclpath + jobapp.MemberCoverLetterFile, out errormessage);
                                }
                                else
                                {
                                    string filepath = string.Format("{0}{1}_Coverletter.txt", (ConfigurationManager.AppSettings["ApplicationUploadCoverLetterPaths"]), jobappid);


                                    StreamWriter sw = File.CreateText(filepath);
                                    sw.Write(txtCoverLetterText.Text);
                                    sw.Close();
                                    //Utils.EncryptFile(stream, filepath);

                                }
                            }

                            // When Use my profile is checked
                            if (rbUseMyProfile.Checked)
                            {

                                //define full virtual path
                                var fullPath = "~/usercontrols/member/ucCVProfileDownload.ascx";

                                //initialize a new page to host the control
                                Page page = new Page();
                                //disable event validation (this is a workaround to handle the "RegisterForEventValidation can only be called during Render()" exception)
                                page.EnableEventValidation = false;

                                //load the control and add it to the page's form
                                JXTPortal.Website.usercontrols.member.ucCVProfileDownload control = (JXTPortal.Website.usercontrols.member.ucCVProfileDownload)page.LoadControl(fullPath);
                                control.Setup(SessionData.Member.MemberId);
                                page.Controls.Add(control);

                                //call RenderControl method to get the generated HTML
                                string html = RenderControl(page);

                                byte[] file = new PDFCreator().ConvertHTMLToPDF(html);

                                jobapp.MemberResumeFile = string.Format("{0}_Resume_Profile_{1}.pdf", jobappid, SessionData.Member.MemberId); // using memberid is as suffix of file name
                                jobapp.AppliedWith = "Profile";

                                if (useFTP)
                                {
                                    ftpclient.Host = ftpresumepath;
                                    ftpclient.Username = ftpusername;
                                    ftpclient.Password = ftppassword;
                                    ftpclient.UploadFileFromStream(new MemoryStream(file), ftpresumepath + jobapp.MemberResumeFile, out errormessage);
                                }
                                else
                                {
                                    string filepath = string.Format("{0}{1}_Resume_Profile_{2}.pdf", (ConfigurationManager.AppSettings["ApplicationUploadResumePaths"]), jobappid, SessionData.Member.MemberId);
                                    Utils.EncryptFile(new MemoryStream(file), filepath);
                                }
                            }

                            // Custom Fields
                            HybridDictionary customEmailFields = new HybridDictionary();

                            // People Bank Custom Fields
                            if (pnlPeopleBankLeft.Visible && pnlPeopleBankRight.Visible)
                            {
                                customEmailFields.Add("CUSTOM_LANDLINE", CommonService.EncodeString(tbLandLine.Text));
                                customEmailFields.Add("CUSTOM_STATE", ddlState.SelectedValue);
                                customEmailFields.Add("CUSTOM_RESIDENCYSTATUS", ddlResidencyStatus.SelectedValue);
                            }

                            // Custom Safe Search Fields
                            if (pnlSafeSearchLeft.Visible && pnlSafeSearchRight.Visible)
                            {
                                customEmailFields.Add("CUSTOM_ADDRESS", CommonService.EncodeString(tbAddress.Text));
                                customEmailFields.Add("CUSTOM_COUNTRY", CommonService.EncodeString(tbCountry.Text));
                                customEmailFields.Add("CUSTOM_POSTCODE", CommonService.EncodeString(tbPostcode.Text));
                                customEmailFields.Add("CUSTOM_CONTACTNO", CommonService.EncodeString(tbContactNumber.Text));
                                customEmailFields.Add("CUSTOM_WILLINGTORELOCATE", ddlWillingToRelocate.SelectedItem.Text);
                                customEmailFields.Add("CUSTOM_PREFERREDEMPLOYMENT", ddlPreferredEmployment.SelectedItem.Text);
                                customEmailFields.Add("CUSTOM_SALARY", ddlSalary.SelectedItem.Text);
                            }

                            if (JobApplicationService.Update(jobapp))
                            {
                                int siteid = SessionData.Site.SiteId;
                                using (Entities.Jobs job = JobsService.GetByJobId(Convert.ToInt32(JobID)))
                                {
                                    if (job != null)
                                    {
                                        siteid = job.SiteId;
                                    }
                                }

                                MailService.SendMemberJobApplicationEmail(member);

                                // CUSTOM - ENWORLD SITES don't send advertiser emails.
                                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EnworldSiteID"]) &&
                                        !ConfigurationManager.AppSettings["EnworldSiteID"].Contains(" " + SessionData.Site.MasterSiteId + " "))
                                {
                                    MailService.SendAdvertiserJobApplicationEmail(member, jobapp, customEmailFields, siteid, jobapplicationemail);
                                }        
           
                                Response.Redirect(DynamicPagesService.GetDynamicPageUrl(JXTPortal.SystemPages.JOBAPPLY_SUCCESS, "", "", ""));
                            }
                            else
                            {
                                // litMessage.Text = string.Format(CommonFunction.GetResourceValue("LabelJobApplyFailed"));
                                apply.Enabled = true;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            if (LoginCheck())
            {
                phLoginPanel.Visible = false;
                phSocialLoggedIn.Visible = false;
                phBoardyLoggedIn.Visible = true;
                phJustApplyTop.Visible = false;
                phJustApplyTopSection.Visible = false;
                phJustApplyTopLeft.Visible = false;
                phJustApplyLeft.Visible = false;
                phJustApplyBottomLeft.Visible = false;
                phJustApplyTopRight.Visible = false;
                phJustApplyRight.Visible = false;
                phJustApplyBottomRight.Visible = false;
                phJustApplyBottom.Visible = false;
                phJustApplyBottomSection.Visible = false;
                cvUseProfile.Visible = true;
                ltBoardyFirstName.Text = SessionData.Member.FirstName;

                if (cbRememberMe.Checked)
                {
                    HttpCookie memberCookie = new HttpCookie(PortalConstants.SiteCookie.MemberCookie);

                    memberCookie.Value = Common.Utils.Encrypt(SessionData.Member.MemberId.ToString() + "-" + SessionData.Site.AuthToken, true);
                    memberCookie.HttpOnly = true;

                    // If SSL then secure the cookie
                    if (Request.IsSecureConnection)
                        memberCookie.Secure = true;

                    //advertiserCookie["user"] = Common.Utils.Encrypt(SessionData.AdvertiserUser.AdvertiserUserId.ToString(), true);
                    memberCookie.Expires = DateTime.Now.AddDays(7); //last for 1 week
                    Response.Cookies.Add(memberCookie);

                }

                Response.Redirect(string.Format("/applyjob/{0}-jobs/{1}/{2}", Profession, JobName, JobID));

                LoadResume();
                LoadCoverLetter();
            }
            else
            {
                phLoginError.Visible = true;
            }
        }

        /*
        private void JobApplicationSyncWithSalesForce(Entities.Members thisMember, int jxtJobApplicationID)
        {
            JobApplication thisApplication;
            Entities.Jobs thisJob;
            
            #region Data Retrieval
            thisApplication = JobApplicationService.GetByJobApplicationId(jxtJobApplicationID);
            if (thisApplication == null)
            {
                //error
                return;
            }

            thisJob = JobsService.GetByJobId(thisApplication.JobId.Value);

            if (thisJob == null)
            {
                //error
                return;
            }


            #endregion

            string SFContactID;
            SalesforceMemberSync memberSync = new SalesforceMemberSync();
            //Calling this will ensure the member's record will be available on the SalesForce, true flag denotes no check on member's account is validated or not
            bool contactSyncSuccess = memberSync.CheckContactAndSaveInSalesForce(thisMember, thisMember.SiteId, true, out SFContactID);

            if (contactSyncSuccess && !string.IsNullOrEmpty(SFContactID))
            {
                SalesforceIntegration sfInt = new SalesforceIntegration();

                {
                    //upload attachement
                    byte[] fileByte = null;


                    bool useFTP = (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FTPJobApplyResumeUrl"]));
                    string ftpclpath = string.Empty;
                    string ftpresumepath = string.Empty;
                    string ftpusername = string.Empty;
                    string ftppassword = string.Empty;
                    string errormessage = string.Empty;
                    FtpClient ftpclient = new FtpClient();
                    if (useFTP)
                    {
                        ftpclpath = ConfigurationManager.AppSettings["FTPJobApplyCoverLetterUrl"];
                        ftpresumepath = ConfigurationManager.AppSettings["FTPJobApplyResumeUrl"];
                        ftpusername = ConfigurationManager.AppSettings["FTPJobApplyUsername"];
                        ftppassword = ConfigurationManager.AppSettings["FTPJobApplyPassword"];
                    }

                    if (!string.IsNullOrEmpty(thisApplication.MemberResumeFile))
                    {
                        if (useFTP)
                        {
                            Stream downloadedfile = null;


                            ftpclient.Host = ftpresumepath;
                            ftpclient.Username = ftpusername;
                            ftpclient.Password = ftppassword;

                            ftpclient.DownloadFileToClient(ftpresumepath + thisApplication.MemberResumeFile, ref downloadedfile, out errormessage);

                            //if (string.IsNullOrEmpty(errormessage) && downloadedfile.Length > 0)
                            //{
                            //    fileByte = downloadedfile.
                            //}
                        }
                        else
                        {
                            string strFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ApplicationUploadResumePaths"]);
                            string strFileFullPath = strFilePath + thisApplication.MemberResumeFile;
                            string strMimeType = Entities.PortalEnums.MimeTypes.GetMimeType(System.IO.Path.GetExtension(strFileFullPath));

                            if (System.IO.File.Exists(strFileFullPath))
                            {
                                // Get the Mime Type of the File
                                if (strMimeType.Length > 0)
                                    this.Response.ContentType = strMimeType;
                                else
                                    this.Response.ContentType = "application/octet-stream";

                                this.Response.AppendHeader("Content-Disposition", "attachment;filename=" + thisApplication.MemberResumeFile);
                                this.Response.BinaryWrite(Utils.DecryptFile(strFileFullPath));
                                this.Response.End();
                                //this.Response.Flush();

                            }
                        }
                    }

                    //if (fileByte != null)
                    //{
                    //    String file64String = Convert.ToBase64String(fileByte);
                    //    string jsonString = @"{ ""ParentId"" : """ + thisMember.ExternalMemberId + @""", ""Name"" : """ + resumeFile.MemberFileName + @""", ""ContentType"":""application/octet-stream"", ""Body"": """ + file64String + @""" }";
                    //    string entityID, error;
                    //    sfInt.EntityPost("Attachment", jsonString, out entityID, out error);
                    //}
                }

                {
                    //create application
                    string jsonString = @"{ ""ts2__Candidate_Contact__c"" : """ + thisMember.ExternalMemberId + @""", ""ts2__Job__c"" : """ + thisJob.RefNo + @"""}";
                    string SFApplicationID, errorMsg;
                    bool postSuccess = sfInt.EntityPost("ts2__Job__c", jsonString, out SFApplicationID, out errorMsg);
                }
            }



        }*/

        private bool LoginCheck()
        {
            bool valid = false;

            JXTPortal.Entities.Members member = MembersService.GetBySiteIdUsername(SessionData.Site.MasterSiteId, tbUserName.Text);
            if (member != null && member.Valid && member.Validated && member.Password == CommonService.EncryptMD5(tbPassword.Text))
            {
                // SALESFORCE - Update the details from Sales force
                if (GetFromSalesforceAndSave(member.EmailAddress, member.ExternalMemberId))
                {
                    member = MembersService.GetByMemberId(member.MemberId);
                }

                SessionService.RemoveAdvertiserUser();
                SessionService.SetMember(member);
                valid = true;

                // Update Last Login Date of the Member
                member.LastLogon = DateTime.Now;
                MembersService.Update(member);

                CheckMemberApplied(JobID);
                member.Dispose();
            }


            return valid;
        }

        private void SetSocialMediaLoginButtons()
        {
            //Get Integration Details
            AdminIntegrations.Integrations integrations = IntegrationsService.AdminIntegrationsForSiteGet(SessionData.Site.SiteId);

            //Facebook login button
            if (integrations.Facebook != null && !string.IsNullOrWhiteSpace(integrations.Facebook.ApplicationID) && !string.IsNullOrWhiteSpace(integrations.Facebook.ApplicationSecret) && integrations.Facebook.Valid)
            {
                phApplyWithFacebook.Visible = true;
            }

            if (integrations.Indeed != null && !string.IsNullOrWhiteSpace(integrations.Indeed.APIToken) && !string.IsNullOrWhiteSpace(integrations.Indeed.APISecret) && integrations.Indeed.Valid)
            {
                phApplyWithIndeed.Visible = true;
            }

            //linkedin login button
            //if (!string.IsNullOrWhiteSpace(integrations..ApplicationID) && !string.IsNullOrWhiteSpace(integrations.Google.ApplicationSecret) && integrations.Google.Valid)
            //    ggLoginBtn.Visible = true;
            string linkedinapi = string.Empty;
            using (TList<JXTPortal.Entities.GlobalSettings> tgs = GlobalSettingsService.GetBySiteId(SessionData.Site.SiteId))
            {
                if (tgs.Count > 0)
                {
                    JXTPortal.Entities.GlobalSettings gs = tgs[0];
                    if (!string.IsNullOrEmpty(gs.LinkedInApi))
                    {
                        linkedinapi = gs.LinkedInApi;
                        phApplyWithLinkedIn.Visible = true;
                    }
                }
            }

            // If the site is a JOBBOARD then Linkedin is HIDDEN
            if (SessionData.Site.IsJobBoard)
            {
                phApplyWithLinkedIn.Visible = false;
            }
        }


        #region SALESFORCE

        /// <summary>
        /// SALESFORCE - If Member doesn't exist in Platform check if exist in SALESFORCE and grab the details from Salesforce AND send reset password email to member.
        /// </summary>
        /// <param name="strEmail"></param>
        private bool GetFromSalesforceAndSave(string strEmail, string SalesForceContactID)
        {
            SalesforceMemberSync memberSync = new SalesforceMemberSync();
            int memberid = 0; string errormsg = string.Empty;

            // Get Candidate from Salesforce by email
            // And If candidate exists in Sales force, save in Boardy platform.
            if (memberSync.GetContactFromSalesForceAndSave(strEmail, SalesForceContactID, SessionData.Site.MasterSiteId, ref memberid, ref errormsg) && memberid > 0)
            {
                return true;
            }

            return false;
        }

        #endregion

        protected void ibApplyWithFacebook_Click(object sender, ImageClickEventArgs e)
        {
            //Get Integration Details
            AdminIntegrations.Integrations integrations = IntegrationsService.AdminIntegrationsForSiteGet(SessionData.Site.SiteId);

            if (integrations.Facebook != null && !string.IsNullOrEmpty(integrations.Facebook.ApplicationID))
            {
                string http = "http://";
                if (Request.IsSecureConnection)
                    http = "https://";

                string urlsuffix = http + HttpContext.Current.Request.Url.Host;

                oAuthFacebook _oauth = new oAuthFacebook();
                _oauth.ClientID = integrations.Facebook.ApplicationID;
                _oauth.RedirectURI = urlsuffix + "/oauthcallback.aspx?cbtype=facebook&cbaction=apply&id=" + JobID.ToString() + "&profession=" + Profession + "&jobname=" + JobName;
                string token = _oauth.Authorize();
                Response.Write(token);
                Response.Redirect(token);

            }

        }



        private string RenderControl(System.Web.UI.Control control)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            HtmlTextWriter writer = new HtmlTextWriter(sw);

            control.RenderControl(writer);
            return sb.ToString();
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            using (JXTPortal.Entities.Jobs job = JobsService.GetByJobId(JobID))
            {
                Response.Redirect("~/" + Utils.GetJobUrl(JobID, job.JobFriendlyName, Profession));
            }
        }
    }
}