﻿using System;
using System.ComponentModel;

namespace JXTPortal.Entities
{
	/// <summary>
	///		The data structure representation of the 'AdvertiserAccountType' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IAdvertiserAccountType 
	{
		/// <summary>			
		/// AdvertiserAccountTypeID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "AdvertiserAccountType"</remarks>
		System.Int32 AdvertiserAccountTypeId { get; set; }
				
		
		
		/// <summary>
		/// AdvertiserAccountTypeName : 
		/// </summary>
		System.String  AdvertiserAccountTypeName  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _advertisersAdvertiserAccountTypeId
		/// </summary>	
		TList<Advertisers> AdvertisersCollection {  get;  set;}	

		#endregion Data Properties

	}
}

