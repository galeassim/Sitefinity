﻿
/*
	File Generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file SqlLanguagesProvider.cs instead.
*/

#region using directives

using System;
using System.Data;
using System.Data.Common;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using System.Collections;
using System.Collections.Specialized;

using System.Diagnostics;
using JXTPortal.Entities;
using JXTPortal.Data;
using JXTPortal.Data.Bases;

#endregion

namespace JXTPortal.Data.SqlClient
{
	///<summary>
	/// This class is the SqlClient Data Access Logic Component implementation for the <see cref="Languages"/> entity.
	///</summary>
	public abstract partial class SqlLanguagesProviderBase : LanguagesProviderBase
	{
		#region Declarations
		
		string _connectionString;
	    bool _useStoredProcedure;
	    string _providerInvariantName;
			
		#endregion "Declarations"
			
		#region Constructors
		
		/// <summary>
		/// Creates a new <see cref="SqlLanguagesProviderBase"/> instance.
		/// </summary>
		public SqlLanguagesProviderBase()
		{
		}
	
	/// <summary>
	/// Creates a new <see cref="SqlLanguagesProviderBase"/> instance.
	/// Uses connection string to connect to datasource.
	/// </summary>
	/// <param name="connectionString">The connection string to the database.</param>
	/// <param name="useStoredProcedure">A boolean value that indicates if we should use stored procedures or embedded queries.</param>
	/// <param name="providerInvariantName">Name of the invariant provider use by the DbProviderFactory.</param>
	public SqlLanguagesProviderBase(string connectionString, bool useStoredProcedure, string providerInvariantName)
	{
		this._connectionString = connectionString;
		this._useStoredProcedure = useStoredProcedure;
		this._providerInvariantName = providerInvariantName;
	}
		
	#endregion "Constructors"
	
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
    /// <value><c>true</c> if we choose to use use stored procedures; otherwise, <c>false</c>.</value>
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
	
		#region Get Many To Many Relationship Functions
		#endregion
	
