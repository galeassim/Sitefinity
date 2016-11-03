﻿/*
	File Generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file ViewSiteAreaLocationCountry.cs instead.
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
///	This class is the base repository for the CRUD operations on the ViewSiteAreaLocationCountry objects.
/// </summary>
public abstract partial class SqlViewSiteAreaLocationCountryProviderBase : ViewSiteAreaLocationCountryProviderBase
{
	
	string _connectionString;
    bool _useStoredProcedure;
    string _providerInvariantName;
		
	#region Constructors
	
	/// <summary>
	/// Creates a new <see cref="SqlViewSiteAreaLocationCountryProviderBase"/> instance.
	/// Uses connection string to connect to datasource.
	/// </summary>
	protected SqlViewSiteAreaLocationCountryProviderBase()
	{		
	}
	
	/// <summary>
	/// Creates a new <see cref="SqlViewSiteAreaLocationCountryProviderBase"/> instance.
	/// Uses connection string to connect to datasource.
	/// </summary>
	/// <param name="connectionString">The connection string to the database.</param>
	/// <param name="useStoredProcedure">A boolean value that indicates if we use stored procedures or embedded queries.</param>
	/// <param name="providerInvariantName">Name of the invariant provider use by the DbProviderFactory.</param>
	public SqlViewSiteAreaLocationCountryProviderBase(string connectionString, bool useStoredProcedure, string providerInvariantName)
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
	/// <returns>Returns a typed collection of ViewSiteAreaLocationCountry objects.</returns>
	public override VList<ViewSiteAreaLocationCountry> GetAll(TransactionManager transactionManager, int start, int pageLength, out int count)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewSiteAreaLocationCountry_Get_List", _useStoredProcedure);
		
		IDataReader reader = null;
		//Create Collection
		VList<ViewSiteAreaLocationCountry> rows = new VList<ViewSiteAreaLocationCountry>();
		
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
	/// <returns>Returns a typed collection of ViewSiteAreaLocationCountry objects.</returns>
	public override VList<ViewSiteAreaLocationCountry> Get(TransactionManager transactionManager, string whereClause, string orderBy, int start, int pageLength, out int count)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewSiteAreaLocationCountry_Get", _useStoredProcedure);

		database.AddInParameter(commandWrapper, "@WhereClause", DbType.String, whereClause);
		database.AddInParameter(commandWrapper, "@OrderBy", DbType.String, orderBy);
	
		IDataReader reader = null;
		//Create Collection
		VList<ViewSiteAreaLocationCountry> rows = new VList<ViewSiteAreaLocationCountry>();
		
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
	/// <returns>Returns a typed collection of ViewSiteAreaLocationCountry objects.</returns>
	public override VList<ViewSiteAreaLocationCountry> Find(TransactionManager transactionManager, IFilterParameterCollection parameters, string orderBy, int start, int pageLength, out int count)
	{
		SqlFilterParameterCollection filter = null;
		
		if (parameters == null)
			filter = new SqlFilterParameterCollection();
		else 
			filter = parameters.GetParameters();
			
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewSiteAreaLocationCountry_Find_Dynamic", typeof(ViewSiteAreaLocationCountryColumn), filter, orderBy, start, pageLength);
		
		SqlFilterParameter param;

		for ( int i = 0; i < filter.Count; i++ )
		{
			param = filter[i];
			database.AddInParameter(commandWrapper, param.Name, param.DbType, param.GetValue());
		}

		VList<ViewSiteAreaLocationCountry> rows = new VList<ViewSiteAreaLocationCountry>();
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
	

	#region ViewSiteAreaLocationCountry_GetBySiteLocationIdSiteAreaId
	
	/// <summary>
	///	This method wraps the 'ViewSiteAreaLocationCountry_GetBySiteLocationIdSiteAreaId' stored procedure. 
	/// </summary>
		/// <param name="siteLocationId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="siteAreaId"> A <c>System.Int32?</c> instance.</param>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <remark>This method is generated from a stored procedure.</remark>
	public override void GetBySiteLocationIdSiteAreaId(TransactionManager transactionManager, int start, int pageLength, System.Int32? siteLocationId, System.Int32? siteAreaId)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewSiteAreaLocationCountry_GetBySiteLocationIdSiteAreaId", true);
		
		database.AddInParameter(commandWrapper, "@SiteLocationId", DbType.Int32,  siteLocationId );
		database.AddInParameter(commandWrapper, "@SiteAreaId", DbType.Int32,  siteAreaId );
		

		try
		{
			
			if (transactionManager != null)
			{	
				Utility.ExecuteNonQuery(transactionManager, commandWrapper);
			}
			else
			{
				Utility.ExecuteNonQuery(database, commandWrapper);
			}
			
			
			
			return;
		}
		catch(SqlException ex)
		{
			throw new DataException("A data access error occured, please check inner SqlException.", ex);
		}
	}
	#endregion

	#region ViewSiteAreaLocationCountry_Get_List
	
	/// <summary>
	///	This method wraps the 'ViewSiteAreaLocationCountry_Get_List' stored procedure. 
	/// </summary>
	/// <param name="start">Row number at which to start reading.</param>
	/// <param name="pageLength">Number of rows to return.</param>
	/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
	/// <remark>This method is generated from a stored procedure.</remark>
	/// <returns>A <see cref="DataSet"/> instance.</returns>
	public override DataSet Get_List(TransactionManager transactionManager, int start, int pageLength)
	{
		SqlDatabase database = new SqlDatabase(this._connectionString);
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewSiteAreaLocationCountry_Get_List", true);
		
		

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

	#region ViewSiteAreaLocationCountry_Get
	
	/// <summary>
	///	This method wraps the 'ViewSiteAreaLocationCountry_Get' stored procedure. 
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
		DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.ViewSiteAreaLocationCountry_Get", true);
		
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