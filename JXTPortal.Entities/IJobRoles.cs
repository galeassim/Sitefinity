﻿using System;
using System.ComponentModel;

namespace JXTPortal.Entities
{
	/// <summary>
	///		The data structure representation of the 'JobRoles' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IJobRoles 
	{
		/// <summary>			
		/// JobRoleID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "JobRoles"</remarks>
		System.Int32 JobRoleId { get; set; }
				
		
		
		/// <summary>
		/// JobID : 
		/// </summary>
		System.Int32?  JobId  { get; set; }
		
		/// <summary>
		/// JobArchiveID : 
		/// </summary>
		System.Int32?  JobArchiveId  { get; set; }
		
		/// <summary>
		/// RoleID : 
		/// </summary>
		System.Int32?  RoleId  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		#endregion Data Properties

	}
}

