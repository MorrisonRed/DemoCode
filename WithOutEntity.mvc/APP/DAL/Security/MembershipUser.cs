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
using System.Runtime.CompilerServices;

//==========================================================================================
//Membership User OBJECT
//
//==========================================================================================
//<XmlRoot("membershipuser")> _
//<Serialization.XmlTypeAttribute(Namespace:="http://services.morrisonred.com"), _
//Serialization.XmlRootAttribute("morrisonred", [Namespace]:="http://services.morrisonred.com", IsNullable:=False)> _
namespace CustomSecurity
{
    /// <summary>
    /// Exposes and updates membership user information in the membership data store.
    /// </summary>
    [DataObject]
    [XmlRoot("membershipuser")]
    [Serializable()]
    public class MembershipUser
    {
        private Guid _uid;
        private Guid _appid;
        private string _pswd;
        private int _pswdformat;
        private string _pswdsalt;
        private string _email;
        private string _pswdQuestion;
        private string _pswdAnswer;
        private Boolean _isApproved;
        private Boolean _isLockedOut;
        private DateTime _createdDate;
        private DateTime _lastLoginDate;
        private DateTime _lastPswdChangeDate;
        private DateTime _lastLockoutDate;
        private int _failedPswdAttemptCount;
        private DateTime _failedPswdAttemptWindowStart;
        private int _failedPswdAnswerAttemptCount;
        private DateTime _failedPswdAnswerAttemptWindowStart;
        private string _comment;

        [XmlIgnore(), NonSerialized()]
        private static Exception _LastError;

