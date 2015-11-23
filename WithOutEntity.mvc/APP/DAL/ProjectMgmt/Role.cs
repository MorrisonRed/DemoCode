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
    /// Project Role
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("role")]
    public class Role
    {
        /*
         * roleid(int) = project role id
         * pid(string) = project id
         * name (string) = role name 
         * 
        */

        #region "Public Properties"
        /// <summary>
        /// Get/Set the role's identifier
        /// </summary>
        [XmlElement(ElementName = "roleid")]
        public int RoleID { get; set; }
        /// <summary>
        /// Get/Set the assoicated projects identifier
        /// </summary>
        [XmlElement(ElementName = "pid")]
        public Guid PID { get; set; }
        /// <summary>
        /// Get/Set the Name or the role name
        /// </summary>
        [XmlElement(ElementName = "name")]
        public String Name { get; set; }

        /// <summary>
        /// Get/Set last error thrown
        /// </summary>
        [XmlIgnore()]
        public static Exception GetLastError { get; set; }
        #endregion

        #region "Constructors and Destructors"
        /// <summary>
        /// Instanicate new Project Role
        /// </summary>
        public Role()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate new project role from [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        /// <returns></returns>
        internal Role(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["ProjRoleID"] != System.DBNull.Value) { RoleID = Convert.ToInt16(dr["ProjRoleID"].ToString()); }
                if (dr["pid"] != System.DBNull.Value) { PID = new Guid(dr["pid"].ToString()); }
                if (dr["ProjRoleName"] != System.DBNull.Value) { Name = (String)dr["ProjRoleName"]; }
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
                if (dr["ProjRoleID"] != System.DBNull.Value) { RoleID = Convert.ToInt16(dr["ProjRoleID"].ToString()); }
                if (dr["pid"] != System.DBNull.Value) { PID = new Guid(dr["pid"].ToString()); }
                if (dr["ProjRoleName"] != System.DBNull.Value) { Name = (String)dr["ProjRoleName"]; }

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
        /// <param name="data">as project role</param>
        /// <returns></returns>
        internal Boolean SetBase(Role data)
        {
            try
            {
                RoleID = data.RoleID;
                PID = data.PID;
                Name = data.Name;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "Functions and Sub Routines"
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
        public DataTable ToDataTable(List<Role> roles)
        {
            Type type = typeof(Role);
            var properties = type.GetProperties();

            DataTable dt = new DataTable();
            //add columns
            foreach (PropertyInfo info in properties)
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }

            //add row for each role in list
            foreach (Role r in roles)
            {
                foreach (PropertyInfo info in properties)
                {
                    try
                    {
                        DataRow dr = dt.NewRow();
                        info.GetValue(r, null);
                        dr[info.Name] = info.GetValue(r, null);

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
        /// Load Project Role for [proleid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="proleid">as project role identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_roles_projects]</TableName>
        public bool Load(string pconstring, int proleid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_roles_projects "));
                        query.Append(string.Format(" WHERE ProjRoleID=?proleid; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?proleid", MySqlDbType.VarChar, 50)).Value = proleid.ToString();

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
        /// Add Project Role To System
        /// </summary>
        /// <param name="pconstring">as Data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects_roles]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addProjectRole", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 200)).Value = Name.Trim();


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
        /// Add Project Role To System
        /// </summary>
        /// <param name="pconstring">as Data source connection string</param>
        /// <param name="pname">as project role name</param>
        /// <param name="ppid">as project's identifier</param>
        /// <returns></returns>
        /// <TableName>[projects_roles]</TableName>
        public static bool Add(string pconstring, Guid ppid, string pname)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addProjectRole", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = ppid.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 200)).Value = pname.Trim();


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
        /// Update Project Role in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects_roles]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateProjectRole", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?proleid", MySqlDbType.Int16)).Value = RoleID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 200)).Value = Name.Trim();

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
        /// Remove Project role from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects_roles]</TableName>
        public Boolean Delete(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteProjectRole", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?proleid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
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
        /// Remove Project roles from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="proleid">as projects roles identifier</param>
        /// <returns></returns>
        /// <TableName>[projects_roles]</TableName>
        public static Boolean Delete(String pconstring, int proleid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteProjectRole", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?proleid", MySqlDbType.Int16)).Value = proleid.ToString();
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
        /// Return DataTable of all projects in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_roles_projects]</TableName>
        public static DataTable dgRoles(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_roles_projects");
                        query.Append(" ORDER BY `ProjRoleName` ASC;");
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
        /// Return DataTable of all projects where [colname] contains [colval]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as value to search for</param>
        /// <returns></returns>
        /// <TableName>[vw_roles_projects]</TableName>
        public static DataTable dgRoles(string pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_roles_projects");
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
        /// Write Role to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of Role</param>
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

                    xmlSer = new XmlSerializer(typeof(Role));
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
        /// Create Role from File
        /// </summary>
        /// <param name="xmlpath">as source location of Role </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(Role));
                Role output = (Role)xs.Deserialize(fs);
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
        /// <param name="data">as Role</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(Role data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Role));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(Role));
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
        /// Return Role from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of Role</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Role XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Role));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                Role output = default(Role);
                output = (Role)xmlSer.Deserialize(string_reader);
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