using System;
using System.Web;
using System.Linq;
using System.Data.Common;
//using System.Transactions;
using System.Configuration;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Collections.Specialized;

namespace Providers
{
    /// <summary>
    /// Generic Database Provider
    /// </summary>
    public partial class DbProvider : Provider
    {
        private string _constring;
        private string _parmPrefix;
        private string _tblPrefix;

        #region Public Properties

        #endregion

        #region Constructors and Destructors
        public DbProvider()
        {


        }
        #endregion

        #region Functions and Sub Routines
        /// <summary>
        /// Returns a formatted parameter name to include this DbRoleProvider instance's paramPrefix.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        private string FormatParamName(string parameterName)
        {
            return String.Format("{0}{1}", this._parmPrefix, parameterName);
        }
        /// <summary>
        /// Creates a new DbConnectionHelper for this DbBlogProvider instance.
        /// </summary>
        /// <returns></returns>
        private DbConnectionHelper CreateConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[_constring];
            return new DbConnectionHelper(settings);
        }

        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="name">Configuration name</param>
        /// <param name="config">Configuration settings</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
            {
                name = "DbBlogProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Generic Database Blog Provider");
            }

            base.Initialize(name, config);

            if (config["connectionStringName"] == null)
            {
                // default to BlogEngine
                config["connectionStringName"] = "BlogEngine";
            }

            this._constring = config["connectionStringName"];
            config.Remove("connectionStringName");

            if (config["tablePrefix"] == null)
            {
                // default
                config["tablePrefix"] = "be_";
            }

            this._tblPrefix = config["tablePrefix"];
            config.Remove("tablePrefix");

            if (config["parmPrefix"] == null)
            {
                // default
                config["parmPrefix"] = "@";
            }

            this._parmPrefix = config["parmPrefix"];
            config.Remove("parmPrefix");

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                var attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                {
                    throw new ProviderException(string.Format("Unrecognized attribute: {0}", attr));
                }
            }
        }

        /// <summary>
        /// Load date from DataStore
        /// </summary>
        /// <returns></returns>
        public bool LoadTest(int id)
        {
            bool breturn = false;
            using (var conn = this.CreateConnection())
            {
                if (conn.HasConnection)
                {
                    string query = string.Format("SELECT Settings FROM {0}DataStoreSettings WHERE BlogId = {1}blogid", this._tblPrefix, this._parmPrefix);
                    using (var cmd = conn.CreateTextCommand(query))
                    {
                        cmd.Parameters.Add(conn.CreateParameter("", id));

                        cmd.ExecuteScalar();

                        breturn = true;
                    }
                }                
            }

            return breturn;
        }

        #endregion
    }
}