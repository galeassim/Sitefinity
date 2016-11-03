﻿	

#region Using Directives
using System;
using System.ComponentModel;
using System.Collections;
using System.Xml.Serialization;
using System.Data;

using JXTPortal.Entities;
using JXTPortal.Entities.Validation;

using JXTPortal.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;

#endregion

namespace JXTPortal
{		
	/// <summary>
	/// An component type implementation of the 'RelatedDynamicPages' table.
	/// </summary>
	/// <remarks>
	/// All custom implementations should be done here.
	/// </remarks>
	[CLSCompliant(true)]
	public partial class RelatedDynamicPagesService : JXTPortal.RelatedDynamicPagesServiceBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the RelatedDynamicPagesService class.
		/// </summary>
		public RelatedDynamicPagesService() : base()
		{
		}
		#endregion Constructors
		
	}//End Class

} // end namespace