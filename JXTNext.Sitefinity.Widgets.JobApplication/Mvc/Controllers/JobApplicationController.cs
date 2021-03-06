﻿using JXTNext.Sitefinity.Common.Helpers;
using JXTNext.Sitefinity.Connector.BusinessLogics;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Common;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Member;
using JXTNext.Sitefinity.Widgets.Job.Mvc.StringResources;
using JXTNext.Sitefinity.Widgets.JobApplication.Mvc.Models.JobApplication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Workflow;
using JXTNext.FileHandler.FileHandlerServices.Google_Drive;
using JXTNext.FileHandler.FileHandlerServices.Dropbox;
using JXTNext.FileHandler.Models.Base;
using JXTNext.FileHandler.Models.Google_Drive;
using JXTNext.FileHandler.Models.Dropbox;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Claims;
using System.Web.Security;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Services;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Advertisers;
using JXTNext.Sitefinity.Services.Intefaces;
using Telerik.Sitefinity.Abstractions;
using JXTNext.Sitefinity.Widgets.Authentication.Mvc.Models.JXTNextResume;
using System.Dynamic;
using JXTNext.Sitefinity.Widgets.JobApplication.Mvc.Models;
using JXTNext.Sitefinity.Widgets.Social.Mvc.Helpers;
using JXTNext.Sitefinity.Common.Models.JobApplication;
using JXTNext.Sitefinity.Common.Extensions;
using JXTNext.Common.FileManager;
using JXTNext.Common.FileManager.Models.S3;
using JXTNext.Sitefinity.Common.Constants;

namespace JXTNext.Sitefinity.Widgets.Job.Mvc.Controllers
{
    [EnhanceViewEngines]
    [Localization(typeof(JobApplicationResources))]
    [ControllerToolboxItem(Name = "JobApplication_MVC", Title = "Job Application", SectionName = "JXTNext.JobApplication", CssClass = JobApplicationController.WidgetIconCssClass)]
    public class JobApplicationController : Controller
    {
        IBusinessLogicsConnector _blConnector;
        IJobApplicationService _jobApplicationService;
        /// <summary>
        /// Gets or sets the name of the template that widget will be displayed.
        /// </summary>
        /// <value></value>
        public string TemplateName
        {
            get
            {
                return this.templateName;
            }

            set
            {
                this.templateName = value;
            }
        }

        private readonly SiteSettingsHelper _siteSettingsHelper;
        public JobApplicationController(IBusinessLogicsConnector blConnector, IJobApplicationService jobApplicationService)
        {
            _siteSettingsHelper = new SiteSettingsHelper();
            _jobApplicationService = jobApplicationService;
            _blConnector = blConnector;
            if (string.IsNullOrWhiteSpace(this.SerializedApplyWithSocialMedia))
                this.SerializedApplyWithSocialMedia = ApplyWithSocialMedia.SerializedSocialMediaInit();
        }


        [RelativeRoute("{*tags}")]
        public ActionResult RouteHandler(string[] tags, int? jobid)
        {
            if (Request.IsAjaxRequest()) // Ajax calls
            {
                if (tags != null && tags.Length > 0)
                {
                    var routePath = tags.FirstOrDefault();
                    if (routePath.ToUpper().Contains("CHECKUSER"))
                    {
                        if (Request.Form["email"] != null)
                        {
                            return CheckUser(Request.Form["email"]);
                        }
                    }
                    else if (routePath.ToUpper().Contains("ISJOBAPPLIED"))
                    {
                        if (Request.Form["jobId"] != null)
                        {
                            int jobId;
                            if (Int32.TryParse(Request.Form["jobId"], out jobId))
                            {
                                return IsJobApplied(jobId);
                            }
                        }
                    }
                    else if (routePath.ToUpper().Contains("VALIDATEUSER"))
                    {
                        if (Request.Form["email"] != null)
                        {
                            return ValidateUser(Request.Form["email"], Request.Form["password"], Convert.ToBoolean(Request.Form["staySignedIn"]), Convert.ToBoolean(Request.Form["isUserLoggedIn"]));
                        }
                    }
                }
            }
            else // Non-Ajax calls
            {
                if (tags != null && tags.Length > 0)
                {
                    string urlRoute = tags.FirstOrDefault();
                    if (urlRoute.ToUpper().Contains("APPLYJOB"))
                    {
                        ApplyJobModel applyJobModel = new ApplyJobModel();
                        applyJobModel.JobId = Convert.ToInt32(Request.Form["JobId"]);
                        applyJobModel.FirstName = Request.Form["FirstName"];
                        applyJobModel.LastName = Request.Form["LastName"];
                        applyJobModel.PhoneNumber = Request.Form["PhoneNumber"];
                        applyJobModel.Email = Request.Form["Email"];
                        applyJobModel.Password = Request.Form["Password"];
                        applyJobModel.UploadFilesResume = Request.Form["UploadFilesResume"];
                        applyJobModel.UploadFilesCoverLetter = Request.Form["UploadFilesCoverLetter"];
                        applyJobModel.UploadFilesDocuments = Request.Form["UploadFilesDocuments"];
                        applyJobModel.ResumeSelectedType = Request.Form["ResumeSelectedType"];
                        applyJobModel.CoverLetterSelectedType = Request.Form["CoverLetterSelectedType"];
                        applyJobModel.DocumentsSelectedType = Request.Form["DocumentsSelectedType"];
                        applyJobModel.ContactDetails = Request.Form["ContactDetails"];
                        applyJobModel.ApplicationEmail = Request.Form["ApplicationEmail"];
                        applyJobModel.CompanyName = Request.Form["CompanyName"];
                        applyJobModel.UrlReferral = Request.Form["UrlReferral"];

                        return ApplyJob(applyJobModel);
                    }

                    string jobIdStr = urlRoute.Substring(0, urlRoute.LastIndexOf('/')).Split('/').Last();
                    if (Int32.TryParse(jobIdStr, out int jobId))
                    {
                        return Index(jobId);
                    }
                }
            }

            // Default redirect to Index action
            return Index(null);

        }

