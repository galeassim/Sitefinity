﻿using System;
using System.ComponentModel;

namespace JXTPortal.Entities
{
	/// <summary>
	///		The data structure representation of the 'ScreeningQuestionsTemplates' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IScreeningQuestionsTemplates 
	{
		/// <summary>			
		/// ScreeningQuestionsTemplateId : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "ScreeningQuestionsTemplates"</remarks>
		System.Int32 ScreeningQuestionsTemplateId { get; set; }
				
		
		
		/// <summary>
		/// TemplateName : 
		/// </summary>
		System.String  TemplateName  { get; set; }
		
		/// <summary>
		/// SiteId : 
		/// </summary>
		System.Int32  SiteId  { get; set; }
		
		/// <summary>
		/// Visible : 
		/// </summary>
		System.Boolean  Visible  { get; set; }
		
		/// <summary>
		/// LastModified : 
		/// </summary>
		System.DateTime?  LastModified  { get; set; }
		
		/// <summary>
		/// LastModifiedBy : 
		/// </summary>
		System.Int32?  LastModifiedBy  { get; set; }
		
		/// <summary>
		/// CreatedByAdvertiserId : 
		/// </summary>
		System.Int32?  CreatedByAdvertiserId  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _jobsScreeningQuestionsTemplateId
		/// </summary>	
		TList<Jobs> JobsCollection {  get;  set;}	


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _screeningQuestionsMappingsScreeningQuestionsTemplateId
		/// </summary>	
		TList<ScreeningQuestionsMappings> ScreeningQuestionsMappingsCollection {  get;  set;}	


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _screeningQuestionsTemplateOwnersScreeningQuestionsTemplateId
		/// </summary>	
		TList<ScreeningQuestionsTemplateOwners> ScreeningQuestionsTemplateOwnersCollection {  get;  set;}	

		#endregion Data Properties

	}
}

