namespace Business
{
	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Diagnostics.Contracts;
	using System.IO;
	using System.Threading.Tasks;

	using MySql.Data.MySqlClient;

	/// <summary>
	/// Provides methods for interaction with a SQL database.
	/// </summary>
	public class Database
	{
		#region Fields
		private readonly MySqlConnectionStringBuilder connectionString;

		private int commandTimeout = 30;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the wait time before terminating the attempt to execute a command and generate an error.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is a negative number.</exception>
		public int CommandTimeout
		{
			get
			{
				return this.commandTimeout;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", "Out of range");
				}

				this.commandTimeout = value;
			}
		}

		///// <summary>
		///// Gets the name of the SQL database this <see cref="T:System.Data.Database"/> instance represents.
		///// </summary>
		//public string Name
		//{
		//	get { return this.connectionString.InitialCatalog; }
		//}

		///// <summary>
		///// Gets the server to which this <see cref="T:System.Data.Database"/> instance connects.
		///// </summary>
		//public string Server
		//{
		//	get { return this.connectionString.DataSource; }
		//}
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Data.Database"/> class with the specified connection string.
		/// </summary>
		/// <param name="connectionString">A <see cref="T:System.String"/> containing the connection parameters.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="connectionString"/> is null.</exception>
		public Database(string connectionString)
		{
			if (connectionString == null)
			{
				throw new ArgumentNullException("connectionString");
			}

			Contract.EndContractBlock();
										
			this.connectionString = new MySqlConnectionStringBuilder(connectionString);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Data.Database"/> class using the specified <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/>
		/// </summary>
		/// <param name="connectionString">An <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> instance representing the connection string.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="connectionString"/> is null.</exception>
		public Database(MySqlConnectionStringBuilder connectionString)
		{
			if (connectionString == null)
			{
				throw new ArgumentNullException("connectionString");
			}

			Contract.EndContractBlock();

			this.connectionString = connectionString;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Starts a database transaction.
		/// </summary>
		/// <returns>An object representing the new transaction.</returns>
		/// <exception cref="System.Data.SqlClient.SqlException">Parallel transactions are not allowed when using Multiple Active Result Sets (MARS).</exception>
		/// <exception cref="System.InvalidOperationException">Parallel transactions are not supported.</exception>
		public MySqlTransaction BeginTransaction()
		{
			var connection = new MySqlConnection(this.connectionString.ConnectionString);

			connection.Open();

			return connection.BeginTransaction();
		}

		/// <summary>
		/// Creates the database represented by this instance on the database server if it doesn't already exist.
		/// </summary>
		//public void Create()
		//{
		//	using (var connection = new MySqlConnection())
		//	{
		//		connection.ConnectionString = string.Join(";", this.Server, "master", this.connectionString.UserID, this.connectionString.Password);

		//		using (var command = new MySqlCommand("CREATE DATABASE @DatabaseName", connection))
		//		{
		//			command.CommandTimeout = this.commandTimeout;
		//			command.Parameters.Add(new MySqlParameter("@DatabaseName", this.Name));

		//			connection.Open();

		//			command.ExecuteNonQuery();
		//		}
		//	}
		//}

		/// <summary>
		/// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
		/// </summary>
		/// <param name="query">The query to execute.</param>
		/// <param name="parameters">Any SQL parameters the query may need.</param>
		/// <returns>A <see cref="T:System.Int32"/> the contains the number of rows affected.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="query"/> is null.</exception>
		public int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}

			Contract.EndContractBlock();

			using (var connection = new MySqlConnection(this.connectionString.ConnectionString))
			{
				using (var command = new MySqlCommand(query, connection))
				{
					if (parameters != null)
					{
						command.Parameters.AddRange(parameters);
					}

					command.CommandTimeout = this.commandTimeout;

					connection.Open();

					return command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Asynchronous version of <see cref="M:System.Data.Database.ExecuteNonQuery"/>, which executes a Transact-SQL statement against the connection and returns the number of rows affected.
		/// </summary>
		/// <param name="query">The query to execute.</param>
		/// <param name="parameters">Any SQL parameters the query may need.</param>
		/// <returns>A <see cref="T:System.Int32"/> the contains the number of rows affected.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="query"/> is null.</exception>
		public async Task<int> ExecuteNonQueryAsync(string query, params MySqlParameter[] parameters)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}

			Contract.EndContractBlock();

			using (var connection = new MySqlConnection(this.connectionString.ConnectionString))
			{
				using (var command = new MySqlCommand(query, connection))
				{
					if (parameters != null)
					{
						command.Parameters.AddRange(parameters);
					}

					command.CommandTimeout = this.commandTimeout;

					await connection.OpenAsync();

					return await command.ExecuteNonQueryAsync();
				}
			}
		}

		/// <summary>
		/// Executes the specified query against the connection and returns the result.
		/// </summary>
		/// <param name="query">The query to execute.</param>
		/// <param name="parameters">Any SQL parameters the query may need.</param>
		/// <returns>A <see cref="T:System.Data.DataSet"/> containing the query's results.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="query"/> is null.</exception>
		public DataSet ExecuteQuery(string query, params MySqlParameter[] parameters)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}

//Console.WriteLine(@"Query: 
//" + query);

