﻿/*
	File Generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file ViewJobSearch.cs instead.
*/

#region Using directives

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JXTPortal.Entities;
using JXTPortal.Data.Bases;

#endregion

namespace JXTPortal.Data.SqlClient
{
/// <summary>
///	This class is the base repository for the CRUD operations on the ViewJobSearch objects.
/// </summary>
public abstract partial class SqlViewJobSearchProviderBase : ViewJobSearchProviderBase
{
	
	string _connectionString;
    bool _useStoredProcedure;
    string _providerInvariantName;
		
	#region Constructors
	
	/// <summary>
	/// Creates a new <see cref="SqlViewJobSearchProviderBase"/> instance.
	/// Uses connection string to connect to datasource.
	/// </summary>
	protected SqlViewJobSearchProviderBase()
	{		
	}
	
	/// <summary>
	/// Creates a new <see cref="SqlViewJobSearchProviderBase"/> instance.
	/// Uses connection string to connect to datasource.
	/// </summary>
	/// <param name="connectionString">The connection string to the database.</param>
	/// <param name="useStoredProcedure">A boolean value that indicates if we use stored procedures or embedded queries.</param>
	/// <param name="providerInvariantName">Name of the invariant provider use by the DbProviderFactory.</param>
	public SqlViewJobSearchProviderBase(string connectionString, bool useStoredProcedure, string providerInvariantName)
	{
		this._connectionString = connectionString;
		this._useStoredProcedure = useStoredProcedure;
		this._providerInvariantName = providerInvariantName;
	}
			
	#endregion 
	
	#region Public properties
	/// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    /// <value>The connection string.</value>
    public string ConnectionString
	{
		get {return this._connectionString;}
		set {this._connectionString = value;}
	}
	
	/// <summary>
    /// Gets or sets a value indicating whether to use stored procedures.
    /// </summary>
    /// <value><c>true</c> if we choose to use stored procedures; otherwise, <c>false</c>.</value>
	public bool UseStoredProcedure
	{
		get {return this._useStoredProcedure;}
		set {this._useStoredProcedure = value;}
	}
	
	/// <summary>
    /// Gets or sets the invariant provider name listed in the DbProviderFactories machine.config section.
    /// </summary>
    /// <value>The name of the provider invariant.</value>
    public string ProviderInvariantName
    {
        get { return this._providerInvariantName; }
        set { this._providerInvariantName = value; }
    }
	#endregion
		
	
	#region GetAll Methods
	
	/// <summary>
	/// Gets All rows from the DataSource.
	/// </summary>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object.</param>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="count">The total number of rows in the data source.</param>
	/// <remarks></remarks>
	/// <returns>Returns a typed collection of ViewJobSearch objects.</returns>
	public override VList<ViewJobSearch> GetAll(TransactionManager transactionManager, int start, int pageLength, out int count)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewJobSearch_Get_List", _useStoredProcedure);
		
		IDataReader reader = null;
		//Create Collection
		VList<ViewJobSearch> rows = new VList<ViewJobSearch>();
		
		try
		{
			if (transactionManager != null)
			{
				reader = Utility.ExecuteReader(transactionManager, commandWrapper);
			}
			else
			{
				reader = Utility.ExecuteReader(database, commandWrapper);
			}
		
			Fill(reader, rows, start, pageLength);
			count = rows.Count;

			if(reader.NextResult())
			{
				if(reader.Read())
				{
					count = reader.GetInt32(0);
				}
			}
		}
		finally
		{
			if (reader != null)
				reader.Close();
		}
		return rows;
	}//end getall
	
	#endregion
	
	#region Get Methods
			
	/// <summary>
	/// Gets a page of rows from the DataSource.
	/// </summary>
	/// <param name="whereClause">Specifies the condition for the rows returned by a query (Name='John Doe', Name='John Doe' AND Id='1', Name='John Doe' OR Id='1').</param>
	/// <param name="orderBy">Specifies the sort criteria for the rows in the DataSource (Name ASC; BirthDay DESC, Name ASC);</param>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="count">The total number of rows in the data source.</param>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <remarks></remarks>
	/// <returns>Returns a typed collection of ViewJobSearch objects.</returns>
	public override VList<ViewJobSearch> Get(TransactionManager transactionManager, string whereClause, string orderBy, int start, int pageLength, out int count)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewJobSearch_Get", _useStoredProcedure);

