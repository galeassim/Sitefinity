﻿using System;
using System.ComponentModel;

namespace JXTPortal.Entities
{
	/// <summary>
	///		The data structure representation of the 'DynamicContent' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IDynamicContent 
	{
		/// <summary>			
		/// DynamicContentID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "DynamicContent"</remarks>
		System.Int32 DynamicContentId { get; set; }
				
		
		
		/// <summary>
		/// SiteID : 
		/// </summary>
		System.Int32  SiteId  { get; set; }
		
		/// <summary>
		/// LanguageID : 
		/// </summary>
		System.Int32  LanguageId  { get; set; }
		
		/// <summary>
		/// DynamicContentType : 
		/// </summary>
		System.Int32  DynamicContentType  { get; set; }
		
		/// <summary>
		/// Title : 
		/// </summary>
		System.String  Title  { get; set; }
		
		/// <summary>
		/// Introduction : 
		/// </summary>
		System.String  Introduction  { get; set; }
		
		/// <summary>
		/// Description : 
		/// </summary>
		System.String  Description  { get; set; }
		
		/// <summary>
		/// LastModifiedBy : 
		/// </summary>
		System.Int32?  LastModifiedBy  { get; set; }
		
		/// <summary>
		/// LastModifiedDate : 
		/// </summary>
		System.DateTime  LastModifiedDate  { get; set; }
		
		/// <summary>
		/// Active : 
		/// </summary>
		System.Boolean  Active  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		#endregion Data Properties

	}
}