        // GET: JobApplication
        [HttpGet]
        public ActionResult Index(int? jobid)
        {
            JobApplicationViewModel jobApplicationViewModel = GetJobApplicationConfigurations(JobApplicationStatus.Available, "Upload your files to Apply");
            ViewBag.CssClass = this.CssClass;
            ViewBag.SeekResumeError = false;
            ViewBag.JobRecordExists = false;
            var uploadFileInfo = this.SerializedCloudSettingsParams == null ? null : JsonConvert.DeserializeObject<JobApplicationUploadFilesModel>(this.SerializedCloudSettingsParams);

            jobApplicationViewModel.UploadFiles = uploadFileInfo;
            jobApplicationViewModel.JobId = jobid.HasValue ? jobid.Value : 0;
            jobApplicationViewModel.ApplyWithSocialMediaInfo = GetSelectedSocialMediaItems();
            bool isUserLoggedIn = false;
            string userEmail = String.Empty;
            string userFirstName = String.Empty;
            var currentIdentity = ClaimsManager.GetCurrentIdentity();
            
            Log.Write($"currentIdentity.IsAuthenticated = {currentIdentity.IsAuthenticated}", ConfigurationPolicy.ErrorLog);
            if (Request.QueryString["error"] == "resume")
            {
                ViewBag.SeekResumeError = true;
                ViewBag.SeekResumeErrorMessage = "Resume is not available in your seek profile. Please upload and reapply for the job.";
            }

            if (Request.QueryString["error"] == "exists")
            {
                ViewBag.JobRecordExists = true;
                ViewBag.JobRecordExistsErrorMessage = "Job already applied.";
            }
            if (Request.QueryString["error"] == "denied")
            {
                ViewBag.SeekAccessError = true;
                ViewBag.SeekAccessErrorMessage = "Seek access denied. Please allow access to apply job.";
            }

            var curUserId = currentIdentity.UserId;
            Log.Write($"currentIdentity.UserId = {currentIdentity.UserId}", ConfigurationPolicy.ErrorLog);
            if (curUserId != Guid.Empty)
            {
                Log.Write($"currentIdentity is not empty", ConfigurationPolicy.ErrorLog);
                UserProfileManager userMgr = UserProfileManager.GetManager();
                UserManager mgr = UserManager.GetManager("Default");
                User user = mgr.GetUser(curUserId);

                SitefinityProfile profile = null;
                if (user != null)
                {
                    Log.Write($"user is not null", ConfigurationPolicy.ErrorLog);
                    profile = userMgr.GetUserProfile<SitefinityProfile>(user);
                }
                    


                var currUser = SitefinityHelper.GetUserById(currentIdentity.UserId);
                if (currUser != null)
                {
                    Log.Write($"curent user is not null", ConfigurationPolicy.ErrorLog);
                    userEmail = currUser.Email;
                    userFirstName = SitefinityHelper.GetUserFirstNameById(currUser.Id);
                    Log.Write($"userFirstName = {userFirstName}", ConfigurationPolicy.ErrorLog);
                }

                if (jobid.HasValue)
                {
                    ViewBag.IsJobApplied = _isMemberAppliedJob(jobid.Value);
                    Log.Write($"job id has value", ConfigurationPolicy.ErrorLog);
                    Log.Write($"IsJobApplied = {ViewBag.IsJobApplied}", ConfigurationPolicy.ErrorLog);
                }
                    

                isUserLoggedIn = true;
                ViewBag.isLoggedIn = true;
                ViewBag.loginEmail = userEmail;
            }

            //if (currentIdentity.IsAuthenticated)
            //{
            //    var currUser = SitefinityHelper.GetUserById(currentIdentity.UserId);
            //    if (currUser != null)
            //    {
            //        userEmail = currUser.Email;
            //        userFirstName = SitefinityHelper.GetUserFirstNameById(currUser.Id);
            //    }

            //    if(jobid.HasValue)
            //        ViewBag.IsJobApplied = _isMemberAppliedJob(jobid.Value);

            //    isUserLoggedIn = true;
            //    ViewBag.isLoggedIn = true;
            //    ViewBag.loginEmail = userEmail;
            //}

            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            ViewBag.UserEmail = userEmail;
            ViewBag.UserFirstName = userFirstName;
            ViewBag.RegisterPageUrl = SitefinityHelper.GetPageUrl(this.RegisterPageId);
            ViewBag.PostBackMessage = TempData["PostBackMessage"];
            ViewBag.EmailTemplateId = this.EmailTemplateId;
            ViewBag.AdvertiserEmailTemplateId = this.AdvertiserEmailTemplateId;
            Log.Write($"AdvertiserEmailTemplateId  {ViewBag.AdvertiserEmailTemplateId}", ConfigurationPolicy.ErrorLog);
            if (!this.JobApplicationSuccessPageId.IsNullOrEmpty())
                ViewBag.SuccessPageUrl = SitefinityHelper.GetPageUrl(this.JobApplicationSuccessPageId);

            // These values are required for the Indeed apply
            if(jobid.HasValue)
            {
                IGetJobListingRequest jobListingRequest = new JXTNext_GetJobListingRequest { JobID = jobid.Value };
                IGetJobListingResponse jobListingResponse = _blConnector.GuestGetJob(jobListingRequest);
                ViewBag.JobTitle = jobListingResponse.Job.Title;
                jobApplicationViewModel.ApplicationEmail = jobListingResponse.Job.CustomData["ApplicationMethod.ApplicationEmail"];
                jobApplicationViewModel.ContactDetails = jobListingResponse.Job.CustomData["ContactDetails"];
                ViewBag.CompanyName = jobListingResponse.Job.CustomData["CompanyName"];
                ViewBag.JobLocation = jobListingResponse.Job.CustomData["CountryLocationArea[0].Filters[0].Value"];
                jobApplicationViewModel.UrlReferral = Request.QueryString["source"];
                TempData["source"] = jobApplicationViewModel.UrlReferral;
                Log.Write($"jobApplicationViewModel.UrlReferral  {jobApplicationViewModel.UrlReferral}", ConfigurationPolicy.ErrorLog);
            }

            if (isUserLoggedIn && !string.IsNullOrEmpty(userEmail))
            {
                Log.Write($"isUserLoggedIn  {isUserLoggedIn}", ConfigurationPolicy.ErrorLog);
                Log.Write($"userEmail  {userEmail}", ConfigurationPolicy.ErrorLog);
                var response = _blConnector.GetMemberByEmail(userEmail);
                List<SelectListItem> myResumes = new List<SelectListItem>();
                myResumes.Add(new SelectListItem { Text = "SELECT YOUR RESUME", Value = "0" });
                if (response.Member?.ResumeFiles != null)
                {
                    var resumeList = JsonConvert.DeserializeObject<List<ProfileResume>>(response.Member.ResumeFiles);
                    
                    foreach (var item in resumeList)
                    {
                        var datestr = item.UploadDate.ToShortDateString();
                        myResumes.Add(new SelectListItem { Text = datestr + " - " + item.FileFullName, Value = item.Id.ToString() });
                    }
                }
                ViewBag.ResumeList = myResumes;
                Log.Write($"Resume process is completed ", ConfigurationPolicy.ErrorLog);
            }            

            // linked-in data
            ViewBag.CustomerClientId = LinkedInHelper.CustomerClientId;
            ViewBag.CustomerIntegrationContext = LinkedInHelper.CustomerIntegrationContext;
            ViewBag.LinkedInSignInUrl = LinkedInHelper.CreateSignInUrl(HttpContext.Request.Url.AbsoluteUri, jobApplicationViewModel.JobId.ToString());
            ViewBag.LinkedInApplyUrl = LinkedInHelper.CreateApplyUrl();

            jobApplicationViewModel.HideDropBox = (this.HideDropBox == true);
            jobApplicationViewModel.HideGoogleDrive = (this.HideGoogleDrive == true);

            Log.Write($"Index method end ", ConfigurationPolicy.ErrorLog);
            var fullTemplateName = this.templateNamePrefix + this.TemplateName;
            return View(fullTemplateName, jobApplicationViewModel);
        }

