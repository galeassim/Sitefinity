﻿using System;
using System.ComponentModel;

namespace JXTPortal.Entities
{
	/// <summary>
	///		The data structure representation of the 'Consultants' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IConsultants 
	{
		/// <summary>			
		/// ConsultantID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "Consultants"</remarks>
		System.Int32 ConsultantId { get; set; }
				
		
		
		/// <summary>
		/// SiteID : 
		/// </summary>
		System.Int32  SiteId  { get; set; }
		
		/// <summary>
		/// LanguageID : 
		/// </summary>
		System.Int32?  LanguageId  { get; set; }
		
		/// <summary>
		/// Title : 
		/// </summary>
		System.String  Title  { get; set; }
		
		/// <summary>
		/// FirstName : 
		/// </summary>
		System.String  FirstName  { get; set; }
		
		/// <summary>
		/// Email : 
		/// </summary>
		System.String  Email  { get; set; }
		
		/// <summary>
		/// Phone : 
		/// </summary>
		System.String  Phone  { get; set; }
		
		/// <summary>
		/// Mobile : 
		/// </summary>
		System.String  Mobile  { get; set; }
		
		/// <summary>
		/// PositionTitle : 
		/// </summary>
		System.String  PositionTitle  { get; set; }
		
		/// <summary>
		/// OfficeLocation : 
		/// </summary>
		System.String  OfficeLocation  { get; set; }
		
		/// <summary>
		/// Categories : 
		/// </summary>
		System.String  Categories  { get; set; }
		
		/// <summary>
		/// Location : 
		/// </summary>
		System.String  Location  { get; set; }
		
		/// <summary>
		/// FriendlyURL : 
		/// </summary>
		System.String  FriendlyUrl  { get; set; }
		
		/// <summary>
		/// ShortDescription : 
		/// </summary>
		System.String  ShortDescription  { get; set; }
		
		/// <summary>
		/// Testimonial : 
		/// </summary>
		System.String  Testimonial  { get; set; }
		
		/// <summary>
		/// FullDescription : 
		/// </summary>
		System.String  FullDescription  { get; set; }
		
		/// <summary>
		/// ConsultantData : 
		/// </summary>
		System.String  ConsultantData  { get; set; }
		
		/// <summary>
		/// LinkedInURL : 
		/// </summary>
		System.String  LinkedInUrl  { get; set; }
		
		/// <summary>
		/// TwitterURL : 
		/// </summary>
		System.String  TwitterUrl  { get; set; }
		
		/// <summary>
		/// FacebookURL : 
		/// </summary>
		System.String  FacebookUrl  { get; set; }
		
		/// <summary>
		/// GoogleURL : 
		/// </summary>
		System.String  GoogleUrl  { get; set; }
		
		/// <summary>
		/// Link : 
		/// </summary>
		System.String  Link  { get; set; }
		
		/// <summary>
		/// WechatURL : 
		/// </summary>
		System.String  WechatUrl  { get; set; }
		
		/// <summary>
		/// FeaturedTeamMember : 
		/// </summary>
		System.Int32  FeaturedTeamMember  { get; set; }
		
		/// <summary>
		/// ImageURL : 
		/// </summary>
		System.Byte[]  ImageUrl  { get; set; }
		
		/// <summary>
		/// VideoURL : 
		/// </summary>
		System.String  VideoUrl  { get; set; }
		
		/// <summary>
		/// BlogRSS : 
		/// </summary>
		System.String  BlogRss  { get; set; }
		
		/// <summary>
		/// NewsRSS : 
		/// </summary>
		System.String  NewsRss  { get; set; }
		
		/// <summary>
		/// JobRSS : 
		/// </summary>
		System.String  JobRss  { get; set; }
		
		/// <summary>
		/// TestimonialsRSS : 
		/// </summary>
		System.String  TestimonialsRss  { get; set; }
		
		/// <summary>
		/// Valid : 
		/// </summary>
		System.Int32  Valid  { get; set; }
		
		/// <summary>
		/// MetaTitle : 
		/// </summary>
		System.String  MetaTitle  { get; set; }
		
		/// <summary>
		/// MetaDescription : 
		/// </summary>
		System.String  MetaDescription  { get; set; }
		
		/// <summary>
		/// MetaKeywords : 
		/// </summary>
		System.String  MetaKeywords  { get; set; }
		
		/// <summary>
		/// LastModifiedBy : 
		/// </summary>
		System.Int32?  LastModifiedBy  { get; set; }
		
		/// <summary>
		/// LastModified : 
		/// </summary>
		System.DateTime?  LastModified  { get; set; }
		
		/// <summary>
		/// Sequence : 
		/// </summary>
		System.Int32  Sequence  { get; set; }
		
		/// <summary>
		/// LastName : 
		/// </summary>
		System.String  LastName  { get; set; }
		
		/// <summary>
		/// ConsultantsXML : 
		/// </summary>
		System.String  ConsultantsXml  { get; set; }
		
		/// <summary>
		/// ConsultantImageUrl : 
		/// </summary>
		System.String  ConsultantImageUrl  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		#endregion Data Properties

	}
}


