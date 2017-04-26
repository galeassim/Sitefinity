﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JXTPortal;
using JXTPortal.Entities;
using JXTPortal.Client.Salesforce;
using JXTPortal.Client.Bullhorn;
using JXTPortal.Common;
using System.Configuration;
using System.IO;

namespace JXTPortal.Website.members
{
    public partial class validate : System.Web.UI.Page
    {
        #region Declaration

        private MembersService _membersService;
        private MemberFilesService _memberFilesService;
        private DynamicPagesService _dynamicPagesService = null;
        private string bucketName = ConfigurationManager.AppSettings["AWSS3BucketName"];
        private string privateBucketName = ConfigurationManager.AppSettings["AWSS3PrivateBucketName"];
        private string memberFileFolder, memberFileFolderFormat;
        #endregion

        #region Properties
        public IFileManager FileManagerService { get; set; }

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

        private DynamicPagesService DynamicPagesService
        {
            get
            {
                if (_dynamicPagesService == null)
                {
                    _dynamicPagesService = new DynamicPagesService();
                }
                return _dynamicPagesService;
            }
        }

        private MemberFilesService MemberFilesService
        {
            get
            {
                if (_memberFilesService == null)
                {
                    _memberFilesService = new MemberFilesService();
                }
                return _memberFilesService;
            }
        }
        #endregion

        #region Page Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            CommonPage.SetBrowserPageTitle(Page, "Member Validation");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionData.Site.IsUsingS3)
            {
                memberFileFolder = ConfigurationManager.AppSettings["FTPHost"] + ConfigurationManager.AppSettings["MemberRootFolder"] + "/" + ConfigurationManager.AppSettings["MemberFilesFolder"];
                memberFileFolderFormat = "{0}/{1}/";
                string ftphosturl = ConfigurationManager.AppSettings["FTPHost"];
                string ftpusername = ConfigurationManager.AppSettings["FTPJobApplyUsername"];
                string ftppassword = ConfigurationManager.AppSettings["FTPJobApplyPassword"];
                FileManagerService = new FTPClientFileManager(ftphosturl, ftpusername, ftppassword);
            }
            else
            {
                memberFileFolder = ConfigurationManager.AppSettings["AWSS3MemberRootFolder"] + ConfigurationManager.AppSettings["AWSS3MemberFilesFolder"];
                memberFileFolderFormat = "{0}/{1}";
            }

            ltMemberValidation.Text = CommonFunction.GetResourceValue("LabelMemberValidation");

            if (!Page.IsPostBack)
            {
                string result = ValidateEmail();
                if (string.IsNullOrEmpty(result))
                {
                    //ltMessage.Text = "You have been validated successfully.";
                    Response.Redirect(DynamicPagesService.GetDynamicPageUrl(JXTPortal.SystemPages.MEMBER_VALIDATION_SUCCESS, "", "", ""));
                }
                else
                {
                    ltMessage.Text = result;
                }
            }
        }

        #endregion

        #region Method

        private string ValidateEmail()
        {
            bool isValid = false;

            if (Request.QueryString["User"] != null && Request.QueryString["ValidationID"] != null)
            {
                using (JXTPortal.Entities.Members member = MembersService.GetBySiteIdUsername(SessionData.Site.MasterSiteId, System.Web.HttpUtility.UrlDecode(Request.QueryString["User"])))
                {
                    if (member != null && member.Validated)
                    {
                        return CommonFunction.GetResourceValue("LabelMemberValidated");
                    }

                    if (member != null && !member.Validated && member.ValidateGuid.Value.ToString() == Request.QueryString["ValidationID"].ToLower())
                    {
                        member.Validated = true;
                        member.Valid = true;
                        member.LastModifiedDate = DateTime.Now; // This is important for sending data for the application

                        //We will try the Bullhorn executions first before we actually call the update to the local db
                        //this is because if bullhorn login failed, this gets redirected to BH to re-authorize.
                        //and if we updated the advertiser account status first, when we comes back from BH re-authorize, it will
                        //never hit the BH sync function
                        //Bullhorn Integration
                        #region BH member sync
                        BullhornRESTAPI BullhornRESTAPI = new BullhornRESTAPI(JXTPortal.Entities.SessionData.Site.SiteId);
                        if (BullhornRESTAPI.integrations != null && BullhornRESTAPI.integrations.Bullhorn != null && BullhornRESTAPI.integrations.Bullhorn.EnableCandidateSync)
                        {
                            string errorMsg = string.Empty;

                            //get the files if any for this candidate (Resume and Cover Letter)
                            List<MemberFiles> memberFiles = MemberFilesService.GetByMemberId(member.MemberId).ToList();

                            MemberFiles thisResume = memberFiles.Where(c => c.DocumentTypeId.HasValue && c.DocumentTypeId == 2).SingleOrDefault();
                            MemberFiles thisCoverLetter = memberFiles.Where(c => c.DocumentTypeId.HasValue && c.DocumentTypeId == 1).SingleOrDefault();

                            string errormessage = string.Empty;
                            
                            byte[] resumecontent = null;
                            byte[] coverlettercontent = null; 
                            string filepath = string.Empty;
                            Stream ms = null;

                            if (thisResume != null)
                            {
                                if (!string.IsNullOrEmpty(thisResume.MemberFileUrl))
                                {
                                    ms = FileManagerService.DownloadFile(privateBucketName, string.Format(memberFileFolderFormat, memberFileFolder, thisResume.MemberId), thisResume.MemberFileUrl, out errormessage);

                                    resumecontent = ((MemoryStream)ms).ToArray();
                                }
                                else
                                {
                                    resumecontent = thisResume.MemberFileContent;
                                }
                            }

                            if (thisCoverLetter != null)
                            {
                                if (!string.IsNullOrWhiteSpace(thisCoverLetter.MemberFileUrl))
                                {
                                    ms = null;
                                    ms = FileManagerService.DownloadFile(privateBucketName, string.Format(memberFileFolderFormat, memberFileFolder, thisResume.MemberId), thisCoverLetter.MemberFileUrl, out errormessage);
                                    coverlettercontent = ((MemoryStream)ms).ToArray();
                                }
                                else
                                {
                                    coverlettercontent = thisCoverLetter.MemberFileContent;
                                }
                            }

                            BullhornRESTAPI.SyncCandidate(member,
                                    thisResume != null ? resumecontent : null,
                                    thisResume != null ? thisResume.MemberFileName : null,
                                    thisCoverLetter != null ? coverlettercontent : null,
                                    thisCoverLetter != null ? thisCoverLetter.MemberFileName : null,
                                    out errorMsg);
                        }
                        #endregion

                        if (MembersService.Update(member))
                        {
                            isValid = true;
                            SessionService.RemoveAdvertiserUser();
                            SessionService.SetMember(member);

                            // SALESFORCE - Check if contact new/exists in Salesforce and insert/update in Salesforce.
                            SalesforceMemberSync memberSync = new SalesforceMemberSync(SessionData.Site.SiteId);
                            memberSync.CheckContactAndSaveInSalesForce(member, member.SiteId);                            
                            return string.Empty;
                        }
                    }
                }
            }

            return CommonFunction.GetResourceValue("LabelMemberInvalid");
        }

        #endregion
    }
}