		database.AddInParameter(commandWrapper, "@WhereClause", DbType.String, whereClause);
		database.AddInParameter(commandWrapper, "@OrderBy", DbType.String, orderBy);
	
		IDataReader reader = null;
		//Create Collection
		VList<ViewJobSearch> rows = new VList<ViewJobSearch>();
		
		try
		{
			if (transactionManager != null)
			{
				reader = Utility.ExecuteReader(transactionManager,commandWrapper);
			}
			else
			{
				reader = Utility.ExecuteReader(database, commandWrapper);
			}

			Fill(reader, rows, start, pageLength);
			count = rows.Count;

			if(reader.NextResult())
			{
				if(reader.Read())
				{
					count = reader.GetInt32(0);
				}
			}
		}
		finally
		{
		     if (reader != null)
		     	 reader.Close();
		}
		return rows;
	}
	
	#endregion
	
	#region Find Methods
	
	#region Parameterized Find Methods
	
	/// <summary>
	/// Returns rows from the DataSource that meet the parameter conditions.
	/// </summary>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <param name="parameters">A collection of <see cref="SqlFilterParameter"/> objects.</param>
	/// <param name="orderBy">Specifies the sort criteria for the rows in the DataSource (Name ASC; BirthDay DESC, Name ASC);</param>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="count">out. The number of rows that match this query.</param>
	/// <returns>Returns a typed collection of ViewJobSearch objects.</returns>
	public override VList<ViewJobSearch> Find(TransactionManager transactionManager, IFilterParameterCollection parameters, string orderBy, int start, int pageLength, out int count)
	{
		SqlFilterParameterCollection filter = null;
		
		if (parameters == null)
			filter = new SqlFilterParameterCollection();
		else 
			filter = parameters.GetParameters();
			
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewJobSearch_Find_Dynamic", typeof(ViewJobSearchColumn), filter, orderBy, start, pageLength);
		
		SqlFilterParameter param;

		for ( int i = 0; i < filter.Count; i++ )
		{
			param = filter[i];
			database.AddInParameter(commandWrapper, param.Name, param.DbType, param.GetValue());
		}

		VList<ViewJobSearch> rows = new VList<ViewJobSearch>();
		IDataReader reader = null;
		
		try
		{
			if ( transactionManager != null )
			{
				reader = Utility.ExecuteReader(transactionManager, commandWrapper);
			}
			else
			{
				reader = Utility.ExecuteReader(database, commandWrapper);
			}
			
			Fill(reader, rows, start, pageLength);
			count = rows.Count;
			
			if ( reader.NextResult() )
			{
				if ( reader.Read() )
				{
					count = reader.GetInt32(0);
				}
			}
		}
		finally
		{
			if ( reader != null )
				reader.Close();
		}
		
		return rows;
	}
	
	#endregion Parameterized Find Methods

	#endregion 

	#region Custom Methods
	

	#region ViewJobSearch_GetBySearchFilter
	
	/// <summary>
	///	This method wraps the 'ViewJobSearch_GetBySearchFilter' stored procedure. 
	/// </summary>
		/// <param name="keyword"> A <c>System.String</c> instance.</param>
		/// <param name="siteId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="advertiserId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="currencyId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="salaryLowerBand"> A <c>System.Decimal?</c> instance.</param>
		/// <param name="salaryUpperBand"> A <c>System.Decimal?</c> instance.</param>
		/// <param name="salaryTypeId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="workTypeId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="professionId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="roleId"> A <c>System.String</c> instance.</param>
		/// <param name="countryId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="locationId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="areaId"> A <c>System.String</c> instance.</param>
		/// <param name="dateFrom"> A <c>System.DateTime?</c> instance.</param>
		/// <param name="pageIndex"> A <c>System.Int32?</c> instance.</param>
		/// <param name="pageSize"> A <c>System.Int32?</c> instance.</param>
		/// <param name="orderBy"> A <c>System.String</c> instance.</param>
		/// <param name="jobTypeIds"> A <c>System.String</c> instance.</param>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <remark>This method is generated from a stored procedure.</remark>
	/// <returns>A <see cref="VList&lt;ViewJobSearch&gt;"/> instance.</returns>
	public override VList<ViewJobSearch> GetBySearchFilter(TransactionManager transactionManager, int start, int pageLength, System.String keyword, System.Int32? siteId, System.Int32? advertiserId, System.Int32? currencyId, System.Decimal? salaryLowerBand, System.Decimal? salaryUpperBand, System.Int32? salaryTypeId, System.Int32? workTypeId, System.Int32? professionId, System.String roleId, System.Int32? countryId, System.Int32? locationId, System.String areaId, System.DateTime? dateFrom, System.Int32? pageIndex, System.Int32? pageSize, System.String orderBy, System.String jobTypeIds)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewJobSearch_GetBySearchFilter", true);
		
		database.AddInParameter(commandWrapper, "@Keyword", DbType.String,  keyword );
		database.AddInParameter(commandWrapper, "@SiteId", DbType.Int32,  siteId );
		database.AddInParameter(commandWrapper, "@AdvertiserId", DbType.Int32,  advertiserId );
		database.AddInParameter(commandWrapper, "@CurrencyID", DbType.Int32,  currencyId );
		database.AddInParameter(commandWrapper, "@SalaryLowerBand", DbType.Decimal,  salaryLowerBand );
		database.AddInParameter(commandWrapper, "@SalaryUpperBand", DbType.Decimal,  salaryUpperBand );
		database.AddInParameter(commandWrapper, "@SalaryTypeID", DbType.Int32,  salaryTypeId );
		database.AddInParameter(commandWrapper, "@WorkTypeID", DbType.Int32,  workTypeId );
		database.AddInParameter(commandWrapper, "@ProfessionID", DbType.Int32,  professionId );
		database.AddInParameter(commandWrapper, "@RoleID", DbType.AnsiString,  roleId );
		database.AddInParameter(commandWrapper, "@CountryID", DbType.Int32,  countryId );
		database.AddInParameter(commandWrapper, "@LocationID", DbType.Int32,  locationId );
		database.AddInParameter(commandWrapper, "@AreaID", DbType.AnsiString,  areaId );
		database.AddInParameter(commandWrapper, "@DateFrom", DbType.DateTime,  dateFrom );
		database.AddInParameter(commandWrapper, "@PageIndex", DbType.Int32,  pageIndex );
		database.AddInParameter(commandWrapper, "@PageSize", DbType.Int32,  pageSize );
		database.AddInParameter(commandWrapper, "@OrderBy", DbType.AnsiString,  orderBy );
		database.AddInParameter(commandWrapper, "@JobTypeIDs", DbType.AnsiString,  jobTypeIds );
		

		try
		{
			
			IDataReader reader = null;

			if (transactionManager != null)
			{	
				reader = Utility.ExecuteReader(transactionManager, commandWrapper);
			}
			else
			{
				reader = Utility.ExecuteReader(database, commandWrapper);
			}			
			
			// Create Collection
				VList<ViewJobSearch> rows = new VList<ViewJobSearch>();
				try
				{  
					Fill(reader, rows, start, pageLength);
				}
				finally
				{
					if (reader != null) 
						reader.Close();
				}
				
				
				
				return rows;
		}
		catch(SqlException ex)
		{
			throw new DataException("A data access error occured, please check inner SqlException.", ex);
		}
	}
	#endregion

	#region ViewJobSearch_GetPremiumSearchFilter
	
	/// <summary>
	///	This method wraps the 'ViewJobSearch_GetPremiumSearchFilter' stored procedure. 
	/// </summary>
		/// <param name="siteId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="professionId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="roleId"> A <c>System.String</c> instance.</param>
		/// <param name="orderBy"> A <c>System.String</c> instance.</param>
		/// <param name="pageSize"> A <c>System.Int32?</c> instance.</param>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <remark>This method is generated from a stored procedure.</remark>
	/// <returns>A <see cref="VList&lt;ViewJobSearch&gt;"/> instance.</returns>
	public override VList<ViewJobSearch> GetPremiumSearchFilter(TransactionManager transactionManager, int start, int pageLength, System.Int32? siteId, System.Int32? professionId, System.String roleId, System.String orderBy, System.Int32? pageSize)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewJobSearch_GetPremiumSearchFilter", true);
		
		database.AddInParameter(commandWrapper, "@SiteId", DbType.Int32,  siteId );
		database.AddInParameter(commandWrapper, "@ProfessionID", DbType.Int32,  professionId );
		database.AddInParameter(commandWrapper, "@RoleID", DbType.AnsiString,  roleId );
		database.AddInParameter(commandWrapper, "@OrderBy", DbType.AnsiString,  orderBy );
		database.AddInParameter(commandWrapper, "@PageSize", DbType.Int32,  pageSize );
		

		try
		{
			
			IDataReader reader = null;

			if (transactionManager != null)
			{	
				reader = Utility.ExecuteReader(transactionManager, commandWrapper);
			}
			else
			{
				reader = Utility.ExecuteReader(database, commandWrapper);
			}			
			
			// Create Collection
				VList<ViewJobSearch> rows = new VList<ViewJobSearch>();
				try
				{  
					Fill(reader, rows, start, pageLength);
				}
				finally
				{
					if (reader != null) 
						reader.Close();
				}
				
				
				
				return rows;
		}
		catch(SqlException ex)
		{
			throw new DataException("A data access error occured, please check inner SqlException.", ex);
		}
	}
	#endregion

	#region ViewJobSearch_Get_List
	
	/// <summary>
	///	This method wraps the 'ViewJobSearch_Get_List' stored procedure. 
	/// </summary>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <remark>This method is generated from a stored procedure.</remark>
	/// <returns>A <see cref="DataSet"/> instance.</returns>
	public override DataSet Get_List(TransactionManager transactionManager, int start, int pageLength)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewJobSearch_Get_List", true);
		
		

		try
		{
			
			DataSet ds = null;
			
			if (transactionManager != null)
			{	
				ds = Utility.ExecuteDataSet(transactionManager, commandWrapper);
			}
			else
			{
				ds = Utility.ExecuteDataSet(database, commandWrapper);
			}
			
			
			
			return ds;	
		}
		catch(SqlException ex)
		{
			throw new DataException("A data access error occured, please check inner SqlException.", ex);
		}
	}
	#endregion

	#region ViewJobSearch_GetBySearchFilterGoogleMap
	
	/// <summary>
	///	This method wraps the 'ViewJobSearch_GetBySearchFilterGoogleMap' stored procedure. 
	/// </summary>
		/// <param name="keyword"> A <c>System.String</c> instance.</param>
		/// <param name="siteId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="advertiserId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="currencyId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="salaryLowerBand"> A <c>System.Decimal?</c> instance.</param>
		/// <param name="salaryUpperBand"> A <c>System.Decimal?</c> instance.</param>
		/// <param name="salaryTypeId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="workTypeId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="professionId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="roleId"> A <c>System.String</c> instance.</param>
		/// <param name="countryId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="locationId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="areaId"> A <c>System.String</c> instance.</param>
		/// <param name="orderBy"> A <c>System.String</c> instance.</param>
		/// <param name="jobTypeIds"> A <c>System.String</c> instance.</param>
		/// <param name="northEastLat"> A <c>System.Double?</c> instance.</param>
		/// <param name="northEastLng"> A <c>System.Double?</c> instance.</param>
		/// <param name="southWestLat"> A <c>System.Double?</c> instance.</param>
		/// <param name="southWestLng"> A <c>System.Double?</c> instance.</param>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <remark>This method is generated from a stored procedure.</remark>
	/// <returns>A <see cref="VList&lt;ViewJobSearch&gt;"/> instance.</returns>
	public override VList<ViewJobSearch> GetBySearchFilterGoogleMap(TransactionManager transactionManager, int start, int pageLength, System.String keyword, System.Int32? siteId, System.Int32? advertiserId, System.Int32? currencyId, System.Decimal? salaryLowerBand, System.Decimal? salaryUpperBand, System.Int32? salaryTypeId, System.Int32? workTypeId, System.Int32? professionId, System.String roleId, System.Int32? countryId, System.Int32? locationId, System.String areaId, System.String orderBy, System.String jobTypeIds, System.Double? northEastLat, System.Double? northEastLng, System.Double? southWestLat, System.Double? southWestLng)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewJobSearch_GetBySearchFilterGoogleMap", true);
		
		database.AddInParameter(commandWrapper, "@Keyword", DbType.String,  keyword );
		database.AddInParameter(commandWrapper, "@SiteId", DbType.Int32,  siteId );
		database.AddInParameter(commandWrapper, "@AdvertiserId", DbType.Int32,  advertiserId );
		database.AddInParameter(commandWrapper, "@CurrencyID", DbType.Int32,  currencyId );
		database.AddInParameter(commandWrapper, "@SalaryLowerBand", DbType.Decimal,  salaryLowerBand );
		database.AddInParameter(commandWrapper, "@SalaryUpperBand", DbType.Decimal,  salaryUpperBand );
		database.AddInParameter(commandWrapper, "@SalaryTypeID", DbType.Int32,  salaryTypeId );
		database.AddInParameter(commandWrapper, "@WorkTypeID", DbType.Int32,  workTypeId );
		database.AddInParameter(commandWrapper, "@ProfessionID", DbType.Int32,  professionId );
		database.AddInParameter(commandWrapper, "@RoleID", DbType.AnsiString,  roleId );
		database.AddInParameter(commandWrapper, "@CountryID", DbType.Int32,  countryId );
		database.AddInParameter(commandWrapper, "@LocationID", DbType.Int32,  locationId );
		database.AddInParameter(commandWrapper, "@AreaID", DbType.AnsiString,  areaId );
		database.AddInParameter(commandWrapper, "@OrderBy", DbType.AnsiString,  orderBy );
		database.AddInParameter(commandWrapper, "@JobTypeIDs", DbType.AnsiString,  jobTypeIds );
		database.AddInParameter(commandWrapper, "@NorthEastLat", DbType.Double,  northEastLat );
		database.AddInParameter(commandWrapper, "@NorthEastLng", DbType.Double,  northEastLng );
		database.AddInParameter(commandWrapper, "@SouthWestLat", DbType.Double,  southWestLat );
		database.AddInParameter(commandWrapper, "@SouthWestLng", DbType.Double,  southWestLng );
		

		try
		{
			
			IDataReader reader = null;

			if (transactionManager != null)
			{	
				reader = Utility.ExecuteReader(transactionManager, commandWrapper);
			}
			else
			{
				reader = Utility.ExecuteReader(database, commandWrapper);
			}			
			
			// Create Collection
				VList<ViewJobSearch> rows = new VList<ViewJobSearch>();
				try
				{  
					Fill(reader, rows, start, pageLength);
				}
				finally
				{
					if (reader != null) 
						reader.Close();
				}
				
				
				
				return rows;
		}
		catch(SqlException ex)
		{
			throw new DataException("A data access error occured, please check inner SqlException.", ex);
		}
	}
	#endregion

	#region ViewJobSearch_GetBySearchFilterRedefine
	
	/// <summary>
	///	This method wraps the 'ViewJobSearch_GetBySearchFilterRedefine' stored procedure. 
	/// </summary>
		/// <param name="keyword"> A <c>System.String</c> instance.</param>
		/// <param name="siteId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="advertiserId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="currencyId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="salaryTypeId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="salaryLowerBand"> A <c>System.Decimal?</c> instance.</param>
		/// <param name="salaryUpperBand"> A <c>System.Decimal?</c> instance.</param>
		/// <param name="workTypeId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="professionId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="roleId"> A <c>System.String</c> instance.</param>
		/// <param name="countryId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="locationId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="areaId"> A <c>System.String</c> instance.</param>
		/// <param name="dateFrom"> A <c>System.DateTime?</c> instance.</param>
		/// <param name="jobTypeIds"> A <c>System.String</c> instance.</param>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <remark>This method is generated from a stored procedure.</remark>
	/// <returns>A <see cref="DataSet"/> instance.</returns>
	public override DataSet GetBySearchFilterRedefine(TransactionManager transactionManager, int start, int pageLength, System.String keyword, System.Int32? siteId, System.Int32? advertiserId, System.Int32? currencyId, System.Int32? salaryTypeId, System.Decimal? salaryLowerBand, System.Decimal? salaryUpperBand, System.Int32? workTypeId, System.Int32? professionId, System.String roleId, System.Int32? countryId, System.Int32? locationId, System.String areaId, System.DateTime? dateFrom, System.String jobTypeIds)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewJobSearch_GetBySearchFilterRedefine", true);
		
		database.AddInParameter(commandWrapper, "@Keyword", DbType.String,  keyword );
		database.AddInParameter(commandWrapper, "@SiteId", DbType.Int32,  siteId );
		database.AddInParameter(commandWrapper, "@AdvertiserId", DbType.Int32,  advertiserId );
		database.AddInParameter(commandWrapper, "@CurrencyId", DbType.Int32,  currencyId );
		database.AddInParameter(commandWrapper, "@SalaryTypeID", DbType.Int32,  salaryTypeId );
		database.AddInParameter(commandWrapper, "@SalaryLowerBand", DbType.Decimal,  salaryLowerBand );
		database.AddInParameter(commandWrapper, "@SalaryUpperBand", DbType.Decimal,  salaryUpperBand );
		database.AddInParameter(commandWrapper, "@WorkTypeID", DbType.Int32,  workTypeId );
		database.AddInParameter(commandWrapper, "@ProfessionID", DbType.Int32,  professionId );
		database.AddInParameter(commandWrapper, "@RoleID", DbType.AnsiString,  roleId );
		database.AddInParameter(commandWrapper, "@CountryID", DbType.Int32,  countryId );
		database.AddInParameter(commandWrapper, "@LocationID", DbType.Int32,  locationId );
		database.AddInParameter(commandWrapper, "@AreaID", DbType.AnsiString,  areaId );
		database.AddInParameter(commandWrapper, "@DateFrom", DbType.DateTime,  dateFrom );
		database.AddInParameter(commandWrapper, "@JobTypeIDs", DbType.AnsiString,  jobTypeIds );
		

		try
		{
			
			DataSet ds = null;
			
			if (transactionManager != null)
			{	
				ds = Utility.ExecuteDataSet(transactionManager, commandWrapper);
			}
			else
			{
				ds = Utility.ExecuteDataSet(database, commandWrapper);
			}
			
			
			
			return ds;	
		}
		catch(SqlException ex)
		{
			throw new DataException("A data access error occured, please check inner SqlException.", ex);
		}
	}
	#endregion

	#region ViewJobSearch_Get
	
	/// <summary>
	///	This method wraps the 'ViewJobSearch_Get' stored procedure. 
	/// </summary>
		/// <param name="whereClause"> A <c>System.String</c> instance.</param>
		/// <param name="orderBy"> A <c>System.String</c> instance.</param>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <remark>This method is generated from a stored procedure.</remark>
	/// <returns>A <see cref="DataSet"/> instance.</returns>
	public override DataSet Get(TransactionManager transactionManager, int start, int pageLength, System.String whereClause, System.String orderBy)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewJobSearch_Get", true);
		
		database.AddInParameter(commandWrapper, "@WhereClause", DbType.AnsiString,  whereClause );
		database.AddInParameter(commandWrapper, "@OrderBy", DbType.AnsiString,  orderBy );
		

		try
		{
			
			DataSet ds = null;
			
			if (transactionManager != null)
			{	
				ds = Utility.ExecuteDataSet(transactionManager, commandWrapper);
			}
			else
			{
				ds = Utility.ExecuteDataSet(database, commandWrapper);
			}
			
			
			
			return ds;	
		}
		catch(SqlException ex)
		{
			throw new DataException("A data access error occured, please check inner SqlException.", ex);
		}
	}
	#endregion

	#endregion


	}//end class
} // end namespace