			Contract.EndContractBlock();

			using (var connection = new MySqlConnection(this.connectionString.ConnectionString))
			{
				using (var command = new MySqlCommand(query, connection))
				{
					if (parameters != null)
					{
						command.Parameters.AddRange(parameters);
					}

					command.CommandTimeout = this.commandTimeout;

					using (var adapter = new MySqlDataAdapter(command))
					{
						var ds = new DataSet();

						connection.Open();

						adapter.Fill(ds);

						return ds;
					}
				}
			}
		}

		/// <summary>
		/// Asynchronous version of <see cref="M:System.Data.Database.ExecuteQuery"/>, which executes the specified query against the connection and returns the result.
		/// </summary>
		/// <param name="query">The query to execute.</param>
		/// <param name="parameters">Any SQL parameters the query may need.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="query"/> is null.</exception>
		public async Task<DataSet> ExecuteQueryAsync(string query, params MySqlParameter[] parameters)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			Contract.EndContractBlock();

			using (var connection = new MySqlConnection(this.connectionString.ConnectionString))
			{
				using (var command = new MySqlCommand(query, connection))
				{
					if (parameters != null)
					{
						command.Parameters.AddRange(parameters);
					}

					command.CommandTimeout = this.commandTimeout;

					using (var adapter = new MySqlDataAdapter(command))
					{
						var ds = new DataSet();

						await connection.OpenAsync();

						await Task.Run(() => adapter.Fill(ds));

						return ds;
					}
				}
			}
		}

		/// <summary>
		/// Executes the <paramref name="query"/>, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
		/// </summary>
		/// <param name="query">The query to execute.</param>
		/// <param name="parameters">Any SQL parameters the query may need.</param>
		/// <returns>A <see cref="T:System.Data.DataSet"/> containing the query's results.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="query"/> is null.</exception>
		public object ExecuteScalar(string query, params MySqlParameter[] parameters)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			Contract.EndContractBlock();

			using (var connection = new MySqlConnection(this.connectionString.ConnectionString))
			{
				using (var command = new MySqlCommand(query, connection))
				{
					if (parameters != null)
					{
						command.Parameters.AddRange(parameters);
					}

					command.CommandTimeout = this.CommandTimeout;

					connection.Open();

					return command.ExecuteScalar();
				}
			}
		}

		/// <summary>
		/// Asynchronous version of <see cref="M:System.Data.Database.ExecuteScalar"/>, which executes the <paramref name="query"/>, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
		/// </summary>
		/// <param name="query">The query to execute.</param>
		/// <param name="parameters">Any SQL parameters the query may need.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="query"/> is null.</exception>
		public async Task<object> ExecuteScalarAsync(string query, params MySqlParameter[] parameters)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			Contract.EndContractBlock();

			using (var connection = new MySqlConnection(this.connectionString.ConnectionString))
			{
				using (var command = new MySqlCommand(query, connection))
				{
					if (parameters != null)
					{
						command.Parameters.AddRange(parameters);
					}

					command.CommandTimeout = this.CommandTimeout;

					await connection.OpenAsync();

					return await command.ExecuteScalarAsync();
				}
			}
		}

		/// <summary>
		/// Execute the SQL script in the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file containing the SQL script.</param>
		/// <param name="parameters">Any SQL parameters the script may need.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="fileName"/> is null.</exception>
		/// <exception cref="System.IO.FileNotFoundException"><paramref name="fileName"/> could not be found.</exception>
		public void ExecuteScript(string fileName, params MySqlParameter[] parameters)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentNullException(nameof(fileName));
			}

			Contract.EndContractBlock();

			using (var reader = new StreamReader(fileName))
			{
				var batches = reader.ReadToEnd().Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var batch in batches)
				{
					this.ExecuteNonQuery(batch, parameters);
				}
			}
		}

		/// <summary>
		/// Asynchronous version of <see cref="M:ExecuteScript"/>, 
		/// </summary>
		/// <param name="fileName">The name of the file containing the SQL script.</param>
		/// <param name="parameters">Any SQL parameters the script may need.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="fileName"/> is null.</exception>
		/// <exception cref="System.IO.FileNotFoundException"><paramref name="fileName"/> could not be found.</exception>
		public async Task ExecuteScriptAsync(string fileName, params MySqlParameter[] parameters)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentNullException("fileName");
			}

			Contract.EndContractBlock();

			using (var reader = new StreamReader(fileName))
			{
				var fileText = await reader.ReadToEndAsync();
				var batches = fileText.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var batch in batches)
				{
					await this.ExecuteNonQueryAsync(batch, parameters);
				}
			}
		}

		/// <summary>
		/// Returns a value indicating whether the database represented by this instance exists on the database server.
		/// </summary>
		/// <returns><c>true</c> if the database exists on the server; otherwise, <c>false</c>.</returns>
		//public bool Exists()
		//{
		//	using (var connection = new MySqlConnection())
		//	{
		//		connection.ConnectionString = string.Join(";", this.Server, "master", this.connectionString.UserID, this.connectionString.Password);

		//		using (var command = new MySqlCommand("SELECT COUNT(*) FROM sysdatabases WHERE name = @DatabaseName", connection))
		//		{
		//			command.CommandTimeout = this.CommandTimeout;
		//			command.Parameters.Add(new MySqlParameter("@DatabaseName", this.Name));

		//			connection.Open();

		//			return command.ExecuteNonQuery() != 0;
		//		}
		//	}
		//}

		/// <summary>
		/// Returns whether the specified table exists in the database.
		/// </summary>
		/// <param name="tableName">The name of the table to check.</param>
		/// <param name="schema">The name of the schema to locate the table in.</param>
		/// <returns><c>true</c> if a table with the specified name exists; otherwise, <c>false</c>.</returns>
		public bool TableExists(string tableName, string schema = "dbo")
		{
			if (string.IsNullOrWhiteSpace(tableName))
			{
				throw new ArgumentNullException(nameof(tableName));
			}

			if (string.IsNullOrWhiteSpace(schema))
			{
				throw new ArgumentNullException(nameof(schema));
			}

			Contract.EndContractBlock();

			return (int)this.ExecuteScalar(string.Format("SELECT CASE WHEN EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}') THEN 1 ELSE 0 END", schema, tableName)) == 1;
		}

		/// <summary>
		/// Asynchronous version of <see cref="M:System.Data.Database.TableExists"/>, which returns whether the specified table exists in the database.
		/// </summary>
		/// <param name="tableName">The name of the table to check.</param>
		/// <param name="schema">The name of the schema to locate the table in.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task<bool> TableExistsAsync(string tableName, string schema = "dbo")
		{
			if (string.IsNullOrWhiteSpace(tableName))
			{
				throw new ArgumentNullException(nameof(tableName));
			}

			if (string.IsNullOrWhiteSpace(schema))
			{
				throw new ArgumentNullException(nameof(schema));
			}

			Contract.EndContractBlock();

			return (int)(await this.ExecuteScalarAsync(string.Format("SELECT CASE WHEN EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}') THEN 1 ELSE 0 END", schema, tableName))) == 1;
		}

		/// <summary>
		/// Returns whether the specified trigger exists in the database.
		/// </summary>
		/// <param name="triggerName">The name of the trigger to check.</param>
		/// <param name="schema">The name of the schema to locate the trigger in.</param>
		/// <returns><c>true</c> if a trigger with the specified name exists; otherwise, <c>false</c>.</returns>
		public bool TriggerExists(string triggerName, string schema = "dbo")
		{
			if (string.IsNullOrWhiteSpace(triggerName))
			{
				throw new ArgumentNullException("triggerName");
			}

			if (string.IsNullOrWhiteSpace(schema))
			{
				throw new ArgumentNullException("schema");
			}

			Contract.EndContractBlock();

			return
				(int)
				this.ExecuteScalar(
					string.Format(
						"SELECT CASE WHEN EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'TR')) THEN 1 ELSE 0 END",
						schema,
						triggerName)) == 1;
		}

		/// <summary>
		/// Asynchronous version of <see cref="M:System.Data.Database.TriggerExists"/>, which returns whether the specified trigger exists in the database.
		/// </summary>
		/// <param name="triggerName">The name of the trigger to check.</param>
		/// <param name="schema">The name of the schema to locate the trigger in.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task<bool> TriggerExistsAsync(string triggerName, string schema = "dbo")
		{
			if (string.IsNullOrWhiteSpace(triggerName))
			{
				throw new ArgumentNullException(nameof(triggerName));
			}

			if (string.IsNullOrWhiteSpace(schema))
			{
				throw new ArgumentNullException(nameof(schema));
			}

			Contract.EndContractBlock();

			return
				(int)
				(await this.ExecuteScalarAsync(
					string.Format(
						"SELECT CASE WHEN EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'TR')) THEN 1 ELSE 0 END",
						schema,
						triggerName))) == 1;
		}

        /// <summary>
		/// Executes the stored procedure that we parsed with the neccesarry parameters
		/// </summary>
		/// <param name="storedProcedure">The Stored Procedure to execute</param>
		/// <param name="parameters">The MySQL Parameters we need</param>
		/// <returns>A dataset</returns>
		public DataSet CallStoredProcedure(string storedProcedure, params MySqlParameter[] parameters)
        {
            if (storedProcedure == null)
            {
                throw new ArgumentNullException("storedProcedure");
            }

            Contract.EndContractBlock();

            using (var connection = new MySqlConnection(this.connectionString.ConnectionString))
            {
                using (var command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedure;
                    command.CommandTimeout = this.commandTimeout;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        var ds = new DataSet();

                        connection.Open();

                        adapter.Fill(ds);

                        return ds;
                    }
                }
            }
        }

        /// <summary>
        /// Executes the stored procedure that we parsed with the neccesarry parameters
        /// </summary>
        /// <param name="storedProcedure">The Stored Procedure to execute</param>
        /// <param name="parameters">The MySQL Parameters we need</param>
        /// <returns>The number of affected rows</returns>
        public int CallNonStoredProcedure(string storedProcedure, params MySqlParameter[] parameters)
        {
            if (storedProcedure == null)
            {
                throw new ArgumentNullException("storedProcedure");
            }

            Contract.EndContractBlock();

            using (var connection = new MySqlConnection(this.connectionString.ConnectionString))
            {
                using (var command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedure;
                    command.CommandTimeout = this.commandTimeout;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    command.CommandTimeout = this.commandTimeout;

                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes the stored procedure that we parsed with the neccesarry parameters
        /// </summary>
        /// <param name="storedProcedure">The Stored Procedure to execute</param>
        /// <param name="parameters">The MySQL Parameters we need</param>
        /// <returns>the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.</returns>
        public object CallScalarStoredProcedure(string storedProcedure, params MySqlParameter[] parameters)
        {
            if (storedProcedure == null)
            {
                throw new ArgumentNullException("storedProcedure");
            }

            Contract.EndContractBlock();

            using (var connection = new MySqlConnection(this.connectionString.ConnectionString))
            {
                using (var command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedure;
                    command.CommandTimeout = this.commandTimeout;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    command.CommandTimeout = this.commandTimeout;

                    connection.Open();

                    return command.ExecuteScalar();
                }
            }
        }
        #endregion
    }
}
