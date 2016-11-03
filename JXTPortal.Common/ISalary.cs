﻿using System;
using System.ComponentModel;

namespace JXTPortal.Entities
{
	/// <summary>
	///		The data structure representation of the 'Salary' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface ISalary 
	{
		/// <summary>			
		/// SalaryID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "Salary"</remarks>
		System.Int32 SalaryId { get; set; }
				
		
		
		/// <summary>
		/// SalaryTypeID : 
		/// </summary>
		System.Int32  SalaryTypeId  { get; set; }
		
		/// <summary>
		/// SalaryName : 
		/// </summary>
		System.String  SalaryName  { get; set; }
		
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


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _siteSalarySalaryId
		/// </summary>	
		TList<SiteSalary> SiteSalaryCollection {  get;  set;}	

		#endregion Data Properties

	}
}