        [HttpPost]
        public ActionResult ApplyJob(ApplyJobModel applyJobModel)
        {
            int applicationResultID = applyJobModel.JobId;
            int memberID = 2;
            JobApplicationViewModel jobApplicationViewModel;
            var fullTemplateName = this.templateNamePrefix + this.TemplateName;
            var ovverideEmail = applyJobModel.Email;
            // Create user if the user does not exists
            MembershipCreateStatus membershipCreateStatus;
            ViewBag.PostBackMessage = null;
            ViewBag.IsUserLoggedIn = false;
            EmailNotificationSettings registrationEmailNotificationSettings = null;

            if (applyJobModel != null && !string.IsNullOrEmpty(applyJobModel.Email))
            {
                applyJobModel.Email = applyJobModel.Email.Trim(',');
            }
            
            var currentIdentity = ClaimsManager.GetCurrentIdentity();
            
            if (SitefinityHelper.IsUserLoggedIn() && currentIdentity.IsAuthenticated) // User already logged in
            {
                Log.Write($"ApplyJob user login, SitefinityHelper.IsUserLoggedIn() = {SitefinityHelper.IsUserLoggedIn()} and currentIdentity.IsAuthenticated = {currentIdentity.IsAuthenticated}", ConfigurationPolicy.ErrorLog);
                var currUser = SitefinityHelper.GetUserById(currentIdentity.UserId);
                if (currUser != null)
                    ovverideEmail = currUser.Email;
            }
            else //user not logged in
            {
                if (!string.IsNullOrEmpty(applyJobModel.Email))
                {
                    Telerik.Sitefinity.Security.Model.User existingUser = SitefinityHelper.GetUserByEmail(applyJobModel.Email);

                    if (existingUser != null)
                    {
                        #region Entered Email exists in Sitefinity User list
                        //instantiate the Sitefinity user manager
                        //if you have multiple providers you have to pass the provider name as parameter in GetManager("ProviderName") in your case it will be the asp.net membership provider user
                        UserManager userManager = UserManager.GetManager();
                        if (userManager.ValidateUser(applyJobModel.Email, applyJobModel.Password))
                        {
                            Log.Write($"ValidateUser is successfully.", ConfigurationPolicy.ErrorLog);
                            //if you need to get the user instance use the out parameter
                            Telerik.Sitefinity.Security.Model.User userToAuthenticate = null;
                            SecurityManager.AuthenticateUser(userManager.Provider.Name, applyJobModel.Email, applyJobModel.Password, false, out userToAuthenticate);
                            if (userToAuthenticate == null)
                            {
                                jobApplicationViewModel = GetJobApplicationConfigurations(JobApplicationStatus.NotAbleToLoginCreatedUser, "Unable to process your job application. Please try logging in and re-apply for the job.");
                                return View("JobApplication.Simple", jobApplicationViewModel);
                            }
                            else
                            {
                                ovverideEmail = userToAuthenticate.Email;
                            }
                        }
                        else
                        {
                            jobApplicationViewModel = GetJobApplicationConfigurations(JobApplicationStatus.NotAbleToLoginCreatedUser, "Unable to process your job application. Please try logging in and re-apply for the job.");
                            return View("JobApplication.Simple", jobApplicationViewModel);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Entered email does not exists in sitefinity User list
                        membershipCreateStatus = SitefinityHelper.CreateUser(applyJobModel.Email, applyJobModel.Password, applyJobModel.FirstName, applyJobModel.LastName, applyJobModel.Email, applyJobModel.PhoneNumber,
                        null, null, true, true);

                        if (membershipCreateStatus != MembershipCreateStatus.Success)
                        {
                            jobApplicationViewModel = GetJobApplicationConfigurations(JobApplicationStatus.NotAbleToCreateUser, "Unable to create user. Please register from");
                            return View("JobApplication.Simple", jobApplicationViewModel);
                        }
                        else
                        {
                            /// Instantiate Registration email template
                            if(this.RegistrationEmailTemplateId != null)
                            {
                                registrationEmailNotificationSettings = _createRegistrationEmailTemplate(new JobApplicationEmailTemplateModel()
                                {
                                    FromFirstName = this.EmailTemplateSenderName,
                                    FromLastName = null,
                                    FromEmail = this.EmailTemplateSenderEmailAddress,
                                    ToFirstName = applyJobModel.FirstName,
                                    ToLastName = applyJobModel.LastName,
                                    ToEmail = applyJobModel.Email,
                                    Subject = SitefinityHelper.GetCurrentSiteEmailTemplateTitle(this.RegistrationEmailTemplateId),
                                    HtmlContent = SitefinityHelper.GetCurrentSiteEmailTemplateHtmlContent(this.RegistrationEmailTemplateId),
                                    Attachments = null
                                });
                            }
                           

                            //instantiate the Sitefinity user manager
                            //if you have multiple providers you have to pass the provider name as parameter in GetManager("ProviderName") in your case it will be the asp.net membership provider user
                            UserManager userManager = UserManager.GetManager();
                            if (userManager.ValidateUser(applyJobModel.Email, applyJobModel.Password))
                            {
                                //if you need to get the user instance use the out parameter
                                Telerik.Sitefinity.Security.Model.User userToAuthenticate = null;
                                SecurityManager.AuthenticateUser(userManager.Provider.Name, applyJobModel.Email, applyJobModel.Password, false, out userToAuthenticate);
                                if (userToAuthenticate == null)
                                {
                                    jobApplicationViewModel = GetJobApplicationConfigurations(JobApplicationStatus.NotAbleToLoginCreatedUser, "Unable to process your job application. Please try logging in and re-apply for the job.");
                                    return View("JobApplication.Simple", jobApplicationViewModel);
                                }
                                else
                                {
                                    ovverideEmail = userToAuthenticate.Email;
                                }
                            }
                        }
                        #endregion
                    }
                }
            }


            ViewBag.IsUserLoggedIn = true;
            JobApplicationAttachmentSource sourceResume = GetAttachmentSourceType(applyJobModel.ResumeSelectedType);
            JobApplicationAttachmentSource sourceCoverLetter = GetAttachmentSourceType(applyJobModel.CoverLetterSelectedType);
            JobApplicationAttachmentSource sourceDocuments = GetAttachmentSourceType(applyJobModel.DocumentsSelectedType);


            List<JobApplicationAttachmentUploadItem> attachments = GatherAttachments(sourceResume, sourceCoverLetter, sourceDocuments, applyJobModel.UploadFilesResume, applyJobModel.UploadFilesCoverLetter, applyJobModel.UploadFilesDocuments, ovverideEmail);
            
            string resumeAttachmentPath = GetAttachmentPath(attachments, JobApplicationAttachmentType.Resume).FirstOrDefault();

            if (String.IsNullOrEmpty(resumeAttachmentPath))
            {
                Log.Write($"Something went wrong while uploading resume, please review the uploaded resume.", ConfigurationPolicy.ErrorLog);
                //prompt error message for contact
                //jobApplicationViewModel = GetJobApplicationConfigurations(JobApplicationStatus.Technical_Issue, "Unable to attach files to application");
                TempData["PostBackMessage"] = "Unable to attach resume. Please review it";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            string coverletterAttachmentPath = GetAttachmentPath(attachments, JobApplicationAttachmentType.Coverletter).FirstOrDefault();
            List<string> documentsAttachmentPath = GetAttachmentPath(attachments, JobApplicationAttachmentType.Documents);

            #region Email Notification
            // Email Notification Settings
            // In the desinger form those are going to be provided by separator as semicolon(;)
            

            List<dynamic> emailAttachments = new List<dynamic>();

            foreach (var item in attachments)
            {
                dynamic emailAttachment = new ExpandoObject();
                emailAttachment.FileStream = item.FileStream;
                emailAttachment.FileName = item.FileName;

                emailAttachments.Add(emailAttachment);

            }


            
            

            Log.Write($"currentIdentity.IsAuthenticated value is {currentIdentity.IsAuthenticated}", ConfigurationPolicy.ErrorLog);
            string loginUserName = null;
            if (currentIdentity.IsAuthenticated)
            {
                loginUserName = SitefinityHelper.GetUserFullNameById(currentIdentity.UserId);
            }
            else
            {
                var user = SitefinityHelper.GetUserByEmail(ovverideEmail);
                if(user != null)
                {
                    loginUserName = SitefinityHelper.GetUserFullNameById(ClaimsManager.GetCurrentIdentity().UserId);
                }
                else
                {
                    loginUserName = string.Empty;
                }

            }


            EmailNotificationSettings emailNotificationSettings = this.EmailTemplateId != null ? 
                _createApplicantEmailTemplate(new JobApplicationEmailTemplateModel()
                {
                    FromFirstName = this.EmailTemplateSenderName,
                    FromLastName = null,
                    FromEmail = this.EmailTemplateSenderEmailAddress,
                    ToFirstName = loginUserName,
                    ToLastName = null,
                    ToEmail = ovverideEmail,
                    Subject = SitefinityHelper.GetCurrentSiteEmailTemplateTitle(this.EmailTemplateId),
                    HtmlContent = SitefinityHelper.GetCurrentSiteEmailTemplateHtmlContent(this.EmailTemplateId),
                    Attachments = null
                }) : null;

            EmailNotificationSettings advertiserEmailNotificationSettings = this.AdvertiserEmailTemplateId != null ? 
                _createAdvertiserEmailTemplate(new JobApplicationEmailTemplateModel()
                {
                    FromFirstName = loginUserName,
                    FromLastName = null,
                    FromEmail = ovverideEmail,
                    ToEmail = applyJobModel.ApplicationEmail,
                    ToFirstName = applyJobModel.ContactDetails.GetFirstName(),
                    ToLastName = applyJobModel.ContactDetails.GetLastName(),
                    Subject = SitefinityHelper.GetCurrentSiteEmailTemplateTitle(this.AdvertiserEmailTemplateId),
                    HtmlContent = SitefinityHelper.GetCurrentSiteEmailTemplateHtmlContent(this.AdvertiserEmailTemplateId),
                    Attachments = emailAttachments
                }) : null;


            #endregion

            #region 

            //FileUploads
            Log.Write($"Before upload", ConfigurationPolicy.ErrorLog);
            var resumeAttachment = attachments.Where(x => x.AttachmentType == JobApplicationAttachmentType.Resume).FirstOrDefault();
            JobApplicationAttachmentUploadItem resumeToProfile = new JobApplicationAttachmentUploadItem();
            string docExtension = resumeAttachment.FileName.Split('.').Last();
            
            resumeToProfile.Id = Guid.NewGuid().ToString();
            string documentTitle = resumeToProfile.Id.ToString() + "_" + resumeAttachment.FileName;
            resumeToProfile.AttachmentType = resumeAttachment.AttachmentType;
            resumeToProfile.FileName = resumeAttachment.FileName;
            resumeToProfile.FileStream = new MemoryStream();
            resumeAttachment.FileStream.CopyTo(resumeToProfile.FileStream);
            resumeToProfile.PathToAttachment = resumeToProfile.Id+"_"+resumeAttachment.FileName;
            resumeToProfile.Status = "Ready";


            attachments.ForEach(c => ProcessFileUpload(ref c));
            Log.Write($"After Upload", ConfigurationPolicy.ErrorLog);
            bool hasFailedUpload = attachments.Where(c => c.Status != "Completed").Any();

            if (hasFailedUpload)
            {
                Log.Write($"Upload error", ConfigurationPolicy.ErrorLog);
                //prompt error message for contact
                //jobApplicationViewModel = GetJobApplicationConfigurations(JobApplicationStatus.Technical_Issue, "Unable to attach files to application");
                TempData["PostBackMessage"] = "Unable to attach files to application.";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }

            #endregion

            //Create Application 
            IMemberApplicationResponse response = _blConnector.MemberCreateJobApplication(
                new JXTNext_MemberApplicationRequest {
                    ApplyResourceID = applicationResultID,
                    MemberID = memberID,
                    ResumePath = resumeAttachmentPath,
                    CoverletterPath = coverletterAttachmentPath,
                    DocumentsPath = documentsAttachmentPath,
                    EmailNotification = emailNotificationSettings,
                    AdvertiserEmailNotification = advertiserEmailNotificationSettings,
                    AdvertiserName = applyJobModel.ContactDetails,
                    CompanyName = applyJobModel.CompanyName,
                    UrlReferral = applyJobModel.UrlReferral,
                    RegistrationEmailNotification = registrationEmailNotificationSettings
                },
                ovverideEmail);

            var isJobApplicationSuccess = false;

            if (response.Success && response.ApplicationID.HasValue)
            {
                isJobApplicationSuccess = true;
                jobApplicationViewModel = GetJobApplicationConfigurations(JobApplicationStatus.Applied_Successful, "Your application was successfully processed");
                if (sourceResume != JobApplicationAttachmentSource.Saved)
                {
                    bool profileUploadResult = AddUploadedResumeToProfileDashBoard(resumeToProfile, ovverideEmail);
                    if (!profileUploadResult)
                    {
                        TempData["PostBackMessage"] = "Unable to attach resume to Profile";
                        return Redirect(Request.UrlReferrer.PathAndQuery);
                    }
                }
            }
            else
            {
                TempData["PostBackMessage"] = response.Errors.First();
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }

            #region Redirect to thank you page on success
            // When the job appliction is success we need to redirect to thank you page
            if (isJobApplicationSuccess && !this.JobApplicationSuccessPageId.IsNullOrEmpty())
            {
                var successPageUrl = SitefinityHelper.GetPageUrl(this.JobApplicationSuccessPageId);
                return Redirect(successPageUrl);
            }
            #endregion


            return View("JobApplication.Simple", jobApplicationViewModel);
        }

        [HttpPost]
        public JsonResult CheckUser(string email)
        {
            var user = SitefinityHelper.GetUserByEmail(email);
            bool isUserExists = false;

            if (user != null)
                isUserExists = true;


            return new JsonResult { Data = isUserExists };
        }

        [HttpPost]
        public JsonResult ValidateUser(string email, string password, bool staySignedIn, bool isUserLoggedIn)
        {
            try
            {
                Log.Write($"ValidateUser method1" , ConfigurationPolicy.ErrorLog);
                bool isUserVerified = true;
                bool isMemberUser = false;
                bool isUserSignedIn = false;
                string firstName = null;
                List<SelectListItem> myResumes = new List<SelectListItem>();
                if (!isUserLoggedIn)
                {
                    Log.Write($"ValidateUser user is not logged In isUserLoggedIn : "+ isUserLoggedIn, ConfigurationPolicy.ErrorLog);
                    isUserVerified = SitefinityHelper.IsUserVerified(email, password);
                    
                    if (isUserVerified)
                    {
                        Log.Write($"ValidateUser isUserVerified : "+ isUserVerified, ConfigurationPolicy.ErrorLog);
                        var user = SitefinityHelper.GetUserByEmail(email);
                        if (user != null)
                        {
                            Log.Write($"ValidateUser user in not null", ConfigurationPolicy.ErrorLog);
                            isMemberUser = SitefinityHelper.IsUserInRole(user, "Member");
                            var memberResponse = _blConnector.GetMemberByEmail(user.Email);
                            if(memberResponse.Member == null)
                            {
                                UserProfileManager userProfileManager = UserProfileManager.GetManager();
                                UserProfile profile = userProfileManager.GetUserProfile(user.Id, typeof(SitefinityProfile).FullName);
                                var fName = Telerik.Sitefinity.Model.DataExtensions.GetValue(profile, "FirstName");
                                var lName = Telerik.Sitefinity.Model.DataExtensions.GetValue(profile, "LastName");
                                JXTNext_MemberRegister memberReg = new JXTNext_MemberRegister
                                {
                                    Email = user.Email,
                                    FirstName = fName.ToString(),
                                    LastName = lName.ToString(),
                                    Password = user.Password
                                };

                                if(_blConnector.MemberRegister(memberReg, out string errorMessage))
                                {
                                    memberResponse = _blConnector.GetMemberByEmail(user.Email);
                                }
                            }
                            firstName = memberResponse.Member?.FirstName;
                            if (memberResponse.Member?.ResumeFiles != null)
                            {
                                Log.Write($"ValidateUser ResumeFiles is not null", ConfigurationPolicy.ErrorLog);
                                var resumeList = JsonConvert.DeserializeObject<List<ProfileResume>>(memberResponse.Member.ResumeFiles);
                                Log.Write($"ValidateUser ResumeFiles resumeList.Count : " + resumeList.Count, ConfigurationPolicy.ErrorLog);
                                myResumes.Add(new SelectListItem { Text = "SELECT YOUR CV", Value = "0" });
                                foreach (var item in resumeList)
                                {
                                    var datestr = item.UploadDate.ToShortDateString();
                                    myResumes.Add(new SelectListItem { Text = datestr + " - " + item.FileFullName, Value = item.Id.ToString() });
                                }
                            }
                        }

                    }

                }
                else
                {
                    var currUser = SitefinityHelper.GetUserById(ClaimsManager.GetCurrentIdentity().UserId);
                    if (currUser != null)
                    {
                        isMemberUser = SitefinityHelper.IsUserInRole(currUser, "Member");
                        Log.Write($"ValidateUser currUser  isMemberUser : "+ isMemberUser, ConfigurationPolicy.ErrorLog);
                        firstName = SitefinityHelper.GetUserFirstNameById(currUser.Id);
                    }
                        
                }

                #region Entered Email exists in JXTNext Member list
                if (isMemberUser)
                {
                    //instantiate the Sitefinity user manager
                    //if you have multiple providers you have to pass the provider name as parameter in GetManager("ProviderName") in your case it will be the asp.net membership provider user
                    UserManager userManager = UserManager.GetManager();
                    if (userManager.ValidateUser(email, password))
                    {
                        //if you need to get the user instance use the out parameter
                        Telerik.Sitefinity.Security.Model.User userToAuthenticate = null;

                        SecurityManager.AuthenticateUser(userManager.Provider.Name, email, password, staySignedIn, out userToAuthenticate);
                        if (userToAuthenticate != null)
                        {
                            isUserSignedIn = true;
                            Log.Write($"ValidateUser userToAuthenticate : " + userToAuthenticate, ConfigurationPolicy.ErrorLog);
                        }
                    }
                }
                #endregion

                Log.Write($"ValidateUser isUserVerified : " + isUserVerified, ConfigurationPolicy.ErrorLog);
                Log.Write($"ValidateUser isMemberUser : " + isMemberUser, ConfigurationPolicy.ErrorLog);
                Log.Write($"ValidateUser isUserSignedIn : " + isUserSignedIn, ConfigurationPolicy.ErrorLog);
                Log.Write($"ValidateUser myResumes : " + JsonConvert.SerializeObject(myResumes), ConfigurationPolicy.ErrorLog);
                var response = new
                {
                    IsUserVerified = isUserVerified,
                    IsUserMember = isMemberUser,
                    IsUserSignedIn = isUserSignedIn,
                    FirstName = firstName,
                    myResumes = JsonConvert.SerializeObject(myResumes)
                };

                return new JsonResult { Data = response };
            }
            catch (Exception ex)
            {
                Log.Write($"ValidateUser exception" + ex.Message, ConfigurationPolicy.ErrorLog);
                throw ex;
            }
            
        }


        [HttpPost]
        public JsonResult IsJobApplied(int jobId)
        {
            try
            {
                IGetJobListingRequest jobListingRequest = new JXTNext_GetJobListingRequest { JobID = jobId };
                IGetJobListingResponse jobListingResponse = _blConnector.GuestGetJob(jobListingRequest);
                long expiryDate = (long)DateTime.Now.ToUniversalTime().Subtract(UnixEpoch).TotalMilliseconds;
                bool isJobExpired = false;
                if (jobListingResponse.Job != null && jobListingResponse.Job.ExpiryDate <= expiryDate)
                    isJobExpired = true;
                bool isJobApplied = _isMemberAppliedJob(jobId);
                var result = new { IsJobExpired = isJobExpired, IsJobApplied = isJobApplied };
                return new JsonResult { Data = result };
            }
            catch (Exception ex)
            {
                Log.Write($"IsJobApplied exception = " + ex.Message, ConfigurationPolicy.ErrorLog);
                var result = new
                {
                    Error = true
                };
                return new JsonResult { Data = result };
            }
            
        }

        private bool _isMemberAppliedJob(int jobId)
        {
            bool isJobApplied = false;
            Log.Write($"IsJobApplied method1", ConfigurationPolicy.ErrorLog);
            JXTNext_MemberAppliedJobByIdResponse appliedJobresponse = _blConnector.MemberAppliedJobGetByJobId(jobId) as JXTNext_MemberAppliedJobByIdResponse;
            Log.Write($"IsJobApplied method appliedJobresponse.Success = " + appliedJobresponse.Success, ConfigurationPolicy.ErrorLog);
            Log.Write($"IsJobApplied method appliedJobresponse.MemberAppliedJobById = " + appliedJobresponse.MemberAppliedJobById, ConfigurationPolicy.ErrorLog);

            if (appliedJobresponse.Success)
            {
                isJobApplied = true;
            }

            if (appliedJobresponse.Errors != null && appliedJobresponse.Errors.Count > 0)
            {
                Log.Write($"IsJobApplied method error = " + appliedJobresponse.Errors.FirstOrDefault().ToString(), ConfigurationPolicy.ErrorLog);
            }

            Log.Write($"IsJobApplied isJobApplied 1 = " + isJobApplied, ConfigurationPolicy.ErrorLog);
            return isJobApplied;
        }

        private JobApplicationAttachmentSource GetAttachmentSourceType(string sourceType)
        {
            if (sourceType.ToUpper() == "DROPBOX")
                return JobApplicationAttachmentSource.Dropbox;
            if (sourceType.ToUpper() == "GOOGLEDRIVE")
                return JobApplicationAttachmentSource.GoogleDrive;
            if (sourceType.ToUpper() == "SAVED")
                return JobApplicationAttachmentSource.Saved;
            return JobApplicationAttachmentSource.Local;

        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
        }

        
        private void FetchFromAmazonS3(string providerName, string libraryName, string itemTitle)
        {
            LibrariesManager librariesManager = LibrariesManager.GetManager(providerName);
            var docLibs = librariesManager.GetDocumentLibraries();

            foreach (var lib in docLibs)
            {
                if (lib.Title.ToLower() == libraryName)
                {
                    var document = lib.Items().Where(item => item.Title == itemTitle).FirstOrDefault();
                    var stream = librariesManager.Download(document);
                }
            }

        }

        private S3FileManagerResponse UploadToAmazonS3(Guid masterDocumentId, string providerName, string libName, string fileName, Stream fileStream)
        {
            try
            {
                
                //IAmazonS3 _s3Client = new AmazonS3Client(new BasicAWSCredentials(settingsHelper.GetAmazonS3AccessKeyId(), settingsHelper.GetAmazonS3SecretKey()), RegionEndpoint.GetBySystemName(settingsHelper.GetAmazonS3RegionEndpoint()));
                S3FilemanagerService fileManagerService = new S3FilemanagerService(_siteSettingsHelper.GetAmazonS3RegionEndpoint(), _siteSettingsHelper.GetAmazonS3AccessKeyId(), _siteSettingsHelper.GetAmazonS3SecretKey());
                var response = fileManagerService.PostObjectToProvider<S3FileManagerResponse, S3FileManagerRequest>(
                        new S3FileManagerRequest
                        {
                            FileName = masterDocumentId.ToString() + "_" + fileName,
                            Directory = _siteSettingsHelper.GetAmazonS3UrlName()+ "/" + libName,
                            FileStream = fileStream,
                            S3BucketName = _siteSettingsHelper.GetAmazonS3BucketName(),
                            ContentType = AmazonS3Constants.DocumentContentType
                        });
                
                return response;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        //private string UploadToAmazonS3(Guid masterDocumentId, string providerName, string libName, string fileName, Stream fileStream)
        //{

        //    LibrariesManager librariesManager = LibrariesManager.GetManager(providerName);
        //    string url = null;
        //    if (librariesManager != null)
        //    {
        //        var libManagerSecurityCheckStatus = librariesManager.Provider.SuppressSecurityChecks;
        //        try
        //        {
                    
        //            // Make sure that supress the security checks so that everyone can upload the files
        //            librariesManager.Provider.SuppressSecurityChecks = true;
        //            Document document = librariesManager.GetDocuments().Where(i => i.Id == masterDocumentId).FirstOrDefault();

        //            if (document == null)
        //            {
        //                //The document is created as master. The masterDocumentId is assigned to the master version.
        //                document = librariesManager.CreateDocument(masterDocumentId);

        //                //Set the parent document library.
        //                DocumentLibrary documentLibrary = librariesManager.GetDocumentLibraries().Where(d => d.Title == libName).SingleOrDefault();
        //                document.Parent = documentLibrary;

        //                //Set the properties of the document.
        //                string documentTitle = masterDocumentId.ToString() + "_" + fileName;
        //                document.Title = documentTitle;
        //                document.DateCreated = DateTime.UtcNow;
        //                document.PublicationDate = DateTime.UtcNow;
        //                document.LastModified = DateTime.UtcNow;
        //                document.UrlName = Regex.Replace(documentTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
        //                document.MediaFileUrlName = Regex.Replace(documentTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
        //                document.ApprovalWorkflowState = "Published";

        //                //Upload the document file.
        //                string fileExtension = "." + fileName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last();
        //                librariesManager.Upload(document, fileStream, fileExtension ?? string.Empty);

        //                //Recompiles and validates the url of the document.
        //                librariesManager.RecompileAndValidateUrls(document);

        //                //Save the changes.
        //                librariesManager.SaveChanges();
        //                url = document.Url;
        //                //Publish the DocumentLibraries item. The live version acquires new ID.
        //                var bag = new Dictionary<string, string>();
        //                bag.Add("ContentType", typeof(Document).FullName);

        //                // Run with elevatede privilages so that everybody can upload files
        //                SystemManager.RunWithElevatedPrivilege(d => WorkflowManager.MessageWorkflow(masterDocumentId, typeof(Document), null, "Publish", false, bag));
        //            }
        //        }

        //        finally
        //        {
        //            // Reset the suppress security checks
        //            librariesManager.Provider.SuppressSecurityChecks = libManagerSecurityCheckStatus;
        //        }
        //    }
        //    else
        //    {
        //        Log.Write($"UploadToAmazonS3 libraryManager is null ", ConfigurationPolicy.ErrorLog);
        //    }
            
            
        //    return url;
        //}

        private JobApplicationViewModel GetJobApplicationConfigurations(JobApplicationStatus status, string message)
        {
            return new JobApplicationViewModel
            {
                ApplicationTitle = message,
                ApplicationStatus = status,
                ApplicationAttachments = new List<JobApplicationAttachmentItem>
                {
                    //Resume
                    new JobApplicationAttachmentItem
                    {
                        AttachementType = JobApplicationAttachmentType.Resume,
                        Title = "Resume",
                        Enabled = true,
                        AttachementFileUploadKey = JobApplicationAttachmentSettings.APPLICATION_RESUME_UPLOAD_KEY,
                        Integrations = null
                    },
                    //Coverletter
                    new JobApplicationAttachmentItem
                    {
                        AttachementType = JobApplicationAttachmentType.Coverletter,
                        Title = "Cover Letter",
                        Enabled = true,
                        AttachementFileUploadKey = JobApplicationAttachmentSettings.APPLICATION_COVERLETTER_UPLOAD_KEY,
                        Integrations = null
                    },
                    //Documents
                    new JobApplicationAttachmentItem
                    {
                        AttachementType = JobApplicationAttachmentType.Documents,
                        Title = "Documents",
                        Enabled = true,
                        AttachementFileUploadKey = JobApplicationAttachmentSettings.APPLICATION_DOCUMENTS_UPLOAD_KEY,
                        Integrations = null
                    }
                },
                ApplicationMessage = null
            };
        }

        private List<JobApplicationAttachmentUploadItem> GatherAttachments(JobApplicationAttachmentSource sourceResume, JobApplicationAttachmentSource sourceCoverLetter, JobApplicationAttachmentSource sourceDocuments, string uploadFilesResumeJSON, string uploadFilesCoverLetterJSON, string uploadFilesDocumentsJSON,string loginUserEmail)
        {
            List<JobApplicationAttachmentUploadItem> attachmentItems = new List<JobApplicationAttachmentUploadItem>();
            
            if (sourceResume == JobApplicationAttachmentSource.Saved)
            {
                if (!string.IsNullOrEmpty(loginUserEmail))
                {

                    var memberResponse = _blConnector.GetMemberByEmail(loginUserEmail);
                    if (memberResponse.Member?.ResumeFiles != null)
                    {
                        var resumeList = JsonConvert.DeserializeObject<List<ProfileResume>>(memberResponse.Member.ResumeFiles);
                        var selectedResume = resumeList.Where(x => x.Id.ToString() == uploadFilesResumeJSON).FirstOrDefault();
                        if (selectedResume != null)
                        {
                            var resumeUploadStream = _jobApplicationService.GetFileStreamFromAmazonS3(JobApplicationAttachmentSettings.PROFILE_RESUME_UPLOAD_KEY, 1, selectedResume.UploadPathToAttachment);
                            if (resumeUploadStream != null)
                            {
                                JobApplicationAttachmentUploadItem item = GatherSavedResumeAttachmentDetails(JobApplicationAttachmentType.Resume, resumeUploadStream, selectedResume.FileFullName);
                                if (item != null)
                                    attachmentItems.Add(item);
                            }
                        }

                    }
                }
            }

            if (sourceResume == JobApplicationAttachmentSource.Local)
            {
                bool hasResumeUpload = Request.Files.AllKeys.Contains(JobApplicationAttachmentSettings.APPLICATION_RESUME_UPLOAD_KEY);
                if (hasResumeUpload)
                {
                    JobApplicationAttachmentUploadItem item = GatherAttachmentDetails(JobApplicationAttachmentType.Resume, Request.Files[JobApplicationAttachmentSettings.APPLICATION_RESUME_UPLOAD_KEY]);
                    if (item != null)
                        attachmentItems.Add(item);
                }
            }

            if (sourceCoverLetter == JobApplicationAttachmentSource.Local)
            {
                bool hasCoverletterUpload = Request.Files.AllKeys.Contains(JobApplicationAttachmentSettings.APPLICATION_COVERLETTER_UPLOAD_KEY);
                if (hasCoverletterUpload)
                {
                    JobApplicationAttachmentUploadItem item = GatherAttachmentDetails(JobApplicationAttachmentType.Coverletter, Request.Files[JobApplicationAttachmentSettings.APPLICATION_COVERLETTER_UPLOAD_KEY]);
                    if (item != null)
                        attachmentItems.Add(item);
                }
            }

            if (sourceDocuments == JobApplicationAttachmentSource.Local)
            {
                bool hasDocumentsUpload = Request.Files.AllKeys.Contains(JobApplicationAttachmentSettings.APPLICATION_DOCUMENTS_UPLOAD_KEY);
                if (hasDocumentsUpload)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        if (Request.Files.AllKeys[i] == JobApplicationAttachmentSettings.APPLICATION_DOCUMENTS_UPLOAD_KEY)
                        {
                            JobApplicationAttachmentUploadItem item = GatherAttachmentDetails(JobApplicationAttachmentType.Documents, Request.Files[i]);
                            if (item != null)
                                attachmentItems.Add(item);
                        }
                    }

                }
            }

            // Processing the Resume
            if (sourceResume == JobApplicationAttachmentSource.GoogleDrive)
            {
                var googleDriveModel = JsonConvert.DeserializeObject<UploadFilesFormPostModel>(uploadFilesResumeJSON);
                JobApplicationAttachmentUploadItem item = GetAttachementFromGoogleDrive(googleDriveModel, JobApplicationAttachmentType.Resume);
                attachmentItems.Add(item);
            }

            if (sourceResume == JobApplicationAttachmentSource.Dropbox)
            {
                var dropboxDriveModel = JsonConvert.DeserializeObject<UploadFilesFormPostModel>(uploadFilesResumeJSON);
                JobApplicationAttachmentUploadItem item = GetAttachementFromDropbox(dropboxDriveModel, JobApplicationAttachmentType.Resume);
                attachmentItems.Add(item);
            }

            // Processing Cover Letter
            if (sourceCoverLetter == JobApplicationAttachmentSource.GoogleDrive)
            {
                var googleDriveModel = JsonConvert.DeserializeObject<UploadFilesFormPostModel>(uploadFilesCoverLetterJSON);
                JobApplicationAttachmentUploadItem item = GetAttachementFromGoogleDrive(googleDriveModel, JobApplicationAttachmentType.Coverletter);
                attachmentItems.Add(item);
            }

            if (sourceCoverLetter == JobApplicationAttachmentSource.Dropbox)
            {
                var dropboxDriveModel = JsonConvert.DeserializeObject<UploadFilesFormPostModel>(uploadFilesCoverLetterJSON);
                JobApplicationAttachmentUploadItem item = GetAttachementFromDropbox(dropboxDriveModel, JobApplicationAttachmentType.Coverletter);
                attachmentItems.Add(item);
            }

            // Processing Documents
            if (sourceDocuments == JobApplicationAttachmentSource.GoogleDrive)
            {
                var googleDriveModel = JsonConvert.DeserializeObject<List<UploadFilesFormPostModel>>(uploadFilesDocumentsJSON);
                foreach (var googleFile in googleDriveModel)
                {
                    JobApplicationAttachmentUploadItem item = GetAttachementFromGoogleDrive(googleFile, JobApplicationAttachmentType.Documents);
                    attachmentItems.Add(item);
                }
            }

            if (sourceDocuments == JobApplicationAttachmentSource.Dropbox)
            {
                var dropboxDriveModel = JsonConvert.DeserializeObject<List<UploadFilesFormPostModel>>(uploadFilesDocumentsJSON);
                foreach (var dropboxFile in dropboxDriveModel)
                {
                    JobApplicationAttachmentUploadItem item = GetAttachementFromDropbox(dropboxFile, JobApplicationAttachmentType.Documents);
                    attachmentItems.Add(item);
                }                
            }

            return attachmentItems;
        }

        private JobApplicationAttachmentUploadItem GetAttachementFromGoogleDrive(UploadFilesFormPostModel googleFilesInfo, JobApplicationAttachmentType attachmentType)
        {
            GoogleDriveFileHandlerService googleDriveFileHandleService = new GoogleDriveFileHandlerService();
            SiteSettingsHelper siteSettingsHelper = new SiteSettingsHelper();
            string clientId = siteSettingsHelper.GetCurrentSiteGoogleClientId();
            string clientSecret = siteSettingsHelper.GetCurrentSiteGoogleClientSecret();
            GoogleDriveFileHandlerRequestModel baseFileHandle = new GoogleDriveFileHandlerRequestModel()
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                OAuthToken = googleFilesInfo.AuthToken,
                FileId = googleFilesInfo.Field,
                FileName = googleFilesInfo.FileName,
                FileUrl = googleFilesInfo.UrlPath,
                MimeType = googleFilesInfo.MIMEType
            };

            JobApplicationAttachmentUploadItem item = null;
            var googleFileResonse = googleDriveFileHandleService.ProcessFileDownload<GoogleDriveFileHandlerResponseModel, GoogleDriveFileHandlerRequestModel>(baseFileHandle);

            if (googleFileResonse.FileSuccessStatus)
            {
                Guid identifier = Guid.NewGuid();
                string docExtension = googleFilesInfo.FileName.Split('.').Last();
                string documentTitle = identifier.ToString() + "_" + googleFilesInfo.FileName;
                item = new JobApplicationAttachmentUploadItem
                {
                    Id = identifier.ToString(),
                    AttachmentType = attachmentType,
                    FileName = googleFilesInfo.FileName,
                    FileStream = googleFileResonse.FileStream,
                    PathToAttachment = Regex.Replace(documentTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-")+ "."+docExtension,
                    Status = "Ready"
                };
            }

            return item;
        }

        private JobApplicationAttachmentUploadItem GetAttachementFromDropbox(UploadFilesFormPostModel dropboxFilesInfo, JobApplicationAttachmentType attachmentType)
        {
            DropboxFileHandlerService dropboxFileHandleService = new DropboxFileHandlerService();
            DropboxFileHandlerRequestModel baseFileHandle = new DropboxFileHandlerRequestModel()
            {
                FileName = dropboxFilesInfo.FileName,
                FileUrl = dropboxFilesInfo.UrlPath
            };

            var dropboxFileResonse = dropboxFileHandleService.ProcessFileDownload<DropboxFileHandlerResponseModel, DropboxFileHandlerRequestModel>(baseFileHandle);
            JobApplicationAttachmentUploadItem item = null;
            
            if (dropboxFileResonse.FileSuccessStatus)
            {
                Guid identifier = Guid.NewGuid();
                string docExtension = dropboxFilesInfo.FileName.Split('.').Last();
                string documentTitle = identifier.ToString() + "_" + dropboxFilesInfo.FileName;
                item = new JobApplicationAttachmentUploadItem
                {
                    Id = identifier.ToString(),
                    AttachmentType = attachmentType,
                    FileName = dropboxFilesInfo.FileName,
                    FileStream = dropboxFileResonse.FileStream,
                    PathToAttachment = Regex.Replace(documentTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-") + "." + docExtension,
                    Status = "Ready"
                };
            }

            return item;
        }

        private List<string> GetAttachmentPath(List<JobApplicationAttachmentUploadItem> attachmentItems, JobApplicationAttachmentType attachmentType)
        {
            List<JobApplicationAttachmentUploadItem> item = attachmentItems.Where(c => c.AttachmentType == attachmentType).ToList();
            List<string> paths = new List<string>();

            foreach (var path in item)
            {
                paths.Add(path.PathToAttachment);
            }

            return paths;
        }

        private JobApplicationAttachmentUploadItem GatherSavedResumeAttachmentDetails(JobApplicationAttachmentType attachmentType, Stream file, string fileName)
        {
            if (file != null )
            {
                Guid identifier = Guid.NewGuid();
                string docExtension = fileName.Split('.').Last();
                string documentTitle = identifier.ToString() + "_" + fileName;
                return new JobApplicationAttachmentUploadItem
                {
                    Id = identifier.ToString(),
                    AttachmentType = attachmentType,
                    FileName = fileName,
                    FileStream = file,
                    PathToAttachment = Regex.Replace(documentTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-") + "." + docExtension,
                    Status = "Ready"
                };
            }
            return null;
        }

        private JobApplicationAttachmentUploadItem GatherAttachmentDetails(JobApplicationAttachmentType attachmentType, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                Guid identifier = Guid.NewGuid();
                string docExtension = file.FileName.Split('.').Last();
                string documentTitle = identifier.ToString() + "_" + file.FileName;
                
                return new JobApplicationAttachmentUploadItem
                {
                    Id = identifier.ToString(),
                    AttachmentType = attachmentType,
                    FileName = file.FileName,
                    FileStream = file.InputStream,
                    PathToAttachment = Regex.Replace(documentTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-") + "." + docExtension,
                    Status = "Ready"
                };
            }
            return null;
        }

        private void ProcessResumeFileUpload(ref JobApplicationAttachmentUploadItem uploadItem)
        {
            var libName = JobApplicationAttachmentSettings.PROFILE_RESUME_UPLOAD_LIBRARY;

            try
            {
                var response = UploadToAmazonS3(Guid.Parse(uploadItem.Id), _siteSettingsHelper.GetAmazonS3ProviderName(), libName, uploadItem.PathToAttachment, uploadItem.FileStream);
                if (response != null && response.Success)
                {
                    uploadItem.FileUrl = null;
                    uploadItem.Status = "Completed";
                }
                else
                {
                    uploadItem.Status = "Failed";
                    uploadItem.Message = response?.Errors.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                uploadItem.Status = "Failed";
                uploadItem.Message = ex.Message;
            }
        }

        private void ProcessFileUpload(ref JobApplicationAttachmentUploadItem uploadItem)
        {
            var libName = FileUploadLibraryGet(uploadItem.AttachmentType);

            try
            {
                var response = UploadToAmazonS3(Guid.Parse(uploadItem.Id), _siteSettingsHelper.GetAmazonS3ProviderName(), libName, uploadItem.PathToAttachment, uploadItem.FileStream);
                if (response != null && response.Success)
                {
                    uploadItem.FileUrl = null;
                    uploadItem.Status = "Completed";
                }
                else
                {
                    uploadItem.Status = "Failed";
                    uploadItem.Message = response?.Errors.FirstOrDefault();
                }
                
            }
            catch (Exception ex)
            {
                Log.Write($"ProcessFileUpload {ex.Message}", ConfigurationPolicy.ErrorLog);
                uploadItem.Status = "Failed";
                uploadItem.Message = ex.Message;
            }
        }

        private string FileUploadLibraryGet(JobApplicationAttachmentType attachmentType)
        {
            switch (attachmentType)
            {
                case JobApplicationAttachmentType.Resume:
                    return JobApplicationAttachmentSettings.APPLICATION_RESUME_UPLOAD_LIBRARY;
                case JobApplicationAttachmentType.Coverletter:
                    return JobApplicationAttachmentSettings.APPLICATION_COVERLETTER_UPLOAD_LIBRARY;
                case JobApplicationAttachmentType.Documents:
                    return JobApplicationAttachmentSettings.APPLICATION_DOCUMENTS_UPLOAD_LIBRARY;
                default:
                    return null;
            }
        }

        private List<ApplyWithSocialMedia> GetSelectedSocialMediaItems()
        {
            var socialMediaLinks = JsonConvert.DeserializeObject<List<ApplyWithSocialMedia>>(this.SerializedApplyWithSocialMedia);
            var selectedLinks = socialMediaLinks.Where(l => l.Selected == true).ToList();

            return selectedLinks;
        }

        

        private bool AddUploadedResumeToProfileDashBoard(JobApplicationAttachmentUploadItem resume,String Email)
        {
            try
            {
                
                ProcessResumeFileUpload(ref resume);

                ProfileResumeJsonModel resumeJson = new ProfileResumeJsonModel()
                {
                    Id = Guid.Parse(resume.Id),
                    UploadDate = DateTime.Now,
                    FileName = resume.FileName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).First(),
                    UploadPathToAttachment = resume.Id.ToString() + "_" + resume.FileName,
                    FileExtension = resume.FileName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last(),
                    FileUrl = resume.FileUrl
                };

                var res = _blConnector.GetMemberByEmail(Email);
                List<ProfileResumeJsonModel> resumeList = new List<ProfileResumeJsonModel>();
                if (res.Member != null && res.Member.ResumeFiles != null)
                    resumeList = JsonConvert.DeserializeObject<List<ProfileResumeJsonModel>>(res.Member.ResumeFiles);


                resumeList.Add(resumeJson);
                res.Member.ResumeFiles = JsonConvert.SerializeObject(resumeList);
                _blConnector.UpdateMember(res.Member);
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        private EmailNotificationSettings _createAdvertiserEmailTemplate(JobApplicationEmailTemplateModel emailTemplate)
        {
            EmailNotificationSettings advertiserEmailTemplate = _addEmailNotificationSettings(emailTemplate);

            // CC and BCC emails
            _addCCToEmailTemplate(ref advertiserEmailTemplate, this.AdvertiserEmailTemplateCC);

            _addBCCToEmailTemplate(ref advertiserEmailTemplate, this.AdvertiserEmailTemplateBCC);

            return advertiserEmailTemplate;
        }

        private EmailNotificationSettings _createApplicantEmailTemplate(JobApplicationEmailTemplateModel emailTemplate)
        {
            EmailNotificationSettings applicantEmailTemplate = _addEmailNotificationSettings(emailTemplate);
            // CC and BCC emails
            _addCCToEmailTemplate(ref applicantEmailTemplate, this.EmailTemplateCC);

            _addBCCToEmailTemplate(ref applicantEmailTemplate, this.EmailTemplateBCC);

            return applicantEmailTemplate;
        }

        private EmailNotificationSettings _createRegistrationEmailTemplate(JobApplicationEmailTemplateModel emailTemplate)
        {
            EmailNotificationSettings registerEmailTemplate = _addEmailNotificationSettings(emailTemplate);
            // CC and BCC emails
            _addCCToEmailTemplate(ref registerEmailTemplate, this.RegistrationEmailTemplateCC);

            _addBCCToEmailTemplate(ref registerEmailTemplate, this.RegistrationEmailTemplateBCC);

            return registerEmailTemplate;
        }

        private EmailNotificationSettings _addEmailNotificationSettings(JobApplicationEmailTemplateModel emailTemplate)
        {
            EmailNotificationSettings emailNotificationSettings = new EmailNotificationSettings(new EmailTarget(emailTemplate.FromFullName, emailTemplate.FromEmail),
                                                                                                                    new EmailTarget(emailTemplate.ToFullName, emailTemplate.ToEmail),
                                                                                                                    emailTemplate.Subject,
                                                                                                                    emailTemplate.HtmlContent, emailTemplate.Attachments);


            return emailNotificationSettings;
        }

        private void _addCCToEmailTemplate(ref EmailNotificationSettings emailNotificationSettings, string emailTemplateCC)
        {
            if (!emailTemplateCC.IsNullOrEmpty())
            {
                foreach (var ccEmail in emailTemplateCC.Split(';'))
                {
                    emailNotificationSettings?.AddCC(String.Empty, ccEmail);
                }
            }
        }

        private void _addBCCToEmailTemplate(ref EmailNotificationSettings emailNotificationSettings, string emailTemplateBCC)
        {
            if (!emailTemplateBCC.IsNullOrEmpty())
            {
                foreach (var bccEmail in emailTemplateBCC.Split(';'))
                {
                    emailNotificationSettings?.AddBCC(String.Empty, bccEmail);
                }
            }
        }

        public string ItemType
        {
            get { return this._itemType; }
            set { this._itemType = value; }
        }
        public string EmailTemplateProviderName
        {
            get { return SitefinityHelper.GetCurrentSiteEmailTemplateProviderName(); }
        }


        public bool HideGoogleDrive { get; set; }
        public bool HideDropBox { get; set; }
        public string EmailTemplateId { get; set; }
        public string EmailTemplateName { get; set; }
        public string EmailTemplateCC { get; set; }
        public string EmailTemplateBCC { get; set; }
        public string EmailTemplateSenderName { get; set; }
        public string EmailTemplateSenderEmailAddress { get; set; }
        public string EmailTemplateEmailSubject { get; set; }
        
        public string AdvertiserEmailTemplateId { get; set; }
        public string AdvertiserEmailTemplateName { get; set; }
        public string AdvertiserEmailTemplateCC { get; set; }
        public string AdvertiserEmailTemplateBCC { get; set; }
        public string AdvertiserEmailTemplateSenderName { get; set; }
        public string AdvertiserEmailTemplateSenderEmailAddress { get; set; }
        public string AdvertiserEmailTemplateEmailSubject { get; set; }

        
        public string RegistrationEmailTemplateId { get; set; }
        public string RegistrationEmailTemplateName { get; set; }
        public string RegistrationEmailTemplateCC { get; set; }
        public string RegistrationEmailTemplateBCC { get; set; }
        public string RegistrationEmailTemplateSenderName { get; set; }
        public string RegistrationEmailTemplateSenderEmailAddress { get; set; }
        public string RegistrationEmailTemplateEmailSubject { get; set; }

        public string CssClass { get; set; }
        public string SerializedCloudSettingsParams { get; set; }
        public string RegisterPageId { get; set; }
        public string JobApplicationSuccessPageId { get; set; }
        public string SerializedApplyWithSocialMedia { get; set; }

        internal const string WidgetIconCssClass = "sfMvcIcn";
        private string templateName = "Simple";
        private string templateNamePrefix = "JobApplication.";
        private string _itemType = "Telerik.Sitefinity.DynamicTypes.Model.StandardEmailTemplate.EmailTemplate";
        private string _emailTemplateProviderName = "OpenAccessProvider";
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
