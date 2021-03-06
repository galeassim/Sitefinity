﻿using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace JXTNext.Sitefinity.Widgets.Social.Mvc.StringResources
{
    [ObjectInfo(typeof(SocialHandlerResources), ResourceClassId = "SocialHandlerResources", Title = "SocialHandlerResourcesTitle", Description = "SocialHandlerResourcesDescription")]
    public class SocialHandlerResources : Resource
    {
        #region Constructions

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialHandlerResources" /> class.
        /// </summary>
        public SocialHandlerResources()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialHandlerResources" /> class.
        /// </summary>
        /// <param name="dataProvider">The data provider.</param>
        public SocialHandlerResources(ResourceDataProvider dataProvider)
            : base(dataProvider)
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets phrase: Where to display search results?
        /// </summary>
        [ResourceEntry("Template",
            Value = "Template",
            Description = "phrase : Template")]
        public string Template
        {
            get
            {
                return this["Template"];
            }
        }

        /// <summary>
        /// Gets phrase: Where to display details
        /// </summary>
        [ResourceEntry("DisplaySuccess",
            Value = "Where to redirect to the job application success?",
            Description = "phrase : Where to redirect to the job application success")]
        public string DisplaySuccess
        {
            get
            {
                return this["DisplaySuccess"];
            }
        }

        /// <summary>
        /// Gets phrase: Where to display search results?
        /// </summary>
        [ResourceEntry("SearchResultsPage",
            Value = "Where to redirect to the job search results?",
            Description = "phrase : Where to redirect to the job search results?")]
        public string SearchResultsPage
        {
            get
            {
                return this["SearchResultsPage"];
            }
        }

        /// <summary>
        /// Gets phrase: Where to display details
        /// </summary>
        [ResourceEntry("SelectSuccessPage",
            Value = "Select Success Page",
            Description = "phrase : Select Success Page")]
        public string SelectSuccessPage
        {
            get
            {
                return this["SelectSuccessPage"];
            }
        }

        /// <summary>
        /// Gets phrase: Where to search results
        /// </summary>
        [ResourceEntry("SelectResultsPage",
            Value = "Select Job Restults Page",
            Description = "phrase : Select Job Restults Page")]
        public string SelectResultsPage
        {
            get
            {
                return this["SelectResultsPage"];
            }
        }

        /// <summary>
        /// Gets phrase: This is the page where you have dropped search results widget
        /// </summary>
        [ResourceEntry("EmailCC",
            Value = "Enter CC E-mail list separated by semicolon:",
            Description = "phrase : Enter CC E-mail list separated by semicolon")]
        public string EmailCC
        {
            get
            {
                return this["EmailCC"];
            }
        }

        /// <summary>
        /// Gets phrase: Where to display search results?
        /// </summary>
        [ResourceEntry("EmailBCC",
            Value = "Enter BCC E-mail list separated by semicolon:",
            Description = "phrase : Enter BCC E-mail list separated by semicolon")]
        public string EmailBCC
        {
            get
            {
                return this["EmailBCC"];
            }
        }

        /// <summary>
        /// Gets phrase: This is the page where you have dropped search results widget
        /// </summary>
        [ResourceEntry("SenderName",
            Value = "Enter sender name:",
            Description = "phrase : Enter from sender name")]
        public string SenderName
        {
            get
            {
                return this["SenderName"];
            }
        }

        /// <summary>
        /// Gets phrase: This is the page where you have dropped search results widget
        /// </summary>
        [ResourceEntry("SenderEmailAddress",
            Value = "Enter sender E-mail address:",
            Description = "phrase : Enter sender email address")]
        public string SenderEmailAddress
        {
            get
            {
                return this["SenderEmailAddress"];
            }
        }

        /// <summary>
        /// Gets phrase: This is the page where you have dropped search results widget
        /// </summary>
        [ResourceEntry("EmailSubject",
            Value = "Enter E-mail subject:",
            Description = "phrase : Enter email subject")]
        public string EmailSubject
        {
            get
            {
                return this["EmailSubject"];
            }
        }

        [ResourceEntry("EmailTemplateIdResource",
            Value = "Enter E-mail subject:",
            Description = "phrase : Enter email subject")]
        public string EmailTemplateIdResource
        {
            get
            {
                return this["EmailTemplateIdResource"];
            }
        }

        [ResourceEntry("SelectAdvertiserEmailTemplate",
            Value = "Select Advertiser Email Template:",
            Description = "phrase : Select Advertiser Email Template")]
        public string SelectAdvertiserEmailTemplate
        {
            get
            {
                return this["SelectAdvertiserEmailTemplate"];
            }
        }
        
        [ResourceEntry("AdvertiserEmailTemplateIdResource",
            Value = "Advertiser Email Template Id:",
            Description = "phrase : Advertiser Email Template Id")]
        public string AdvertiserEmailTemplateIdResource
        {
            get
            {
                return this["AdvertiserEmailTemplateIdResource"];
            }
        }

        /// <summary>
        /// Gets phrase: This is the page where you have dropped search results widget
        /// </summary>
        [ResourceEntry("SelectEmailTemplate",
            Value = "Select E-mail template",
            Description = "phrase : Select E-mail template")]
        public string SelectEmailTemplate
        {
            get
            {
                return this["SelectEmailTemplate"];
            }
        }

        /// <summary>
        /// Gets phrase : CSS classes
        /// </summary>
        [ResourceEntry("CssClasses",
            Value = "CSS classes",
            Description = "phrase : CSS classes")]
        public string CssClasses
        {
            get
            {
                return this["CssClasses"];
            }
        }

        [ResourceEntry("SelectRegistrationEmailTemplate",
            Value = "Select Registration E-mail template",
            Description = "phrase : Select Registration E-mail template")]
        public string SelectRegistrationEmailTemplate
        {
            get
            {
                return this["SelectRegistrationEmailTemplate"];
            }
        }

        /// <summary>
        /// Gets phrase: This is the page where you have dropped search results widget
        /// </summary>
        [ResourceEntry("AdvertiserEmailCC",
            Value = "Enter Advertiser CC E-mail list separated by semicolon:",
            Description = "phrase : Enter Advertiser CC E-mail list separated by semicolon")]
        public string AdvertiserEmailCC
        {
            get
            {
                return this["AdvertiserEmailCC"];
            }
        }

        /// <summary>
        /// Gets phrase: Where to display search results?
        /// </summary>
        [ResourceEntry("AdvertiserEmailBCC",
            Value = "Enter Advertiser BCC E-mail list separated by semicolon:",
            Description = "phrase : Enter Advertiser BCC E-mail list separated by semicolon")]
        public string AdvertiserEmailBCC
        {
            get
            {
                return this["AdvertiserEmailBCC"];
            }
        }

        /// <summary>
        /// Gets phrase: This is the page where you have dropped search results widget
        /// </summary>
        [ResourceEntry("RegistrationEmailCC",
            Value = "Enter Registration CC E-mail list separated by semicolon:",
            Description = "phrase : Enter Registration CC E-mail list separated by semicolon")]
        public string RegistrationEmailCC
        {
            get
            {
                return this["RegistrationEmailCC"];
            }
        }

        /// <summary>
        /// Gets phrase: Where to display search results?
        /// </summary>
        [ResourceEntry("RegistrationEmailBCC",
            Value = "Enter Registration BCC E-mail list separated by semicolon:",
            Description = "phrase : Enter Registration BCC E-mail list separated by semicolon")]
        public string RegistrationEmailBCC
        {
            get
            {
                return this["RegistrationEmailBCC"];
            }
        }


        #endregion
    }
}
