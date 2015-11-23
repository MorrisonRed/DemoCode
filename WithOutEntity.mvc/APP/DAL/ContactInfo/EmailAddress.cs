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

namespace ContactInfo
{
    /// <summary>
    /// Email Address Definition
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("emailaddress")]
    public class EmailAddress
    {
        /* 
         * id (int)
         * cntid (guid)
         * type (emailaddresstype)
         * email (string)
         * displayas (string)
        */

        #region "Public Properties"
        /// <summary>
        /// Get/Set Email's ID
        /// </summary>
        [XmlElement(ElementName = "id")]
        public int ID { get; set; }
        /// <summary>
        /// Get/Set Email's associate Contact Card
        /// </summary>
        [XmlElement(ElementName = "cntid")]
        public Guid CNTID { get; set; }
        /// <summary>
        /// Get/Set Email Type
        /// </summary>
        [XmlElement(ElementName = "type")]
        public EmailAddressType Type { get; set; }
        /// <summary>
        /// Get/Set Email address
        /// </summary>
        [XmlElement(ElementName = "email")]
        public String Email { get; set; }
        /// <summary>
        /// Get/Set Display text for email
        /// </summary>
        [XmlElement(ElementName = "displayas")]
        public String DisplayAs { get; set; }

        /// <summary>
        /// Get last error thrown by object 
        /// </summary>
        [XmlIgnore]
        public static Exception GetLastError { get; set; }
        #endregion

        #region "Constructors and Destructors"
        /// <summary>
        /// Instanicate New Email Address
        /// </summary>
        public EmailAddress()
        {
            SetBase();
        }
        /// <summary>
		/// Instanicate new Email Address from [dr]
		/// </summary>
		/// <param name="pconstring">as data source connection string</param>
		/// <param name="dr">as data.datarow</param>
        internal EmailAddress(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["cntemID"] != System.DBNull.Value) { ID = Convert.ToInt16(dr["cntemID"].ToString()); }
                if (dr["cntid"] != System.DBNull.Value) { CNTID = new Guid(dr["cntid"].ToString()); }
                if (dr["cntemType"] != System.DBNull.Value) { Type = (EmailAddressType)Convert.ToInt16(dr["cntemType"].ToString()); }
                if (dr["cntemEmail"] != System.DBNull.Value) { Email = (String)dr["cntemEmail"]; }
                if (dr["cntemDisplayAs"] != System.DBNull.Value) { DisplayAs = (String)dr["cntemDisplayAs"]; }
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBase to values of String.Empty
        /// </summary>
        private void SetBase()
        {
            Email = "";
            DisplayAs = "";
        }
        /// <summary>
        /// Set MyBase to Value of [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data.datarow</param>
        internal bool SetBase(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["cntemID"] != System.DBNull.Value) { ID = Convert.ToInt16(dr["cntemID"].ToString()); }
                if (dr["cntid"] != System.DBNull.Value) { CNTID = new Guid(dr["cntid"].ToString()); }
                if (dr["cntemType"] != System.DBNull.Value) { Type = (EmailAddressType)Convert.ToInt16(dr["cntemType"].ToString()); }
                if (dr["cntemEmail"] != System.DBNull.Value) { Email = (String)dr["cntemEmail"]; }
                if (dr["cntemDisplayAs"] != System.DBNull.Value) { DisplayAs = (String)dr["cntemDisplayAs"]; }

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBase to values of [data]
        /// </summary>
        /// <param name="data">as Email Address</param>
        /// <returns></returns>
        internal bool SetBase(EmailAddress data)
        {
            ID = data.ID;
            CNTID = data.CNTID;
            Email = data.Email;
            DisplayAs = data.DisplayAs;

            return true;
        }
        #endregion

        #region "Functions and Sub Routines"
        public override string ToString()
        {
            return Email.Trim();
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

            Type type = typeof(ContactCard);
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
        /// Load Email Address for [id]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pid">as Email's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_Emails_ContactCards]</TableName>
        public bool Load(string pconstring, int pid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_Emails_ContactCards "));
                        query.Append(string.Format(" WHERE `cntemID`=?cntemID; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?cntemID", MySqlDbType.Int16)).Value = pid.ToString();

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
        /// Add Email Address to contact card
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Emails]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addContactCardEmail", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pCNTID", MySqlDbType.VarChar, 50)).Value = CNTID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pType", MySqlDbType.Int16)).Value = Convert.ToInt16(Type);
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEmail", MySqlDbType.VarChar, 128)).Value = Email.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDisplayAs", MySqlDbType.VarChar, 128)).Value = DisplayAs.ToString();

                        var obj = sqlcomm.ExecuteScalar();

                        if (IsNumeric(obj.ToString()))
                        {
                            ID = Convert.ToInt16(obj);
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
            }
        }

        /// <summary>
        /// Update Email Address in contact card
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Emails]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_udpateContactCardEmail", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pID", MySqlDbType.Int16)).Value = ID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCNTID", MySqlDbType.VarChar, 50)).Value = CNTID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pType", MySqlDbType.Int16)).Value = Convert.ToInt16(Type);
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEmail", MySqlDbType.VarChar, 128)).Value = Email.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDisplayAs", MySqlDbType.VarChar, 128)).Value = DisplayAs.ToString();

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
        /// Remove Email Address from contact card
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Emails]</TableName>
        public bool Delete(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteContactCardEmail", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pID", MySqlDbType.Int16)).Value = ID.ToString();

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
        /// Remove Email Address [id] from contact card
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pid">as email addresses identifier</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Emails]</TableName>
        public static bool Delete(string pconstring, int pid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteContactCardEmail", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pID", MySqlDbType.Int16)).Value = pid.ToString();

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
        /// Return List of Contact Email's in system 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcntid">as contact cards identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_Emails_ContactCards]</TableName>
        public static List<EmailAddress> ToList(string pconstring, Guid pcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_Emails_ContactCards` "));
                        query.Append(string.Format(" WHERE `cntid`=?cntid "));
                        query.Append(string.Format(" ORDER BY `cntemEmail` ASC;"));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?cntid", MySqlDbType.VarChar, 50)).Value = pcntid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return new List<EmailAddress>();
                            }

                            List<EmailAddress> lst = new List<EmailAddress>();
                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            {
                                lst.Add(new EmailAddress(pconstring, dr));
                            }

                            return lst;
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
        /// Return DataTable of all email in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_Emails_ContactCards]</TableName>
        public static DataTable dgEmails(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_Emails_ContactCards` "));
                        query.Append(string.Format(" ORDER BY `cntemEmail` ASC;"));
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
        /// <summary>
        /// Return DataTable of all emails for [cntid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcntid">as contact cards identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_Emails_ContactCards]</TableName>
        public static DataTable dgEmails(string pconstring, Guid pcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_Emails_ContactCards` "));
                        query.Append(string.Format(" WHERE `cntid`=?cntid "));
                        query.Append(string.Format(" ORDER BY `cntemEmail` ASC;"));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?cntid", MySqlDbType.VarChar, 50)).Value = pcntid.ToString();

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
        /// Write EmailAddress to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of EmailAddress</param>
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

                    xmlSer = new XmlSerializer(typeof(EmailAddress));
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
        /// Create EmailAddress from File
        /// </summary>
        /// <param name="xmlpath">as source location of EmailAddress</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(EmailAddress));
                EmailAddress output = (EmailAddress)xs.Deserialize(fs);
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
        /// <param name="data">as EmailAddress</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(EmailAddress data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(EmailAddress));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(EmailAddress));
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
        /// Return EmailAddress from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of EmailAddress</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static EmailAddress XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(EmailAddress));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                EmailAddress output = default(EmailAddress);
                output = (EmailAddress)xmlSer.Deserialize(string_reader);
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