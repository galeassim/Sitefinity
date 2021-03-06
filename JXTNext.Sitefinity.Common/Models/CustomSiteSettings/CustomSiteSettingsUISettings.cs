﻿using System;
using System.Configuration;
using System.Runtime.Serialization;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Web.Configuration;

namespace JXTNext.Sitefinity.Common.Models.CustomSiteSettings
{
    [ObjectInfo(typeof(ConfigDescriptions), Title = "UICustomSiteSettingsConfigDescriptions", Description = "UICustomSiteSettingsConfigDescriptions")]
    public class CustomSiteSettingsUISettings : ConfigElement
    {
        public CustomSiteSettingsUISettings(ConfigElement parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Gets or sets the name of the time zone.
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("googleAPIKey")]
        [DescriptionResource(typeof(ConfigDescriptions), "GoogleAPIKey")]

        [DataMember]
        public virtual String CurrentGoogleAPIKey
        {
            get
            {
                return (String)this["googleAPIKey"];
            }
            set
            {
                this["googleAPIKey"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Google Client ID
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("googleClientId")]
        [DescriptionResource(typeof(ConfigDescriptions), "GoogleClientId")]

        [DataMember]
        public virtual String CurrentGoogleClientId
        {
            get
            {
                return (String)this["googleClientId"];
            }
            set
            {
                this["googleClientId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Google Client Secret
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("googleClientSecret")]
        [DescriptionResource(typeof(ConfigDescriptions), "GoogleClientSecret")]

        [DataMember]
        public virtual String CurrentGoogleClientSecret
        {
            get
            {
                return (String)this["googleClientSecret"];
            }
            set
            {
                this["googleClientSecret"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Google Client API Key
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("googleClientAPIKey")]
        [DescriptionResource(typeof(ConfigDescriptions), "GoogleClientAPIKey")]

        [DataMember]
        public virtual String CurrentGoogleClientAPIKey
        {
            get
            {
                return (String)this["googleClientAPIKey"];
            }
            set
            {
                this["googleClientAPIKey"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Google TagManager Key
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("googleTagManagerKey")]
        [DescriptionResource(typeof(ConfigDescriptions), "GoogleTagManagerKey")]

        [DataMember]
        public virtual String CurrentGoogleTagManagerKey
        {
            get
            {
                return (String)this["googleTagManagerKey"];
            }
            set
            {
                this["googleTagManagerKey"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Dropbox Client ID
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("dropboxAppId")]
        [DescriptionResource(typeof(ConfigDescriptions), "DropboxAppId")]

        [DataMember]
        public virtual String CurrentDropboxAppId
        {
            get
            {
                return (String)this["dropboxAppId"];
            }
            set
            {
                this["dropboxAppId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Google Client Secret
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("dropboxAppSecret")]
        [DescriptionResource(typeof(ConfigDescriptions), "DropboxAppSecret")]

        [DataMember]
        public virtual String CurrentDropboxAppSecret
        {
            get
            {
                return (String)this["dropboxAppSecret"];
            }
            set
            {
                this["dropboxAppSecret"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Dropbox Client API Key
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("dropboxClientAPIKey")]
        [DescriptionResource(typeof(ConfigDescriptions), "DropboxClientAPIKey")]

        [DataMember]
        public virtual String CurrentDropboxClientAPIKey
        {
            get
            {
                return (String)this["dropboxClientAPIKey"];
            }
            set
            {
                this["dropboxClientAPIKey"] = value;
            }
        }

        #region Seek

        /// <summary>
        /// Gets or sets the Dropbox Client API Key
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("seekClientId")]
        [DescriptionResource(typeof(ConfigDescriptions), "SeekClientId")]

        [DataMember]
        public virtual String CurrentSeekClientId
        {
            get
            {
                return (String)this["seekClientId"];
            }
            set
            {
                this["seekClientId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Dropbox Client API Key
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("seekClientSecret")]
        [DescriptionResource(typeof(ConfigDescriptions), "SeekClientSecret")]

        [DataMember]
        public virtual String CurrentSeekClientSecret
        {
            get
            {
                return (String)this["seekClientSecret"];
            }
            set
            {
                this["seekClientSecret"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Dropbox Client API Key
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("seekClientAdvertiserId")]
        [DescriptionResource(typeof(ConfigDescriptions), "SeekClientAdvertiserId")]

        [DataMember]
        public virtual String CurrentSeekClientAdvertiserId
        {
            get
            {
                return (String)this["seekClientAdvertiserId"];
            }
            set
            {
                this["seekClientAdvertiserId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Dropbox Client API Key
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("seekRedirectUri")]
        [DescriptionResource(typeof(ConfigDescriptions), "SeekRedirectUri")]

        [DataMember]
        public virtual String CurrentSeekRedirectUri
        {
            get
            {
                return (String)this["seekRedirectUri"];
            }
            set
            {
                this["seekRedirectUri"] = value;
            }
        }

        #endregion

        #region Indeed
        /// <summary>
        /// Gets or sets the Indeed API Token
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("indeedClientAPIToken")]
        [DescriptionResource(typeof(ConfigDescriptions), "IndeedClientAPIToken")]

        [DataMember]
        public virtual String CurrentIndeedClientAPIToken
        {
            get
            {
                return (String)this["indeedClientAPIToken"];
            }
            set
            {
                this["indeedClientAPIToken"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Indeed Client Secret
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("indeedClientSecret")]
        [DescriptionResource(typeof(ConfigDescriptions), "IndeedClientSecret")]

        [DataMember]
        public virtual String CurrentIndeedClientSecret
        {
            get
            {
                return (String)this["indeedClientSecret"];
            }
            set
            {
                this["indeedClientSecret"] = value;
            }
        }
        #endregion

        #region Instagram
        /// <summary>
        /// Gets or sets the Instagram API Token
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("instagramClientIdToken")]
        [DescriptionResource(typeof(ConfigDescriptions), "InstagramClientIdToken")]

        [DataMember]
        public virtual String CurrentInstagramClientIdToken
        {
            get
            {
                return (String)this["instagramClientIdToken"];
            }
            set
            {
                this["instagramClientIdToken"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Instagram Client Secret
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("instagramClientSecret")]
        [DescriptionResource(typeof(ConfigDescriptions), "InstagramClientSecret")]

        [DataMember]
        public virtual String CurrentInstagramClientSecret
        {
            get
            {
                return (String)this["instagramClientSecret"];
            }
            set
            {
                this["instagramClientSecret"] = value;
            }
        }

        [ConfigurationProperty("instagramAccessToken")]
        [DescriptionResource(typeof(ConfigDescriptions), "InstagramAccessToken")]

        [DataMember]
        public virtual String CurrentInstagramAccessToken
        {
            get
            {
                return (String)this["instagramAccessToken"];
            }
            set
            {
                this["instagramAccessToken"] = value;
            }
        }

        [ConfigurationProperty("instagramExpiration")]
        [DescriptionResource(typeof(ConfigDescriptions), "InstagramExpiration")]

        [DataMember]
        public virtual int CurrentInstagramExpiration
        {
            get
            {
                return (int)this["instagramExpiration"];
            }
            set
            {
                this["instagramExpiration"] = value;
            }
        }

        [ConfigurationProperty("instagramMaxItems")]
        [DescriptionResource(typeof(ConfigDescriptions), "InstagramMaxItems")]

        [DataMember]
        public virtual int CurrentInstagramMaxItems
        {
            get
            {
                return (int)this["instagramMaxItems"];
            }
            set
            {
                this["instagramMaxItems"] = value;
            }
        }
        #endregion

        #region Amazon S3 Bucket
        /// <summary>
        /// Gets or sets the Instagram API Token
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("amazonS3AccessKeyId")]
        [DescriptionResource(typeof(ConfigDescriptions), "AmazonS3AccessKeyId")]

        [DataMember]
        public virtual String CurrentAmazonS3AccessKeyId
        {
            get
            {
                return (String)this["amazonS3AccessKeyId"];
            }
            set
            {
                this["amazonS3AccessKeyId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Instagram Client Secret
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("amazonS3SecretKey")]
        [DescriptionResource(typeof(ConfigDescriptions), "AmazonS3SecretKey")]

        [DataMember]
        public virtual String CurrentAmazonS3SecretKey
        {
            get
            {
                return (String)this["amazonS3SecretKey"];
            }
            set
            {
                this["amazonS3SecretKey"] = value;
            }
        }

        [ConfigurationProperty("amazonS3BucketName")]
        [DescriptionResource(typeof(ConfigDescriptions), "AmazonS3BucketName")]

        [DataMember]
        public virtual String CurrentAmazonS3BucketName
        {
            get
            {
                return (String)this["amazonS3BucketName"];
            }
            set
            {
                this["amazonS3BucketName"] = value;
            }
        }

        [ConfigurationProperty("amazonS3RegionEndpoint")]
        [DescriptionResource(typeof(ConfigDescriptions), "AmazonS3RegionEndpoint")]

        [DataMember]
        public virtual String CurrentAmazonS3RegionEndpoint
        {
            get
            {
                return (String)this["amazonS3RegionEndpoint"];
            }
            set
            {
                this["amazonS3RegionEndpoint"] = value;
            }
        }

        [ConfigurationProperty("amazonS3ProviderName")]
        [DescriptionResource(typeof(ConfigDescriptions), "AmazonS3ProviderName")]

        [DataMember]
        public virtual String CurrentAmazonS3ProviderName
        {
            get
            {
                return (String)this["amazonS3ProviderName"];
            }
            set
            {
                this["amazonS3ProviderName"] = value;
            }
        }


        [ConfigurationProperty("amazonS3ApplicationName")]
        [DescriptionResource(typeof(ConfigDescriptions), "AmazonS3ApplicationName")]

        [DataMember]
        public virtual String CurrentAmazonS3ApplicationName
        {
            get
            {
                return (String)this["amazonS3ApplicationName"];
            }
            set
            {
                this["amazonS3ApplicationName"] = value;
            }
        }

        [ConfigurationProperty("amazonS3UrlName")]
        [DescriptionResource(typeof(ConfigDescriptions), "AmazonS3UrlName")]

        [DataMember]
        public virtual String CurrentAmazonS3UrlName
        {
            get
            {
                return (String)this["amazonS3UrlName"];
            }
            set
            {
                this["amazonS3UrlName"] = value;
            }
        }


        #endregion

        #region LinkedIn
        /// <summary>
        /// Gets or sets the LinkedIn client id
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("linkedInClientId")]
        [DescriptionResource(typeof(ConfigDescriptions), "LinkedInClientId")]
        [DataMember]
        public virtual String CurrentLinkedInClientId
        {
            get
            {
                return (String)this["linkedInClientId"];
            }
            set
            {
                this["linkedInClientId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the LinkedIn client secret.
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("linkedInClientSecret")]
        [DescriptionResource(typeof(ConfigDescriptions), "LinkedInClientSecret")]
        [DataMember]
        public virtual String CurrentLinkedInClientSecret
        {
            get
            {
                return (String)this["linkedInClientSecret"];
            }
            set
            {
                this["linkedInClientSecret"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the LinkedIn customer client id.
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("linkedInCustomerClientId")]
        [DescriptionResource(typeof(ConfigDescriptions), "LinkedInCustomerClientId")]
        [DataMember]
        public virtual String CurrentLinkedInCustomerClientId
        {
            get
            {
                return (String)this["linkedInCustomerClientId"];
            }
            set
            {
                this["linkedInCustomerClientId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the LinkedIn customer client secret.
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("linkedInCustomerClientSecret")]
        [DescriptionResource(typeof(ConfigDescriptions), "LinkedInCustomerClientSecret")]
        [DataMember]
        public virtual String CurrentLinkedInCustomerClientSecret
        {
            get
            {
                return (String)this["linkedInCustomerClientSecret"];
            }
            set
            {
                this["linkedInCustomerClientSecret"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the LinkedIn customer integration context.
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("linkedInCustomerIntegrationContext")]
        [DescriptionResource(typeof(ConfigDescriptions), "LinkedInCustomerIntegrationContext")]
        [DataMember]
        public virtual String CurrentLinkedInCustomerIntegrationContext
        {
            get
            {
                return (String)this["linkedInCustomerIntegrationContext"];
            }
            set
            {
                this["linkedInCustomerIntegrationContext"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the LinkedIn social handler url.
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("linkedInSocialHandlerUrl")]
        [DescriptionResource(typeof(ConfigDescriptions), "LinkedInSocialHandlerUrl")]
        [DataMember]
        public virtual String CurrentLinkedInSocialHandlerUrl
        {
            get
            {
                return (String)this["linkedInSocialHandlerUrl"];
            }
            set
            {
                this["linkedInSocialHandlerUrl"] = value;
            }
        }
        #endregion

        [ConfigurationProperty("cultureIsEnabled")]
        [DescriptionResource(typeof(ConfigDescriptions), "CultureIsEnabled")]

        [DataMember]
        public virtual String CurrentCultureIsEnabled
        {
            get
            {
                return (String)this["cultureIsEnabled"];
            }
            set
            {
                this["cultureIsEnabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the time zone.
        /// </summary>
        /// <value>The name of the time zone.</value>
        [ConfigurationProperty("jobCurrencySymbol")]
        [DescriptionResource(typeof(ConfigDescriptions), "JobCurrencySymbol")]
        [DataMember]
        public virtual String JobCurrencySymbol
        {
            get
            {
                return (String)this["jobCurrencySymbol"];
            }
            set
            {
                this["jobCurrencySymbol"] = value;
            }
        }
    }
}