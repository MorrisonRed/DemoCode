using System;
using System.Web;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Collections.Generic;
using System.Configuration.Provider;


namespace Providers
{
    /// <summary>
    /// Summary description for DbConnectionHelper
    /// </summary>
    public class DbConnectionHelper : IDisposable
    {
        private DbConnection _conn;
        private bool _hasConnection;
        private DbProviderFactory _dbProvFactory;

		private bool isDisposed;

        #region Public Properties
        /// <summary>
        /// Returns the DbConnection of this instance.
        /// </summary>
        public DbConnection Connection
        {
            get { return this._conn; }
        }
        /// <summary>
        /// Gets whether the Connection of this ConnectionHelper instance is null.
        /// </summary>
        public bool HasConnection
        {
            get { return this._hasConnection; }
        }
        /// <summary>
        /// Gets the DbProviderFactory used by this ConnectionHelper instance.
        /// </summary>
        public DbProviderFactory Provider
        {
            get { return this._dbProvFactory; }
        }
        #endregion
		
        #region Construtors and Destructors
        /// <summary>
		/// Creates as new DbConnectionHelper instance from teh given ConnectionStringSettings.
		/// </summary>
		/// <param name="settings"></param>
        public DbConnectionHelper(ConnectionStringSettings settings) : this(settings.ProviderName, settings.ConnectionString)
        {
            
        }
		/// <summary>
		/// Creates a new DbConnectionHelper instance from the given provider name and database connection string..
		/// </summary>
		/// <param name="providerName"></param>
		/// <param name="connectionString"></param>
        public DbConnectionHelper(string providerName, string connectionString)
        {
            this._dbProvFactory = DbProviderFactories.GetFactory(providerName);
            this._conn = this._dbProvFactory.CreateConnection();

            this._hasConnection = (this._conn != null);
            if (this._hasConnection)
            {
                this._conn.ConnectionString = connectionString;
                this._conn.Open();
            }
        }


		/// <summary>
		/// Disposes this DbConnectionHelper and its underlying connection
		/// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
		/// <summary>
		/// Dispose of DbConnectionHelper
		/// </summary>
		/// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            try
            {
                if (!this.isDisposed && disposing)
                {
                    if (this._conn != null)
                    {
                        this._conn.Dispose();
                    }
                }
            }
            finally
            {
                this._dbProvFactory = null;
                this._conn = null;
                this._hasConnection = false;
                this.isDisposed = true;
            }
        }
        /// <summary>
        /// Check where DbConnectionHelper is instanicated
        /// </summary>
        private void CheckedDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("DbConnectionHelper");
            }
        }
        #endregion


        #region Functions and Sub Routines
		/// <summary>
		/// Return new DbCommand from this Connection
		/// </summary>
		/// <returns></returns>
        public DbCommand CreateCommand()
        {
            this.CheckedDisposed();
            return this.Connection.CreateCommand();
        }
		/// <summary>
		/// Return new DbCommand with the given command tet.  CommandType is automatically set to CommandType.Text.
		/// </summary>
		/// <param name="commandText"></param>
		/// <returns></returns>
        public DbCommand CreateTextCommand(string commandText)
        {
            var comm = this.CreateCommand();
            comm.CommandText = commandText;
            comm.CommandType = CommandType.Text;
            return comm;
        }

        /// <summary>
        /// Uses this ConnectionHelper's Provider to create a DbParameter instance with the given parameter name and value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="dbType">The DB type.</param>
        /// <param name="size">The size/length of the parameter.</param>
        /// <returns></returns>
		public DbParameter CreateParameter(string parameterName, object value, DbType? dbType, int? size)
		{
            this.CheckedDisposed();

            var param = this.Provider.CreateParameter();

            if (param == null)
            {
                throw new NullReferenceException("DbProvider");
            }
            else 
            {
                param.ParameterName = parameterName;
                param.Value = value;
                if (dbType.HasValue)
                    param.DbType = dbType.Value;
                if (size.HasValue)
                    param.Size = size.Value;

                return param;
            }
		}
        /// <summary>
        /// Uses this ConnectionHelper's Provider to create a DbParameter instance with the given parameter name and value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="dbType">The DB type.</param>
        /// <returns></returns>
        public DbParameter CreateParameter(string parameterName, object value, DbType dbType)
        {
            return CreateParameter(parameterName, value, dbType, null);
        }
        /// <summary>
        /// Uses this ConnectionHelper's Provider to create a DbParameter instance with the given parameter name and value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns></returns>
        public DbParameter CreateParameter(string parameterName, object value)
        {
            return CreateParameter(parameterName, value, null, null);
        }
        #endregion
    }
}