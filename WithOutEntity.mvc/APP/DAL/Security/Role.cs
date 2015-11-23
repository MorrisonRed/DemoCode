using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;

//==========================================================================================
//Role User OBJECT
//
//==========================================================================================
//<XmlRoot("membershipuser")> _
//<Serialization.XmlTypeAttribute(Namespace:="http://services.morrisonred.com"), _
//Serialization.XmlRootAttribute("morrisonred", [Namespace]:="http://services.morrisonred.com", IsNullable:=False)> _
namespace CustomSecurity
{
    [DataObject]
    [XmlRoot("role")]
    [Serializable()]
    public class Role
    {
        private int _id;
        private Guid _appid; 
        private Guid _roleid;
        private string _name;
        private string _desc;
        private Boolean _issys;
        private DateTime _timestamp;

        private MembershipProvider _app;

        [XmlIgnore(), NonSerialized()]
        private static Exception _LastError;

        #region Public Properties
        /// <summary>
        /// Get/Set the role's uniqe identifier
        /// </summary>
        [Display (Name="Role Id")]
        [XmlElement(ElementName = "roleid")]
        public Guid RoleID
        {
            get { return _roleid; }
            set { _roleid = value; }
        }
        /// <summary>
        /// Get/Set the assoicated application that this roles is assinged to 
        /// </summary>
        [Display(Name = "Application Id")]
        [XmlElement(ElementName = "appid")]public Guid APPID
        {
            get { return _appid; }
            set { _appid = value; }
        }
        /// <summary>
        /// Get Set the name of the role
        /// </summary>
        [Display(Name = "Role Name")]
        [XmlElement(ElementName = "name", DataType = "string")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Get/Set the description of the role
        /// </summary>
        [Display(Name = "Role Description")]
        [XmlElement(ElementName = "desc", DataType = "string")]
        public string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }
        /// <summary>
        /// Get/Set whether the role has system rights
        /// </summary>
        [Display(Name = "Is System")]
        [XmlElement(ElementName = "issys")]
        public bool IsSystem
        {
            get { return _issys; }
            set { _issys = value; }
        }

        /// <summary>
        /// Get/Set the assoicated application (membership provider) assoicated with this role
        /// </summary>
        [XmlElement(ElementName = "app")] 
        public MembershipProvider Application
        {
            get { return _app; }
            set { _app = value; }
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
        /// Instaciate New Role
        /// </summary>
        public Role()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate new Role for roleid
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="proleid">as role's identifier</param>
        public Role(string pconstring, Guid proleid)
        {
            SetBase();
            Load(pconstring, proleid);
        }
        /// <summary>
        /// Set MyBase to values of dr
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data.datarow</param>
        internal Role(string pconstring, System.Data.DataRow dr)
        {
            try
            {
                if (!(dr["APPID"] == null)) { _appid = new Guid(dr["APPID"].ToString()); }
                if (!(dr["RoleID"] == null)) { _roleid = new Guid(dr["RoleID"].ToString()); }
                if (!(dr["RoleName"] == null)) { _name = (String)dr["RoleName"]; }
                if (!(dr["RoleDescription"] == null)) { _desc = (String)dr["RoleDescription"]; }
                if (!(dr["RoleisSys"] == null)) { _issys = Convert.ToBoolean(dr["RoleisSys"]); }
                if (!(dr["RoleTimestamp"] == null)) { _timestamp = (DateTime)dr["RoleTimestamp"]; }

                _app = new MembershipProvider(dr);
            }
            catch (Exception ex)
            {
                _LastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBase to values of String.Empty
        /// </summary>
        private void SetBase()
        {
            _name = string.Empty;
            _desc = string.Empty;
        }
        /// <summary>
        /// Set MyBase to values of [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data.datarow</param>
        /// <returns></returns>
        internal Boolean SetBase(string pconstring, System.Data.DataRow dr)
        {
            try 
            {
                if (!(dr["APPID"] == null)) { _appid = new Guid(dr["APPID"].ToString()); }
                if (!(dr["RoleID"] == null)) { _roleid = new Guid(dr["RoleID"].ToString()); }
                if (!(dr["RoleName"] == null)) { _name = (String)dr["RoleName"]; }
                if (!(dr["RoleDescription"] == null)) { _desc = (String)dr["RoleDescription"]; }
                if (!(dr["RoleisSys"] == null)) { _issys = Convert.ToBoolean(dr["RoleisSys"]); }
                if (!(dr["RoleTimestamp"] == null)) { _timestamp = (DateTime)dr["RoleTimestamp"]; }
                
                _app = new MembershipProvider(dr);

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
        /// <summary>
        /// Returns the role common name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _name;
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
        private Boolean TryStrToGuid(String s, out Guid value)
        {
            try
            {
                value = new Guid(s);
                return true;
            }
            catch (FormatException)
            {
                value = Guid.Empty;
                return false;
            }
        }
        private string DBValueToString(object value)
        {
            if (value != System.DBNull.Value) { return value.ToString(); }
            return "";
        }
        /// <summary>
        /// Return current project as Data Table
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            Type type = typeof(Role);
            var properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                try
                {
                    info.GetValue(this, null);
                    dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                    dt.Rows[0][info.Name] = info.GetValue(this, null);
                }
                catch
                {

                }
            }

            return dt;
        }

        /// <summary>
        /// Load Role [roleid] 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="proleid">as role's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_roles_applications]</TableName>
        public Boolean Load(string pconstring, Guid proleid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += " SELECT * FROM `vw_roles_applications`";
                        query += " WHERE `roleid`=?roleid;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?roleid", proleid.ToString()));

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblRoles");

                            if (ds.Tables["tblRoles"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return false;
                            }

                            return SetBase(pconstring, ds.Tables["tblRoles"].Rows[0]);
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
        /// Add Role to system
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <returns></returns>
        /// <TableName>[roles]</TableName>
        public Boolean Add(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addRole", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pAPPID", MySqlDbType.VarChar, 50)).Value = _appid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pName", MySqlDbType.VarChar, 255)).Value = _name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDesc", MySqlDbType.VarChar, 255)).Value = _desc.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pisSYS", MySqlDbType.Bit)).Value = _issys ? 1 : 0;

