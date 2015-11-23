using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    [DataObject]
    [XmlRoot("sesion")]
    [Serializable()]
    public class Session
    {

        private int _id; 
        private Guid _sid;
        private Guid _uid;
        private string _username;
        private DateTime _expiry;

        //browser settings
        private string _LOGON_USER;
        private string _AUTH_TYPE;
        private string _REMOTE_ADDR;
        private string _REMOTE_HOST;
        private string _SERVER_NAME;
        private string _SERVER_PORT;
        private string _SERVER_PROTOCOL;
        private string _SERVER_SOFTWARE;

        [XmlIgnore(), NonSerialized()]
        private static Exception _LastError;

        #region "Public Properties"
        /// <summary>
        /// Get/Set ID
        /// </summary>
        [XmlElement(ElementName = "id", DataType = "int")]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Get/Set the session's identifier
        /// </summary>
        public Guid SID
        {
            get { return _sid; }
            set { _sid = value; }
        }
        /// <summary>
        /// Get/Set the user's identifier
        /// </summary>
        public Guid UID
        {
            get { return _uid; }
            set { _uid = value; }
        }
        /// <summary>
        /// Get/Set the user's login name
        /// </summary>
        [XmlElement(ElementName = "username", DataType = "string")]
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// Get/Set the date stored for the UTC data/time of expiry for the http cookie expiration
        /// </summary>
        public System.DateTime Expiry
        {
            get { return _expiry; }
            set { _expiry = value; }
        }

        /// <summary>
        /// Get/Set ServerVariable "LOGON_USER"
        /// </summary>
        public string LOGON_USER
        {
            get { return _LOGON_USER; }
            set { _LOGON_USER = value; }
        }
        /// <summary>
        /// Get/Set ServerVariable "AUTH_TYPE" 
        /// </summary>
        public string AUTH_TYPE
        {
            get { return _AUTH_TYPE; }
            set { _AUTH_TYPE = value; }
        }
        /// <summary>
        /// Get/Set ServerVariable "REMOTE_ADDR"
        /// </summary>
        public string REMOTE_ADDR
        {
            get { return _REMOTE_ADDR; }
            set { _REMOTE_ADDR = value; }
        }
        /// <summary>
        /// Get/Set ServerVariable "REMOTE_HOST"
        /// </summary>
        public string REMOTE_HOST
        {
            get { return _REMOTE_HOST; }
            set { _REMOTE_HOST = value; }
        }
        /// <summary>
        /// Get/Set ServerVariable "SERVER_NAME"
        /// </summary>
        public string SERVER_NAME
        {
            get { return _SERVER_NAME; }
            set { _SERVER_NAME = value; }
        }
        /// <summary>
        /// Get/Set ServerVariable "SERVER_PORT"
        /// </summary>
        public string SERVER_PORT
        {
            get { return _SERVER_PORT; }
            set { _SERVER_PORT = value; }
        }
        /// <summary>
        /// Get/Set ServerVariable "SERVER_PROTOCOL" 
        /// </summary>
        public string SERVER_PROTOCOL
        {
            get { return _SERVER_PROTOCOL; }
            set { _SERVER_PROTOCOL = value; }
        }
        /// <summary>
        /// Get/Set ServerVariable "SERVER_SOFTWARE"
        /// </summary>
        public string SERVER_SOFTWARE
        {
            get { return _SERVER_SOFTWARE; }
            set { _SERVER_SOFTWARE = value; }
        }

        /// <summary>
        /// Returns the Last Error that was encountered
        /// </summary>
        public static Exception GetLastError
        {
            get { return _LastError; }
        }
        #endregion

        #region "Constructor and Destructors"
        /// <summary>
        /// Instanicate New Session
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Session()
        {
	        SetBase();
        }
        /// <summary>
        /// Instanicate New Session for [sid] and [uid]
        /// </summary>
        /// <param name="constring">as data source connection string</param>
        /// <param name="sid">as session identifier</param>
        /// <param name="uid">as user's identifier</param>
        /// <remarks></remarks>
        public Session(string constring, Guid sid, Guid uid)
        {
	        SetBase();
	        Load(constring, sid, uid);
        }
        /// <summary>
        /// Instanicate New Session for [sid] and [username]
        /// </summary>
        /// <param name="constring">as data source connection string</param>
        /// <param name="sid">as session identifier</param>
        /// <param name="username">as user's login</param>
        /// <remarks></remarks>
        public Session(string constring, Guid sid, string username)
        {
	        SetBase();
	        Load(constring, sid, username);
        }
        /// <summary>
        /// Instanciate New Session
        /// </summary>
        /// <param name="constring">as data source connection string</param>
        /// <param name="dr">as datarow</param>
        /// <remarks></remarks>
        private Session(string constring, System.Data.DataRow dr)
        {
	        try 
            {
                if (dr["ID"] != System.DBNull.Value) { _id = Convert.ToInt32(dr["ID"].ToString()); }
                if (dr["SID"] != System.DBNull.Value) { _sid = new Guid(dr["SID"].ToString()); }
                if (dr["UID"] != System.DBNull.Value) { _uid = new Guid(dr["UID"].ToString()); }
                if (dr["UserName"] != System.DBNull.Value) { _username = (String)dr["UserName"]; }
                if (dr["sesExpiry"] != System.DBNull.Value) { _expiry = (DateTime)(dr["sesExpiry"]); }

		        //If Not IsDBNull([dr]("ID")) Then _LOGON_USER = dr("LOGON_USER")
		        //If Not IsDBNull([dr]("ID")) Then _AUTH_TYPE = dr("AUTH_TYPE")
		        //If Not IsDBNull([dr]("ID")) Then _REMOTE_ADDR = dr("REMOTE_ADDR")
		        //If Not IsDBNull([dr]("ID")) Then _REMOTE_HOST = dr("REMOTE_HOST")
		        //If Not IsDBNull([dr]("ID")) Then _SERVER_NAME = dr("SERVER_NAME")
		        //If Not IsDBNull([dr]("ID")) Then _SERVER_PROTOCOL = dr("SERVER_PROTOCOL")
		        //If Not IsDBNull([dr]("ID")) Then _SERVER_PORT = dr("SERVER_PORT")
		        //If Not IsDBNull([dr]("ID")) Then _SERVER_SOFTWARE = dr("SERVER_SOFTWARE")

	        } catch (Exception ex) {
		        _LastError = ex;
	        }
        }
        /// <summary>
        /// Set MyBase to String.Empty
        /// </summary>
        /// <remarks></remarks>
        private void SetBase()
        {
	        _LOGON_USER = string.Empty;
	        _AUTH_TYPE = string.Empty;
	        _REMOTE_ADDR = string.Empty;
	        _REMOTE_HOST = string.Empty;
	        _SERVER_NAME = string.Empty;
	        _SERVER_PORT = string.Empty;
	        _SERVER_PROTOCOL = string.Empty;
	        _SERVER_SOFTWARE = string.Empty;
        }
        /// <summary>
        /// Set MyBase to value of [dr]
        /// </summary>
        /// <param name="constring">as data source connection string</param>
        /// <param name="dr">as Data.DataRow</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool SetBase(string constring, System.Data.DataRow dr)
        {
	        try 
            {
                if (dr["ID"] != System.DBNull.Value) { _id = Convert.ToInt32(dr["ID"].ToString()); }
                if (dr["SID"] != System.DBNull.Value) { _sid = new Guid(dr["SID"].ToString()); }
                if (dr["UID"] != System.DBNull.Value) { _uid = new Guid(dr["UID"].ToString()); }
                if (dr["UserName"] != System.DBNull.Value) { _username = (String)dr["UserName"]; }
                if (dr["sesExpiry"] != System.DBNull.Value) { _expiry = (DateTime)(dr["sesExpiry"]); }

                //If Not IsDBNull([dr]("ID")) Then _LOGON_USER = dr("LOGON_USER")
                //If Not IsDBNull([dr]("ID")) Then _AUTH_TYPE = dr("AUTH_TYPE")
                //If Not IsDBNull([dr]("ID")) Then _REMOTE_ADDR = dr("REMOTE_ADDR")
                //If Not IsDBNull([dr]("ID")) Then _REMOTE_HOST = dr("REMOTE_HOST")
                //If Not IsDBNull([dr]("ID")) Then _SERVER_NAME = dr("SERVER_NAME")
                //If Not IsDBNull([dr]("ID")) Then _SERVER_PROTOCOL = dr("SERVER_PROTOCOL")
                //If Not IsDBNull([dr]("ID")) Then _SERVER_PORT = dr("SERVER_PORT")
                //If Not IsDBNull([dr]("ID")) Then _SERVER_SOFTWARE = dr("SERVER_SOFTWARE")

		        return true;
	        } catch (Exception ex) {
		        _LastError = ex;
		        return false;
	        }
        }
        #endregion
     
#region Functions and Sub Routines
        public override string ToString()
        {
            return _username.ToString();
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
        public static Boolean IsNumeric (System.Object Expression)
        {
            if(Expression == null || Expression is DateTime)
                return false;

            if(Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal 
                || Expression is Single || Expression is Double || Expression is Boolean)
                return true;
            try 
            {
                if(Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                    return true;
                } catch 
                {
                    // just dismiss errors but return false
                } 
                return false;
            }

        /// <summary>
        /// Summary: 
        ///   Authenticate session for [uid] and [sid]
        /// 
        /// Note:
        ///   If expiry less than current utc date time then return false.
        ///   If session id does not exist for user id then return false.
        ///   If uid does not exit for session id then return false.  
        /// </summary>
        /// <param name="constring">as data source conenction string</param>
        /// <param name="uid">as user's identifier</param>
        /// <param name="sid">as session's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_sessions]</TableName>
        public bool AuthenticateSession(string constring, Guid sid, Guid uid)
        {
            //get session from data source

            Session s = new Session(constring, sid, uid);
            //nothing means no session exists and users needs to login again
            if (s == null)
                return false;

            //expired session users needs to login again
            if (s.Expiry < DateTime.Now.ToUniversalTime())
            {
                Delete(constring, s.ID);
                return false;
            }
            else
            {
                //session is good so the sid is updated and old session deleted
                //these values need to be updated in user cookie
                _sid = Guid.NewGuid();
                _uid = uid;
                _username = s.UserName;
                _expiry = s.Expiry;

                //add new session 
                if (Add(constring))
                {
                    //remove old session 
                    Delete(constring, s.ID);
                    return true;
                }
                else
                {
                    //error adding new session 
                    return false;
                }
            }
        }
        /// <summary>
        /// Summary: 
        ///   Authenticate session for [username] and [sid]
        /// 
        /// Note:
        ///   If expiry less than current utc date time then return false.
        ///   If session id does not exist for user id then return false.
        ///   If uid does not exit for session id then return false.  
        /// </summary>
        /// <param name="constring">as data source conenction string</param>
        /// <param name="username">as user's login</param>
        /// <param name="sid">as session's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_sessions]</TableName>
        public bool AuthenticateSession(string constring, Guid sid, string username)
        {
            //get session from data source

            Session s = new Session(constring, sid, username);
            //nothing means no session exists and users needs to login again
            if (s == null)
                return false;
            if (s.ID <= 0)
                return false;

            //expired session users needs to login again
            if (s.Expiry < DateTime.Now.ToUniversalTime())
            {
                Delete(constring, s.ID);
                return false;
            }
            else
            {
                //session is good so the sid is updated and old session deleted
                //these values need to be updated in user cookie
                _sid = Guid.NewGuid();
                _uid = s.UID;
                _username = username;
                _expiry = s.Expiry;

                //add new session 
                if (Add(constring))
                {
                    //remove old session 
                    Delete(constring, s.ID);
                    return true;
                }
                else
                {
                    //error adding new session 
                    return false;
                }
            }
        }

        /// <summary>
        /// Load session for [uid] and [sid]
        /// </summary>
        /// <param name="constring">as data source conenction string</param>
        /// <param name="uid">as user's identifier</param>
        /// <param name="sid">as session's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_sessions]</TableName>
        public bool Load(string constring, Guid sid, Guid uid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(constring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_sessions`";
                        query += " WHERE `uid`=?uid AND `sid`=?sid";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?uid", MySqlDbType.VarChar, 50)).Value = uid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?sid", MySqlDbType.VarChar, 50)).Value = sid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblSession");

                            if (ds.Tables["tblSession"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return false;
                            }

                            return SetBase(constring, ds.Tables["tblSession"].Rows[0]);
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
        /// Load session for [username] and [sid]
        /// </summary>
        /// <param name="constring">as data source conenction string</param>
        /// <param name="username">as user's login</param>
        /// <param name="sid">as session's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_sessions]</TableName>
        public bool Load(string constring, Guid sid, string username)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(constring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_sessions`";
                        query += " WHERE `username`=?username AND `sid`=?sid";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?username", MySqlDbType.VarChar, 50)).Value = username;
                        sqlcomm.Parameters.Add(new MySqlParameter("?sid", MySqlDbType.VarChar, 50)).Value = sid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblSession");

                            if (ds.Tables["tblSession"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return false;
                            }

                            return SetBase(constring, ds.Tables["tblSession"].Rows[0]);
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
        /// Add Session to system 
        /// </summary>
        /// <param name="constring">as data source conenction string</param>
        /// <returns></returns>
        /// <TableName>[sessions]</TableName>
        public bool Add(string constring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(constring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addSession", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = _uid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?psid", MySqlDbType.VarChar, 50)).Value = _sid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pusername", MySqlDbType.VarChar, 50)).Value = _username;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pexpiry", MySqlDbType.Datetime)).Value = _expiry;


                        //if the return value is not numeric then there was an error in the stored procedures
                        string sqlresult = sqlcomm.ExecuteScalar().ToString();
                        if (IsNumeric(sqlresult))
                        {
                            _id = Convert.ToInt32(sqlresult);
                            return true;
                        }
                        else
                        {
                            _LastError = new Exception(sqlresult);
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
        /// Delete Session from system
        /// </summary>
        /// <param name="constring">as data source conenction string</param>
        /// <returns></returns>
        /// <TableName>[sessions]</TableName>
        public bool Delete(string constring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(constring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteSession", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pid", MySqlDbType.Int32)).Value = _id;

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
        /// Delete Session from system
        /// </summary>
        /// <param name="constring">as data source conenction string</param>
        /// <param name="id">as session id</param>
        /// <returns></returns>
        /// <TableName>[sessions]</TableName>
        public static bool Delete(string constring, int id)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(constring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteSession", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pid", MySqlDbType.Int32)).Value = id;

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