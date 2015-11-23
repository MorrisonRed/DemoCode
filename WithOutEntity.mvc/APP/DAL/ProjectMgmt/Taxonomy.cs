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

namespace ProjectMgmt
{
    /// <summary>
    /// Taxonomy classification of projects
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("project")]
    public class Taxonomy
    {
        /*
        * tid (int) = taxonomy identifier
        * name (string)  = the term
        * descripton (string) = a desciption of the term
        * weight (int) = the weight in terms to the relations of other terms
       */
        #region "Public properties"
        /// <summary>
        /// Get/Set the Taxonomy identifier
        /// </summary>
        [XmlElement(ElementName = "tid")]
        public int TID { get; set; }
        /// <summary>
        /// Get/Set the Taxonomy term
        /// </summary>
        [XmlElement(ElementName = "name")]
        public String Name { get; set; }
        /// <summary>
        /// Get/Set Description of the term
        /// </summary>
        [XmlElement(ElementName = "description")]
        public String Description { get; set; }
        /// <summary>
        /// Get/Set Weight in terms of other terms
        /// </summary>
        [XmlElement(ElementName = "weight")]
        public int Weight { get; set; }

        /// <summary>
        /// Get/Set Last Error Thrown
        /// </summary>
        [XmlIgnore()]
        public static Exception GetLastError { get; set; }
        #endregion

        #region "Constructors and Destructors"
        /// <summary>
        /// Instanciate New Project Taxonomy Term
        /// </summary>
        public Taxonomy()
        {
            SetBase();
        }
        /// <summary>
        /// Set My Base to values of [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        /// <returns></returns>
        internal Taxonomy(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["tid"] != System.DBNull.Value) { TID = Convert.ToInt16(dr["tid"]); }
                if (dr["TxName"] != System.DBNull.Value) { Name = (String)dr["TxName"]; }
                if (dr["TxDesc"] != System.DBNull.Value) { Description = (String)dr["TxDesc"]; }
                if (dr["TxWeight"] != System.DBNull.Value) { Weight = Convert.ToInt16(dr["TxWeight"]); }
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBase to values of string.empty
        /// </summary>
        private void SetBase()
        {
            Name = "";
            Description = "";
        }
        /// <summary>
        /// Set MyBase to values of [data]
        /// </summary>
        /// <param name="data">as taxonomy</param>
        /// <returns></returns>
        internal Boolean SetBase(Taxonomy data)
        {
            try
            {
                TID = data.TID;
                Name = data.Name;
                Description = data.Description;
                Weight = data.Weight;

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set My Base to values of [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        /// <returns></returns>
        internal Boolean SetBase(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["tid"] != System.DBNull.Value) { TID = Convert.ToInt16(dr["tid"]); }
                if (dr["TxName"] != System.DBNull.Value) { Name = (String)dr["TxName"]; }
                if (dr["TxDesc"] != System.DBNull.Value) { Description = (String)dr["TxDesc"]; }
                if (dr["TxWeight"] != System.DBNull.Value) { Weight = Convert.ToInt16(dr["TxWeight"]); }

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        #endregion

        #region Functions and Sub Routines
        public override string ToString()
        {
            return this.Name.ToString();
        }
        public static bool IsNumeric(string stringToTest)
        {
            int num;
            return int.TryParse(stringToTest, out num);
        }
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
        private bool ColumnExists(string col, DataRow dr)
        {
            if (dr.Table.Columns.Contains(col))
            {
                return true;
            }
            return false;
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

            Type type = typeof(Project);
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
        /// Return DataTable representation of a list of projects
        /// </summary>
        /// <param name="projects">as list of projects</param>
        /// <returns></returns>
        public DataTable ToDataTable(List<Project> projects)
        {
            Type type = typeof(Project);
            var properties = type.GetProperties();

            DataTable dt = new DataTable();
            //add columns
            foreach (PropertyInfo info in properties)
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }

            //add row for each project in list
            foreach (Project p in projects)
            {
                foreach (PropertyInfo info in properties)
                {
                    try
                    {
                        DataRow dr = dt.NewRow();
                        info.GetValue(p, null);
                        dr[info.Name] = info.GetValue(p, null);

                        dt.Rows.Add(dr);
                    }
                    catch
                    {

                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Load Taxonomy for [ptid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ptid">as taxonomy identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_taxonomyprojects]</TableName>
        public bool Load(string pconstring, int ptid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_taxonomyprojects "));
                        query.Append(string.Format(" WHERE tid=?ptid; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptid", MySqlDbType.Int16)).Value = ptid.ToString();

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
        /// Return taxonomy for [ptid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ptid">as taxonomy identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_taxonomyprojects]</TableName>
        public static Taxonomy getTaxonomy(string pconstring, int ptid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_taxonomyprojects "));
                        query.Append(string.Format(" WHERE tid=?ptid; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptid", MySqlDbType.Int16)).Value = ptid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return null;
                            }

                            return new Taxonomy(pconstring, ds.Tables["tbl"].Rows[0]);
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
        /// Add Taxonomy term to system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[taxonomy_projects]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addTaxonomyProject", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pName", MySqlDbType.VarChar, 255)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDesc", MySqlDbType.VarString)).Value = Description.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pWeight", MySqlDbType.Int16)).Value = Weight;
                        
                        string s = DBValueToString(sqlcomm.ExecuteScalar());
                        if (IsNumeric(s))
                        {
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
                        throw ex;
                        //return false;
                    }
                }
            }
        }
        /// <summary>
        /// Add Taxonomy classification [tid] to project [pid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ptid">as taxonomy identifier</param>
        /// <param name="ppid">as project's identifier</param>
        /// <returns></returns>
        /// <TableName>[projects_taxonomy]</TableName>
        public static bool AddToProject(string pconstring, int ptid, Guid ppid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("INSERT INTO projects_taxonomy (tid, pid)");
                        query.Append(string.Format(" VALUES (?tid, ?pid);"));
                        sqlcomm.CommandText = query.ToString();
                        
                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?tid", MySqlDbType.Int32)).Value = ptid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pid", MySqlDbType.VarChar, 50)).Value = ppid.ToString();

                        sqlcomm.ExecuteNonQuery();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        throw ex;
                        //return false;
                    }
                }
            }
        }

        /// <summary>
        /// Update Taxonomy in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[taxonomy_projects]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateTaxonomyProject", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptid", MySqlDbType.Int16)).Value = TID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pName", MySqlDbType.VarChar, 255)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDesc", MySqlDbType.VarString)).Value = Description.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pWeight", MySqlDbType.Int16)).Value = Weight;

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
        /// Remove Taxonomy from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[taxonomy_projects]</TableName>
        public Boolean Delete(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteTaxonomyProject", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptid", MySqlDbType.Int16)).Value = TID.ToString();
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
        /// Remove Taxonomy from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ptid">as projects's unique identifier</param>
        /// <returns></returns>
        /// <TableName>[taxonomy_projects]</TableName>
        public static Boolean Delete(String pconstring, int ptid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteTaxonomyProject", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptid", MySqlDbType.Int16)).Value = ptid.ToString();
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
        /// Remove Taxonomy classification [tid] from project [pid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ptid">as taxonomy identifier</param>
        /// <param name="ppid">as project's identifier</param>
        /// <returns></returns>
        /// <TableName>[projects_taxonomy]</TableName>
        public static bool DeleteFromProject(string pconstring, int ptid, Guid ppid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("DELETE FROM projects_taxonomy");
                        query.Append(string.Format(" WHERE tid=?tid AND pid=?pid;"));
                        sqlcomm.CommandText = query.ToString();
                        
                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?tid", MySqlDbType.Int32)).Value = ptid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pid", MySqlDbType.VarChar, 50)).Value = ppid.ToString();

                        sqlcomm.ExecuteNonQuery();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        throw ex;
                        //return false;
                    }
                }
            }
        }

        /// <summary>
        /// Return List of Taxonomy terms
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_projects_taxonomy]</TableName>
        public static List<Taxonomy> ListFor(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_projects_taxonomy");
                        query.Append(string.Format(" ORDER BY `TxName` ASC;"));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return null;
                            }

                            List<Taxonomy> lst = new List<Taxonomy>();
                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            {
                                lst.Add(new Taxonomy(pconstring, dr));
                            }
                            return lst;
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
        /// Return List of Taxonomy assinged to project [pid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ppid">as projects identifer</param>
        /// <returns></returns>
        /// <TableName>[vw_taxonomyforprojects]</TableName>
        public static List<Taxonomy> ListFor(string pconstring, Guid ppid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_taxonomyforprojects");
                        query.Append(" WHERE `pid`=?pid");
                        query.Append(string.Format(" ORDER BY `TxName` ASC;"));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pid", MySqlDbType.VarChar, 50)).Value = ppid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return null;
                            }

                            List<Taxonomy> lst = new List<Taxonomy>();
                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            {
                                lst.Add(new Taxonomy(pconstring, dr));
                            }
                            return lst;
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
        /// Return DataTable of all taxonomy in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_taxonomyprojects]</TableName>
        public static DataTable dgTaxonomy(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_taxonomyprojects");
                        query.Append(" ORDER BY `TxName` ASC;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return null;
                            }

                            return ds.Tables["tbl"].Copy();
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
        /// Return DataTable of all taxonomy in system where [colname] contains [colval]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as value to search for</param>
        /// <returns></returns>
        /// <TableName>[vw_taxonomyprojects]</TableName>
        public static DataTable dgTaxonomy(string pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_taxonomyprojects");
                        query.Append(string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
                        query.Append(string.Format(" ORDER BY `{0}` ASC;", pcolname));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return null;
                            }

                            return ds.Tables["tbl"].Copy();
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
        /// Return DataTable of Taxonomy assinged to project [pid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ppid">as projects identifer</param>
        /// <returns></returns>
        /// <TableName>[vw_taxonomyforprojects]</TableName>
        public static DataTable dgProjectTaxonomy(string pconstring, Guid ppid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_taxonomyforprojects");
                        query.Append(" WHERE `pid`=?pid");
                        query.Append(string.Format(" ORDER BY `TxName` ASC;"));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pid", MySqlDbType.VarChar, 50)).Value = ppid.ToString();

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
                        throw ex;
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
        /// Write taxonomy to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of taxonomy</param>
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

                    xmlSer = new XmlSerializer(typeof(Taxonomy));
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
        /// Create Taxonomy from File
        /// </summary>
        /// <param name="xmlpath">as source location of Taxonomy </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(Taxonomy));
                Taxonomy output = (Taxonomy)xs.Deserialize(fs);
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
        /// <param name="data">as Taxonomy</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(Taxonomy data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Taxonomy));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(Taxonomy));
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
        /// Return Taxonomy from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of Taxonomy</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Taxonomy XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Taxonomy));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                Taxonomy output = default(Taxonomy);
                output = (Taxonomy)xmlSer.Deserialize(string_reader);
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