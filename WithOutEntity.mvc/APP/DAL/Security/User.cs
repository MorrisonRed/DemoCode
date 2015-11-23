using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Text;
using System.Reflection;
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
    [XmlRoot("user")]
    [Serializable()]
    public class User
    {
        private int _id; 
        private Guid _uid;
        private Guid _appid;
        private String _username;
        private bool _isAnonymous;
        private DateTime _lastActivityDate;
        private DateTime _timestamp;

        private bool _isAuthenticated;

        private UserDemographics _demographics;
        private MembershipUser _membership;
        private Role _role; 

        [XmlIgnore(), NonSerialized()]
        private static Exception _LastError;

    #region Public Properties
        /// <summary>
        /// Summary:
        ///     Gets or sets the user's identifier
        ///     
        /// Returns:
        ///     membership user's identifier
        /// </summary>
        [XmlElement(ElementName = "uid")]
        public Guid UID {
	        get { return _uid; }
	        set { _uid = value; }
        }
        /// <summary>
        /// Summary: 
        ///     Gets or sets the assoicated application assinged to the current user.
        ///     
        /// Reurns:
        ///     Application-specific Identifier for the user.
        /// </summary>
        [XmlElement(ElementName = "appid")]
        public Guid APPID
        {
            get { return _appid; }
            set { _appid = value; }
        }
        /// <summary>
        /// Summary:
        ///     Gets or sets the current user's login name
        ///     
        /// Returns:
        ///     unique user's name in system
        /// </summary>
        [XmlElement(ElementName = "username", DataType = "string")]
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// Summary:
        ///     Gets or sets whether the current user is logging in anonymously
        ///     
        /// Returns:
        ///     User is currently logged in anonymous if true; otherwise false.
        /// </summary>
        [XmlElement(ElementName = "isanonymous", DataType = "boolean")]
        public bool IsAnonymous
        {
            get { return _isAnonymous ; }
            set { _isAnonymous  = value; }
        }
        /// <summary>
        /// Summary:
        ///     Gets/Sets the last data/time that the user did an action within the site
        ///     
        /// Returns:
        ///     The data/time that the users last performed an activity
        /// </summary>
        [XmlElement(ElementName="lastActivityDate")]
        public DateTime LastActivityDate
        {
            get { return _lastActivityDate; }
            set { value = _lastActivityDate; }
        }

        /// <summary>
        /// Summary: 
        ///  Gets or sets whether the current user's has been authenticated. As in whether the user 
        ///   has logged into the system via username and password, verse login cookie.  Note that 
        ///   this attribute is not stored in the database but only exist for the life time of the 
        ///   the session.
        /// 
        /// Returns: 
        ///   True is the user has been authenticated with username and password; otherwise false
        /// </summary>
        [XmlElement(ElementName = "isauthenticated")]
        public bool IsAuthenticiated
        {
            get { return _isAuthenticated; }
            set { _isAuthenticated = value; }
        }

        /// <summary>
        /// Get/Set the user's demographic information
        /// </summary>
        [XmlElement(ElementName="demographics")]
        public UserDemographics Demographics
        {
            get { return _demographics; }
            set { _demographics = value; }
        }
        /// <summary>
        /// Get/Set the Membership record assoicated with this user
        /// </summary>
        [XmlElement(ElementName = "membership")]
        public MembershipUser Membership
        {
            get { return _membership; }
            set { _membership = value; }
        }
        /// <summary>
        /// Get/Set the role assigned to this user
        /// </summary>
        [XmlElement(ElementName = "role")]
        public Role Role
        {
            get { return _role; }
            set { _role = value; }
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
        public int CompareTo(User other)
        {
            return _uid.CompareTo(other.UID);
        }
        /// <summary>
        /// Instanicate New User
        /// </summary>
        public User()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate New User for puid
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="puid">as user's identifier</param>
        public User(String pconstring, Guid puid)
        {
            _uid = puid;
            Load(pconstring, puid);
        }
        /// <summary>
        /// Instanciate New User from [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data.datarow</param>
        internal User(string pconstring, System.Data.DataRow dr)
        {
            try
            {
                if (dr["UID"] != System.DBNull.Value) { _uid = new Guid(dr["UID"].ToString()); }
                if (dr["APPID"] != System.DBNull.Value) { _appid = new Guid(dr["APPID"].ToString()); }
                if (dr["UserName"] != System.DBNull.Value) { _username = (String)dr["UserName"]; }
                if (dr["UserIsAnonymous"] != System.DBNull.Value) { _isAnonymous = Convert.ToBoolean(dr["UserIsAnonymous"]); }
                if (dr["UserLastActivityDate"] != System.DBNull.Value) { _lastActivityDate = (DateTime)dr["UserLastActivityDate"]; }
                if (dr["UserTimestamp"] != System.DBNull.Value) { _timestamp = (DateTime)dr["UserTimestamp"]; }


                _demographics = new UserDemographics(dr);
                //load membership information for the current application
                _membership = new MembershipUser(pconstring, _uid, _appid);
                //load role for user
                _role = Role.getUserRoleForApplication(pconstring, _uid, _appid);
            }
            catch (Exception ex)
            {
                _LastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBase to String.Empty
        /// </summary>
        private void SetBase()
        {
            _username = String.Empty;
            _demographics = new UserDemographics();
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
                if (dr["UID"] != System.DBNull.Value) {_uid = new Guid(dr["UID"].ToString());}
                if (dr["APPID"] != System.DBNull.Value) { _appid = new Guid(dr["APPID"].ToString()); }
                if (dr["UserName"] != System.DBNull.Value) { _username = (String)dr["UserName"]; }
                if (dr["UserIsAnonymous"] != System.DBNull.Value) { _isAnonymous  =  Convert.ToBoolean(dr["UserIsAnonymous"]); }
                if (dr["UserLastActivityDate"] != System.DBNull.Value) { _lastActivityDate = (DateTime)dr["UserLastActivityDate"]; }
                if (dr["UserTimestamp"] != System.DBNull.Value) { _timestamp = (DateTime)dr["UserTimestamp"]; }

                _demographics = new UserDemographics(dr);
                //load membership information for the current application
                _membership = new MembershipUser(pconstring, _uid, _appid);
                //load role for user
                _role = Role.getUserRoleForApplication(pconstring, _uid, _appid);

                return true;
            }
            catch (Exception ex)
            {
                _LastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBase to values of [data]
        /// </summary>
        /// <param name="data">as user</param>
        /// <returns></returns>
        internal Boolean SetBase(User data)
        {
            try
            {
                _uid = data.UID;
                _appid = data.APPID;
                _username = data.UserName;
                _isAnonymous = data.IsAnonymous;
                _lastActivityDate = data.LastActivityDate; 
                _timestamp = data._timestamp;

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

            Type type = typeof(User);
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
        /// Return true if the user object is considered empty by the system; otherwise false
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsEmpty()
        {
            bool bResult = false;
            if (_uid == null || _uid == Guid.Empty)
                bResult = true;
            if (string.IsNullOrEmpty(_username))
                bResult = true;

            return bResult;
        }

        /// <summary>
        /// Return the valid pswdhash for the designated [pusername] and application [pappid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pappid">as application to get password hash for</param>
        /// <param name="pusername">as username to get password hash for</param>
        /// <returns></returns>
        public static string getPasswordHash(string pconstring, Guid pappid, string pusername)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        sqlconn.Open();

                        string query = string.Empty;
                        query = "SELECT `MemPswd` FROM `vw_users_memberships`";
                        query += " WHERE `UserName`=?username AND `APPID`=?appid";
                        query += " LIMIT 0,1;";
                        sqlcomm.CommandText = query;

                        sqlcomm.Parameters.Add(new MySqlParameter("?appid", MySqlDbType.VarChar, 50)).Value = pappid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?username", MySqlDbType.VarChar, 50)).Value = pusername;

                        string pswdhash = (String)sqlcomm.ExecuteScalar();
                        return pswdhash;
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        throw ex;
                    }  
                }
            }
        }
        /// <summary>
        /// SUMMARY:
        ///   Return the user's identifer and valid pswdhash as as pipe "|" delimited string 
        /// 
        /// NOTE:
        ///   uid|pswd
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pappid">as application to get password hash for</param>
        /// <param name="pusername">as username to get password hash for</param>
        /// <returns></returns>
        public static string getUIDAndPasswordHash(string pconstring, Guid pappid, string pusername)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        sqlconn.Open();

                        string query = string.Empty;
                        query = "SELECT `UID`, `MemPswd` FROM `vw_users_memberships`";
                        query += " WHERE `UserName`=?username AND `APPID`=?appid";
                        query += " LIMIT 0,1;";
                        sqlcomm.CommandText = query;

                        sqlcomm.Parameters.Add(new MySqlParameter("?appid", MySqlDbType.VarChar, 50)).Value = pappid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?username", MySqlDbType.VarChar, 50)).Value = pusername;

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0) { return null; }

                            string uidandpswd;

                            System.Data.DataRow dr = ds.Tables["tbl"].Rows[0];
                            uidandpswd = (string)dr[0];
                            uidandpswd += "|";
                            uidandpswd += (string)dr[1];

                            return uidandpswd;
                        }
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return null;
                        //throw ex;
                    }
                }
            }
        }
        
        /// <summary>
        /// Load User for [puid]
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <param name="puid">as user's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_usersdemographics]</TableName>
        public Boolean Load(String pconstring, Guid puid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_dgUserDemo_UID", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = puid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblUsers");

                            if (ds.Tables["tblUsers"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return false;
                            }

                            return SetBase(pconstring, ds.Tables["tblUsers"].Rows[0]);
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
        /// Register user in system, creating all required records for membership
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <returns></returns>
        /// <TableName>[users],[memberships],[roles]</TableName>
        public Boolean registerUser(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_registerUser", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pAppid", MySqlDbType.VarChar, 50)).Value =  _appid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pUsername", MySqlDbType.VarChar, 50)).Value =  _username.Trim().ToLower();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pisAnonymous", MySqlDbType.Bit)).Value =  (_isAnonymous == true) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?plastActivityDate", MySqlDbType.Date)).Value =  DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswd", MySqlDbType.VarChar, 128)).Value =  _membership.Password.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswdFormat", MySqlDbType.Int16)).Value =  _membership.PasswordFormat;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswdSalt", MySqlDbType.VarChar, 128)).Value = _membership.PasswordSalt.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEmail", MySqlDbType.VarChar, 255)).Value = _membership.Email.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswdQuestion", MySqlDbType.VarChar, 255)).Value = _membership.PasswordQuestion.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswdAnswer", MySqlDbType.VarChar, 128)).Value = _membership.PasswordAnswer.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pisApproved", MySqlDbType.Bit)).Value = (_membership.Approved == true) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pisLockedOut", MySqlDbType.Bit)).Value = (_membership.LockedOut == true) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCreatedDate", MySqlDbType.Date)).Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?pLastLoginDate", MySqlDbType.Date)).Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?pLastPswdChangeDate", MySqlDbType.Date)).Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?pComment", MySqlDbType.VarChar, 255)).Value = _membership.Comment.ToString();


                        //if the return value is not a guid then there was an error in the stored procedures
                        string sqlresult = (String)sqlcomm.ExecuteScalar();
                        Guid puid;
                        
                        if (TryStrToGuid(sqlresult, out puid) == true)
                        {
                            _uid = puid;
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
        /// Add User to system
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <returns></returns>
        /// <TableName>[users]</TableName>
        public Boolean Add(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addUser", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pAppid", MySqlDbType.VarChar, 50)).Value = _appid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pUsername", MySqlDbType.VarChar, 50)).Value = _username.Trim().ToLower();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pisAnonymous", MySqlDbType.Bit)).Value = (_isAnonymous == true) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?plastActivityDate", MySqlDbType.Date)).Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                        //if the return value is not a guid then there was an error in the stored procedures
                        _uid = (Guid)sqlcomm.ExecuteScalar();
                        if (_uid != null && _uid != Guid.Empty)
                        {
                            return true;
                        }
                        else
                        {
                            _LastError = new Exception("failed to add user to system");
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
        /// Update User in system
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <returns></returns>
        /// <TableName>[users]</TableName>
        public Boolean Update(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateUser", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = _uid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pappid", MySqlDbType.VarChar, 50)).Value = _appid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pUsername", MySqlDbType.VarChar, 50)).Value = _username.Trim().ToLower();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pisAnonymous", MySqlDbType.Bit)).Value = (_isAnonymous == true) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?plastActivityDate", MySqlDbType.Date)).Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

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
        /// Return DataTable of all users from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_usersdemographics]</TableName>
        public static System.Data.DataTable dgUsers(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_usersdemographics`";
                        query += " ORDER BY `UserLName` ASC, `UserFName` ASC;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblUsers");

                            if(ds.Tables["tblUsers"].Rows.Count == 0 )
                            {
                                _LastError = new Exception("no data found");
                                return new DataTable();
                            }

                            return ds.Tables["tblUsers"].Copy();
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
        /// Return DataTable of all users from system where pcolname contains pcolval
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as value to search on</param>
        /// <returns></returns>
        /// <TableName>[vw_usersdemographics]</TableName>
        public static System.Data.DataTable dgUsers(String pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_usersdemographics`";
                        query += string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim());
                        query += " ORDER BY `UserLName` ASC, `UserFName` ASC;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblUsers");

                            if (ds.Tables["tblUsers"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return new DataTable();
                            }

                            return ds.Tables["tblUsers"].Copy();
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

    #region Encryption and Decryption
        public static string Left(string text, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "length must be > 0");
            else if (length == 0 || text.Length == 0)
                return "";
            else if (text.Length <= length)
                return text;
            else
                return text.Substring(0, length);
        }
        /// <summary>
        /// Decrypt Elements
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool DecryptElements()
        {
            try
            {
                //If Not String.IsNullOrEmpty(_ds) Then _ds = DecryptElement(_ds)
                //If Not String.IsNullOrEmpty(_cat) Then _cat = DecryptElement(_cat)
                return true;
            }
            catch (Exception ex)
            {
                _LastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Encrypt Elements 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool EncryptElements()
        {
            try
            {
                //If Not String.IsNullOrEmpty(_ds) Then _ds = DecryptElement(_ds)
                //If Not String.IsNullOrEmpty(_cat) Then _cat = DecryptElement(_cat)
                return true;
            }
            catch (Exception ex)
            {
                _LastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Returns Encrypted String using Password Encryption Key 
        /// </summary>
        /// <param name="text">plain text</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string EncryptElement(string text)
        {
            return Encrypt(text, "&%#@?,:*");
        }
        /// <summary>
        /// Returns Decrypted String using Password Encryption Key
        /// </summary>
        /// <param name="text">encrypted text</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string DecryptElement(string text)
        {
            return Decrypt(text, "&%#@?,:*");
        }
        /// <summary>
        /// Returns Encypted String [text] using specified Encryption Key [EncryKey]
        /// </summary>
        /// <param name="text">plain text</param>
        /// <param name="EncryKey">encryption key</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string Encrypt(string text, string EncryKey)
        {
            byte[] byKey = {};
            byte[] IV = {0x12,0x34,0x56,0x78,0x90,0xab,0xcd,0xef};

            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(Left(EncryKey, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Returns Decrypted String [strWord] using specified Encryption Key [strEncryKey]
        /// </summary>
        /// <param name="text">encrypted text</param>
        /// <param name="EcryKey">decription key</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string Decrypt(string text, string EcryKey)
        {
            byte[] byKey = {};
            byte[] IV = {0x12,0x34,0x56,0x78,0x90,0xab,0xcd,0xef};
            byte[] inputByteArray = new byte[text.Length + 1];

            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(Left(EcryKey, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(text);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;

                return encoding.GetString(ms.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    #endregion 

    #region XML Functions
        /// <summary>
        /// Write User to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of User</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLSerializeToFile(string xmlpath)
        {
            try
            {
                if (EncryptElements())
                {
                    FileStream fs = default(FileStream);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    XmlSerializer xmlSer = default(XmlSerializer);

                    xmlSer = new XmlSerializer(typeof(User));
                    fs = new FileStream(xmlpath, FileMode.Create, FileAccess.Write);
                    xmlSer.Serialize(fs, this);
                    fs.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _LastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Create User from File
        /// </summary>
        /// <param name="xmlpath">as source location of User </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(User));
                User output = (User)xs.Deserialize(fs);
                if (SetBase(output))
                {
                    return DecryptElements();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _LastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Return Serialized String version of [data] Object
        /// </summary>
        /// <param name="data">as User</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(User data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(User));
                MemoryStream ms = new MemoryStream();
                StreamReader strReader = default(StreamReader);
                string output = null;

                xmlSer.Serialize(ms, data);
                ms.Position = 0;
                strReader = new StreamReader(ms);
                output = strReader.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                _LastError = ex;
                return null;
            }
        }
        /// <summary>
        /// Return Serialized String
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private string XMLSerializeToString()
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(User));
                MemoryStream ms = new MemoryStream();
                StreamReader strReader = default(StreamReader);
                string output = null;

                xmlSer.Serialize(ms, this);
                ms.Position = 0;
                strReader = new StreamReader(ms);
                output = strReader.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                _LastError = ex;
                return null;
            }
        }
        /// <summary>
        /// Return User from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of User</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static User XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(User));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                User output = default(User);
                output = (User)xmlSer.Deserialize(string_reader);
                return output;
            }
            catch (Exception ex)
            {
                _LastError = ex;
                return null;
            }
        }
    #endregion
    }
}
