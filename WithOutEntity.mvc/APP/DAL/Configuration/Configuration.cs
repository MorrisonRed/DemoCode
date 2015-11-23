using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;
using System.ComponentModel;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Configuration
{
    [DataObject]
    [XmlRoot("config")]
    [Serializable()]
    public class Configuration
    {

        private Hashtable _items;

        #region Public Properties
        /// <summary>
        /// Get the Configuration Items
        /// </summary>
        public Hashtable Items
        {
            get { return _items; }
        }
        /// <summary>
        /// Get Last Error Thown
        /// </summary>
        [XmlIgnore()]
        public static Exception GetLastError { get; set; }
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Instanicate New Configuration
        /// </summary>
        public Configuration()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate New Configuration
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        public Configuration(string pconstring)
        {
            SetBase();
            Load(pconstring);
        }
        /// <summary>
        /// Set Base to values of string.empty
        /// </summary>
        private void SetBase()
        {
            _items = new Hashtable();
        }
        #endregion

        #region Functions and Sub Routines
        public override string ToString()
        {
            if (Items == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry k in Items)
            {
                sb.Append(string.Format("{0}->{1};", k.Key, k.Value));
            }
            return sb.ToString();
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
        private string DBValueToString(object value)
        {
            if (value != System.DBNull.Value && value != null) { return value.ToString(); }
            return "";
        }
        /// <summary>
        /// Return the Number of Items in the collection
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            if (_items == null) return 0;

            return _items.Count;
        }
        /// <summary>
        /// Return true is key exist in the collection
        /// </summary>
        /// <param name="pkey">as key to search for</param>
        /// <returns></returns>
        public bool hasKey(string pkey)
        {
            if (_items == null) return false;

            object o = Items[pkey];
            if (o == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Load Configuration 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[config]</TableName>
        public Boolean Load(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(" SELECT * FROM config;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();
                        MySqlDataReader sqlr = sqlcomm.ExecuteReader();
                        if (sqlr.HasRows)
                        {
                            _items = new Hashtable();
                            var key = "";
                            var value = "";
                            while (sqlr.Read())
                            {
                                key = sqlr["key"].ToString();
                                value = sqlr["value"].ToString();
                                if (!string.IsNullOrEmpty(key))
                                {
                                    _items.Add(key, value);
                                }
                            }

                            return true;
                        }
                        else
                        {
                            GetLastError = new Exception("no data found");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Add Configuration to system
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <param name="pkey">as configuration setting key</param>
        /// <param name="pvalue">as configuration setting value</param>
        /// <returns></returns>
        /// <TableName>[config]</TableName>
        public Boolean Add(String pconstring, string pkey, string pvalue)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("INSERT INTO config (`key`,`value`)");
                        query.Append(" VALUES(?key,?value);");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?key", MySqlDbType.VarChar, 50)).Value = pkey;
                        sqlcomm.Parameters.Add(new MySqlParameter("?value", MySqlDbType.VarChar, 256)).Value = pvalue;

                        sqlcomm.ExecuteNonQuery();

                        //add item to collection
                        if (Items == null) _items = new Hashtable();
                        _items.Add(pkey, pvalue);

                        return true;

                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Update Configuration in system
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <param name="pkey">as configuration setting key</param>
        /// <param name="pvalue">as configuration setting value</param>
        /// <returns></returns>
        /// <TableName>[config]</TableName>
        public Boolean Update(String pconstring, string pkey, string pvalue)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("UPDATE config SET value=?value WHERE `key`=?key;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?key", MySqlDbType.VarChar, 50)).Value = pkey;
                        sqlcomm.Parameters.Add(new MySqlParameter("?value", MySqlDbType.VarChar, 256)).Value = pvalue;

                        sqlcomm.ExecuteNonQuery();

                        //update item in collection
                        if (Items != null) _items[pkey] = pvalue;

                        return true;
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Delete Configuration from system
        /// </summary>
        /// <param name="pconstring">as data source conenction string</param>
        /// <param name="pkey">as configuration setting key</param>
        /// <returns></returns>
        /// <TableName>[config]</TableName>
        public Boolean Delete(String pconstring, string pkey)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("DELETE FROM config WHERE `key`=?key;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?key", MySqlDbType.VarChar, 50)).Value = pkey;

                        sqlcomm.ExecuteNonQuery();

                        //remove item from collection
                        if (Items != null) _items.Remove(pkey);

                        return true;
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Return DataTable of all configuration settings in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[config]</TableName>
        public static System.Data.DataTable dgConfigSettings(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string query = "";
                        query += "SELECT * FROM config";
                        query += " ORDER BY `key` ASC;";
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
        /// Return DataTable of all configuration settings where [colname] contains [colval]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as text value to search for</param>
        /// <returns></returns>
        /// <TableName>[config]</TableName>
        public static System.Data.DataTable dgConfigSettings(String pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string query = "";
                        query += "SELECT * FROM config";
                        query += string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim());
                        query += string.Format(" ORDER BY `{0}` ASC;", pcolname.Trim());
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
        #endregion
    }
}
