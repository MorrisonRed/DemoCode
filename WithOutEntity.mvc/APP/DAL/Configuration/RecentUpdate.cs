using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Web;
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

namespace Configuration
{
    [DataObject]
    [XmlRoot("recentupdate")]
    [Serializable()]
    public class RecentUpdate
    {
        /*
         * ruid int
         * title varchar(50)
         * icon varchar(255)
         * link varchar(255)
         * pubdate datetime
         * comments varchar(140)
         * description mediumtext
         * 
         */
        #region "Public Properties"
        /// <summary>
        /// Get/Set the Recent Updates Identifier
        /// </summary>
        [XmlElement(ElementName="ruid")]
        public Int32 RUID { get; set; }
        /// <summary>
        /// Get/Set the Title of the update
        /// </summary>
        [XmlElement(ElementName="title")]
        public String Title { get; set; }
        /// <summary>
        /// Get/Set the path to the assoicated icon
        /// </summary>
        [XmlElement(ElementName="icon")]
        public String Icon { get; set; }
        /// <summary>
        /// Get/Set the link assoicated with the update
        /// </summary>
        [XmlElement(ElementName="link")]
        public String Link { get; set; }
        /// <summary>
        /// Get/Set the date the update was published
        /// </summary>
        [XmlElement(ElementName="pubdate")]
        public DateTime PublishedDate { get; set; }
        /// <summary>
        /// Get/Set a short comment about the update 140
        /// </summary>
        [XmlElement(ElementName="comments")]
        public String Comments { get; set; }
        /// <summary>
        /// Get/Set the full description about the update
        /// </summary>
        [XmlElement(ElementName="description")]
        public String Description { get; set; }

        /// <summary>
        /// Get Last Error Thrown
        /// </summary>
        [XmlIgnore()]
        public static Exception GetLastError { get; set; }
        #endregion

