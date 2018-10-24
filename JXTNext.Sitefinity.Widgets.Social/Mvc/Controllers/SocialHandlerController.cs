﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using System.ComponentModel;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using JXTNext.Sitefinity.Widgets.Social.Mvc.Logics;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Common;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Member;
using JXTNext.Sitefinity.Services.Intefaces;
using JXTNext.Sitefinity.Connector.BusinessLogics;
using JXTNext.Sitefinity.Widgets.Social.Mvc.Models;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Model;
using JXTNext.Sitefinity.Widgets.Social.Mvc.StringResources;
using JXTNext.Sitefinity.Common.Helpers;
using Telerik.Sitefinity.Security.Claims;
using JXTNext.Sitefinity.Services.Intefaces.Models.JobApplication;
using System.IO;
using Telerik.Sitefinity.Abstractions;

namespace JXTNext.Sitefinity.Widgets.Social.Mvc.Controllers
{
    [EnhanceViewEngines]
    [Localization(typeof(SocialHandlerResources))]
    [ControllerToolboxItem(Name = "SocialHandler_MVC", Title = "Social handler", SectionName = "JXTNext.Social", CssClass = SocialHandlerController.WidgetIconCssClass)]
    public class SocialHandlerController : Controller
    {
        SocialHandlerLogics _socialHandlerLogics;
        public string CssClass { get; set; }
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

        IJobApplicationService _jobApplicationService;
        IBusinessLogicsConnector _blConnector;

        public SocialHandlerController(SocialHandlerLogics socialHandlerLogics, IJobApplicationService jobApplicationService, IBusinessLogicsConnector blConnector)
        {
            _socialHandlerLogics = socialHandlerLogics;
            _jobApplicationService = jobApplicationService;
            _blConnector = blConnector;
        }

