using System;
using System.IO;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Collections;
using MySql.Data.MySqlClient; 
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace EventMessage
{

    /// <summary>
    /// Event Message Object
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("eventmessage")]
    public class EventMessage
    {
        private DateTime _timestamp;

        #region "Public Properties"
        /// <summary>
        /// Get/Set the Event Message ID
        /// </summary>
        [XmlElement(ElementName = "eventid")]
        public int EventID { get; set; }
        /// <summary>
        /// Get/Set the Event Level
        /// </summary>
        [XmlElement(ElementName = "level")]
        public EventLevel Level { get; set; }
        /// <summary>
        /// Get/Set the Event Action
        /// </summary>
        [XmlElement(ElementName = "action")]
        public EventAction Action { get; set; }
        /// <summary>
        /// Get/Set the Event Result
        /// </summary>
        [XmlElement(ElementName = "result")]
        public EventResult Result { get; set; }
        /// <summary>
        /// Get/Set the name of the application 
        /// </summary>
        [XmlElement(ElementName = "app")]
        public string Application { get; set; }
        /// <summary>
        /// Get/Set the version of the Application
        /// </summary>
        [XmlElement(ElementName = "appver")]
        public string ApplicationVersion { get; set; }
        /// <summary>
        /// Get/Set the Operation Code assoicated to the Event
        /// </summary>
        [XmlElement(ElementName = "opcode")]
        public string OperationCode { get; set; }
        /// <summary>
        /// Get/Set Key Words
        /// </summary>
        [XmlElement(ElementName = "keywords")]
        public string KeyWords { get; set; }
        /// <summary>
        /// Get/Set the Event Date and Time UTC
        /// </summary>
        [XmlElement(ElementName = "eventdate")]
        public DateTime EventDateTime { get; set; }
        /// <summary>
        /// Get/Set the assoicated user's account with the event
        /// </summary>
        [XmlElement(ElementName = "uid")]
        public Guid UID { get; set; }
        /// <summary>
        /// Get/Set the client's IP address
        /// </summary>
        [XmlElement(ElementName = "ip")]
        public string IP { get; set; }
        /// <summary>
        /// Get/Set the assoicated url for the event
        /// </summary>
        [XmlElement(ElementName = "url")]
        public string URL { get; set; }

        /// <summary>
        /// Get/Set the message(s) assoicated with the event
        /// </summary>
        [XmlArray(ElementName = "eventdata")]
        [XmlArrayItem(ElementName = "data")]
        public List<String> Data { get; set; }

        /// <summary>
        /// Get last error thrown by object
        /// </summary>
        [XmlIgnore]
        public static Exception GetLastError { get; set; }
        #endregion

        #region "Contructors and Destructors"
        /// <summary>
        /// Instanicate New Event Message 
        /// </summary>
        public EventMessage()
        {
            SetBase();
        }
        /// <summary>
        /// Instanciate New Event Message
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="peventid">as event message id</param>
        public EventMessage(string pconstring, int peventid)
        {
            SetBase();
            Load(pconstring, peventid);
        }
        /// <summary>
        /// Instanciate New Event Message
        /// </summary>
        /// <param name="level">as event level</param>
        /// <param name="action">as event action</param>
        /// <param name="result">as event result</param>
        /// <param name="data">as event messages in list</param>
        /// <param name="app">as application</param>
        /// <param name="appver">as application version</param>
        /// <param name="opcode">as operation code</param>
        /// <param name="keys">as key words</param>
        /// <param name="uid">us user's identifier</param>
        /// <param name="ip">as client's ip address</param>
        /// <param name="url">as assoicated page url</param>
        public EventMessage(EventLevel level, EventAction action, EventResult result, List<String> data, String app,
                            String appver = "", String opcode = "", String keys = "", String uid = "", String ip = "", String url = "")
        {
            SetBase();
            Level = level;
            Action = action;
            Result = result;
            Data = data;
            Application = app;
            ApplicationVersion = appver;
            OperationCode = opcode;
            KeyWords = keys;
            EventDateTime = DateTime.UtcNow;
            IP = ip;
            URL = url;
            if (!String.IsNullOrEmpty(uid)) UID = new Guid(uid);
        }
        /// Instanciate New Event Message
        /// </summary>
        /// <param name="level">as event level</param>
        /// <param name="action">as event action</param>
        /// <param name="result">as event result</param>
        /// <param name="message">as event message</param>
        /// <param name="app">as application</param>
        /// <param name="appver">as application version</param>
        /// <param name="opcode">as operation code</param>
        /// <param name="keys">as key words</param>
        /// <param name="uid">us user's identifier</param>
        /// <param name="ip">as client's ip address</param>
        /// <param name="url">as assoicated page url</param>
        public EventMessage(EventLevel level, EventAction action, EventResult result, String message, String app,
                            String appver = "", String opcode = "", String keys = "", String uid = "", String ip = "", String url = "")
        {
            SetBase();

            List<String> lstmsg = new List<String>();
            lstmsg.Add(message);

            Level = level;
            Action = action;
            Result = result;
            Data = lstmsg;
            Application = app;
            ApplicationVersion = appver;
            OperationCode = opcode;
            KeyWords = keys;
            EventDateTime = DateTime.UtcNow;
            IP = ip;
            URL = url;
            if (!String.IsNullOrEmpty(uid)) UID = new Guid(uid);
        }
        /// <summary>
        /// Instanicate New Event Message from datarow
        /// </summary>
        /// <param name="pconstring">as datat source connection string</param>
        /// <param name="dr">as data.datarow</param>
        internal EventMessage(string pconstring, System.Data.DataRow dr)
        {
            try
            {
                //must deserialize first to have meta data column override xml
                if (dr["EventXML"] != System.DBNull.Value) { XMLDeserializeFromString(dr["EventXML"].ToString()); }

                if (dr["EventID"] != System.DBNull.Value) { EventID = Convert.ToInt32(dr["EventID"].ToString()); }
                if (dr["EventLevel"] != System.DBNull.Value) { Level = (EventLevel)Convert.ToInt16(dr["EventLevel"].ToString()); }
                if (dr["EventAction"] != System.DBNull.Value) { Action = (EventAction)Convert.ToInt16(dr["EventAction"].ToString()); }
                if (dr["EventResult"] != System.DBNull.Value) { Result = (EventResult)Convert.ToInt16(dr["EventResult"].ToString()); }
                if (dr["EventApp"] != System.DBNull.Value) { Application = (String)dr["EventApp"]; }
                if (dr["EventAppVer"] != System.DBNull.Value) { ApplicationVersion = (String)dr["EventAppVer"]; }
                if (dr["EventOpCode"] != System.DBNull.Value) { OperationCode = (String)dr["EventOpCode"]; }
                if (dr["EventKeyWords"] != System.DBNull.Value) { KeyWords = (String)dr["EventKeyWords"]; }
                if (dr["EventTime"] != System.DBNull.Value) { EventDateTime = (DateTime)dr["EventTime"]; }
                if (dr["EventIP"] != System.DBNull.Value) { IP = (String)dr["EventIP"]; }
                if (dr["UID"] != System.DBNull.Value) { UID = new Guid(dr["UID"].ToString()); }
                if (dr["EventURL"] != System.DBNull.Value) { URL = (String)dr["EventURL"]; }
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set My Base values to String.Empty
        /// </summary>
        private void SetBase()
        {
            Application = string.Empty;
            ApplicationVersion = string.Empty;
            OperationCode = string.Empty;
            KeyWords = string.Empty;
            EventDateTime = DateTime.UtcNow;
            IP = string.Empty;
            URL = string.Empty;
            Data = new List<String>();
        }
        /// <summary>
        /// Set My Base to values of [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        /// <returns></returns>
        internal Boolean SetBase(string pconstring, System.Data.DataRow dr)
        {
            try
            {
                //must deserialize first to have meta data column override xml
                if (dr["EventXML"] != System.DBNull.Value) { XMLDeserializeFromString(dr["EventXML"].ToString()); }

                if (dr["EventID"] != System.DBNull.Value) { EventID = Convert.ToInt32(dr["EventID"].ToString()); }
                if (dr["EventLevel"] != System.DBNull.Value) { Level = (EventLevel)Convert.ToInt16(dr["EventLevel"].ToString()); }
                if (dr["EventAction"] != System.DBNull.Value) { Action = (EventAction)Convert.ToInt16(dr["EventAction"].ToString()); }
                if (dr["EventResult"] != System.DBNull.Value) { Result = (EventResult)Convert.ToInt16(dr["EventResult"].ToString()); }
                if (dr["EventApp"] != System.DBNull.Value) { Application = (String)dr["EventApp"]; }
                if (dr["EventAppVer"] != System.DBNull.Value) { ApplicationVersion = (String)dr["EventAppVer"]; }
                if (dr["EventOpCode"] != System.DBNull.Value) { OperationCode = (String)dr["EventOpCode"]; }
                if (dr["EventKeyWords"] != System.DBNull.Value) { KeyWords = (String)dr["EventKeyWords"]; }
                if (dr["EventTime"] != System.DBNull.Value) { EventDateTime = (DateTime)dr["EventTime"]; }
                if (dr["EventIP"] != System.DBNull.Value) { IP = (String)dr["EventIP"]; }
                if (dr["EventUID"] != System.DBNull.Value) { UID = new Guid(dr["EventUID"].ToString()); }
                if (dr["EventURL"] != System.DBNull.Value) { URL = (String)dr["EventURL"]; }

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set My Base to values of data
        /// </summary>
        /// <param name="data">as Event Message</param>
        /// <returns></returns>
        protected Boolean SetBase(EventMessage data)
        {
            try
            {
                EventID = data.EventID;
                Level = data.Level;
                Action = data.Action;
                Result = data.Result;
                Application = data.Application;
                ApplicationVersion = data.ApplicationVersion;
                OperationCode = data.OperationCode;
                KeyWords = data.KeyWords;
                EventDateTime = data.EventDateTime;
                UID = data.UID;
                IP = data.IP;
                URL = data.URL;
                Data = data.Data;
                _timestamp = data._timestamp;

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        #endregion

        #region "Functions and Sub Routines"
        public override string ToString()
        {
            return "Event #" + EventID.ToString();
        }
        /// <summary>
        /// Return true if the string is numeric
        /// </summary>
        /// <param name="stringToTest">as string to test</param>
        /// <returns></returns>
        public static bool IsNumeric(string stringToTest)
        {
            int num;
            return int.TryParse(stringToTest, out num);
        }
        /// <summary>
        /// Return true if column exists in table
        /// </summary>
        /// <param name="col">as column name</param>
        /// <param name="table">as data table</param>
        /// <returns></returns>
        private bool ColumnExists(string col, DataTable table)
        {
            bool flag;
            IEnumerator enumerator = table.Columns.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    if (((DataColumn)enumerator.Current).ColumnName != col)
                    {
                        continue;
                    }
                    flag = true;
                    return flag;
                }
                return false;
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return flag;
        }
        /// <summary>
        /// Return true if column existing in datarow
        /// </summary>
        /// <param name="col">as column name</param>
        /// <param name="dr">as data row</param>
        /// <returns></returns>
        private bool ColumnExists(string col, DataRow dr)
        {
            if (dr.Table.Columns.Contains(col))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Convert Object to type string
        /// </summary>
        /// <param name="value">value to convert to string</param>
        /// <returns></returns>
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

            Type type = typeof(EventMessage);
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
        /// Load Event Message for [eventid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="peventid">as Event Message ID</param>
        /// <returns></returns>
        /// <TableName>[vw_eventmessages]</TableName>
        public bool Load(string pconstring, int peventid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_eventmessages "));
                        query.Append(string.Format(" WHERE EventID=?EventID; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?EventID", MySqlDbType.Int64)).Value = peventid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return false;
                            }

                            return SetBase(pconstring, ds.Tables["tbl"].Rows[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// Return Event Message for [eventid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="peventid">as Event Message ID</param>
        /// <returns></returns>
        /// <TableName>[vw_eventmessages]</TableName>
        public static EventMessage getMessage(string pconstring, int peventid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_eventmessages "));
                        query.Append(string.Format(" WHERE EventID=?EventID; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?EventID", MySqlDbType.Int64)).Value = peventid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return null;
                            }

                            return new EventMessage(pconstring, ds.Tables["tbl"].Rows[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Add Event Message to system 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[eventmessages]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using(MySqlCommand sqlcomm = new MySqlCommand("usp_addEventMessage", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        string suid = (UID != null && UID != Guid.Empty) ? UID.ToString(): "";

                        sqlcomm.Parameters.Add(new MySqlParameter("?pEventLevel", MySqlDbType.Int16)).Value = (int)Level;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEventAction", MySqlDbType.Int16)).Value = (int)Action;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEventResult", MySqlDbType.Int16)).Value = (int)Result;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pApp", MySqlDbType.VarChar, 50)).Value = Application;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pAppVer", MySqlDbType.VarChar, 50)).Value = ApplicationVersion;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pOpCode", MySqlDbType.VarChar, 50)).Value = OperationCode;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pKeywords", MySqlDbType.VarChar, 200)).Value = KeyWords;
                        //this value is set in the stored procedure
                        //SqlComm.Parameters.Add(new MySqlParameter("?pEventTime", MySqlDbType.VarChar, 50)).Value =  _eventtime.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = suid;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pIP", MySqlDbType.VarChar, 50)).Value = IP;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pURL", MySqlDbType.VarChar, 255)).Value = URL;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pXML", MySqlDbType.VarString)).Value = this.XMLSerializeToString();

                        var obj = sqlcomm.ExecuteScalar();
                        EventID = Convert.ToInt32(obj);
                        return true;
                    }
                    catch(Exception ex)
                    {
                        GetLastError = ex;
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// Add Event Message to system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="level">as event level</param>
        /// <param name="action">as event action</param>
        /// <param name="result">as event result</param>
        /// <param name="data">as event messages in list</param>
        /// <param name="app">as application</param>
        /// <param name="appver">as application version</param>
        /// <param name="opcode">as operation code</param>
        /// <param name="keys">as key words</param>
        /// <param name="user">as assoicated user</param>
        /// <param name="ip">as client's ip address</param>
        /// <param name="url">as assoicated page url</param>
        /// <returns></returns>
        public static bool Add(string pconstring, EventLevel level, EventAction action, EventResult result, List<String> data, String app,
                            String appver = "", String opcode = "", String keys = "", CustomSecurity.User user = null, String ip = "", String url = "")
        {
            String suid = "";
            if (user != null && user.UID != null) suid = user.UID.ToString();
            EventMessage ev = new EventMessage(level:level, action:action, result:result, app:app, data:data, appver:appver, opcode:opcode, keys:keys.ToLower(), uid:suid, ip:ip, url:url);

            return ev.Add(pconstring);
        }
        /// <summary>
        /// Add Event Message to system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="level">as event level</param>
        /// <param name="action">as event action</param>
        /// <param name="result">as event result</param>
        /// <param name="message">as event messages in list</param>
        /// <param name="app">as application</param>
        /// <param name="appver">as application version</param>
        /// <param name="opcode">as operation code</param>
        /// <param name="keys">as key words</param>
        /// <param name="user">as assoicated user</param>
        /// <param name="ip">as client's ip address</param>
        /// <param name="url">as assoicated page url</param>
        /// <returns></returns>
        public static bool Add(string pconstring, EventLevel level, EventAction action, EventResult result, String message, String app,
                            String appver = "", String opcode = "", String keys = "", CustomSecurity.User user = null, String ip = "", String url = "")
        {
            String suid = "";
            if (user != null && user.UID != null) suid = user.UID.ToString();
            EventMessage ev = new EventMessage(level: level, action: action, result: result, app: app, message: message, appver: appver, opcode: opcode, keys: keys.ToLower(), uid: suid, ip: ip, url: url);

            return ev.Add(pconstring);
        }

        /// <summary>
        /// Remove EventMessage from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="eventid">as event message id</param>
        /// <returns>[eventmessages]</returns>
        public static bool Delete(string pconstring, int peventid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteEventMessage", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?peventid", MySqlDbType.Int64)).Value = peventid.ToString();

                        sqlcomm.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Return DataTable of all event messages from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_eventmessages_users]</TableName>
        public static System.Data.DataTable dgEvents(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_eventmessages_users`";
                        query += " ORDER BY `EventTime` DESC;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return new DataTable();
                            }

                            return ds.Tables["tbl"].Copy();
                        }
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return null;
                    }
                }
            }
        }
        /// <summary>
        /// Return DataTable of all event messages from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_eventmessages_users]</TableName>
        public static System.Data.DataTable dgEvents(String pconstring, String pcolname, String pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_eventmessages_users`"));
                        if (!string.IsNullOrEmpty(pcolname)) query.Append(string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
                        query.Append(" ORDER BY `EventTime` DESC;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return new DataTable();
                            }

                            return ds.Tables["tbl"].Copy();
                        }
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
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
                GetLastError = ex;
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
                GetLastError = ex;
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
        /// Write EventMessage to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of EventMessage</param>
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

                    xmlSer = new XmlSerializer(typeof(EventMessage));
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
                GetLastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Create EventMessage from File
        /// </summary>
        /// <param name="xmlpath">as source location of EventMessage</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(EventMessage));
                EventMessage output = (EventMessage)xs.Deserialize(fs);
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
                GetLastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Return Serialized String version of [data] Object
        /// </summary>
        /// <param name="data">as EventMessage</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(EventMessage data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(EventMessage));
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
                GetLastError = ex;
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(EventMessage));
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
                GetLastError = ex;
                return null;
            }
        }
        /// <summary>
        /// Return EventMessage from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of RSS</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static EventMessage XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(EventMessage));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                EventMessage output = default(EventMessage);
                output = (EventMessage)xmlSer.Deserialize(string_reader);
                return output;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return null;
            }
        }
        #endregion

    }
}