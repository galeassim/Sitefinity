﻿using JXTNext.Sitefinity.Connector.BusinessLogics;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Advertisers;
using JXTNext.Sitefinity.Connector.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using JXTNext.Sitefinity.Common.Helpers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using System.ComponentModel;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Member;
using JXTNext.Sitefinity.Widgets.User.Mvc.Logics;
using JXTNext.Sitefinity.Widgets.User.Mvc.Models.MemberSavedJob;
using JXTNext.Sitefinity.Widgets.User.Mvc.StringResources;

namespace JXTNext.Sitefinity.Widgets.User.Mvc.Controllers
{
    [EnhanceViewEngines]
    [Localization(typeof(MemberSavedJobsResources))]
    [ControllerToolboxItem(Name = "Member_SavedJobs_MVC", Title = "JXT Member Saved Jobs", SectionName = "JXTNext.Member", CssClass = MemberSavedJobsController.WidgetIconCssClass)]
    public class MemberSavedJobsController : Controller
    {
        internal const string WidgetIconCssClass = "sfMvcIcn";
        string templateNamePrefix = "MemberSavedJobs.";
        private string templateName = "List";

        MemberSavedJobBC _memberSavedJobBC;

        /// <summary>
        /// Gets or sets the name of the template that widget will be displayed.
        /// </summary>
        /// <value></value>
        public string TemplateName { get => this.templateName; set => this.templateName = value; }

        public MemberSavedJobsController(MemberSavedJobBC memberSavedJobBC)
        {
            _memberSavedJobBC = memberSavedJobBC;
        }

        // GET: JobDetails
        public ActionResult Index()
        {
            bool GetListSuccess = _memberSavedJobBC.GetList(out List<MemberSavedJobDisplayItem> displayItems);
            ViewBag.JobDetailsPageUrl = SitefinityHelper.GetPageUrl(this.JobDetailsPageId);

            if (GetListSuccess)
            {
                var fullTemplateName = this.templateNamePrefix + this.TemplateName;
                return View(fullTemplateName, displayItems);
            }

            return null;
        }

        [HttpDelete]
        public ActionResult Index(int savedJobId)
        {
            bool deleteSuccess = _memberSavedJobBC.Delete(savedJobId);

            string message;
            if (deleteSuccess)
                message = "Saved job successfully deleted";
            else
                message = "Unable to process your previous request, please try again.";

            return Index();
        }



        protected override void HandleUnknownAction(string actionName)
        {
            this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
        }

        public string JobDetailsPageId { get; set; }
    }
}