﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using JXTNext.Sitefinity.Connector.BusinessLogics;
using JXTNext.Sitefinity.Connector.Options;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Search;
using JXTNext.Sitefinity.Connector.Options.Models.Job;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Advertisers;
using JXTNext.Sitefinity.Connector.BusinessLogics.Models.Job;
using Telerik.Sitefinity.Security.Claims;
using Newtonsoft.Json;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "JobDetails_MVC", Title = "Job Details", SectionName = "Search", CssClass = JobDetailsController.WidgetIconCssClass)]
    public class JobDetailsController : Controller
    {
        IBusinessLogicsConnector _testBLConnector;
        IOptionsConnector _testOConnector;

        public JobDetailsController(IEnumerable<IBusinessLogicsConnector> _bConnectors, IEnumerable<IOptionsConnector> _oConnectors)
        {
            _testBLConnector = _bConnectors.Where(c => c.ConnectorType == JXTNext.Sitefinity.Connector.IntegrationConnectorType.Test).FirstOrDefault();
            _testOConnector = _oConnectors.Where(c => c.ConnectorType == JXTNext.Sitefinity.Connector.IntegrationConnectorType.Test).FirstOrDefault();
        }

        // GET: JobDetails
        public ActionResult Index(string jobId)
        {
            dynamic dynamicJobDetails = null;
            IGetJobListingRequest jobListingRequest = new Test_GetJobListingRequest { JobID = jobId };
            IGetJobListingResponse jobListingResponse = _testBLConnector.AdvertiserGetJob(jobListingRequest);

            if(jobListingRequest != null)
                dynamicJobDetails = jobListingResponse as dynamic;

            string userName = String.Empty;
            List<string> roles = new List<string>();
            GetCurrentUserInfo(ref userName, ref roles);

            ViewBag.UserName = userName;
            ViewBag.Roles = JsonConvert.SerializeObject(roles);

            return View("Simple", dynamicJobDetails);
        }

        static void GetCurrentUserInfo(ref string userName, ref List<string> roles)
        {
            // Get the current identity 
            var identity = ClaimsManager.GetCurrentIdentity();

            // Get information about the user from the properties of the ClaimsIdentityProxy object
            if (identity != null)
            {
                userName = identity.Name;
                foreach (var rolesInfo in identity.Roles)
                {
                    roles.Add(rolesInfo.Name);
                }
            }
        }

        internal const string WidgetIconCssClass = "sfMvcIcn";
    }
}