        #region Public Properties
        /// <summary>
        /// Get/Set the assoicated user's id with this membership
        /// </summary>
        [XmlElement(ElementName="uid")]
        public Guid UID
        {
            get { return _uid; }
            set { _uid = value; }
        }
        /// <summary>
        /// Get/Set the assoicated application's (membership provider) with this membership
        /// </summary>
        [XmlElement(ElementName = "appid")]
        public Guid APPID
        {
            get { return _appid; }
            set { _appid = value; }
        }
        /// <summary>
        /// Get/Set the assoicated password with this membership account
        /// </summary>
        [XmlElement(ElementName = "pswd", DataType = "string")]
        public string Password
        {
            get { return _pswd; }
            set { _pswd = value; }
        }
        /// <summary>
        /// Get/Set the password format
        /// </summary>
        [XmlElement(ElementName = "pswdformat", DataType = "string")]
        public int PasswordFormat
        {
            get { return _pswdformat; }
            set { _pswdformat = value; }
        }
        /// <summary>
        /// Get/Set the assoicated password salt with this password
        /// </summary>
        [XmlElement(ElementName = "pswdsalt", DataType = "string")]
        public string PasswordSalt
        {
            get { return _pswdsalt; }
            set { _pswdsalt = value; }
        }
        /// <summary>
        /// Get/Set the assoicated e-mail account with membership
        /// </summary>
        [XmlElement(ElementName = "email", DataType = "string")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        /// Get/Set the password recovery question for this membership
        /// </summary>
        [XmlElement(ElementName = "pswdquestion", DataType = "string")]
        public string PasswordQuestion
        {
            get { return _pswdQuestion; }
            set { _pswdQuestion = value; }
        }
        /// <summary>
        /// Get/Set the password recovery answer for this membership
        /// </summary>
        [XmlElement(ElementName = "pswdanswer", DataType = "string")]
        public string PasswordAnswer
        {
            get { return _pswdAnswer; }
            set { _pswdAnswer = value; }
        }
        /// <summary>
        /// Get/Set whether the membership has been approved
        /// </summary>
        [XmlElement(ElementName = "isapproved", DataType = "boolean")]
        public Boolean Approved
        {
            get { return _isApproved; }
            set { _isApproved = value; }
        }
        /// <summary>
        /// Get/Set whether the membership account is locked out.
        /// </summary>
        [XmlElement(ElementName = "islockedout", DataType = "boolean")]
        public Boolean LockedOut
        {
            get { return _isLockedOut; }
            set { _isLockedOut = value; }
        }
        /// <summary>
        /// Get/Set the date and time the membership account was created
        /// </summary>
        [XmlElement(ElementName = "createddate")]
        public DateTime DateCreated
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }
        /// <summary>
        /// Get/Set the date and time that the user last logged in.
        /// </summary>
        [XmlElement(ElementName = "lastlogindate")]
        public DateTime DateLastLogin
        {
            get { return _lastLoginDate; }
            set { _lastLoginDate = value; }
        }
        /// <summary>
        /// Get/Set the date and time the membership account was last locked out.
        /// </summary>
        [XmlElement(ElementName = "lastlockoutdate")]
        public DateTime DateLastLockedOut
        {
            get { return _lastLockoutDate; }
            set { _lastLockoutDate = value; }
        }
        /// <summary>
        /// Get/Set the date and time that the membership users last changed there password.
        /// </summary>
        [XmlElement(ElementName = "lastpswdchangedate")]
        public DateTime DateLastPasswordChange
        {
            get { return _lastPswdChangeDate; }
            set { _lastPswdChangeDate = value; }
        }
        /// <summary>
        /// Get/Set the number of failed attempts by membership user to enter in the correct password
        /// </summary>
        [XmlElement(ElementName = "failedpswdattemptcount", DataType="int")]
        public int FailedPasswordAttemptCount
        {
            get { return _failedPswdAttemptCount; }
            set { _failedPswdAttemptCount = value; }
        }
        /// <summary>
        /// Get/Set the date and time that the user last entered in the incorrect password
        /// </summary>
        [XmlElement(ElementName = "failedPswdAttemptWindowStart")]
        public DateTime FailedPasswordAttemptWindowStart
        {
            get { return _failedPswdAttemptWindowStart; }
            set { _failedPswdAttemptWindowStart = value; }
        }
        /// <summary>
        /// Get/Set the number of failed attempts to answer the password question correclty.
        /// </summary>
        [XmlElement(ElementName = "failedPswdAnswerAttemptCount", DataType = "int")]
        public int FailedPasswordAnswerAttemptCount
        {
            get { return _failedPswdAnswerAttemptCount; }
            set { _failedPswdAnswerAttemptCount = value; }
        }
        /// <summary>
        /// Get/Set the date and time that he user last entered in the incorrect password question answer.
        /// </summary>
        [XmlElement(ElementName = "failedPswdAnswerAttemptWindowStart")]
        public DateTime FailedPasswordAnswerAttemptWindowStart
        {
            get { return _failedPswdAnswerAttemptWindowStart; }
            set { _failedPswdAnswerAttemptWindowStart = value; }
        }
        /// <summary>
        /// Get/Set comments
        /// </summary>
        [XmlElement(ElementName = "comment", DataType = "string")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
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
        /// Instanciate New Membership User
        /// </summary>
        public MembershipUser()
        {
            SetBase();
        }
        /// <summary>
        /// Instanciate New Membership User for puid and pappid
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="puid">as user's unique identifier</param>
        /// <param name="pappid">as application's unique identifier</param>
        public MembershipUser(string pconstring, Guid puid, Guid pappid)
        {
            SetBase();
            Load(pconstring, puid, pappid);
        }
        /// <summary>
        /// Instanicate new membership user from dr
        /// </summary>
        /// <param name="dr">as data.datarow</param>
        internal MembershipUser(System.Data.DataRow dr)
        {
            try
            {
                if (dr["UID"] != System.DBNull.Value) { _uid = new Guid(dr["UID"].ToString()); }
                if (dr["APPID"] != System.DBNull.Value) { _appid = new Guid(dr["APPID"].ToString()); }
                if (dr["MemPswd"] != System.DBNull.Value) { _pswd = (String)dr["MemPswd"]; }
                if (dr["MemPswdFormat"] != System.DBNull.Value) { _pswdformat = (Int32)dr["MemPswdFormat"]; }
                if (dr["MemPswdSalt"] != System.DBNull.Value) { _pswdsalt = (String)dr["MemPswdSalt"]; }
                if (dr["MemEmail"] != System.DBNull.Value) { _email = (String)dr["MemEmail"]; }
                if (dr["MemPswdQuestion"] != System.DBNull.Value) { _pswdQuestion = (String)dr["MemPswdQuestion"]; }
                if (dr["MemPswdAnswer"] != System.DBNull.Value) { _pswdAnswer = (String)dr["MemPswdAnswer"]; }
                if (dr["MemIsApproved"] != System.DBNull.Value) { _isApproved = Convert.ToBoolean(dr["MemIsApproved"]); }
                if (dr["MemIsLockedOut"] != System.DBNull.Value) { _isLockedOut = Convert.ToBoolean(dr["MemIsLockedOut"]); }
                if (dr["MemCreatedDate"] != System.DBNull.Value) { _createdDate = (DateTime)dr["MemCreatedDate"]; }
                if (dr["MemLastLoginDate"] != System.DBNull.Value) { _lastLoginDate = (DateTime)dr["MemLastLoginDate"]; }
                if (dr["MemLastPswdChangeDate"] != System.DBNull.Value) { _lastPswdChangeDate = (DateTime)dr["MemLastPswdChangeDate"]; }
                if (dr["MemLastLockoutDate"] != System.DBNull.Value) { _lastLockoutDate = (DateTime)dr["MemLastLockoutDate"]; }
                if (dr["MemFailedPswdAttemptCount"] != System.DBNull.Value) { _failedPswdAttemptCount = (int)dr["MemFailedPswdAttemptCount"]; }
                if (dr["MemFailedPswdAttemptWindowStart"] != System.DBNull.Value) { _failedPswdAttemptWindowStart = (DateTime)dr["MemFailedPswdAttemptWindowStart"]; }
                if (dr["MemFailedPswdAnswerAttemptCount"] != System.DBNull.Value) { _failedPswdAnswerAttemptCount = (int)dr["MemFailedPswdAnswerAttemptCount"]; }
                if (dr["MemFailedPswdAnswerAttemptWindowsStart"] != System.DBNull.Value) { _failedPswdAnswerAttemptWindowStart = (DateTime)dr["MemFailedPswdAnswerAttemptWindowsStart"]; }
                if (dr["MemComment"] != System.DBNull.Value) { _comment = (String)dr["MemComment"]; }
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
            _pswd = string.Empty;
            _pswdsalt = string.Empty;
            _email = string.Empty;
            _pswdQuestion = string.Empty;
            _pswdAnswer = string.Empty;
            _comment = string.Empty;
        }
        /// <summary>
        /// Set MyBase to values of dr
        /// </summary>
        /// <param name="dr">as data.datarow</param>
        /// <returns></returns>
        internal Boolean SetBase(System.Data.DataRow dr)
        {
            try
            {
                if (dr["UID"] != System.DBNull.Value) { _uid = new Guid(dr["UID"].ToString()); }
                if (dr["APPID"] != System.DBNull.Value) { _appid = new Guid(dr["APPID"].ToString()); }
                if (dr["MemPswd"] != System.DBNull.Value) { _pswd = (String)dr["MemPswd"]; }
                if (dr["MemPswdFormat"] != System.DBNull.Value) { _pswdformat = (Int32)dr["MemPswdFormat"]; }
                if (dr["MemPswdSalt"] != System.DBNull.Value) { _pswdsalt = (String)dr["MemPswdSalt"]; }
                if (dr["MemEmail"] != System.DBNull.Value) { _email = (String)dr["MemEmail"]; }
                if (dr["MemPswdQuestion"] != System.DBNull.Value) { _pswdQuestion = (String)dr["MemPswdQuestion"]; }
                if (dr["MemPswdAnswer"] != System.DBNull.Value) { _pswdAnswer = (String)dr["MemPswdAnswer"]; }
                if (dr["MemIsApproved"] != System.DBNull.Value) { _isApproved = Convert.ToBoolean(dr["MemIsApproved"]); }
                if (dr["MemIsLockedOut"] != System.DBNull.Value) { _isLockedOut = Convert.ToBoolean(dr["MemIsLockedOut"]); }
                if (dr["MemCreatedDate"] != System.DBNull.Value) { _createdDate = (DateTime)dr["MemCreatedDate"]; }
                if (dr["MemLastLoginDate"] != System.DBNull.Value) { _lastLoginDate = (DateTime)dr["MemLastLoginDate"]; }
                if (dr["MemLastPswdChangeDate"] != System.DBNull.Value) { _lastPswdChangeDate = (DateTime)dr["MemLastPswdChangeDate"]; }
                if (dr["MemLastLockoutDate"] != System.DBNull.Value) { _lastLockoutDate = (DateTime)dr["MemLastLockoutDate"]; }
                if (dr["MemFailedPswdAttemptCount"] != System.DBNull.Value) { _failedPswdAttemptCount = (int)dr["MemFailedPswdAttemptCount"]; }
                if (dr["MemFailedPswdAttemptWindowStart"] != System.DBNull.Value) { _failedPswdAttemptWindowStart = (DateTime)dr["MemFailedPswdAttemptWindowStart"]; }
                if (dr["MemFailedPswdAnswerAttemptCount"] != System.DBNull.Value) { _failedPswdAnswerAttemptCount = (int)dr["MemFailedPswdAnswerAttemptCount"]; }
                if (dr["MemFailedPswdAnswerAttemptWindowsStart"] != System.DBNull.Value) { _failedPswdAnswerAttemptWindowStart = (DateTime)dr["MemFailedPswdAnswerAttemptWindowsStart"]; }
                if (dr["MemComment"] != System.DBNull.Value) { _comment = (String)dr["MemComment"]; }

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
        /// <param name="data">as membership user</param>
        /// <returns></returns>
        private bool SetBase(MembershipUser data)
        {
            try
            {
                _uid = data.UID;
                _appid = data.APPID;
                _pswd = data.Password;
                _pswdformat = data.PasswordFormat;
                _pswdsalt = data.PasswordSalt;
                _email = data.Email; 
                _pswdQuestion = data.PasswordQuestion;
                _pswdAnswer = data.PasswordAnswer;
                _isApproved = data.Approved;
                _isLockedOut = data.LockedOut;
                _createdDate = data.DateCreated;
                _lastLoginDate = data.DateLastLogin;
                _lastPswdChangeDate = data.DateLastPasswordChange;
                _lastLockoutDate = data.DateLastLockedOut;
                _failedPswdAttemptCount = data.FailedPasswordAttemptCount;
                _failedPswdAttemptWindowStart = data.FailedPasswordAttemptWindowStart;
                _failedPswdAnswerAttemptCount = data.FailedPasswordAnswerAttemptCount;
                _failedPswdAnswerAttemptWindowStart = data.FailedPasswordAttemptWindowStart;
                _comment = string.Empty;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Functions and Sub Routines
        /// <summary>
        /// Returns the membership email.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _email;
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
        /// Load User Membership for [uid] and [appid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="puid">as user's identifier</param>
        /// <param name="pappid">as application's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_memberships]</TableName>
        public Boolean Load(string pconstring, Guid puid, Guid pappid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_dgMembmership_UIDAPPID", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = puid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pappid", MySqlDbType.VarChar, 50)).Value =  pappid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblUsers");

                            if (ds.Tables["tblUsers"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return false;
                            }

                            return SetBase(ds.Tables["tblUsers"].Rows[0]);
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
        /// Add User membership to system
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <returns></returns>
        /// <TableName>[memberships]</TableName>
        public Boolean Add(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addMembmership", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = _uid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pappid", MySqlDbType.VarChar, 50)).Value = _appid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswd", MySqlDbType.VarChar, 128)).Value = _pswd.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswdFormat", MySqlDbType.Int32)).Value = _pswdformat;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswdSalt", MySqlDbType.VarChar, 128)).Value = _pswdsalt.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEmail", MySqlDbType.VarChar, 255)).Value = _email.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswdQuestion", MySqlDbType.VarChar, 255)).Value = _pswdQuestion.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPswdAnswer", MySqlDbType.VarChar, 128)).Value = _pswdAnswer.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pisApproved", MySqlDbType.Bit)).Value = (_isApproved == true) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pisLockedOut", MySqlDbType.Bit)).Value = (_isLockedOut == true) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCreatedDate", MySqlDbType.Datetime)).Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?pLastLoginDate", MySqlDbType.Datetime)).Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?pLastPswdChangeDate", MySqlDbType.Datetime)).Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?pComment", MySqlDbType.VarChar, 255)).Value = _comment.ToString();


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
        /// Update user membership in system
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <returns></returns>
        /// <TableName>[memberships]</TableName>
        public Boolean Update(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateMembmership", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += "UPDATE `memberships` SET `pswd`=?Pswd,`pswdFormat`=?PswdFormat";
                        query += " ,`pswdSalt`=?PswdSalt,`email`=?Email,`PswdQuestion`=?PswdQuestion";
                        query += " ,`pswdAnswer`=?PswdAnswer, `isApproved`=?isApproved, `isLockedOut`=?isLockedOut";
                        query += " ,`createdDate`=?CreatedDate, `lastLoginDate`=?LastLoginDate, `createdDate`=?CreatedDate";
                        query += " ,`lastLoginDate`=?LastLoginDate, `lastPswdChangeDate`=?LastPswdChangeDate";
                        query += " ,`lastLockoutDate`=?LastLockoutDate, `failedPswdAttemptCount`=?FailedPswdAttemptCount";
                        query += " ,`failedPswdAttemptWindowStart`=?FailedPswdAttemptWindowsStart";
                        query += " ,`failedPswdAnswerAttemptCount`=?FailedPswdAnswerAttemptCount";
                        query += " ,`failedPswdAnswerAttemptWindowsStart`=?FPswdAnwAttWinStart";
                        query += " ,`comment`=?Comment ";
                        query += " WHERE `uid`=?uid AND `appid`=?appid;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?uid", MySqlDbType.VarChar, 50)).Value =  _uid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?appid", MySqlDbType.VarChar, 50)).Value = _appid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?Pswd", MySqlDbType.VarChar, 128)).Value = _pswd.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?PswdFormat", MySqlDbType.Int32)).Value =  _pswdformat;
                        sqlcomm.Parameters.Add(new MySqlParameter("?PswdSalt", MySqlDbType.VarChar, 128)).Value = _pswdsalt.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?Email", MySqlDbType.VarChar, 255)).Value = _email.ToString(); 
                        sqlcomm.Parameters.Add(new MySqlParameter("?PswdQuestion", MySqlDbType.VarChar, 255)).Value = _pswdQuestion.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?PswdAnswer", MySqlDbType.VarChar, 128)).Value = _pswdAnswer.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?isApproved", MySqlDbType.Bit)).Value = (_isApproved == true) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?isLockedOut", MySqlDbType.Bit)).Value = (_isLockedOut == true) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?CreatedDate", MySqlDbType.Datetime)).Value = _createdDate.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?LastLoginDate", MySqlDbType.Datetime)).Value = _lastLoginDate.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?LastPswdChangeDate", MySqlDbType.Datetime)).Value = _lastPswdChangeDate.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?LastLockoutDate", MySqlDbType.Datetime)).Value = _lastLockoutDate.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?FailedPswdAttemptCount", MySqlDbType.Int32)).Value = _failedPswdAttemptCount;
                        sqlcomm.Parameters.Add(new MySqlParameter("?FailedPswdAttemptWindowsStart", MySqlDbType.Datetime)).Value = _failedPswdAttemptWindowStart.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?FailedPswdAnswerAttemptCount", MySqlDbType.Int32)).Value = _failedPswdAnswerAttemptCount;
                        sqlcomm.Parameters.Add(new MySqlParameter("?FPswdAnwAttWinStart", MySqlDbType.Datetime)).Value = _failedPswdAnswerAttemptWindowStart.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?Comment", MySqlDbType.VarChar, 255)).Value = _comment.ToString();


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
        /// Return DataTable of all memberships assigned to user
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="puid">as user's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_memberships_applications]</TableName>
        public static System.Data.DataTable dgMemberships(string pconstring, Guid puid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_memberships_applications`";
                        query += " WHERE `uid`=?uid"; 
                        query += " ORDER BY `AppName`;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?uid", MySqlDbType.VarChar, 50)).Value = puid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblMemberships");

                            if (ds.Tables["tblMemberships"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return null;
                            }

                            return ds.Tables["tblMemberships"].Copy();
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
        /// Return DataTable of all memberships assigned to user containing pcolval in pcolval
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="puid">as user's identifier</param>
        /// <param name="pcolname">as column name to search on</param>
        /// <param name="pcolval">as value containing</param>
        /// <returns></returns>
        /// <TableName>[vw_memberships_applications]</TableName>
        public static System.Data.DataTable dgMemberships(string pconstring, Guid puid, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_memberships_applications`";
                        query += " WHERE `uid`=?uid";
                        query += string.Format(" AND `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()); 
                        query += " ORDER BY `AppName`;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?uid", MySqlDbType.VarChar, 50)).Value = puid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblMemberships");

                            if (ds.Tables["tblMemberships"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return null;
                            }

                            return ds.Tables["tblMemberships"].Copy();
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
        /// Updates the password for the membership user in the membership data store.
        /// </summary>
        /// <param name="oldPassword">the current password for the membership user.</param>
        /// <param name="newPassword">the new password for the membership user.</param>
        /// <returns>true if the update was successfull; otherwise, false.</returns>
        /// <exception>
        /// System.ArgumentException: 
        ///     oldPassword is an empty string - or - newPassword is an empty string.
        ///     
        /// System.ArgumentNullException:
        ///     oldPassword is null - or - newPassword is null.
        ///     
        /// System.PlatformNotSupportException:
        ///     This method is not available.  This can occur if the application targets the 
        ///     .Net Framework 4 Client Profile.  To prevent this exception, override the method, 
        ///     or change the application to target the full version of the .NET Framework.
        /// </exception>
        public virtual bool ChangePassword(string oldPassword, string newPassword)
        {
            return false;
        }
        /// <summary>
        /// Updates the password question and answer for the membership user in the membership data store.
        /// </summary>
        /// <param name="password">The current password for the membership user.</param>
        /// <param name="newPasswordQuestion">The new password question value for th membership user.</param>
        /// <param name="newPasswordAnswer">The new password answer value for the membership user.</param>
        /// <returns>true if the update was successful; otherwise, false.</returns>
        /// <exception>
        /// System.ArgumentException:
        ///     password is an empty string - or - newPasswordQuestion is an empty string - or - 
        ///     newPasswordAnswer is an empty string.
        ///     
        /// System.ArgumentNullException:
        ///     password is null.
        ///     
        /// System.PlatformNotSupportException:
        ///     This method is not available.  This can occur if the application targets the 
        ///     .Net Framework 4 Client Profile.  To prevent this exception, override the method, 
        ///     or change the application to target the full version of the .NET Framework.
        /// </exception>
        public virtual bool ChangePasswordQuestionAndAnswer(string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        /// <summary>
        /// Gets the password for the membership user from the emmbership data store.
        /// </summary>
        /// <returns>The password for the membership user.</returns>
        /// <exception>
        /// System.PlatformNotSupportException:
        ///     This method is not available.  This can occur if the application targets the 
        ///     .Net Framework 4 Client Profile.  To prevent this exception, override the method, 
        ///     or change the application to target the full version of the .NET Framework.
        /// </exception>
        public virtual string GetPassword()
        {
            return null;
        }
        /// <summary>
        /// Gets the passord for the membership user from the membership data store.
        /// </summary>
        /// <param name="passwordAnswer">The password answer for the membership user.</param>
        /// <returns>The password for the emembership user.</returns>
        /// <exception>
        /// System.PlatformNotSupportException:
        ///     This method is not available.  This can occur if the application targets the 
        ///     .Net Framework 4 Client Profile.  To prevent this exception, override the method, 
        ///     or change the application to target the full version of the .NET Framework.
        /// </exception>
        public virtual string GetPassword(string passwordAnswer)
        {
            return null;
        }

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <returns>The new password for the membership user.</returns>
        /// <exception>
        /// System.PlatformNotSupportException:
        ///     This method is not available.  This can occur if the application targets the 
        ///     .Net Framework 4 Client Profile.  To prevent this exception, override the method, 
        ///     or change the application to target the full version of the .NET Framework.
        /// </exception>
        public virtual string ResetPassword()
        {
            return null;
        }
        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="passwordAnswer">The password answer for the membership user.</param>
        /// <returns>The new password for the membership user.</returns>
        /// <exception>
        /// System.PlatformNotSupportException:
        ///     This method is not available.  This can occur if the application targets the 
        ///     .Net Framework 4 Client Profile.  To prevent this exception, override the method, 
        ///     or change the application to target the full version of the .NET Framework.
        /// </exception>
        public virtual string ResetPassword(string passwordAnswer)
        {
            return null;
        }

        /// <summary>
        /// Clears the locked-out state of the user so that the memberhship user can be validated.
        /// </summary>
        /// <returns>true if the membership user was successfully unlocked; otherwise, false;</returns>
        /// <exception>
        /// System.PlatformNotSupportException:
        ///     This method is not available.  This can occur if the application targets the 
        ///     .Net Framework 4 Client Profile.  To prevent this exception, override the method, 
        ///     or change the application to target the full version of the .NET Framework.
        /// </exception>
        public virtual bool UnlockUser()
        {
            return false;
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
                throw ex;
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
            byte[] byKey = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };

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
            byte[] byKey = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
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

                    xmlSer = new XmlSerializer(typeof(MembershipUser));
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
                XmlSerializer xs = new XmlSerializer(typeof(MembershipUser));
                MembershipUser output = (MembershipUser)xs.Deserialize(fs);
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
                return false;
            }
        }
        /// <summary>
        /// Return Serialized String version of [data] Object
        /// </summary>
        /// <param name="data">as User</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(MembershipUser data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(MembershipUser));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(MembershipUser));
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
                return null;
            }
        }
        /// <summary>
        /// Return User from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of User</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static MembershipUser XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(MembershipUser));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                MembershipUser output = default(MembershipUser);
                output = (MembershipUser)xmlSer.Deserialize(string_reader);
                return output;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
