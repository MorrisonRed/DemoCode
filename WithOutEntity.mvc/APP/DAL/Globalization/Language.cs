using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient; 

//==========================================================================================
//User OBJECT
//
//==========================================================================================
//<XmlRoot("language")> _
//<Serialization.XmlTypeAttribute(Namespace:="http://services.morrisonred.com"), _
//Serialization.XmlRootAttribute("morrisonred", [Namespace]:="http://services.morrisonred.com", IsNullable:=False)> _
namespace Globalization
{
    [DataObject]
    [XmlRoot("language")]
    [Serializable()]
    public class Language
    {
        private string _code;
        private string _name_en;
        private string _name_native;
        private string _iso6391;
        private string _iso6392;
        private string _comments;

        [XmlIgnore(), NonSerialized()]
        public static Exception _LastError;

        #region Public Properties
        [Display(Name ="Language Code")]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        [Display(Name = "Language (EN)")]
        public string Name_EN
        {
            get { return _name_en; }
            set { _name_en = value; }
        }
        [Display(Name = "Language (native)")]
        public string Name_Native
        {
            get { return _name_native; }
            set { _name_native = value; }
        }
        [Display(Name = "ISO6391")]
        public string ISO6391
        {
            get { return _iso6391; }
            set { _iso6391 = value; }
        }
        [Display(Name = "ISO6392")]
        public string ISO6392
        {
            get { return _iso6392; }
            set { _iso6392 = value; }
        }
        [Display(Name = "Comments")]
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Instaicate New Language
        /// </summary>
        public Language()
        {
            SetBase();
        }
        /// <summary>
        /// Instanciate New Language
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data.datarow</param>
        /// <returns></returns>
        internal Language(string pconstring, System.Data.DataRow dr)
        {
            try
            {
                if (dr["LangCode"] != System.DBNull.Value) { _code = (String)dr["LangCode"]; }
                if (dr["LangName_en"] != System.DBNull.Value) { _name_en = (String)dr["LangName_en"]; }
                if (dr["LangName_native"] != System.DBNull.Value) { _name_native = (String)dr["LangName_native"]; }
                if (dr["LangISO6391"] != System.DBNull.Value) { _iso6391 = (String)dr["LangISO6391"]; }
                if (dr["LangISO6392"] != System.DBNull.Value) { _iso6392 = (String)dr["LangISO6392"]; }
                if (dr["LangComments"] != System.DBNull.Value) { _comments = (String)dr["LangComments"]; }
            }
            catch (Exception ex)
            {
                _LastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBase to values of string.empty 
        /// </summary>
        private void SetBase()
        { 
            
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
                if (dr["LangCode"] != System.DBNull.Value) { _code = (String)dr["LangCode"]; }
                if (dr["LangName_en"] != System.DBNull.Value) { _name_en = (String)dr["LangName_en"]; }
                if (dr["LangName_native"] != System.DBNull.Value) { _name_native = (String)dr["LangName_native"]; }
                if (dr["LangISO6391"] != System.DBNull.Value) { _iso6391 = (String)dr["LangISO6391"]; }
                if (dr["LangISO6392"] != System.DBNull.Value) { _iso6392 = (String)dr["LangISO6392"]; }
                if (dr["LangComments"] != System.DBNull.Value) { _comments = (String)dr["LangComments"]; }

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
            return _name_en.ToString();
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

        /// <summary>
        /// Return all languages from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_languages]</TableName>
        public static List<Language> ToList(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_languages`";
                        query += " ORDER BY `LangName_en` ASC";
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

                            List<Language> lst = new List<Language>();

                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            {
                                lst.Add(new Language(pconstring, dr));
                            }

                            return lst;
                        }//end using da
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return null;
                    }//end try
                }//end using sqlcomm
            }//end using sqlconn
        }

        /// <summary>
        /// Return all languages from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_languages]</TableName>
        public static System.Data.DataTable dgLanguages(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_languages`";
                        query += " ORDER BY `LangName_en` ASC";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblLang");

                            if (ds.Tables["tblLang"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return null;
                            }

                            return ds.Tables["tblLang"].Copy();
                        }//end using da
                    }
                    catch (Exception ex)
                    {
                        _LastError = ex;
                        return null;
                    }//end try
                }//end using sqlcomm
            }//end using sqlconn
        }

        #endregion 
    }
}