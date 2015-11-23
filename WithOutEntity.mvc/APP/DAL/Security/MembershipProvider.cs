using System;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Configuration.Provider;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Xml.Serialization;
using MySql.Data.MySqlClient; 

//==========================================================================================
//User OBJECT
//
//==========================================================================================
//<XmlRoot("user")> _
//<Serialization.XmlTypeAttribute(Namespace:="http://services.morrisonred.com"), _
//Serialization.XmlRootAttribute("morrisonred", [Namespace]:="http://services.morrisonred.com", IsNullable:=False)> _
namespace CustomSecurity
{
    public class MembershipProvider 
    {
        private int _id;
        private Guid _appid;
        private string _appname;
        private string _desc;

        [XmlIgnore(), NonSerialized()]
        private static Exception _LastError;

        #region Public Properties
        /// <summary>
        /// Get/Set the application's unique identifier
        /// </summary>
        [XmlElement(ElementName="appid")]
        public Guid  ApplicationID
        {
            get { return _appid; }
            set { _appid = value; }
        }
        /// <summary>
        /// Gets/Sets the friendly name used to refer to the provider during configuration.
        /// </summary>
        [XmlElement(ElementName="appname", DataType="string")]
        public string Name
        {
            get { return _appname; }
            set { _appname = value; }
        }
        /// <summary>
        /// Get/Set a brief, friendly description suitable for display in administrative tools or other user interfaces (UIs).
        /// </summary>
        [XmlElement(ElementName="desc", DataType="string")]
        public string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }

        /// <summary>
        /// Get Last Error Thown
        /// </summary>
        [XmlIgnore()]
        public static Exception GetLastError
        {
            get { return _LastError; }
        }
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Instanciate Membership Provider
        /// </summary>
        public MembershipProvider()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate new Membership provider for [appid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pappid">as application's identifier</param>
        public MembershipProvider(string pconstring, Guid pappid)
        {
            SetBase();
            Load(pconstring, pappid);
        }
        /// <summary>
        /// Instanciate New Membership Provider from [dr]
        /// </summary>
        /// <param name="dr">as System.Data.DataRow</param>
        internal MembershipProvider(System.Data.DataRow dr)
        {
            try
            {
                if (ColumnExists("id", dr)) if (!(dr["id"] == null)) { _id = (Int32)dr["id"]; }
                if (ColumnExists("appid", dr)) if (!(dr["appid"] == null)) { _appid = new Guid(dr["appid"].ToString()); }
                if (ColumnExists("AppName", dr)) if (!(dr["AppName"] == null)) { _appname = (String)dr["AppName"]; }
                if (ColumnExists("AppDesc", dr)) if (!(dr["AppDesc"] == null)) { _desc = (String)dr["AppDesc"]; }
            }
            catch (Exception ex)
            {
                _LastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBase to values of string.empty 
        /// </summary>
        private void SetBase()
        {
                 _appname  = string.Empty;
                 _desc = string.Empty; 
        }
        /// <summary>
        /// Set MyBase to values of [dr]
        /// </summary>
        /// <param name="dr">as system.data.datarow</param>
        /// <returns></returns>
        internal Boolean SetBase(System.Data.DataRow dr)
        {
            try
            {
                if (ColumnExists("id", dr)) if (!(dr["id"] == null)) { _id = (Int32)dr["id"]; }
                if (ColumnExists("appid", dr)) if (!(dr["appid"] == null)) { _appid = new Guid(dr["appid"].ToString()); }
                if (ColumnExists("AppName", dr)) if (!(dr["AppName"] == null)) { _appname = (String)dr["AppName"]; }
                if (ColumnExists("AppDesc", dr)) if (!(dr["AppDesc"] == null)) { _desc = (String)dr["AppDesc"]; }
                return true;
            }
            catch (Exception ex)
            {
                _LastError = ex;
                throw ex;
            }
        }
        #endregion

        #region Functions and Sub Routines
        public override string ToString()
        {
            return _appname.ToString();
        }
        private bool ColumnExists(string col, System.Data.DataTable table)
        {
            foreach (System.Data.DataColumn c in table.Columns)
            {
                if (c.ColumnName == col)
                {
                    // The column name exists
                    return true;
                }
            }
            // if you make it here, the column doesn't exist yet
            return false;
        }
        private bool ColumnExists(string col, System.Data.DataRow dr)
        {
            if (dr.Table.Columns.Contains(col))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Load Application Provider for [pappid]
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <param name="pappid">as appicaiton's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_applications]</TableName>
        public Boolean Load(String pconstring, Guid pappid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("SELECT * FROM vw_applications ");
                        sb.Append(" WHERE appid=?appid;");
                        sqlcomm.CommandText = sb.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?appid", MySqlDbType.VarChar, 50)).Value =  pappid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblApps");

                            if (ds.Tables["tblApps"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return false;
                            }

                            return SetBase(ds.Tables["tblApps"].Rows[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Add application to system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[applications]</TableName>
        public Boolean Add(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addApplication", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("pname", _appname.Trim()));
                        sqlcomm.Parameters.Add(new MySqlParameter("pdesc", _desc.Trim()));

                        _appid = (Guid)sqlcomm.ExecuteScalar();
                        if (_appid != null && _appid != Guid.Empty)
                        {
                            return true;
                        }
                        else
                        {
                            _LastError = new Exception("failed to add application to system");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Update application in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[applications]</TableName>
        public Boolean Update(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateApplication", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("pappid", _appname.Trim()));
                        sqlcomm.Parameters.Add(new MySqlParameter("pname", _appname.Trim()));
                        sqlcomm.Parameters.Add(new MySqlParameter("pdesc", _desc.Trim()));

                        sqlcomm.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return false;
                    }
                }
            }
        }
        #endregion
    }
}
