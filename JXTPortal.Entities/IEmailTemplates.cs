﻿using System;
using System.ComponentModel;

namespace JXTPortal.Entities
{
	/// <summary>
	///		The data structure representation of the 'EmailTemplates' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IEmailTemplates 
	{
		/// <summary>			
		/// EmailTemplateID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "EmailTemplates"</remarks>
		System.Int32 EmailTemplateId { get; set; }
				
		
		
		/// <summary>
		/// SiteID : 
		/// </summary>
		System.Int32  SiteId  { get; set; }
		
		/// <summary>
		/// EmailTemplateParentID : 
		/// </summary>
		System.Int32  EmailTemplateParentId  { get; set; }
		
		/// <summary>
		/// EmailCode : 
		/// </summary>
		System.String  EmailCode  { get; set; }
		
		/// <summary>
		/// EmailDescription : 
		/// </summary>
		System.String  EmailDescription  { get; set; }
		
		/// <summary>
		/// EmailSubject : 
		/// </summary>
		System.String  EmailSubject  { get; set; }
		
		/// <summary>
		/// EmailBodyText : 
		/// </summary>
		System.String  EmailBodyText  { get; set; }
		
		/// <summary>
		/// EmailBodyHTML : 
		/// </summary>
		System.String  EmailBodyHtml  { get; set; }
		
		/// <summary>
		/// EmailFields : 
		/// </summary>
		System.String  EmailFields  { get; set; }
		
		/// <summary>
		/// EmailAddressName : 
		/// </summary>
		System.String  EmailAddressName  { get; set; }
		
		/// <summary>
		/// EmailAddressFrom : 
		/// </summary>
		System.String  EmailAddressFrom  { get; set; }
		
		/// <summary>
		/// EmailAddressCC : 
		/// </summary>
		System.String  EmailAddressCc  { get; set; }
		
		/// <summary>
		/// EmailAddressBCC : 
		/// </summary>
		System.String  EmailAddressBcc  { get; set; }
		
		/// <summary>
		/// GlobalTemplate : 
		/// </summary>
		System.Boolean  GlobalTemplate  { get; set; }
		
		/// <summary>
		/// LastModifiedBy : 
		/// </summary>
		System.Int32  LastModifiedBy  { get; set; }
		
		/// <summary>
		/// LastModified : 
		/// </summary>
		System.DateTime  LastModified  { get; set; }
		
		/// <summary>
		/// EmailAddressTo : 
		/// </summary>
		System.String  EmailAddressTo  { get; set; }
		
		/// <summary>
		/// EmailAddressToName : 
		/// </summary>
		System.String  EmailAddressToName  { get; set; }
		
		/// <summary>
		/// EmailAddressToMandatory : 
		/// </summary>
		System.Boolean?  EmailAddressToMandatory  { get; set; }
		
		/// <summary>
		/// LanguageID : 
		/// </summary>
		System.Int32?  LanguageId  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		#endregion Data Properties

	}
}

