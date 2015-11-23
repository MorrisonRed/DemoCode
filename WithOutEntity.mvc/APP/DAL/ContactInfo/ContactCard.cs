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
    /// Contact Card definition
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("contactcard")]
    public class ContactCard
    {
		/* 
		 *cntid (guid)
         *type (contactcardtype)
         *title (string)
         *firstname (string)
         *lastname (string)
         *middlename (string)
         *suffix (string)
         *organization (string)
         *avatar (byte)
         *showinglobaladdresslist (bool)
         *zindex (int)
         *department (string)
         *internalno (string)
         *externalno (string)
         *notes (string)
         * 
         * title_fr (string)
         * department_fr (string)
         * company_fr (string)
         * firstname_fr (string)
         * lastname_fr (string)
         * middlename_fr (string)
         * suffix_fr (string)
         * notes_fr (string)
         * 
         * emails (list)
         * phones (list)
         * address (list)
         * members (list)
         */

        #region "Public Properties"
		/// <summary>
		/// Get/Set Contact Cards Identifier
		/// </summary>
        [XmlElement(ElementName = "cntid")]
        public Guid CNTID { get; set; }
		/// <summary>
		/// Get/Set the contact card type
		/// </summary>
		[XmlElement(ElementName = "type")]
        public ContactCardType Type { get; set; }
		/// <summary>
		/// Get/Set the title
		/// </summary>
		[XmlElement(ElementName = "title")]
		public String Title { get; set; }
		/// <summary>
		/// Get/Set the First Name
		/// </summary>
		[XmlElement(ElementName = "fname")]
        public String FirstName { get; set; }
		/// <summary>
		/// Get/Set the Last Name
		/// </summary>
		[XmlElement(ElementName = "lname")]
        public String LastName { get; set; }
		/// <summary>
		/// Get/Set the Middle Name
		/// </summary>
		[XmlElement(ElementName = "mname")]
        public String MiddleName { get; set; }
		/// <summary>
		/// Get/Set the Name Suffix
		/// </summary>
		[XmlElement(ElementName = "suffix")]
        public String Suffix { get; set; }
		/// <summary>
		/// Get/Set the Organiziation Name
		/// </summary>
		[XmlElement(ElementName = "company")]
        public String Organization { get; set; }
		/// <summary>
		/// Get/Set whether the contact card is to be displayed in the global address list
		/// </summary>
		[XmlElement(ElementName = "showinglobal")]
        public Boolean ShowInGlobalAddressList { get; set; }
		/// <summary>
		/// Get/Set the associated avatar with the contact card
		/// </summary>
		[XmlElement(ElementName = "avatar")]
        public Byte[] Avatar { get; set; }
		/// <summary>
		/// Get/Set the contact's card's stack value index
		/// </summary>
		[XmlElement(ElementName = "zindex")]
        public int ZIndex { get; set; }
		/// <summary>
		/// Get/Set the department name
		/// </summary>
		[XmlElement(ElementName = "department")]
        public String Department { get; set; }
		/// <summary>
		/// Get/Set the internal reference number i.e. what internal users identifed the contact card as 
		/// </summary>
		[XmlElement(ElementName = "internalref")]
        public String InternalNo { get; set; }
		/// <summary>
		/// Get/Set the external reference number i.e. what they identify themselves as
		/// </summary>
		[XmlElement(ElementName = "externalref")]
        public String ExternalNo { get; set; }
		/// <summary>
		/// Get/Set the notes assoicated to this contact card
		/// </summary>
        [XmlElement(ElementName = "notes")]
        public String Notes { get; set; }
		/// <summary>
		/// Get/Set the title (French)
		/// </summary>
		[XmlElement(ElementName = "title_fr")]
        public String Title_FR { get; set; }
		/// <summary>
		/// Get/Set the Department (French)
		/// </summary>
		[XmlElement(ElementName = "department_fr")]
        public String Department_FR { get; set; }
		/// <summary>
		/// Get/Set the Organization (French)
		/// </summary>
		[XmlElement(ElementName = "company_fr")]
        public String Organization_FR { get; set; }
		/// <summary>
		/// Get/Set the first name (French)
		/// </summary>
		[XmlElement(ElementName = "fname_fr")]
        public String FirstName_FR { get; set; }
		/// <summary>
		/// Get/Set the Last Name (French)
		/// </summary>
		[XmlElement(ElementName = "lname_fr")]
        public String LastName_FR { get; set; }
		/// <summary>
		/// Get/Set the Middle Name (French)
		/// </summary>
		[XmlElement(ElementName = "mname_fr")]
        public String MiddleName_FR { get; set; }
		/// <summary>
		/// Get/Set the name suffix (French)
		/// </summary>
		[XmlElement(ElementName = "suffix_fr")]
        public String Suffix_FR { get; set; }
		/// <summary>
		/// Get/Set the notes (French)
		/// </summary>
		[XmlElement(ElementName = "notes_fr")]
        public String Notes_FR { get; set; }

		/// <summary>
		/// Get/Set the assoicated emails with contact card
		/// </summary>
		[XmlArray("emails")]
        [XmlArrayItem("email")]
        public List<EmailAddress> Emails { get; set; }
		/// <summary>
		/// Get/Set the assoicated phone numbers with contact card
		/// </summary>
        [XmlArray("phonenumbers")]
        [XmlArrayItem("phonenumber")]
        public List<PhoneNumber> PhoneNumbers { get; set; }
		/// <summary>
		/// Get/Set the assoicated address with contact card
		/// </summary>
        [XmlArray("addresses")]
        [XmlArrayItem("address")]
        public List<Address> Addresses { get; set; }
		/// <summary>
		/// Get/Set the assoicated contacts cards
		/// </summary>
        [XmlArray("members")]
        [XmlArrayItem("contactcard")]
        public List<ContactCard> Members { get; set; }

		/// <summary>
		/// Get last error thrown by object 
		/// </summary>
		[XmlIgnore]
        public static Exception GetLastError { get; set; }

        #endregion

        #region "Constructors and Destructors"
        /// <summary>
		/// Instanicate New Contact Card
		/// </summary>
        public ContactCard()
        {
            SetBase();
        }
		/// <summary>
		/// Instanicate New contact card for [cntid]
		/// </summary>
		/// <param name="pconstring">as data source connection string</param>
		/// <param name="pcntid">as contact cards identifier</param>
        public ContactCard(string pconstring, Guid pcntid)
        {
            SetBase();
            Load(pconstring, pcntid);
        }
		/// <summary>
		/// Instanicate new Contact Card from [dr]
		/// </summary>
		/// <param name="pconstring">as data source connection string</param>
		/// <param name="dr">as data.datarow</param>
        internal ContactCard(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["cntid"] != System.DBNull.Value) { CNTID = new Guid(dr["cntid"].ToString()); }
                if (dr["cntType"] != System.DBNull.Value) { Type = (ContactCardType)Convert.ToInt16(dr["cntType"].ToString()); }
                if (dr["cntTitle"] != System.DBNull.Value) { Title = (String)dr["cntTitle"]; }
                if (dr["cntFName"] != System.DBNull.Value) { FirstName = (String)dr["cntFName"]; }
                if (dr["cntLName"] != System.DBNull.Value) { LastName = (String)dr["cntLName"]; }
                if (dr["cntMName"] != System.DBNull.Value) { MiddleName = (String)dr["cntMName"]; }
                if (dr["cntSuffix"] != System.DBNull.Value) { Suffix = (String)dr["cntSuffix"]; }
                if (dr["cntCompany"] != System.DBNull.Value) { Organization = (String)dr["cntCompany"]; }
                if (dr["cntAvatar"] != System.DBNull.Value) { Avatar = (Byte[])dr["cntAvatar"]; }
                if (dr["cntShowGlobal"] != System.DBNull.Value) { ShowInGlobalAddressList = Convert.ToBoolean(dr["cntShowGlobal"]); }
                if (dr["cntDepartment"] != System.DBNull.Value) { Department = (String)dr["cntDepartment"]; }
                if (dr["cntInternalRef"] != System.DBNull.Value) { InternalNo = (String)dr["cntInternalRef"]; }
                if (dr["cntExternalRef"] != System.DBNull.Value) { ExternalNo = (String)dr["cntExternalRef"]; }
                if (dr["cntNotes"] != System.DBNull.Value) { Notes = (String)dr["cntNotes"]; }
                if (dr["cntTitle_fr"] != System.DBNull.Value) { Title_FR = (String)dr["cntTitle_fr"]; }
                if (dr["cntFName_fr"] != System.DBNull.Value) { FirstName_FR = (String)dr["cntFName_fr"]; }
                if (dr["cntLName_fr"] != System.DBNull.Value) { LastName_FR = (String)dr["cntLName_fr"]; }
                if (dr["cntMName_fr"] != System.DBNull.Value) { MiddleName_FR = (String)dr["cntMName_fr"]; }
                if (dr["cntSuffix_fr"] != System.DBNull.Value) { Suffix_FR = (String)dr["cntSuffix_fr"]; }
                if (dr["cntCompany_fr"] != System.DBNull.Value) { Organization_FR = (String)dr["cntCompany_fr"]; }
                if (dr["cntDepartment_fr"] != System.DBNull.Value) { Department_FR = (String)dr["cntDepartment_fr"]; }
                if (dr["cntNotes_fr"] != System.DBNull.Value) { Notes_FR = (String)dr["cntNotes_fr"]; }
                if (dr["cntZIndex"] != System.DBNull.Value) { ZIndex = Convert.ToInt16(dr["cntZIndex"]); }
                //if (dr["cntFullName"] != System.DBNull.Value) { Description = (String)dr["cntFullName"]; }
                //if (dr["cntFullName_fr"] != System.DBNull.Value) { Description = (String)dr["cntFullName_fr"]; }

                Emails = EmailAddress.ToList(pconstring, CNTID);
                PhoneNumbers = PhoneNumber.ToList(pconstring, CNTID);
                Addresses = Address.ToList(pconstring, CNTID);
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
		/// <summary>
		/// Set MyBase to string.empty
		/// </summary>
        private void SetBase()
        { 
            Title = "";
			FirstName = "";
			LastName = "";
			MiddleName = "";
			Suffix = "";
			Organization = "";
			Department = "";
			InternalNo = "";
			ExternalNo = "";
			Notes = "";
			Title_FR = "";
			Department_FR = "";
			Organization_FR = "";
			FirstName_FR = "";
			LastName_FR = "";
			MiddleName_FR = "";
			Suffix_FR = "";
			Notes_FR = "";

			Emails = new List<EmailAddress>();
			PhoneNumbers = new List<PhoneNumber>();
			Addresses = new List<Address>();
			Members = new List<ContactCard>();
        }
		/// <summary>
		/// Set MyBase to values of [dr]
		/// </summary>
		/// <param name="pconstring">as data source conntection string</param>
		/// <param name="dr">as data.datarow</param>
		/// <returns></returns>
        internal bool SetBase(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["cntid"] != System.DBNull.Value) { CNTID = new Guid(dr["cntid"].ToString()); }
                if (dr["cntType"] != System.DBNull.Value) { Type = (ContactCardType)Convert.ToInt16(dr["cntType"].ToString()); }
                if (dr["cntTitle"] != System.DBNull.Value) { Title = (String)dr["cntTitle"]; }
                if (dr["cntFName"] != System.DBNull.Value) { FirstName = (String)dr["cntFName"]; }
                if (dr["cntLName"] != System.DBNull.Value) { LastName = (String)dr["cntLName"]; }
                if (dr["cntMName"] != System.DBNull.Value) { MiddleName = (String)dr["cntMName"]; }
                if (dr["cntSuffix"] != System.DBNull.Value) { Suffix = (String)dr["cntSuffix"]; }
                if (dr["cntCompany"] != System.DBNull.Value) { Organization = (String)dr["cntCompany"]; }
                if (dr["cntAvatar"] != System.DBNull.Value) { Avatar = (Byte[])dr["cntAvatar"]; }
                if (dr["cntShowGlobal"] != System.DBNull.Value) { ShowInGlobalAddressList = Convert.ToBoolean(dr["cntShowGlobal"]); }
                if (dr["cntDepartment"] != System.DBNull.Value) { Department = (String)dr["cntDepartment"]; }
                if (dr["cntInternalRef"] != System.DBNull.Value) { InternalNo = (String)dr["cntInternalRef"]; }
                if (dr["cntExternalRef"] != System.DBNull.Value) { ExternalNo = (String)dr["cntExternalRef"]; }
                if (dr["cntNotes"] != System.DBNull.Value) { Notes = (String)dr["cntNotes"]; }
                if (dr["cntTitle_fr"] != System.DBNull.Value) { Title_FR = (String)dr["cntTitle_fr"]; }
                if (dr["cntFName_fr"] != System.DBNull.Value) { FirstName_FR = (String)dr["cntFName_fr"]; }
                if (dr["cntLName_fr"] != System.DBNull.Value) { LastName_FR = (String)dr["cntLName_fr"]; }
                if (dr["cntMName_fr"] != System.DBNull.Value) { MiddleName_FR = (String)dr["cntMName_fr"]; }
                if (dr["cntSuffix_fr"] != System.DBNull.Value) { Suffix_FR = (String)dr["cntSuffix_fr"]; }
                if (dr["cntCompany_fr"] != System.DBNull.Value) { Organization_FR = (String)dr["cntCompany_fr"]; }
                if (dr["cntDepartment_fr"] != System.DBNull.Value) { Department_FR = (String)dr["cntDepartment_fr"]; }
                if (dr["cntNotes_fr"] != System.DBNull.Value) { Notes_FR = (String)dr["cntNotes_fr"]; }
                if (dr["cntZIndex"] != System.DBNull.Value) { ZIndex = Convert.ToInt16(dr["cntZIndex"]); }
                //if (dr["cntFullName"] != System.DBNull.Value) { Description = (String)dr["cntFullName"]; }
                //if (dr["cntFullName_fr"] != System.DBNull.Value) { Description = (String)dr["cntFullName_fr"]; }

                Emails = EmailAddress.ToList(pconstring, CNTID);
                PhoneNumbers = PhoneNumber.ToList(pconstring, CNTID);
                Addresses = Address.ToList(pconstring, CNTID);

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
		/// <param name="data">as contact card</param>
		/// <returns></returns>
        internal bool SetBase(ContactCard data)
        {
            try
            {
                CNTID = data.CNTID;
                Type = data.Type;
                Title = data.Title;
                FirstName = data.FirstName;
                LastName = data.LastName;
                MiddleName = data.MiddleName;
                Suffix = data.Suffix;
                Organization = data.Organization;
                Avatar = data.Avatar;
                ShowInGlobalAddressList = data.ShowInGlobalAddressList;
                Department = data.Department;
                InternalNo = data.InternalNo;
                ExternalNo = data.ExternalNo;
                Notes = data.Notes;
                Title_FR = data.Title_FR;
                FirstName_FR = data.FirstName_FR;
                LastName_FR = data.LastName_FR;
                MiddleName_FR = data.MiddleName_FR;
                Suffix_FR = data.Suffix_FR;
                Organization_FR = data.Organization_FR;
                Department_FR = data.Department_FR;
                Notes_FR = data.Notes_FR;
                ZIndex = data.ZIndex;

                Emails = data.Emails;
                PhoneNumbers = data.PhoneNumbers;
                Addresses = data.Addresses;

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
            return FirstName.Trim() + " " + LastName.Trim();
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
		/// Set current contact card's avatar image
		/// </summary>
		/// <param name="pconstring">as data source connection string</param>
		/// <param name="pbyte">as binary represnetation of avatar image</param>
		/// <returns></returns>
        /// <TableName>[contactcards]</TableName>
        public bool setAvatar(string pconstring, byte[] pbyte)
        {
            if (pbyte == null || pbyte.Length == 0)
            {
                GetLastError = new Exception("image is nothing");
                return false;
            }

            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("UPDATE ContactCards SET `avatar`=?avatar "));
                        query.Append(string.Format(" WHERE cntid=?cntid; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?cntid", MySqlDbType.VarChar, 50)).Value = CNTID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?avatar", MySqlDbType.LongBlob)).Value = pbyte;

                        sqlcomm.ExecuteNonQuery();
                        Avatar = pbyte;
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
		/// Add contact card [memcntid] as a member of the current contact card
		/// </summary>
		/// <param name="pconstring">as data source connection string</param>
		/// <param name="pmemcntid">as contact card's identifier that is to be added</param>
		/// <returns></returns>
        /// <TableName>[ContactCards_Members]</TableName>
        public bool addMember(string pconstring, Guid pmemcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addContactCardMember", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pcntid", MySqlDbType.VarChar, 50)).Value = CNTID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pmemcntid", MySqlDbType.VarChar, 50)).Value = pmemcntid.ToString();

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
        /// Remove contact card [memcntid] as a member of the current contact card
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pmemcntid">as contact card's identifier that is to be added</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Members]</TableName>
        public bool removeMember(string pconstring, Guid pmemcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteContactCardMember", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pcntid", MySqlDbType.VarChar, 50)).Value = CNTID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pmemcntid", MySqlDbType.VarChar, 50)).Value = pmemcntid.ToString();

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
        /// Add contact card [memcntid]  to [sourceid] members
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="sourceid">as source contact card to add member to</param>
        /// <param name="memcntid">as contact card's identifier that is to be added</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Members]</TableName>
        public static bool addMember(string pconstring, Guid sourceid, Guid memcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addContactCardMember", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcntid", MySqlDbType.VarChar, 50)).Value = sourceid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pmemcntid", MySqlDbType.VarChar, 50)).Value = memcntid.ToString();

                        sqlcomm.ExecuteScalar();
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
        /// Remove contact card [memcntid]  to [parentcntid] members
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="sourceid">as source contact card to add member to</param>
        /// <param name="memcntid">as contact card's identifier that is to be added</param>
        /// <returns></returns>
        /// <TableName>[ContactCards_Members]</TableName>
        public static bool removeMember(string pconstring, Guid parentcntid, Guid memcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteContactCardMember", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcntid", MySqlDbType.VarChar, 50)).Value = parentcntid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pmemcntid", MySqlDbType.VarChar, 50)).Value = memcntid.ToString();

                        sqlcomm.ExecuteScalar();
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
        /// Load Contact Card for [cntid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcntid">as Contact Card's unique identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_ContactCards]</TableName>
        public bool Load(string pconstring, Guid pcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_ContactCards "));
                        query.Append(string.Format(" WHERE cntid=?cntid; "));
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
        /// Add Contact Card to system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[ContactCards]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addContactCard", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
						byte[] byteA = (Avatar == null)? new byte[]{} : Avatar;

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pTitle", MySqlDbType.VarChar, 200)).Value = Title;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCompany", MySqlDbType.VarChar, 50)).Value = Organization;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDepartment", MySqlDbType.VarChar, 50)).Value = Department;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pFName", MySqlDbType.VarChar, 200)).Value = FirstName;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pLName", MySqlDbType.VarChar, 200)).Value = LastName;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pMName", MySqlDbType.VarChar, 50)).Value = MiddleName;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pSuffix", MySqlDbType.VarChar, 10)).Value = Suffix;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pAvatar", MySqlDbType.LongBlob)).Value = byteA;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pType", MySqlDbType.Int16)).Value = Convert.ToInt16(Type);
                        sqlcomm.Parameters.Add(new MySqlParameter("?pshowGlobal", MySqlDbType.Bit)).Value = (ShowInGlobalAddressList) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pzIndex", MySqlDbType.Int16)).Value = ZIndex;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pInternalRef", MySqlDbType.VarChar, 50)).Value = InternalNo;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pExternalRef", MySqlDbType.VarChar, 50)).Value = ExternalNo;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pNotes", MySqlDbType.VarString)).Value = Notes;

                        sqlcomm.Parameters.Add(new MySqlParameter("?pCompany_fr", MySqlDbType.VarChar, 50)).Value = Organization_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDepartment_Fr", MySqlDbType.VarChar, 50)).Value = Department_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pTitle_fr", MySqlDbType.VarChar, 200)).Value = Title_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pFName_fr", MySqlDbType.VarChar, 200)).Value = FirstName_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pLName_fr", MySqlDbType.VarChar, 200)).Value = LastName_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pMName_fr", MySqlDbType.VarChar, 50)).Value = MiddleName_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pSuffix_fr", MySqlDbType.VarChar, 10)).Value = Suffix_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pNotes_fr", MySqlDbType.VarString)).Value = Notes_FR;

						var obj = sqlcomm.ExecuteScalar();
						try 
                        {
							CNTID = new Guid(obj.ToString());
                            return true;
                        }
						catch 
                        {
							GetLastError = new Exception(obj.ToString());
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
        /// Update Contact Card in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[ContactCards]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateContactCard", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        byte[] byteA = (Avatar == null) ? new byte[] { } : Avatar;

                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcntid", MySqlDbType.VarChar, 50)).Value = CNTID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pCompany", MySqlDbType.VarString, 50)).Value = Organization;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDepartment", MySqlDbType.VarString, 50)).Value = Department;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pTitle", MySqlDbType.VarString, 200)).Value = Title;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pFName", MySqlDbType.VarString, 200)).Value = FirstName;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pLName", MySqlDbType.VarString, 200)).Value = LastName;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pMName", MySqlDbType.VarString, 50)).Value = MiddleName;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pSuffix", MySqlDbType.VarString, 10)).Value = Suffix;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pAvatar", MySqlDbType.LongBlob)).Value = byteA;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pType", MySqlDbType.Int16)).Value = Convert.ToInt16(Type);
                        sqlcomm.Parameters.Add(new MySqlParameter("?pshowGlobal", MySqlDbType.Bit)).Value = (ShowInGlobalAddressList) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pzIndex", MySqlDbType.Int16)).Value = ZIndex;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pInternalRef", MySqlDbType.VarChar, 50)).Value = InternalNo;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pExternalRef", MySqlDbType.VarChar, 50)).Value = ExternalNo;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pNotes", MySqlDbType.VarString)).Value = Notes;

                        sqlcomm.Parameters.Add(new MySqlParameter("?pCompany_fr", MySqlDbType.VarString, 50)).Value = Organization_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pDepartment_Fr", MySqlDbType.VarString, 50)).Value = Department_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pTitle_fr", MySqlDbType.VarString, 200)).Value = Title_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pFName_fr", MySqlDbType.VarString, 200)).Value = FirstName_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pLName_fr", MySqlDbType.VarString, 200)).Value = LastName_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pMName_fr", MySqlDbType.VarString, 50)).Value = MiddleName_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pSuffix_fr", MySqlDbType.VarString, 10)).Value = Suffix_FR;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pNotes_fr", MySqlDbType.VarString)).Value = Notes_FR;

                        sqlcomm.ExecuteScalar();
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
        /// Remove Contact Card from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[ContactCards]</TableName>
        public bool Delete(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteContactCard", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcntid", MySqlDbType.VarChar, 50)).Value = CNTID.ToString();

                        sqlcomm.ExecuteScalar();
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
        /// Remove Contact Card [cntid] from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcntid">as contact card's identifier</param>
        /// <returns></returns>
        /// <TableName>[ContactCards]</TableName>
        public static bool Delete(string pconstring, Guid pcntid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteContactCard", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcntid", MySqlDbType.VarChar, 50)).Value = pcntid.ToString();

                        sqlcomm.ExecuteScalar();
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
        /// Return currrently loaded emails to data table
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable EmailsToDataTable() 
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("cntemID", typeof(System.Int16));
            dt.Columns.Add("cntemType", typeof(System.Int16));
            dt.Columns.Add("cntemEmail", typeof(System.String));
            dt.Columns.Add("cntemDisplayAs", typeof(System.String));

            DataRow dr;
            if (Emails !=null && Emails.Count > 0)
            {
                foreach (EmailAddress em in Emails)
                {
                    dr = dt.NewRow();
                    dr["cntemID"] = em.ID;
                    dr["cntemType"] = em.Type;
                    dr["cntemEmail"] = em.Email;
                    dr["cntemDisplayAs"] = em.DisplayAs;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        /// <summary>
        /// Return currrently loaded phone numbers to data table
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable PhoneNumbersToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("cntphID", typeof(System.Int16));
            dt.Columns.Add("cntphType", typeof(System.Int16));
            dt.Columns.Add("cntphAreaCode", typeof(System.String));
            dt.Columns.Add("cntphNumber", typeof(System.String));
            dt.Columns.Add("cntphExt", typeof(System.String));

            DataRow dr;
            if (PhoneNumbers != null && PhoneNumbers.Count > 0)
            {
                foreach (PhoneNumber ph in PhoneNumbers)
                {
                    dr = dt.NewRow();
                    dr["cntphID"] = ph.ID;
                    dr["cntphType"] = ph.Type;
                    dr["cntphAreaCode"] = ph.AreaCode;
                    dr["cntphNumber"] = ph.Number;
                    dr["cntphExt"] = ph.Extension;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        /// <summary>
        /// Return currrently loaded addresses to data table
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable AddressesToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("cntaddrID", typeof(System.Int16));
            dt.Columns.Add("cntaddrType", typeof(System.Int16));
            dt.Columns.Add("cntaddrStreet", typeof(System.String));
            dt.Columns.Add("cntaddrCity", typeof(System.String));
            dt.Columns.Add("cntaddrProv", typeof(System.String));
            dt.Columns.Add("cntaddrPostal", typeof(System.String));
            dt.Columns.Add("cntaddrCountry", typeof(System.String));

            DataRow dr;
            if (Addresses != null && Addresses.Count > 0)
            {
                foreach (Address a in Addresses)
                {
                    dr = dt.NewRow();
                    dr["cntaddrID"] = a.ID;
                    dr["cntaddrType"] = a.Type;
                    dr["cntaddrStreet"] = a.Street;
                    dr["cntaddrCity"] = a.City;
                    dr["cntaddrProv"] = a.Province;
                    dr["cntaddrPostal"] = a.PostalCode;
                    dr["cntaddrCountry"] = a.Country;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// Return DataTable of all contact cards 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_ContactCards]</TableName>
        public static DataTable dgContactCard(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_ContactCards` "));
                        query.Append(string.Format(" ORDER BY `cntLName` ASC, `cntFName` ASC; "));
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
        /// Return DataTable of all contact cards 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ptype">as contact card type</param>
        /// <returns></returns>
        /// <TableName>[vw_ContactCards]</TableName>
        public static DataTable dgContactCard(string pconstring, ContactCardType ptype)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_ContactCards` "));
                        query.Append(string.Format(" WHERE `cntType`=?type "));
                        query.Append(string.Format(" ORDER BY `cntLName` ASC, `cntFName` ASC; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?type", MySqlDbType.Int16)).Value = Convert.ToInt16(ptype);

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
        /// Return DataTable of all contact cards 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ptype">as contact card type</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as values to search for</param>
        /// <returns></returns>
        /// <TableName>[vw_ContactCards]</TableName>
        public static DataTable dgContactCard(string pconstring, ContactCardType ptype, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_ContactCards` "));
                        query.Append(string.Format(" WHERE `cntType`=?type "));
                        if (!string.IsNullOrEmpty(pcolname))
                        {
                            query.Append(string.Format(" `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
                        }
                        query.Append(string.Format(" ORDER BY `cntLName` ASC, `cntFName` ASC; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?type", MySqlDbType.Int16)).Value = Convert.ToInt16(ptype);

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
        /// Return DataTable of all contact cards 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as values to search for</param>
        /// <returns></returns>
        /// <TableName>[vw_ContactCards]</TableName>
        public static DataTable dgContactCard(string pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM `vw_ContactCards` "));
                        if (!string.IsNullOrEmpty(pcolname))
                        {
                            query.Append(string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
                        }
                        query.Append(string.Format(" ORDER BY `cntLName` ASC, `cntFName` ASC; "));
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
        /// Write Contact Card to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of Contact Card</param>
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

                    xmlSer = new XmlSerializer(typeof(ContactCard));
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
        /// Create Contact Card from File
        /// </summary>
        /// <param name="xmlpath">as source location of Contact Card</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(ContactCard));
                ContactCard output = (ContactCard)xs.Deserialize(fs);
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
        /// <param name="data">as ContactCard</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(ContactCard data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(ContactCard));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(ContactCard));
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
        /// Return ContactCard from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of ContactCard</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ContactCard XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(ContactCard));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                ContactCard output = default(ContactCard);
                output = (ContactCard)xmlSer.Deserialize(string_reader);
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