                        //if the return value is not a guid then there was an error in the stored procedures
                        
                        string strReturn = DBValueToString(sqlcomm.ExecuteScalar());
                        if (!string.IsNullOrEmpty(strReturn))
                        {
                            try
                            {
                                _roleid = new Guid(strReturn);
                                return true;
                            }
                            catch
                            {
                                _LastError = new Exception(strReturn);
                                return false;
                            }
                        }
                        else
                        {
                            _LastError = new Exception("unkown error");
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
        /// Update Role in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[roles]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateRole", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pRoleID", MySqlDbType.VarChar, 50)).Value = _roleid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pAPPID", MySqlDbType.VarChar, 50)).Value = _appid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pName", MySqlDbType.VarChar, 255)).Value =  _name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDesc", MySqlDbType.VarChar, 255)).Value = _desc.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pisSYS", MySqlDbType.Bit)).Value = _issys ? 1 : 0;

                        //if the return value is not empty then there was an error in the stored procedures
                        string strReturn = DBValueToString(sqlcomm.ExecuteScalar());
                        if (string.IsNullOrEmpty(strReturn))
                        {
                            return true;
                        }
                        else
                        {
                            _LastError = new Exception(strReturn);
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
        /// Assign user [uid] to role [roleid]
        /// </summary>
        /// <param name="pconstring">as data source conneciton string</param>
        /// <param name="proleid">as role identifier</param>
        /// <param name="puid">as user's identifier</param>
        /// <returns></returns>
        /// <TableName>[users_roles]</TableName>
        public static Boolean AddUserToRole(String pconstring, Guid puid, Guid proleid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addUserToRole", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pRoleID", MySqlDbType.VarChar, 50)).Value = proleid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pUID", MySqlDbType.VarChar, 50)).Value = puid.ToString();

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
        /// <summary>
        /// Remove user [uid] from role [roleid]
        /// </summary>
        /// <param name="pconstring">as data source conneciton string</param>
        /// <param name="proleid">as role identifier</param>
        /// <param name="puid">as user's identifier</param>
        /// <returns></returns>
        /// <TableName>[users_roles]</TableName>
        public static Boolean RemoveUserFromRole(String pconstring, Guid puid, Guid proleid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("DELETE FROM `users_roles`");
                        query.Append(" WHERE `roleid`=?pRoleID AND `uid`=?pUID;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pRoleID", MySqlDbType.VarChar, 50)).Value = proleid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pUID", MySqlDbType.VarChar, 50)).Value = puid.ToString();

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

        /// <summary>
        /// Return role assoicated with user's account
        /// </summary>
        /// <param name="pconstring">as datasource connection string</param>
        /// <param name="puid">as user's unique identifier</param>
        /// <param name="pappid">as application's identifier</param>
        /// <returns>[vw_roles_users]</returns>
        public static Role getUserRoleForApplication(string pconstring, Guid puid, Guid pappid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += " SELECT * FROM `vw_roles_users`";
                        query += " WHERE `uid`=?uid AND `appid`=?appid";
                        query += " ORDER BY `RoleName` ASC;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?uid", MySqlDbType.VarChar, 50)).Value = puid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?appid", MySqlDbType.VarChar, 50)).Value = pappid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblRoles");

                            if (ds.Tables["tblRoles"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return null;
                            }

                            return new Role(pconstring, ds.Tables["tblRoles"].Rows[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Return DataTable of all roles in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_roles_applications]</TableName>
        public static List<Role> ToList(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_roles_applications`";
                        query += " ORDER BY `RoleName` ASC, `AppName` ASC;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return null;
                            }

                            List<Role> lst = new List<Role>();
                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            {
                                lst.Add(new Role(pconstring, dr));
                            }
                            return lst;
                        }
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Return DataTable of all roles in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_roles_applications]</TableName>
        public static System.Data.DataTable dgRoles(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_roles_applications`";
                        query += " ORDER BY `RoleName` ASC, `AppName` ASC;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblRoles");

                            if (ds.Tables["tblRoles"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return null;
                            }

                            return ds.Tables["tblRoles"].Copy();
                        }
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return null;
                    }
                }
            }
        }
        /// <summary>
        /// Return Data Table of roles where [colname] contains [colval]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as value to search on</param>
        /// <returns></returns>
        /// <TableName>[vw_roles_applications]</TableName>
        public static System.Data.DataTable dgRoles(String pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_roles_applications`");
                        query.Append(string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
                        query.Append(" ORDER BY `RoleName` ASC, `AppName` ASC;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblRoles");

                            if (ds.Tables["tblRoles"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return null;
                            }

                            return ds.Tables["tblRoles"].Copy();
                        }
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return null;
                    }
                }
            }
        }
        #endregion
    }
}