		#region Delete Functions
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_languageId">. Primary Key.</param>	
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
        /// <exception cref="System.Exception">The command could not be executed.</exception>
        /// <exception cref="System.Data.DataException">The <paramref name="transactionManager"/> is not open.</exception>
        /// <exception cref="System.Data.Common.DbException">The command could not be executed.</exception>
		public override bool Delete(TransactionManager transactionManager, System.Int32 _languageId)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Delete", _useStoredProcedure);
			database.AddInParameter(commandWrapper, "@LanguageId", DbType.Int32, _languageId);
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "Delete")); 

			int results = 0;
			
			if (transactionManager != null)
			{	
				results = Utility.ExecuteNonQuery(transactionManager, commandWrapper);
			}
			else
			{
				results = Utility.ExecuteNonQuery(database,commandWrapper);
			}
			
			//Stop Tracking Now that it has been updated and persisted.
			if (DataRepository.Provider.EnableEntityTracking)
			{
				string entityKey = EntityLocator.ConstructKeyFromPkItems(typeof(Languages)
					,_languageId);
				EntityManager.StopTracking(entityKey);
			}
			
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "Delete")); 

			commandWrapper = null;
			
			return Convert.ToBoolean(results);
		}//end Delete
		#endregion

		#region Find Functions

		#region Parsed Find Methods
		/// <summary>
		/// 	Returns rows meeting the whereClause condition from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="whereClause">Specifies the condition for the rows returned by a query (Name='John Doe', Name='John Doe' AND Id='1', Name='John Doe' OR Id='1').</param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out. The number of rows that match this query.</param>
		/// <remarks>Operators must be capitalized (OR, AND).</remarks>
		/// <returns>Returns a typed collection of JXTPortal.Entities.Languages objects.</returns>
		public override TList<Languages> Find(TransactionManager transactionManager, string whereClause, int start, int pageLength, out int count)
		{
			count = -1;
			if (whereClause.IndexOf(";") > -1)
				return new TList<Languages>();
	
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Find", _useStoredProcedure);

		bool searchUsingOR = false;
		if (whereClause.IndexOf(" OR ") > 0) // did they want to do "a=b OR c=d OR..."?
			searchUsingOR = true;
		
		database.AddInParameter(commandWrapper, "@SearchUsingOR", DbType.Boolean, searchUsingOR);
		
		database.AddInParameter(commandWrapper, "@LanguageId", DbType.Int32, DBNull.Value);
		database.AddInParameter(commandWrapper, "@LanguageName", DbType.AnsiString, DBNull.Value);
		database.AddInParameter(commandWrapper, "@CharSetMetaTag", DbType.AnsiString, DBNull.Value);
	
			// replace all instances of 'AND' and 'OR' because we already set searchUsingOR
			whereClause = whereClause.Replace(" AND ", "|").Replace(" OR ", "|") ; 
			string[] clauses = whereClause.ToLower().Split('|');
		
			// Here's what's going on below: Find a field, then to get the value we
			// drop the field name from the front, trim spaces, drop the '=' sign,
			// trim more spaces, and drop any outer single quotes.
			// Now handles the case when two fields start off the same way - like "Friendly='Yes' AND Friend='john'"
				
			char[] equalSign = {'='};
			char[] singleQuote = {'\''};
	   		foreach (string clause in clauses)
			{
				if (clause.Trim().StartsWith("languageid ") || clause.Trim().StartsWith("languageid="))
				{
					database.SetParameterValue(commandWrapper, "@LanguageId", 
						clause.Trim().Remove(0,10).Trim().TrimStart(equalSign).Trim().Trim(singleQuote));
					continue;
				}
				if (clause.Trim().StartsWith("languagename ") || clause.Trim().StartsWith("languagename="))
				{
					database.SetParameterValue(commandWrapper, "@LanguageName", 
						clause.Trim().Remove(0,12).Trim().TrimStart(equalSign).Trim().Trim(singleQuote));
					continue;
				}
				if (clause.Trim().StartsWith("charsetmetatag ") || clause.Trim().StartsWith("charsetmetatag="))
				{
					database.SetParameterValue(commandWrapper, "@CharSetMetaTag", 
						clause.Trim().Remove(0,14).Trim().TrimStart(equalSign).Trim().Trim(singleQuote));
					continue;
				}
	
				throw new ArgumentException("Unable to use this part of the where clause in this version of Find: " + clause);
			}
					
			IDataReader reader = null;
			//Create Collection
			TList<Languages> rows = new TList<Languages>();
	
				
			try
			{
				//Provider Data Requesting Command Event
				OnDataRequesting(new CommandEventArgs(commandWrapper, "Find", rows)); 

				if (transactionManager != null)
				{
					reader = Utility.ExecuteReader(transactionManager, commandWrapper);
				}
				else
				{
					reader = Utility.ExecuteReader(database, commandWrapper);
				}		
				
				Fill(reader, rows, start, pageLength);
				
				if(reader.NextResult())
				{
					if(reader.Read())
					{
						count = reader.GetInt32(0);
					}
				}
				
				//Provider Data Requested Command Event
				OnDataRequested(new CommandEventArgs(commandWrapper, "Find", rows)); 
			}
			finally
			{
				if (reader != null) 
					reader.Close();	
					
				commandWrapper = null;
			}
			return rows;
		}

		#endregion Parsed Find Methods
		
		#region Parameterized Find Methods
		
		/// <summary>
		/// 	Returns rows from the DataSource that meet the parameter conditions.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="parameters">A collection of <see cref="SqlFilterParameter"/> objects.</param>
		/// <param name="orderBy">Specifies the sort criteria for the rows in the DataSource (Name ASC; BirthDay DESC, Name ASC);</param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out. The number of rows that match this query.</param>
		/// <returns>Returns a typed collection of JXTPortal.Entities.Languages objects.</returns>
		public override TList<Languages> Find(TransactionManager transactionManager, IFilterParameterCollection parameters, string orderBy, int start, int pageLength, out int count)
		{
			SqlFilterParameterCollection filter = null;
			
			if (parameters == null)
				filter = new SqlFilterParameterCollection();
			else 
				filter = parameters.GetParameters();
				
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Find_Dynamic", typeof(LanguagesColumn), filter, orderBy, start, pageLength);
		
			SqlFilterParameter param;

			for ( int i = 0; i < filter.Count; i++ )
			{
				param = filter[i];
				database.AddInParameter(commandWrapper, param.Name, param.DbType, param.GetValue());
			}

			TList<Languages> rows = new TList<Languages>();
			IDataReader reader = null;
			
			try
			{
				//Provider Data Requesting Command Event
				OnDataRequesting(new CommandEventArgs(commandWrapper, "Find", rows)); 

				if ( transactionManager != null )
				{
					reader = Utility.ExecuteReader(transactionManager, commandWrapper);
				}
				else
				{
					reader = Utility.ExecuteReader(database, commandWrapper);
				}
				
				Fill(reader, rows, 0, int.MaxValue);
				count = rows.Count;
				
				if ( reader.NextResult() )
				{
					if ( reader.Read() )
					{
						count = reader.GetInt32(0);
					}
				}
				
				//Provider Data Requested Command Event
				OnDataRequested(new CommandEventArgs(commandWrapper, "Find", rows)); 
			}
			finally
			{
				if ( reader != null )
					reader.Close();
					
				commandWrapper = null;
			}
			
			return rows;
		}
		
		#endregion Parameterized Find Methods
		
		#endregion Find Functions
	
		#region GetAll Methods
				
		/// <summary>
		/// 	Gets All rows from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out. The number of rows that match this query.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of JXTPortal.Entities.Languages objects.</returns>
        /// <exception cref="System.Exception">The command could not be executed.</exception>
        /// <exception cref="System.Data.DataException">The <paramref name="transactionManager"/> is not open.</exception>
        /// <exception cref="System.Data.Common.DbException">The command could not be executed.</exception>
		public override TList<Languages> GetAll(TransactionManager transactionManager, int start, int pageLength, out int count)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Get_List", _useStoredProcedure);
			
			IDataReader reader = null;
		
			//Create Collection
			TList<Languages> rows = new TList<Languages>();
			
			try
			{
				//Provider Data Requesting Command Event
				OnDataRequesting(new CommandEventArgs(commandWrapper, "GetAll", rows)); 
					
				if (transactionManager != null)
				{
					reader = Utility.ExecuteReader(transactionManager, commandWrapper);
				}
				else
				{
					reader = Utility.ExecuteReader(database, commandWrapper);
				}		
		
				Fill(reader, rows, start, pageLength);
				count = -1;
				if(reader.NextResult())
				{
					if(reader.Read())
					{
						count = reader.GetInt32(0);
					}
				}
				
				//Provider Data Requested Command Event
				OnDataRequested(new CommandEventArgs(commandWrapper, "GetAll", rows)); 
			}
			finally 
			{
				if (reader != null) 
					reader.Close();
					
				commandWrapper = null;	
			}
			return rows;
		}//end getall
		
		#endregion
				
		#region GetPaged Methods
				
		/// <summary>
		/// Gets a page of rows from the DataSource.
		/// </summary>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">Number of rows in the DataSource.</param>
		/// <param name="whereClause">Specifies the condition for the rows returned by a query (Name='John Doe', Name='John Doe' AND Id='1', Name='John Doe' OR Id='1').</param>
		/// <param name="orderBy">Specifies the sort criteria for the rows in the DataSource (Name ASC; BirthDay DESC, Name ASC);</param>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of JXTPortal.Entities.Languages objects.</returns>
		public override TList<Languages> GetPaged(TransactionManager transactionManager, string whereClause, string orderBy, int start, int pageLength, out int count)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_GetPaged", _useStoredProcedure);
		
			
            if (commandWrapper.CommandType == CommandType.Text
                && commandWrapper.CommandText != null)
            {
                commandWrapper.CommandText = commandWrapper.CommandText.Replace(SqlUtil.PAGE_INDEX, string.Concat(SqlUtil.PAGE_INDEX, Guid.NewGuid().ToString("N").Substring(0, 8)));
            }
			
			database.AddInParameter(commandWrapper, "@WhereClause", DbType.String, whereClause);
			database.AddInParameter(commandWrapper, "@OrderBy", DbType.String, orderBy);
			database.AddInParameter(commandWrapper, "@PageIndex", DbType.Int32, start);
			database.AddInParameter(commandWrapper, "@PageSize", DbType.Int32, pageLength);
		
			IDataReader reader = null;
			//Create Collection
			TList<Languages> rows = new TList<Languages>();
			
			try
			{
				//Provider Data Requesting Command Event
				OnDataRequesting(new CommandEventArgs(commandWrapper, "GetPaged", rows)); 

				if (transactionManager != null)
				{
					reader = Utility.ExecuteReader(transactionManager, commandWrapper);
				}
				else
				{
					reader = Utility.ExecuteReader(database, commandWrapper);
				}
				
				Fill(reader, rows, 0, int.MaxValue);
				count = rows.Count;

				if(reader.NextResult())
				{
					if(reader.Read())
					{
						count = reader.GetInt32(0);
					}
				}
				
				//Provider Data Requested Command Event
				OnDataRequested(new CommandEventArgs(commandWrapper, "GetPaged", rows)); 

			}
			catch(Exception)
			{			
				throw;
			}
			finally
			{
				if (reader != null) 
					reader.Close();
				
				commandWrapper = null;
			}
			
			return rows;
		}
		
		#endregion	
		
		#region Get By Foreign Key Functions
	#endregion
	
		#region Get By Index Functions

		#region GetByLanguageId
					
		/// <summary>
		/// 	Gets rows from the datasource based on the PK__Languages__24927208 index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_languageId"></param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query.</param>
		/// <returns>Returns an instance of the <see cref="JXTPortal.Entities.Languages"/> class.</returns>
		/// <remarks></remarks>
        /// <exception cref="System.Exception">The command could not be executed.</exception>
        /// <exception cref="System.Data.DataException">The <paramref name="transactionManager"/> is not open.</exception>
        /// <exception cref="System.Data.Common.DbException">The command could not be executed.</exception>
		public override JXTPortal.Entities.Languages GetByLanguageId(TransactionManager transactionManager, System.Int32 _languageId, int start, int pageLength, out int count)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_GetByLanguageId", _useStoredProcedure);
			
				database.AddInParameter(commandWrapper, "@LanguageId", DbType.Int32, _languageId);
			
			IDataReader reader = null;
			TList<Languages> tmp = new TList<Languages>();
			try
			{
				//Provider Data Requesting Command Event
				OnDataRequesting(new CommandEventArgs(commandWrapper, "GetByLanguageId", tmp)); 

				if (transactionManager != null)
				{
					reader = Utility.ExecuteReader(transactionManager, commandWrapper);
				}
				else
				{
					reader = Utility.ExecuteReader(database, commandWrapper);
				}		
		
				//Create collection and fill
				Fill(reader, tmp, start, pageLength);
				count = -1;
				if(reader.NextResult())
				{
					if(reader.Read())
					{
						count = reader.GetInt32(0);
					}
				}
				
				//Provider Data Requested Command Event
				OnDataRequested(new CommandEventArgs(commandWrapper, "GetByLanguageId", tmp));
			}
			finally 
			{
				if (reader != null) 
					reader.Close();
					
				commandWrapper = null;
			}
			
			if (tmp.Count == 1)
			{
				return tmp[0];
			}
			else if (tmp.Count == 0)
			{
				return null;
			}
			else
			{
				throw new DataException("Cannot find the unique instance of the class.");
			}
			
			//return rows;
		}
		
		#endregion

	#endregion Get By Index Functions

		#region Insert Methods
		/// <summary>
		/// Lets you efficiently bulk insert many entities to the database.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entities">The entities.</param>
		/// <remarks>
		///		After inserting into the datasource, the JXTPortal.Entities.Languages object will be updated
		/// 	to refelect any changes made by the datasource. (ie: identity or computed columns)
		/// </remarks>	
		public override void BulkInsert(TransactionManager transactionManager, TList<JXTPortal.Entities.Languages> entities)
		{
			//System.Data.SqlClient.SqlBulkCopy bulkCopy = new System.Data.SqlClient.SqlBulkCopy(this._connectionString, System.Data.SqlClient.SqlBulkCopyOptions.CheckConstraints); //, null);
			
			System.Data.SqlClient.SqlBulkCopy bulkCopy = null;
	
			if (transactionManager != null && transactionManager.IsOpen)
			{			
				System.Data.SqlClient.SqlConnection cnx = transactionManager.TransactionObject.Connection as System.Data.SqlClient.SqlConnection;
				System.Data.SqlClient.SqlTransaction transaction = transactionManager.TransactionObject as System.Data.SqlClient.SqlTransaction;
				bulkCopy = new System.Data.SqlClient.SqlBulkCopy(cnx, System.Data.SqlClient.SqlBulkCopyOptions.CheckConstraints, transaction); //, null);
			}
			else
			{
				bulkCopy = new System.Data.SqlClient.SqlBulkCopy(this._connectionString, System.Data.SqlClient.SqlBulkCopyOptions.CheckConstraints); //, null);
			}
			
			bulkCopy.BulkCopyTimeout = 360;
			bulkCopy.DestinationTableName = "Languages";
			
			DataTable dataTable = new DataTable();
			DataColumn col0 = dataTable.Columns.Add("LanguageID", typeof(System.Int32));
			col0.AllowDBNull = false;		
			DataColumn col1 = dataTable.Columns.Add("LanguageName", typeof(System.String));
			col1.AllowDBNull = false;		
			DataColumn col2 = dataTable.Columns.Add("CharSetMetaTag", typeof(System.String));
			col2.AllowDBNull = false;		
			
			bulkCopy.ColumnMappings.Add("LanguageID", "LanguageID");
			bulkCopy.ColumnMappings.Add("LanguageName", "LanguageName");
			bulkCopy.ColumnMappings.Add("CharSetMetaTag", "CharSetMetaTag");
			
			foreach(JXTPortal.Entities.Languages entity in entities)
			{
				if (entity.EntityState != EntityState.Added)
					continue;
					
				DataRow row = dataTable.NewRow();
				
					row["LanguageID"] = entity.LanguageId;
							
				
					row["LanguageName"] = entity.LanguageName;
							
				
					row["CharSetMetaTag"] = entity.CharSetMetaTag;
							
				
				dataTable.Rows.Add(row);
			}		
			
			// send the data to the server		
			bulkCopy.WriteToServer(dataTable);		
			
			// update back the state
			foreach(JXTPortal.Entities.Languages entity in entities)
			{
				if (entity.EntityState != EntityState.Added)
					continue;
			
				entity.AcceptChanges();
			}
		}
				
		/// <summary>
		/// 	Inserts a JXTPortal.Entities.Languages object into the datasource using a transaction.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="entity">JXTPortal.Entities.Languages object to insert.</param>
		/// <remarks>
		///		After inserting into the datasource, the JXTPortal.Entities.Languages object will be updated
		/// 	to refelect any changes made by the datasource. (ie: identity or computed columns)
		/// </remarks>	
		/// <returns>Returns true if operation is successful.</returns>
        /// <exception cref="System.Exception">The command could not be executed.</exception>
        /// <exception cref="System.Data.DataException">The <paramref name="transactionManager"/> is not open.</exception>
        /// <exception cref="System.Data.Common.DbException">The command could not be executed.</exception>
		public override bool Insert(TransactionManager transactionManager, JXTPortal.Entities.Languages entity)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Insert", _useStoredProcedure);
			
			database.AddOutParameter(commandWrapper, "@LanguageId", DbType.Int32, 4);
			database.AddInParameter(commandWrapper, "@LanguageName", DbType.AnsiString, entity.LanguageName );
			database.AddInParameter(commandWrapper, "@CharSetMetaTag", DbType.AnsiString, entity.CharSetMetaTag );
			
			int results = 0;
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "Insert", entity));
				
			if (transactionManager != null)
			{
				results = Utility.ExecuteNonQuery(transactionManager, commandWrapper);
			}
			else
			{
				results = Utility.ExecuteNonQuery(database,commandWrapper);
			}
					
			object _languageId = database.GetParameterValue(commandWrapper, "@LanguageId");
			entity.LanguageId = (System.Int32)_languageId;
			
			
			entity.AcceptChanges();
	
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "Insert", entity));

			return Convert.ToBoolean(results);
		}	
		#endregion

		#region Update Methods
				
		/// <summary>
		/// 	Update an existing row in the datasource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="entity">JXTPortal.Entities.Languages object to update.</param>
		/// <remarks>
		///		After updating the datasource, the JXTPortal.Entities.Languages object will be updated
		/// 	to refelect any changes made by the datasource. (ie: identity or computed columns)
		/// </remarks>
		/// <returns>Returns true if operation is successful.</returns>
        /// <exception cref="System.Exception">The command could not be executed.</exception>
        /// <exception cref="System.Data.DataException">The <paramref name="transactionManager"/> is not open.</exception>
        /// <exception cref="System.Data.Common.DbException">The command could not be executed.</exception>
		public override bool Update(TransactionManager transactionManager, JXTPortal.Entities.Languages entity)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Update", _useStoredProcedure);
			
			database.AddInParameter(commandWrapper, "@LanguageId", DbType.Int32, entity.LanguageId );
			database.AddInParameter(commandWrapper, "@LanguageName", DbType.AnsiString, entity.LanguageName );
			database.AddInParameter(commandWrapper, "@CharSetMetaTag", DbType.AnsiString, entity.CharSetMetaTag );
			
			int results = 0;
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "Update", entity));

			if (transactionManager != null)
			{
				results = Utility.ExecuteNonQuery(transactionManager, commandWrapper);
			}
			else
			{
				results = Utility.ExecuteNonQuery(database,commandWrapper);
			}
			
			//Stop Tracking Now that it has been updated and persisted.
			if (DataRepository.Provider.EnableEntityTracking)
				EntityManager.StopTracking(entity.EntityTrackingKey);
			
			
			entity.AcceptChanges();
			
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "Update", entity));

			return Convert.ToBoolean(results);
		}
			
		#endregion
		
		#region Custom Methods
	

		#region Languages_GetPaged
					
		/// <summary>
		///	This method wraps the 'Languages_GetPaged' stored procedure. 
		/// </summary>	
		/// <param name="whereClause"> A <c>System.String</c> instance.</param>
		/// <param name="orderBy"> A <c>System.String</c> instance.</param>
		/// <param name="pageIndex"> A <c>System.Int32?</c> instance.</param>
		/// <param name="pageSize"> A <c>System.Int32?</c> instance.</param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object.</param>
		/// <remark>This method is generated from a stored procedure.</remark>
		/// <returns>A <see cref="DataSet"/> instance.</returns>
		public override DataSet GetPaged(TransactionManager transactionManager, int start, int pageLength , System.String whereClause, System.String orderBy, System.Int32? pageIndex, System.Int32? pageSize)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_GetPaged", true);
			
			database.AddInParameter(commandWrapper, "@WhereClause", DbType.AnsiString,  whereClause );
			database.AddInParameter(commandWrapper, "@OrderBy", DbType.AnsiString,  orderBy );
			database.AddInParameter(commandWrapper, "@PageIndex", DbType.Int32,  pageIndex );
			database.AddInParameter(commandWrapper, "@PageSize", DbType.Int32,  pageSize );
	
			
			DataSet ds = null;
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "GetPaged", (IEntity)null));

			if (transactionManager != null)
			{	
				ds = Utility.ExecuteDataSet(transactionManager, commandWrapper);
			}
			else
			{
				ds = Utility.ExecuteDataSet(database, commandWrapper);
			}
			
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "GetPaged", (IEntity)null));

			

			
			return ds;	
		}
		#endregion

		#region Languages_Delete
					
		/// <summary>
		///	This method wraps the 'Languages_Delete' stored procedure. 
		/// </summary>	
		/// <param name="languageId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object.</param>
		/// <remark>This method is generated from a stored procedure.</remark>
		public override void Delete(TransactionManager transactionManager, int start, int pageLength , System.Int32? languageId)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Delete", true);
			
			database.AddInParameter(commandWrapper, "@LanguageId", DbType.Int32,  languageId );
	
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "Delete", (IEntity)null));

			if (transactionManager != null)
			{	
				Utility.ExecuteNonQuery(transactionManager, commandWrapper );
			}
			else
			{
				Utility.ExecuteNonQuery(database, commandWrapper);
			}
						
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "Delete", (IEntity)null));


				
				return;
		}
		#endregion

		#region Languages_Update
					
		/// <summary>
		///	This method wraps the 'Languages_Update' stored procedure. 
		/// </summary>	
		/// <param name="languageId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="languageName"> A <c>System.String</c> instance.</param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object.</param>
		/// <remark>This method is generated from a stored procedure.</remark>
		public override void Update(TransactionManager transactionManager, int start, int pageLength , System.Int32? languageId, System.String languageName)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Update", true);
			
			database.AddInParameter(commandWrapper, "@LanguageId", DbType.Int32,  languageId );
			database.AddInParameter(commandWrapper, "@LanguageName", DbType.AnsiString,  languageName );
	
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "Update", (IEntity)null));

			if (transactionManager != null)
			{	
				Utility.ExecuteNonQuery(transactionManager, commandWrapper );
			}
			else
			{
				Utility.ExecuteNonQuery(database, commandWrapper);
			}
						
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "Update", (IEntity)null));


				
				return;
		}
		#endregion

		#region Languages_GetByLanguageId
					
		/// <summary>
		///	This method wraps the 'Languages_GetByLanguageId' stored procedure. 
		/// </summary>	
		/// <param name="languageId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object.</param>
		/// <remark>This method is generated from a stored procedure.</remark>
		/// <returns>A <see cref="DataSet"/> instance.</returns>
		public override DataSet GetByLanguageId(TransactionManager transactionManager, int start, int pageLength , System.Int32? languageId)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_GetByLanguageId", true);
			
			database.AddInParameter(commandWrapper, "@LanguageId", DbType.Int32,  languageId );
	
			
			DataSet ds = null;
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "GetByLanguageId", (IEntity)null));

			if (transactionManager != null)
			{	
				ds = Utility.ExecuteDataSet(transactionManager, commandWrapper);
			}
			else
			{
				ds = Utility.ExecuteDataSet(database, commandWrapper);
			}
			
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "GetByLanguageId", (IEntity)null));

			

			
			return ds;	
		}
		#endregion

		#region Languages_Get_List
					
		/// <summary>
		///	This method wraps the 'Languages_Get_List' stored procedure. 
		/// </summary>	
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object.</param>
		/// <remark>This method is generated from a stored procedure.</remark>
		/// <returns>A <see cref="DataSet"/> instance.</returns>
		public override DataSet Get_List(TransactionManager transactionManager, int start, int pageLength )
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Get_List", true);
			
	
			
			DataSet ds = null;
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "Get_List", (IEntity)null));

			if (transactionManager != null)
			{	
				ds = Utility.ExecuteDataSet(transactionManager, commandWrapper);
			}
			else
			{
				ds = Utility.ExecuteDataSet(database, commandWrapper);
			}
			
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "Get_List", (IEntity)null));

			

			
			return ds;	
		}
		#endregion

		#region Languages_Insert
					
		/// <summary>
		///	This method wraps the 'Languages_Insert' stored procedure. 
		/// </summary>	
		/// <param name="languageName"> A <c>System.String</c> instance.</param>
			/// <param name="languageId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object.</param>
		/// <remark>This method is generated from a stored procedure.</remark>
		public override void Insert(TransactionManager transactionManager, int start, int pageLength , System.String languageName, ref System.Int32? languageId)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Insert", true);
			
			database.AddInParameter(commandWrapper, "@LanguageName", DbType.AnsiString,  languageName );
	
			database.AddParameter(commandWrapper, "@LanguageId", DbType.Int32, 4, ParameterDirection.InputOutput, true, 10, 0, string.Empty, DataRowVersion.Current, languageId);
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "Insert", (IEntity)null));

			if (transactionManager != null)
			{	
				Utility.ExecuteNonQuery(transactionManager, commandWrapper );
			}
			else
			{
				Utility.ExecuteNonQuery(database, commandWrapper);
			}
						
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "Insert", (IEntity)null));

			languageId =  Utility.GetParameterValue<System.Int32?>(commandWrapper.Parameters["@LanguageId"]);

				
				return;
		}
		#endregion

		#region Languages_Find
					
		/// <summary>
		///	This method wraps the 'Languages_Find' stored procedure. 
		/// </summary>	
		/// <param name="searchUsingOr"> A <c>System.Boolean?</c> instance.</param>
		/// <param name="languageId"> A <c>System.Int32?</c> instance.</param>
		/// <param name="languageName"> A <c>System.String</c> instance.</param>
		/// <param name="start">Row number at which to start reading.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object.</param>
		/// <remark>This method is generated from a stored procedure.</remark>
		/// <returns>A <see cref="DataSet"/> instance.</returns>
		public override DataSet Find(TransactionManager transactionManager, int start, int pageLength , System.Boolean? searchUsingOr, System.Int32? languageId, System.String languageName)
		{
			SqlDatabase database = new SqlDatabase(this._connectionString);
			DbCommand commandWrapper = StoredProcedureProvider.GetCommandWrapper(database, "dbo.Languages_Find", true);
			
			database.AddInParameter(commandWrapper, "@SearchUsingOR", DbType.Boolean,  searchUsingOr );
			database.AddInParameter(commandWrapper, "@LanguageId", DbType.Int32,  languageId );
			database.AddInParameter(commandWrapper, "@LanguageName", DbType.AnsiString,  languageName );
	
			
			DataSet ds = null;
			
			//Provider Data Requesting Command Event
			OnDataRequesting(new CommandEventArgs(commandWrapper, "Find", (IEntity)null));

			if (transactionManager != null)
			{	
				ds = Utility.ExecuteDataSet(transactionManager, commandWrapper);
			}
			else
			{
				ds = Utility.ExecuteDataSet(database, commandWrapper);
			}
			
			//Provider Data Requested Command Event
			OnDataRequested(new CommandEventArgs(commandWrapper, "Find", (IEntity)null));

			

			
			return ds;	
		}
		#endregion
		#endregion
	}//end class
} // end namespace