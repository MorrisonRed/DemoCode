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
    /// Project Resource
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("resource")]
    public class Resource
    {
        /*
         * rsid (int) = resource id
         * pid (Guid) = associated project's id
         * name (string) = resource name
         * phone (string) = phone number
         * email (string) = email address
         * role (int) = assoicate role for this resource 
         * 
         * To Do
         * Days OFF 
         * 
        */

        #region "Public Properties"
        /// <summary>
        /// Get/Set the resource's identifier
        /// </summary>
        [XmlElement(ElementName = "rsid")]
        public int RSID { get; set; }
        /// <summary>
        /// Get/Set the assoicated projects identifier
        /// </summary>
        [XmlElement(ElementName = "pid")]
        public Guid PID { get; set; }
        /// <summary>
        /// Get/Set the name of the resource
        /// </summary>
        [XmlElement(ElementName = "Name")]
        public String Name { get; set; }
        /// <summary>
        /// Get/Set the resource's contact number
        /// </summary>
        [XmlElement(ElementName = "phone")]
        public String Phone { get; set; }
        /// <summary>
        /// Get/Set the resource's contact email
        /// </summary>
        [XmlElement(ElementName = "email")]
        public String Email { get; set; }
        /// <summary>
        /// Get/Set the resource's default role [Nullable]
        /// </summary>
        [XmlElement(ElementName = "roleid")]
        public int ?RoleID { get; set; }

        /// <summary>
        /// Get Last Error thrown
        /// </summary>
        [XmlIgnore()]
        public static Exception GetLastError { get; set; }
        #endregion

        #region "Constructors and Destructors"
        /// <summary>
        /// Instanicate New Project Resource
        /// </summary>
        public Resource()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate new Resource from [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as datarow</param>
        /// <returns></returns>
        internal Resource(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["ProjResRSID"] != System.DBNull.Value) { RSID = Convert.ToInt16(dr["ProjResRSID"].ToString()); }
                if (dr["pid"] != System.DBNull.Value) { PID = new Guid(dr["pid"].ToString()); }
                if (dr["ProjResName"] != System.DBNull.Value) { Name = (String)dr["ProjResName"]; }
                if (dr["ProjResEmail"] != System.DBNull.Value) { Email = (String)dr["ProjResEmail"]; }
                if (dr["ProjResPhone"] != System.DBNull.Value) { Phone = (String)dr["ProjResPhone"]; }
                if (dr["ProjRoleID"] != System.DBNull.Value) { RoleID = Convert.ToInt16(dr["ProjRoleID"]); }
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
            Name = "";
            Phone = "";
            Email = "";
        }
        /// <summary>
        /// Set MyBase to values of [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as datarow</param>
        /// <returns></returns>
        internal bool SetBase(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["ProjResRSID"] != System.DBNull.Value) { RSID = Convert.ToInt16(dr["ProjResRSID"].ToString()); }
                if (dr["pid"] != System.DBNull.Value) { PID = new Guid(dr["pid"].ToString()); }
                if (dr["ProjResName"] != System.DBNull.Value) { Name = (String)dr["ProjResName"]; }
                if (dr["ProjResEmail"] != System.DBNull.Value) { Email = (String)dr["ProjResEmail"]; }
                if (dr["ProjResPhone"] != System.DBNull.Value) { Phone = (String)dr["ProjResPhone"]; }
                if (dr["ProjRoleID"] != System.DBNull.Value) { RoleID = Convert.ToInt16(dr["ProjRoleID"]); }

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
        /// <param name="data">as resource</param>
        /// <returns></returns>
        internal bool SetBase(Resource data)
        {
            try
            {
                RSID = data.RSID;
                PID = data.PID;
                Name = data.Name;
                Email = data.Email;
                Phone = data.Phone;
                RoleID = data.RoleID;

                return true;
            }
            catch(Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        #endregion

        #region "Function and Sub Routines"
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
        private static string DBValueToString(object value)
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
        public DataTable ToDataTable(List<Resource> data)
        {
            Type type = typeof(Resource);
            var properties = type.GetProperties();

            DataTable dt = new DataTable();
            //add columns
            foreach (PropertyInfo info in properties)
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }

            //add row for each d in list
            foreach (Resource d in data)
            {
                foreach (PropertyInfo info in properties)
                {
                    try
                    {
                        DataRow dr = dt.NewRow();
                        info.GetValue(d, null);
                        dr[info.Name] = info.GetValue(d, null);

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
        /// Load Project Resource for [prsid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="prsid">as project role identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_resources_projects]</TableName>
        public bool Load(string pconstring, int prsid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_resources_projects "));
                        query.Append(string.Format(" WHERE ProjRSID=?prsid; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?prsid", MySqlDbType.VarChar, 50)).Value = prsid.ToString();

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
        /// Add Project Resources To System
        /// </summary>
        /// <param name="pconstring">as Data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects_resources]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addProjectResource", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 200)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pemail", MySqlDbType.VarChar, 254)).Value = Email.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pphone", MySqlDbType.VarChar, 20)).Value = Phone.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?proleid", MySqlDbType.VarChar, 11)).Value = (RoleID.HasValue) ? RoleID.ToString() : "";


                        //if the return value is not a guid then there was an error in the stored procedures
                        string s = DBValueToString(sqlcomm.ExecuteScalar());
                        if (IsNumeric(s))
                        {
                            RoleID = Convert.ToInt16(s);
                            return true;
                        }
                        else
                        {
                            GetLastError = new Exception(s);
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
        /// Add Project Resources To System
        /// </summary>
        /// <param name="pconstring">as Data source connection string</param>
        /// <param name="pname">as project resource name</param>
        /// <param name="ppid">as project's identifier</param>
        /// <param name="pemail">as resources contact email</param>
        /// <param name="pphone">as resources contact phone number</param>
        /// <param name="proleid">as assigned role identifier</param>
        /// <returns></returns>
        /// <TableName>[projects_resources]</TableName>
        public static bool Add(string pconstring, Guid ppid, string pname, string pemail, string pphone, string proleid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addProjectResource", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        int rolevalue = (IsNumeric(proleid)) ? Convert.ToInt16(proleid) : 0;

                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = ppid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 200)).Value = pname.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pemail", MySqlDbType.VarChar, 254)).Value = pemail.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pphone", MySqlDbType.VarChar, 20)).Value = pphone.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?proleid", MySqlDbType.VarChar, 11)).Value = (rolevalue > 0)? proleid.Trim(): "";


                        //if the return value is not a guid then there was an error in the stored procedures
                        string s = DBValueToString(sqlcomm.ExecuteScalar());
                        if (IsNumeric(s))
                        {
                            return true;
                        }
                        else
                        {
                            GetLastError = new Exception(s);
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
        /// Update Project Resource in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects_resources]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateProjectResource", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?prsid", MySqlDbType.Int16)).Value = RSID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 200)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pemail", MySqlDbType.VarChar, 254)).Value = Email.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pphone", MySqlDbType.VarChar, 20)).Value = Phone.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?proleid", MySqlDbType.VarChar, 11)).Value = (RoleID.HasValue) ? RoleID.ToString() : "";

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
        /// Remove Project resource from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects_resources]</TableName>
        public Boolean Delete(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteProjectResource", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?prsid", MySqlDbType.VarChar, 50)).Value = RSID.ToString();
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
        /// Remove Project resource from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="prsid">as projects resource identifier</param>
        /// <returns></returns>
        /// <TableName>[projects_resources]</TableName>
        public static Boolean Delete(String pconstring, int prsid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteProjectResource", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?prsid", MySqlDbType.Int16)).Value = prsid.ToString();
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
        /// Return DataTable of all project resourcs in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_resources_projects]</TableName>
        public static DataTable dgResources(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_resources_projects");
                        query.Append(" ORDER BY `ProjResName` ASC;");
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
        /// Return DataTable of all project resources where [colname] contains [colval]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as value to search for</param>
        /// <returns></returns>
        /// <TableName>[vw_resources_projects]</TableName>
        public static DataTable dgResources(string pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_resources_projects");
                        query.Append(string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
                        query.Append(string.Format(" ORDER BY `{0}` ASC;", pcolname.Trim()));
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
        /// Write Resource to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of Resource</param>
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

                    xmlSer = new XmlSerializer(typeof(Resource));
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
        /// Create Resource from File
        /// </summary>
        /// <param name="xmlpath">as source location of Resource </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(Resource));
                Resource output = (Resource)xs.Deserialize(fs);
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
        /// <param name="data">as Resource</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(Resource data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Resource));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(Resource));
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
        /// Return Resource from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of Resource</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Resource XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Resource));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                Resource output = default(Resource);
                output = (Resource)xmlSer.Deserialize(string_reader);
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