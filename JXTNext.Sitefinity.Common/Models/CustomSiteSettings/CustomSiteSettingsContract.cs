﻿using System;
using System.Runtime.Serialization;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.SiteSettings;

namespace JXTNext.Sitefinity.Common.Models.CustomSiteSettings
{
    [DataContract]
    public class CustomSiteSettingsContract : ISettingsDataContract
    {
        #region DataMembers
        [DataMember]
        public string GoogleAPIKey
        {
            get;
            set;
        }

        [DataMember]
        public string GoogleClientId
        {
            get;
            set;
        }

        [DataMember]
        public string GoogleClientSecret
        {
            get;
            set;
        }

        [DataMember]
        public string GoogleClientAPIKey
        {
            get;
            set;
        }

        [DataMember]
        public string GoogleTagManagerKey
        {
            get;
            set;
        }

        [DataMember]
        public string DropboxAppId
        {
            get;
            set;
        }

        [DataMember]
        public string DropboxAppSecret
        {
            get;
            set;
        }

        [DataMember]
        public string DropboxClientAPIKey
        {
            get;
            set;
        }
        #endregion

        #region LoadDefaults
        public void LoadDefaults(bool forEdit = false)
        {
            CustomSiteSettingsConfig section;
            if (forEdit)
                section = ConfigManager.GetManager().GetSection<CustomSiteSettingsConfig>();
            else
                section = Config.Get<CustomSiteSettingsConfig>();

            this.GoogleAPIKey = section.UICustomSiteSettings.CurrentGoogleAPIKey;
            this.GoogleClientId = section.UICustomSiteSettings.CurrentGoogleClientId;
            this.GoogleClientSecret = section.UICustomSiteSettings.CurrentGoogleClientSecret;
            this.GoogleClientAPIKey = section.UICustomSiteSettings.CurrentGoogleClientAPIKey;
            this.GoogleTagManagerKey = section.UICustomSiteSettings.CurrentGoogleTagManagerKey;

            this.DropboxAppId = section.UICustomSiteSettings.CurrentDropboxAppId;
            this.DropboxAppSecret = section.UICustomSiteSettings.CurrentDropboxAppSecret;
            this.DropboxClientAPIKey = section.UICustomSiteSettings.CurrentDropboxClientAPIKey;
        }

        public void SaveDefaults()
        {
            var manager = ConfigManager.GetManager();
            var section = manager.GetSection<CustomSiteSettingsConfig>();

            section.UICustomSiteSettings.CurrentGoogleAPIKey = this.GoogleAPIKey;
            section.UICustomSiteSettings.CurrentGoogleClientId = this.GoogleClientId;
            section.UICustomSiteSettings.CurrentGoogleClientSecret = this.GoogleClientSecret;
            section.UICustomSiteSettings.CurrentGoogleClientAPIKey = this.GoogleClientAPIKey;
            section.UICustomSiteSettings.CurrentGoogleTagManagerKey = this.GoogleTagManagerKey;

            section.UICustomSiteSettings.CurrentDropboxAppId = this.DropboxAppId;
            section.UICustomSiteSettings.CurrentDropboxAppSecret = this.DropboxAppSecret;
            section.UICustomSiteSettings.CurrentDropboxClientAPIKey = this.DropboxClientAPIKey;

            manager.SaveSection(section);
        }
        #endregion
    }
}