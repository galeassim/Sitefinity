﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Modules.Pages;
using JXTNext.Sitefinity.Widgets.Content.Mvc.Models;

namespace JXTNext.Sitefinity.Widgets.Content.Mvc.Controllers
{
    [EnhanceViewEngines]
    [ControllerToolboxItem(Name = "PageTitle_MVC", Title = "Page Title", SectionName = "JXTNext.Content", CssClass = PageTitleController.WidgetIconCssClass)]
    public class PageTitleController : Controller
    {
        public ActionResult Index()
        {
            string pageTitle = String.Empty;
            if (this.IsDefaultTitle)
                pageTitle = SiteMapBase.GetActualCurrentNode().Title;
            else
                pageTitle = this.CustomPageTitle;

            var viewModel = new PageTitleViewModel() { PageTitle = pageTitle };

            ViewBag.CssClass = CssClass;

            return View("Simple", viewModel);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
        }

        public string CssClass { get; set; }
        public string CustomPageTitle { get; set; }
        public bool IsDefaultTitle { get; set; }
        internal const string WidgetIconCssClass = "sfMvcIcn";
    }
}
