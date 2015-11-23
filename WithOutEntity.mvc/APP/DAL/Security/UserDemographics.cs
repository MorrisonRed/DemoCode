using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient; 


//==========================================================================================
//User Demographics OBJECT
//
//==========================================================================================
//<XmlRoot("user")> _
//<Serialization.XmlTypeAttribute(Namespace:="http://services.morrisonred.com"), _
//Serialization.XmlRootAttribute("morrisonred", [Namespace]:="http://services.morrisonred.com", IsNullable:=False)> _
namespace CustomSecurity
{
    [DataObject]
    [XmlRoot("userdemographics")]
    [Serializable()]
    public class UserDemographics
    {
        private Guid _uid;
        private string _fname;
        private string _lname;
        private int _age;
        private string _gender;
        private DateTime _dob;
        private string _lang;
        private string _country;
        private string _postal;
        private string _phonem;

        [XmlIgnore(), NonSerialized()]
        private static Exception _LastError;

        #region Public Properties
        /// <summary>
        /// Get/Set the user's identifier
        /// </summary>
        [Display(Name = "UID")]
        [XmlElement(ElementName = "uid")]
        public Guid UID
        {
            get { return _uid; }
            set { _uid = value; }
        }
        /// <summary>
        /// Get/Set the user's first name
        /// </summary>
        [Display(Name = "First Name")]
        [XmlElement(ElementName = "fname", DataType = "string")]
        public string FirstName
        {
            get { return _fname; }
            set { _fname = value; }
        }
        /// <summary>
        /// Get/Set the user's last name
        /// </summary>
        [Display(Name = "Last Name")]
        [XmlElement(ElementName = "lname", DataType = "string")]
        public string LastName
        {
            get { return _lname; }
            set { _lname = value; }
        }
        /// <summary>
        /// Summary:
        ///   Get the user's current age
        /// 
        /// Note:
        ///   This is a calculated value based on the current date/time and date of birth
        /// </summary>
        [Display(Name = "Age")]
        [XmlElement(ElementName = "age", DataType = "int")]
        public int Age
        {
            get { return _age; }
        }
        /// <summary>
        /// Summar:
        ///   Get/Set the user's gender (sex)
        /// 
        /// Note:
        ///   M = male; F = female
        /// </summary>
        [Display(Name = "Gender")]
        [XmlElement(ElementName = "gender", DataType = "string")]
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        /// <summary>
        /// Get/Set the user's date of birth
        /// </summary>
        [Display(Name = "Date Of Birth")]
        [XmlElement(ElementName = "dob")]
        public DateTime DateOfBirth
        {
            get { return _dob; }
            set { _dob = value; }
        }
        /// <summary>
        /// Get/Set the user's preferred (primary language)
        /// </summary>
        [Display(Name = "Language")]
        [XmlElement(ElementName="lang", DataType="string")]
        public string Lanaguage
        {
            get { return _lang; }
            set { _lang = value; }
        }
        /// <summary>
        /// Get/Set the associated country code assinged to this user
        /// </summary>
        [Display(Name = "Country")]
        [XmlElement(ElementName="country", DataType="string")]
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }
        /// <summary>
        /// Get/Set the postal code
        /// </summary>
        [Display(Name = "Postal Code")]
        [XmlElement(ElementName="postal", DataType="string")]
        public string PostalCode
        {
            get { return _postal; }
            set { _postal = value; }

        }
        /// <summary>
        /// Get/Set user's mobile phone number
        /// </summary>
        [Display(Name = "Mobile Number")]
        [XmlElement(ElementName="phonem", DataType="string")]
        public string PhoneMobile
        {
            get { return _phonem; }
            set { _phonem = value; }
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
        /// Instanicate New Person Demographics
        /// </summary>
        public UserDemographics()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate new user demographic from [dr]
        /// </summary>
        /// <param name="dr">as system.data.datarow</param>
        internal UserDemographics(System.Data.DataRow dr)
        {
            try
            {
                if (dr["UID"] != System.DBNull.Value) { _uid = new Guid(dr["UID"].ToString()); }
                if (dr["UserFName"] != System.DBNull.Value) { _fname = (String)dr["UserFName"]; }
                if (dr["UserLName"] != System.DBNull.Value) { _lname = (String)dr["UserLName"]; }
                if (dr["UserGender"] != System.DBNull.Value) { _gender = (String)dr["UserGender"]; }
                if (dr["UserDOB"] != System.DBNull.Value) { _dob = (DateTime)dr["UserDOB"]; }
                if (dr["UserLang"] != System.DBNull.Value) { _lang = (String)dr["UserLang"]; }
                if (dr["UserCountry"] != System.DBNull.Value) { _country = (String)dr["UserCountry"]; }
                if (dr["UserPostal"] != System.DBNull.Value) { _postal = (String)dr["UserPostal"]; }
                if (dr["UserPhoneM"] != System.DBNull.Value) { _phonem = (String)dr["UserPhoneM"]; }
            }
            catch (Exception ex)
            {
                _LastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set MyBases to String.Empty 
        /// </summary>
        private void SetBase()
        {
            _fname = "";
            _lname = "";
            _gender = "";
            _lang = "";
            _country = "";
            _postal = "";
            _phonem = "";
        }
        /// <summary>
        /// Set MyBase to values of [dr]
        /// </summary>
        /// <param name="dr">as data.datarow</param>
        /// <returns></returns>
        internal Boolean SetBase(System.Data.DataRow dr)
        {
            try
            {
                if (dr["UID"] != System.DBNull.Value) { _uid = new Guid(dr["UID"].ToString()); }
                if (dr["UserFName"] != System.DBNull.Value) { _fname = (String)dr["UserFName"]; }
                if (dr["UserLName"] != System.DBNull.Value) { _lname = (String)dr["UserLName"]; }
                if (dr["UserGender"] != System.DBNull.Value) { _gender = (String)dr["UserGender"]; }
                if (dr["UserDOB"] != System.DBNull.Value) { _dob = (DateTime)dr["UserDOB"]; }
                if (dr["UserLang"] != System.DBNull.Value) { _lang = (String)dr["UserLang"]; }
                if (dr["UserCountry"] != System.DBNull.Value) { _country = (String)dr["UserCountry"]; }
                if (dr["UserPostal"] != System.DBNull.Value) { _postal = (String)dr["UserPostal"]; }
                if (dr["UserPhoneM"] != System.DBNull.Value) { _phonem = (String)dr["UserPhoneM"]; }
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
            string fullname;
            if (string.IsNullOrEmpty(_fname)) { _fname = ""; }
            if (string.IsNullOrEmpty(_lname)) { _lname = ""; }
            fullname = _fname.ToString() + " " + _lname.ToString();
            return fullname.Trim();
        }
        /// <summary>
        /// Return the Memberships User's Full Name
        /// </summary>
        /// <returns></returns>
        public string ToFullName()
        {
            string fullname;
            if (string.IsNullOrEmpty(_fname)) { _fname = ""; }
            if (string.IsNullOrEmpty(_lname)) { _lname = ""; }
            fullname = _fname.ToString() + " " + _lname.ToString();
            return fullname.Trim();
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
        /// Check whether the user [puid] has a corresponding record for demographics
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="puid">as user's unique identifier</param>
        /// <returns>true if a record does exist; otherwise false;</returns>
        /// <TableName>[users_demographics]</TableName>
        public static bool RecordExists(string pconstring, Guid puid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `users_demographics`";
                        query += " WHERE `uid`=?puid;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = puid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tblUsers");

                            //if there are not row then the user does not have a 
                            //corresponding record in the table
                            if (ds.Tables["tblUsers"].Rows.Count == 0)
                            {
                                return false;
                            }
                            else 
                            {
                                return true;
                            }
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
        /// Load User's Demongraphic information for [uid]
        /// </summary>
        /// <param name="constring">as data source connection string</param>
        /// <param name="uid">as user's identifier</param>
        /// <returns></returns>
        /// <DBTable>vw_usersdemographics</DBTable>
        public Boolean Load(string constring, Guid uid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(constring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_dgUserDemo_UID", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = uid.ToString();
                        
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
        /// Add demographics to sysystem
        /// </summary>
        /// <param name="constring">as data source connection string</param>
        /// <returns>[users_demographics]</returns>
        public Boolean Add(string constring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(constring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addUserDemo", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = _uid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pfname", MySqlDbType.VarChar, 50)).Value = _fname.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?plname", MySqlDbType.VarChar, 50)).Value = _lname.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pgender",MySqlDbType.Char, 1)).Value =  _gender.Trim().ToUpper();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdob", MySqlDbType.Datetime)).Value = _dob.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?plang", MySqlDbType.VarChar, 10)).Value =  _lang.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcountry", MySqlDbType.Char, 3)).Value = _country.Trim().ToUpper();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ppostal", MySqlDbType.VarChar, 10)).Value = _postal.Trim().ToUpper();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pphonem", MySqlDbType.VarChar, 20)).Value = _phonem.Trim();

                        //there is not identity column in this table so there is not return value
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
        /// Update demographics in sysystem
        /// </summary>
        /// <param name="constring">as data source connection string</param>
        /// <returns>[users_demographics]</returns>
        public Boolean Update(string constring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(constring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateUserDemo", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?puid", MySqlDbType.VarChar, 50)).Value = _uid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pfname", MySqlDbType.VarChar, 50)).Value = _fname.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?plname", MySqlDbType.VarChar, 50)).Value = _lname.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pgender", MySqlDbType.Char, 1)).Value = _gender.Trim().ToUpper();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdob", MySqlDbType.Datetime)).Value = _dob.ToString("yyyy-MM-dd HH:mm:ss");
                        sqlcomm.Parameters.Add(new MySqlParameter("?plang", MySqlDbType.VarChar, 10)).Value = _lang.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcountry", MySqlDbType.Char, 3)).Value = _country.Trim().ToUpper();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ppostal", MySqlDbType.VarChar, 10)).Value = _postal.Trim().ToUpper();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pphonem", MySqlDbType.VarChar, 20)).Value = _phonem.Trim();

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
        #endregion 
    }
}