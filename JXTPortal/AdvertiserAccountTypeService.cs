﻿	

#region Using Directives
using System;
using System.ComponentModel;
using System.Collections;
using System.Xml.Serialization;
using System.Data;
using System.Linq;
using JXTPortal.Entities;
using JXTPortal.Entities.Validation;

using JXTPortal.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using JXTPortal.Common;
using System.Configuration;
using System.Collections.Generic;

#endregion

namespace JXTPortal
{		
	/// <summary>
	/// An component type implementation of the 'AdvertiserAccountType' table.
	/// </summary>
	/// <remarks>
	/// All custom implementations should be done here.
	/// </remarks>
	[CLSCompliant(true)]
	public partial class AdvertiserAccountTypeService : JXTPortal.AdvertiserAccountTypeServiceBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the AdvertiserAccountTypeService class.
		/// </summary>
		public AdvertiserAccountTypeService() : base()
		{
		}
		#endregion Constructors

        #region "Methods"

        public List<AdvertiserAccountType> GetTranslatedAdvertiserAccountType(int languageID)
        {
            string xmlprefix = "{0}{2}_{1}.xml";
            string url = string.Format(xmlprefix,
                                        ConfigurationManager.AppSettings["XMLFilesPath"],
                                        SessionData.Language.LanguageId,
                                        PortalConstants.XMLTranslationFiles.XML_ADVERTISERACCOUNTTYPE_FILENAME);

            return XMLLanguageService.Translate(GetAll().ToList(), "AdvertiserAccountTypeId", "AdvertiserAccountTypeName", url);

        }

        #endregion


    }//End Class

} // end namespace