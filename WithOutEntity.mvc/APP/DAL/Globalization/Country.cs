using System;
using System.IO;
using System.Xml;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient; 

//==========================================================================================
//User OBJECT
//
//==========================================================================================
//<XmlRoot("country")> _
//<Serialization.XmlTypeAttribute(Namespace:="http://services.morrisonred.com"), _
//Serialization.XmlRootAttribute("morrisonred", [Namespace]:="http://services.morrisonred.com", IsNullable:=False)> _
namespace Globalization
{
    [DataObject]
    [XmlRoot("country")]
    [Serializable()]
    public class Country
    {
        private string _code;
        private string _name;
        private string _continent;
        private string _region;
        private float _surfacearea;
        private Int16 _indepyear;
        private Int16 _population;
        private float _lifeexpectancy;
        private float _gnp;
        private float _gnpold;
        private string _localname;
        private string _governmentform;
        private string _headofstate;
        private Int16 _capitalid;
        private string _code2;

        [XmlIgnore(), NonSerialized()]
        public static Exception _LastError;

        #region Public Properties

        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Instanicate New Country
        /// </summary>
        public Country()
        {
            SetBase();
        }
        /// <summary>
        /// Set MyBase to values of string.empty
        /// </summary>
        private void SetBase()
        {

        }
        #endregion

        #region Functions and Sub Routines
        public override string ToString()
        {
            return _name.ToString();
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
        /// Return all countries from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_countries]</TableName>
        public static System.Data.DataTable dgCountries(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_countries`";
                        query += " ORDER BY `CountryName` ASC";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblCountry");

                            if (ds.Tables["tblCountry"].Rows.Count == 0)
                            {
                                _LastError = new Exception("no data found");
                                return null;
                            }

                            return ds.Tables["tblCountry"].Copy();
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