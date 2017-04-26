﻿using System;
using System.ComponentModel;

namespace JXTPortal.Entities
{
	/// <summary>
	///		The data structure representation of the 'ScreeningQuestions' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IScreeningQuestions 
	{
		/// <summary>			
		/// ScreeningQuestionId : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "ScreeningQuestions"</remarks>
		System.Int32 ScreeningQuestionId { get; set; }
				
		
		
		/// <summary>
		/// ScreeningQuestionIndex : 
		/// </summary>
		System.Int32  ScreeningQuestionIndex  { get; set; }
		
		/// <summary>
		/// QuestionTitle : 
		/// </summary>
		System.String  QuestionTitle  { get; set; }
		
		/// <summary>
		/// QuestionType : 
		/// </summary>
		System.Int32  QuestionType  { get; set; }
		
		/// <summary>
		/// Mandatory : 
		/// </summary>
		System.Boolean  Mandatory  { get; set; }
		
		/// <summary>
		/// LanguageId : 
		/// </summary>
		System.Int32  LanguageId  { get; set; }
		
		/// <summary>
		/// KnockoutValue : 
		/// </summary>
		System.String  KnockoutValue  { get; set; }
		
		/// <summary>
		/// Options : 
		/// </summary>
		System.String  Options  { get; set; }
		
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
		/// LastModifiedByAdvertiserUserId : 
		/// </summary>
		System.Int32?  LastModifiedByAdvertiserUserId  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _jobApplicationScreeningAnswersScreeningQuestionId
		/// </summary>	
		TList<JobApplicationScreeningAnswers> JobApplicationScreeningAnswersCollection {  get;  set;}	


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _screeningQuestionsMappingsScreeningQuestionId
		/// </summary>	
		TList<ScreeningQuestionsMappings> ScreeningQuestionsMappingsCollection {  get;  set;}	


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _jobScreeningQuestionsScreeningQuestionId
		/// </summary>	
		TList<JobScreeningQuestions> JobScreeningQuestionsCollection {  get;  set;}	

		#endregion Data Properties

	}
}

