﻿using System;
using System.ComponentModel;

namespace JXTPortal.Entities
{
	/// <summary>
	///		The data structure representation of the 'SiteRoles' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface ISiteRoles 
	{
		/// <summary>			
		/// SiteRoleID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "SiteRoles"</remarks>
		System.Int32 SiteRoleId { get; set; }
				
		
		
		/// <summary>
		/// SiteID : 
		/// </summary>
		System.Int32  SiteId  { get; set; }
		
		/// <summary>
		/// RoleID : 
		/// </summary>
		System.Int32  RoleId  { get; set; }
		
		/// <summary>
		/// SiteRoleName : 
		/// </summary>
		System.String  SiteRoleName  { get; set; }
		
		/// <summary>
		/// Valid : 
		/// </summary>
		System.Boolean  Valid  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		#endregion Data Properties

	}
}