        #region "Constructors and Destructors"
        /// <summary>
        /// Instanciate New Recent Update Item
        /// </summary>
        public RecentUpdate()
        {
            SetBase();
        }
        /// <summary>
        /// Instanciate New Recent Update Item
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pruid">as recent update's identifier</param>
        public RecentUpdate(string pconstring, Int32 pruid)
        {
            SetBase();
            Load(pconstring, pruid);
        }
        /// <summary>
        /// Instanicate new RecentUpdate from dr
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        internal RecentUpdate(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["ruid"] != System.DBNull.Value) { RUID = Convert.ToInt32(dr["ruid"].ToString()); }
                if (dr["RUTitle"] != System.DBNull.Value) { Title = (String)dr["RUTitle"]; }
                if (dr["RUIcon"] != System.DBNull.Value) { Icon = (String)dr["RUIcon"]; }
                if (dr["RULink"] != System.DBNull.Value) { Link = (String)dr["RULink"]; }
                if (dr["RUPubDate"] != System.DBNull.Value) { PublishedDate = (DateTime)dr["RUPubDate"]; }
                if (dr["RUComment"] != System.DBNull.Value) { Comments = (String)dr["RUComment"]; }
                if (dr["RUDesc"] != System.DBNull.Value) { Description = (String)dr["RUDesc"]; }
                         
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set Base to string empty
        /// </summary>
        private void SetBase()
        {
            Title = "";
            Icon = "";
            Link = "";
            Comments = "";
            Description = "";
        }
        /// <summary>
        /// Instanicate new RecentUpdate from dr
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        internal Boolean SetBase(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["ruid"] != System.DBNull.Value) { RUID = Convert.ToInt32(dr["ruid"].ToString()); }
                if (dr["RUTitle"] != System.DBNull.Value) { Title = (String)dr["RUTitle"]; }
                if (dr["RUIcon"] != System.DBNull.Value) { Icon = (String)dr["RUIcon"]; }
                if (dr["RULink"] != System.DBNull.Value) { Link = (String)dr["RULink"]; }
                if (dr["RUPubDate"] != System.DBNull.Value) { PublishedDate = (DateTime)dr["RUPubDate"]; }
                if (dr["RUComment"] != System.DBNull.Value) { Comments = (String)dr["RUComment"]; }
                if (dr["RUDesc"] != System.DBNull.Value) { Description = (String)dr["RUDesc"]; }

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set Base to value of [data]
        /// </summary>
        /// <param name="data">as recent update</param>
        /// <returns></returns>
        internal Boolean SetBase(RecentUpdate data)
        {
            try
            {
                Title = data.Title;
                Icon = data.Icon;
                Link = data.Link;
                PublishedDate = data.PublishedDate;
                Comments = data.Comments;
                Description = data.Description;

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
            return Title;
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

            Type type = typeof(RecentUpdate);
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
        /// Load Recent Update for [ruid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pruid">as recent update's  identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_recentupdates]</TableName>
        public bool Load(string pconstring, Int32 pruid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_recentupdates`");
                        query.Append(" WHERE ruid=?ruid; ");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ruid", MySqlDbType.Int32)).Value = pruid.ToString();

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
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Add Recent Update to system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[recentupdates]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addRecentUpdate", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptitle", MySqlDbType.VarChar, 50)).Value = Title.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?picon", MySqlDbType.VarChar, 255)).Value = Icon.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?plink", MySqlDbType.VarChar, 255)).Value = Link.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ppubdate", MySqlDbType.Datetime)).Value = PublishedDate.ToString("yyyy-MM-dd");
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcomments", MySqlDbType.VarChar, 140)).Value = Comments.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarString)).Value = Description.Trim();

                        //if the return value is not a guid then there was an error in the stored procedures
                        string s = DBValueToString(sqlcomm.ExecuteScalar());
                        if (IsNumeric(s))
                        {
                            RUID = Convert.ToInt32(s);
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
        /// Update Recent Update in System
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[rescentupdates]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateRecentUpdate", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pruid", MySqlDbType.Int32)).Value = RUID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ptitle", MySqlDbType.VarChar, 50)).Value = Title.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?picon", MySqlDbType.VarChar, 255)).Value = Icon.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?plink", MySqlDbType.VarChar, 255)).Value = Link.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ppubdate", MySqlDbType.Datetime)).Value = PublishedDate.ToString("yyyy-MM-dd");
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcomments", MySqlDbType.VarChar, 140)).Value = Comments.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarString)).Value = Description.Trim();

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
        /// Remove Recent Update from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[recentupdates]</TableName>
        public Boolean Delete(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteRecentUpdate", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pruid", MySqlDbType.Int32)).Value = RUID.ToString();
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
        /// Remove Recent Update from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pruid">as recent update's unique identifier</param>
        /// <returns></returns>
        /// <TableName>[rss_channels]</TableName>
        public static Boolean Delete(String pconstring, Int32 pruid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteRecentUpdate", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pruid", MySqlDbType.Int32)).Value = pruid.ToString();
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
        /// Return DataTable of all recent updates from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_recentupdates]</TableName>
        public static System.Data.DataTable dgRecentUpdates(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_recentupdates`");
                        query.Append(" ORDER BY `RUPubDate` DESC;");
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
        /// Return DataTable of all recent updates from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as value to search for</param>
        /// <returns></returns>
        /// <TableName>[vw_recentupdates]</TableName>
        public static System.Data.DataTable dgRecentUpdates(String pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_recentupdates`");
                        query.Append(string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
                        query.Append(" ORDER BY `RUPubDate` DESC;");
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
        /// Return DataTable for recent updates stored in xml
        /// </summary>
        /// <param name="xmlpath">as unc path to xml file</param>
        /// <returns></returns>
        public static System.Data.DataTable dgRecentUpdatesXML(String xmlpath)
        {
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlpath);

            //get all items
            XmlNodeList elemList = doc.GetElementsByTagName("item");
            DataTable dt = new DataTable();
            //dt.Columns.Add(new DataColumn("ruid", System.String));
            dt.Columns.Add(new DataColumn("RUTitle", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("RUIcon", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("RULink", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("RUComments", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("RUPubDate", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("RUDesc", Type.GetType("System.String")));

            
            for (int i = 0; i < elemList.Count; i++)
            {
                var node = elemList[i];
                DataRow dr = dt.NewRow();
                //if (node["ruid"] != null) { dr["RUTitle"] = (String)node["ruid"].InnerText; }
                if (node["title"] != null) { dr["RUTitle"] = (String)node["title"].InnerText; }
                if (node["icon"] != null) { dr["RUIcon"] = (String)node["icon"].InnerText; }
                if (node["link"] != null) { dr["RULink"] = (String)node["link"].InnerText; }
                if (node["comments"] != null) { dr["RUComments"] = (String)node["comments"].InnerText; }
                if (node["pubDate"] != null) { dr["RUPubDate"] = (String)node["pubDate"].InnerText; }
                if (node["description"] != null) { dr["RUDesc"] = (String)node["description"].InnerText; }

                dt.Rows.Add(dr);
            } 
            return dt.Copy();
        }

        /// <summary>
        /// Return DataTable of a summation of recent updates assigned 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_recentupdates]</TableName>
        public static System.Data.DataTable dgSummationByMonth(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT CONCAT_WS('-', Year(RUPubDate), Month(RUPubDate), '1') as 'Date',");
                        query.Append(" Year(RUPubDate) as 'Year', Month(RUPubDate) as 'Month', Count(1) as 'Updates'");
                        query.Append(" FROM vw_recentupdates");
                        query.Append(" GROUP BY Year(RUPubDate), Month(RUPubDate)");
                        query.Append(" ORDER BY Year(RUPubDate) DESC, Month(RUPubDate) DESC");
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
        /// Return DataTable for all updates assoicted with the month and year [data]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pdate">as data time to select for</param>
        /// <returns></returns>
        /// <TableName>[vw_recentupdates]</TableName>
        public static DataTable dgRecentUpdatesForMonth(string pconstring, DateTime pdate)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_recentupdates`");
                        query.Append(string.Format(" WHERE Month(`RUPubDate`) = ?Month AND Year(`RUPubDate`) = ?Year"));
                        query.Append(" ORDER BY `RUPubDate` DESC;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?Month", MySqlDbType.Int16)).Value = pdate.Month;
                        sqlcomm.Parameters.Add(new MySqlParameter("?Year", MySqlDbType.Int16)).Value = pdate.Year;

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
        /// Write RecentUpdate to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of RecentUpdate</param>
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

                    xmlSer = new XmlSerializer(typeof(RecentUpdate));
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
        /// Create RecentUpdate from File
        /// </summary>
        /// <param name="xmlpath">as source location of RecentUpdate</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(RecentUpdate));
                RecentUpdate output = (RecentUpdate)xs.Deserialize(fs);
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
        /// <param name="data">as RecentUpdate</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(RecentUpdate data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(RecentUpdate));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(RecentUpdate));
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
        /// Load RecentUpdate from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of RecentUpdate</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserialize(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(RecentUpdate));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                RecentUpdate output = default(RecentUpdate);
                output = (RecentUpdate)xmlSer.Deserialize(string_reader);
                return SetBase(output);
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Return RecentUpdate from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of RecentUpdate</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static RecentUpdate XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(RecentUpdate));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                RecentUpdate output = default(RecentUpdate);
                output = (RecentUpdate)xmlSer.Deserialize(string_reader);
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