        public ActionResult Index(string code, string state, int? JobId)
        {
            SocialMediaJobViewModel viewModel = new SocialMediaJobViewModel();

            try
            {
                // Logging this info for Indeed test
                Log.Write("Social Handler Index Action Start : ", ConfigurationPolicy.ErrorLog);

                // This is the CSS classes enter from More Options
                ViewBag.CssClass = this.CssClass;

                if (string.IsNullOrWhiteSpace(this.TemplateName))
                {
                    this.TemplateName = "SocialHandler.Simple";
                }

                viewModel.Status = JobApplicationStatus.Available;

                if (_socialHandlerLogics != null)
                {
                    Log.Write("_socialHandlerLogics not null", ConfigurationPolicy.ErrorLog);
                    StreamReader reader = new StreamReader(Request.InputStream);
                    string indeedJsonStringData = String.Empty;
                    if (reader != null)
                        indeedJsonStringData = reader.ReadToEnd();

                    var result = _socialHandlerLogics.ProcessSocialHandlerData(code, state, indeedJsonStringData);

                    if(result != null)
                    {
                        Log.Write("_socialHandlerLogics 'result' not null", ConfigurationPolicy.ErrorLog);
                        Log.Write(result.Success + " " + result.JobId, ConfigurationPolicy.ErrorLog);
                        if(result.Errors != null)
                            Log.Write(result.Errors.FirstOrDefault(), ConfigurationPolicy.ErrorLog);
                    }
                    if (result != null && result.Success == true && result.JobId.HasValue)
                    {
                        // Logging this info for Indeed test
                        Log.Write(result.JobId, ConfigurationPolicy.ErrorLog);

                        JobApplicationStatus status = JobApplicationStatus.Available;
                        if (_jobApplicationService != null)
                        {
                            ApplicantInfo applicantInfo = new ApplicantInfo()
                            {
                                FirstName = result.FirstName,
                                LastName = result.LastName,
                                Email = result.Email,
                                Password = "Password123",
                                PhoneNumber = result.PhoneNumber
                            };
                            var overrideEmail = _jobApplicationService.GetOverrideEmail(ref status, applicantInfo, true);
                            Log.Write("overrideEmail is : " + overrideEmail, ConfigurationPolicy.ErrorLog);
                            if (overrideEmail != null && status == JobApplicationStatus.Available)
                            {
                                // Gather Attachments
                                Guid identifier = Guid.NewGuid();
                                JobApplicationAttachmentUploadItem uploadItem = new JobApplicationAttachmentUploadItem()
                                {
                                    Id = identifier.ToString(),
                                    AttachmentType = JobApplicationAttachmentType.Resume,
                                    FileName = result.FileName,
                                    FileStream = result.FileStream,
                                    PathToAttachment = identifier.ToString() + "_" + result.FileName,
                                    Status = "Ready"
                                };

                                List<JobApplicationAttachmentUploadItem> attachments = new List<JobApplicationAttachmentUploadItem>();
                                attachments.Add(uploadItem);

                                string resumeAttachmentPath = JobApplicationAttachmentUploadItem.GetAttachmentPath(attachments, JobApplicationAttachmentType.Resume);
                                string coverletterAttachmentPath = JobApplicationAttachmentUploadItem.GetAttachmentPath(attachments, JobApplicationAttachmentType.Coverletter);

                                string htmlEmailContent = _jobApplicationService.GetHtmlEmailContent(this.EmailTemplateId, this.EmailTemplateProviderName, this._itemType);

                                // Email notification settings
                                EmailNotificationSettings emailNotificationSettings = new EmailNotificationSettings(new EmailTarget(this.EmailTemplateSenderName, this.EmailTemplateSenderEmailAddress),
                                                                                                    new EmailTarget(SitefinityHelper.GetUserFirstNameById(ClaimsManager.GetCurrentIdentity().UserId), overrideEmail),
                                                                                                    this.EmailTemplateEmailSubject,
                                                                                                    htmlEmailContent);
                                // CC and BCC emails
                                if (!this.EmailTemplateCC.IsNullOrEmpty())
                                {
                                    foreach (var ccEmail in this.EmailTemplateCC.Split(';'))
                                    {
                                        emailNotificationSettings.AddCC(String.Empty, ccEmail);
                                    }
                                }

                                if (!this.EmailTemplateBCC.IsNullOrEmpty())
                                {
                                    foreach (var bccEmail in this.EmailTemplateBCC.Split(';'))
                                    {
                                        emailNotificationSettings.AddBCC(String.Empty, bccEmail);
                                    }
                                }

                                //Create Application 
                                IMemberApplicationResponse response = _blConnector.MemberCreateJobApplication(
                                    new JXTNext_MemberApplicationRequest { ApplyResourceID = result.JobId.Value, MemberID = 2, ResumePath = resumeAttachmentPath, CoverletterPath = coverletterAttachmentPath, EmailNotification = emailNotificationSettings },
                                    overrideEmail);

                                if (response.Success && response.ApplicationID.HasValue)
                                {
                                    var hasFailedUpload = _jobApplicationService.UploadFiles(attachments);
                                    if (hasFailedUpload)
                                    {
                                        viewModel.Status = JobApplicationStatus.Technical_Issue; // Unable to attach files
                                        viewModel.Message = "Unable to attach files to application";
                                    }
                                    else
                                    {
                                        viewModel.Status = JobApplicationStatus.Applied_Successful;
                                        viewModel.Message = "Your application was successfully processed.";
                                        if (!this.JobApplicationSuccessPageId.IsNullOrEmpty())
                                        {
                                            var successPageUrl = SitefinityHelper.GetPageUrl(this.JobApplicationSuccessPageId);
                                            return Redirect(successPageUrl);
                                        }
                                    }
                                }
                                else
                                {
                                    viewModel.Status = JobApplicationStatus.Technical_Issue;
                                    viewModel.Message = response.Errors.FirstOrDefault();
                                }
                            }
                            else
                            {
                                Log.Write("overrideEmail is null: ", ConfigurationPolicy.ErrorLog);

                                if (status == JobApplicationStatus.NotAbleToLoginCreatedUser)
                                    viewModel.Message = "Unable to process your job application. Please try logging in and re-apply for the job.";
                                else if (status == JobApplicationStatus.NotAbleToCreateUser)
                                    viewModel.Message = "Unable to create user. Please register from";

                                viewModel.Status = status;
                            }
                        }

                        else
                        {
                            Log.Write("_jobApplicationService is null", ConfigurationPolicy.ErrorLog);
                        }
                    }
                }
            }

            catch(Exception ex)
            {
                Log.Write("Social Handler : Exception Caught" + ex.Message, ConfigurationPolicy.ErrorLog);
            }


            if (!this.JobSearchResultsPageId.IsNullOrEmpty())
                ViewBag.JobSearchResultsUrl = SitefinityHelper.GetPageUrl(this.JobSearchResultsPageId);

            var fullTemplateName = this.templateNamePrefix + this.TemplateName;

            return View(fullTemplateName, viewModel);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
        }

        public string ItemType
        {
            get { return this._itemType; }
            set { this._itemType = value; }
        }
        public string EmailTemplateProviderName
        {
            get { return _emailTemplateProviderName; }
            set { this._emailTemplateProviderName = value; }
        }
        public string EmailTemplateId { get; set; }
        public string EmailTemplateName { get; set; }
        public string EmailTemplateCC { get; set; }
        public string EmailTemplateBCC { get; set; }
        public string EmailTemplateSenderName { get; set; }
        public string EmailTemplateSenderEmailAddress { get; set; }
        public string EmailTemplateEmailSubject { get; set; }
        public string RegisterPageId { get; set; }
        public string JobApplicationSuccessPageId { get; set; }
        public string JobSearchResultsPageId { get; set; }

        internal const string WidgetIconCssClass = "sfMvcIcn";
        private string templateName = "Simple";
        private string templateNamePrefix = "SocialHandler.";
        private string _itemType = "Telerik.Sitefinity.DynamicTypes.Model.StandardEmailTemplate.EmailTemplate";
        private string _emailTemplateProviderName = "OpenAccessProvider";
    }
}