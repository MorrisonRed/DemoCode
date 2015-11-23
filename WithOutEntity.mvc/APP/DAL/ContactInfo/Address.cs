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
    /// Address Definition
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("address")]
    public class Address
    {
        /* 
         * id(int)
         * cntid (guid)
         * type (addresstype)
         * street (string)
         * city (string)
         * prov (string)
         * postal (string)
         * country (string)
        */

        #region "Public Properties"
        /// <summary>
        /// Get/Set Addresses id
        /// </summary>
        [XmlElement(ElementName = "id")]
        public int ID { get; set; }
        /// <summary>
        /// Get/Set Addresses assoicated contact card
        /// </summary>
        [XmlElement(ElementName = "cntid")]
        public Guid CNTID { get; set; }
        /// <summary>
        /// Get/Set Address type
        /// </summary>
        [XmlElement(ElementName = "type")]
        public AddressType Type { get; set; }
        /// <summary>
        /// Get/Set Street Address
        /// </summary>
        [XmlElement(ElementName = "street")]
        public String Street { get; set; }
        /// <summary>
        /// Get/Set City
        /// </summary>
        [XmlElement(ElementName = "city")]
        public String City { get; set; }
        /// <summary>
        /// Get/Set Province
        /// </summary>
        [XmlElement(ElementName = "prov")]
        public String Province { get; set; }
        /// <summary>
        /// Get/Set Country
        /// </summary>
        [XmlElement(ElementName = "country")]
        public String Country { get; set; }
        /// <summary>
        /// Get/Set Postal Code
        /// </summary>
        [XmlElement(ElementName = "postal")]
        public String PostalCode { get; set; }

        /// <summary>
        /// Get last error thrown by object 
        /// </summary>
        [XmlIgnore]
        public static Exception GetLastError { get; set; }
        #endregion

        #region "Constructors and Destructors"
        /// <summary>
        /// Instanicate New Address
        /// </summary>
        public Address()
        {
            SetBase();
        }
        /// <summary>
        /// Instanciate new Address from [dr]
        /// </summary>
        /// <param name="pconstring">as datasource connection string</param>
        /// <param name="dr">as data.datarow</param>
        internal Address(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["cntaddrID"] != System.DBNull.Value) { ID = Convert.ToInt16(dr["cntaddrID"].ToString()); }
                if (dr["cntid"] != System.DBNull.Value) { CNTID = new Guid(dr["cntid"].ToString()); }
                if (dr["cntaddrType"] != System.DBNull.Value) { Type = (AddressType)Convert.ToInt16(dr["cntaddrType"].ToString()); }
                if (dr["cntaddrStreet"] != System.DBNull.Value) { Street = (String)dr["cntaddrStreet"]; }
                if (dr["cntaddrCity"] != System.DBNull.Value) { City = (String)dr["cntaddrCity"]; }
                if (dr["cntaddrProv"] != System.DBNull.Value) { Province = (String)dr["cntaddrProv"]; }
                if (dr["cntaddrPostal"] != System.DBNull.Value) { PostalCode = (String)dr["cntaddrPostal"]; }
                if (dr["cntaddrCountry"] != System.DBNull.Value) { Country = (String)dr["cntaddrCountry"]; }
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
            Street = "";
            City = "";
            Province = "";
            PostalCode = "";
            Country = "";

        }
        /// <summary>
        /// Set MyBase to values of [dr]
        /// </summary>
        /// <param name="pconstring">as datasource connection string</param>
        /// <param name="dr">as data.datarow</param>
        internal bool SetBase(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["cntaddrID"] != System.DBNull.Value) { ID = Convert.ToInt16(dr["cntaddrID"].ToString()); }
                if (dr["cntid"] != System.DBNull.Value) { CNTID = new Guid(dr["cntid"].ToString()); }
                if (dr["cntaddrType"] != System.DBNull.Value) { Type = (AddressType)Convert.ToInt16(dr["cntaddrType"].ToString()); }
                if (dr["cntaddrStreet"] != System.DBNull.Value) { Street = (String)dr["cntaddrStreet"]; }
                if (dr["cntaddrCity"] != System.DBNull.Value) { City = (String)dr["cntaddrCity"]; }
                if (dr["cntaddrProv"] != System.DBNull.Value) { Province = (String)dr["cntaddrProv"]; }
                if (dr["cntaddrPostal"] != System.DBNull.Value) { PostalCode = (String)dr["cntaddrPostal"]; }
                if (dr["cntaddrCountry"] != System.DBNull.Value) { Country = (String)dr["cntaddrCountry"]; }

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBase to values of data
        /// </summary>
        /// <param name="data">as address</param>
        /// <returns></returns>
        internal bool SetBase(Address data)
        {
            try
            {
                ID = data.ID;
                CNTID = data.CNTID;
                Type = data.Type;
                Street = data.Street;
                City = data.City;
                Province = data.Province;
                PostalCode = data.PostalCode;
                Country = data.Country;
                
                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        #endregion

        #region "Functions and Sub Procedures"
        public override string ToString()
        {
            return Street.Trim();
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
        /// Load Address for [id]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pid">as addresses identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_Addresses_ContactCards]</TableName>
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
                        query.Append(string.Format("SELECT * FROM vw_Addresses_ContactCards "));
                        query.Append(string.Format(" WHERE `cntaddrID`=?cntaddrID; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?cntaddrID", MySqlDbType.Int16)).Value = pid.ToString();

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
        /// <TableName>[ContactCards_Addresses]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addContactCardAddress", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pCNTID", MySqlDbType.VarChar, 50)).Value = CNTID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pType", MySqlDbType.Int16)).Value = Convert.ToInt16(Type);
                        sqlcomm.Parameters.Add(new MySqlParameter("?pStreet", MySqlDbType.VarChar, 128)).Value = Street.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCity", MySqlDbType.VarChar, 128)).Value = City.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pProv", MySqlDbType.VarChar, 5)).Value = Province.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPostal", MySqlDbType.VarChar, 5)).Value = PostalCode.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCountry", MySqlDbType.Char, 3)).Value = Country.ToString();

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
        /// Update Address in contact card
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Addresses]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateContactCardAddress", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pID", MySqlDbType.Int16)).Value = ID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCNTID", MySqlDbType.VarChar, 50)).Value = CNTID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pType", MySqlDbType.Int16)).Value = Convert.ToInt16(Type);
                        sqlcomm.Parameters.Add(new MySqlParameter("?pStreet", MySqlDbType.VarChar, 128)).Value = Street.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCity", MySqlDbType.VarChar, 128)).Value = City.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pProv", MySqlDbType.VarChar, 5)).Value = Province.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPostal", MySqlDbType.VarChar, 5)).Value = PostalCode.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCountry", MySqlDbType.Char, 3)).Value = Country.ToString();

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
        /// Remove Address from contact card
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Addresses]</TableName>
        public bool Delete(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteContactCardAddress", sqlconn))
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
        /// Remove Address [id] from contact card
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>\
        /// <param name="pid">as addresses identifier</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Addresses]</TableName>
        public static bool Delete(string pconstring, int pid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteContactCardAddress", sqlconn))
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
        /// Return List of Contact addresses in system 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcntid">as contact card's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_Addresses_ContactCards]</TableName>
        public static List<Address> ToList(string pconstring, Guid pcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_Addresses_ContactCards` "));
                        query.Append(string.Format(" WHERE `cntid`=?cntid "));
                        query.Append(string.Format("  ORDER BY `cntaddrCountry` ASC, `cntaddrProv` ASC, `cntaddrCity` ASC;"));
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
                                return new List<Address>();
                            }

                            List<Address> lst = new List<Address>();
                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            {
                                lst.Add(new Address(pconstring, dr));
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
        /// <TableName>[vw_Addresses_ContactCards]</TableName>
        public static DataTable dgAddresses(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_Addresses_ContactCards` "));
                        query.Append(string.Format("  ORDER BY `cntaddrCountry` ASC, `cntaddrProv` ASC, `cntaddrCity` ASC;"));
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
        /// <TableName>[vw_Addresses_ContactCards]</TableName>
        public static DataTable dgAddresses(string pconstring, Guid pcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_Addresses_ContactCards` "));
                        query.Append(string.Format(" WHERE `cntid`=?cntid "));
                        query.Append(string.Format("  ORDER BY `cntaddrCountry` ASC, `cntaddrProv` ASC, `cntaddrCity` ASC;"));
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
        /// Write Address to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of Address</param>
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

                    xmlSer = new XmlSerializer(typeof(Address));
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
        /// Create Address from File
        /// </summary>
        /// <param name="xmlpath">as source location of Address</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(Address));
                Address output = (Address)xs.Deserialize(fs);
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
        /// <param name="data">as Address</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(Address data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Address));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(Address));
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
        /// Return Address from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of Address</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Address XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Address));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                Address output = default(Address);
                output = (Address)xmlSer.Deserialize(string_reader);
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