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
    /// Project description and scope
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("project")]
    public class Project
    {
        /*
         * pid (guid) = project code
         * icon (string)  = url
         * code (string) = unique code
         * description (string) = project name
         * folder (string) = relative path to project folder
         * caption (string) = brief descrption of project (144 characters)
         * estamatedstartdate (datetime) = date project is estimated to start
         * estimatedenddate (datetime) = date project is estimated to end
         * actualstartdate (datetime) = date project actually started
         * actualenddate (datetime) = date project acutally ended
         * tasts (list) = assoicated tasks to this project
         * url (string) = url to project if applicable
         * organization (string) = organization this project is assoicated with
         * 
         * tasks (list) = list of tasks assinged to this project
         * taxonomies (list) = list of taxonomy categories assinged to this project
         * 
        */
        private string p;
        private string _projcode;

        #region Public Properties
        /// <summary>
        /// Get/Set the project's unique identifier
        /// </summary>
        [XmlElement(ElementName = "pid")]
        public Guid PID { get; set; }
        /// <summary>
        /// Get/Set the assoicated Icon assinged to this Project
        /// </summary>
        [XmlElement(ElementName = "icon")]
        public string Icon { get; set; }
        /// <summary>
        /// Get/Set a unique project code for the project
        /// </summary>
        [XmlElement(ElementName = "code")]
        public string Code { get; set; }
        /// <summary>
        /// Get/Set the project's name or title
        /// </summary>
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        /// <summary>
        /// Get/Set the project's descripiton
        /// </summary>
        [XmlElement(ElementName = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// Get/Set the project folder for store assoicated files and images
        /// </summary>
        [XmlElement(ElementName = "folder")]
        public string Folder { get; set; }
        /// <summary>
        /// Get/Set the caption (short teaser) about the projects
        /// </summary>
        [XmlElement(ElementName = "caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Get/Set the Estimated start date for the project
        /// </summary>
        [XmlElement(ElementName = "eststartdate")]
        public DateTime? EstimatedStartDate { get; set; }
        /// <summary>
        /// Get/Set the Estimated end date for the project
        /// </summary>
        [XmlElement(ElementName = "estenddate")]
        public DateTime? EstimatedEndDate { get; set; }
        /// <summary>
        /// Get/Set the Actual start date for the project
        /// </summary>
        [XmlElement(ElementName = "actstartdate")]
        public DateTime? ActualStartDate { get; set; }
        /// <summary>
        /// Get/Set the Actual end date for the project
        /// </summary>
        [XmlElement(ElementName = "actenddate")]
        public DateTime? ActualEndDate { get; set; }

        /// <summary>
        /// Get/Set Assoicate url for this project
        /// </summary>
        [XmlElement(ElementName = "url")]
        public String URL {get; set;}
        /// <summary>
        /// Get/Set Organization assoicated with this project
        /// </summary>
        [XmlElement(ElementName = "organization")]
        public String Organization { get; set; } 

        /// <summary>
        /// Get/Set Tasks assigned to this project
        /// </summary>
        [XmlArray("tasks")]
        [XmlArrayItem("task")]
        public List<Task> Tasks { get; set; }

        /// <summary>
        /// Get/Set Taxonomies/Characteristics assinged to this project
        /// </summary>
        [XmlArray("taxonomies")]
        [XmlArrayItem("taxonomy")]
        public List<Taxonomy> Taxonomies { get; set; }

        /// <summary>
        /// Get Last Error thrown by object
        /// </summary>
        [XmlIgnore]
        public static Exception GetLastError { get; set; }
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Instanicate new Project
        /// </summary>
        public Project()
        {
            SetBase();
        }
        /// <summary>
        /// Instanciate New Project for [pid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ppid">as project's identifier</param>
        public Project(string pconstring, Guid ppid)
        {
            SetBase();
            Load(pconstring, ppid);
        }
        /// <summary>
        /// Instanciate Project based on code value
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcode">as project's unique code</param>
        public Project(string pconstring, string pcode)
        {
            this.Load(pconstring, pcode);
        }
        /// <summary>
        /// Instanicate new Project from [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        internal Project(string pconstring, System.Data.DataRow dr)
        {
            try
            {
                if (dr["pid"] != System.DBNull.Value) { PID = new Guid(dr["pid"].ToString()); }
                if (dr["ProjIcon"] != System.DBNull.Value) { Icon = (String)dr["ProjIcon"]; }
                if (dr["ProjCode"] != System.DBNull.Value) { Code = (String)dr["ProjCode"]; }
                if (dr["ProjName"] != System.DBNull.Value) { Name = (String)dr["ProjName"]; }
                if (dr["ProjDesc"] != System.DBNull.Value) { Description = (String)dr["ProjDesc"]; }
                if (dr["ProjEstStartDate"] != System.DBNull.Value) { EstimatedStartDate = (DateTime)dr["ProjEstStartDate"]; }
                if (dr["ProjEstEndDate"] != System.DBNull.Value) { EstimatedEndDate = (DateTime)dr["ProjEstEndDate"]; }
                if (dr["ProjActStartDate"] != System.DBNull.Value) { EstimatedStartDate = (DateTime)dr["ProjActStartDate"]; }
                if (dr["ProjActEndDate"] != System.DBNull.Value) { ActualEndDate = (DateTime)dr["ProjActEndDate"]; }
                if (dr["ProjFolder"] != System.DBNull.Value) { Folder = (String)dr["ProjFolder"]; }
                if (dr["ProjCaption"] != System.DBNull.Value) { Caption = (String)dr["ProjCaption"]; }
                if (dr["ProjURL"] != System.DBNull.Value) { URL = (String)dr["ProjURL"]; }
                if (dr["ProjOrg"] != System.DBNull.Value) { Organization = (String)dr["ProjOrg"]; }

                //load tasks
                //load taxonomies
                Taxonomies = Taxonomy.ListFor(pconstring, PID);
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set base to values of string.Empty
        /// </summary>
        private void SetBase()
        {
            Icon = string.Empty;
            Code = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Folder = string.Empty;
            Caption = string.Empty;
            URL = string.Empty;
            Organization = string.Empty;
        }
        /// <summary>
        /// Set MyBase to values of [data]
        /// </summary>
        /// <param name="data">as project</param>
        /// <returns></returns>
        internal Boolean SetBase(Project data)
        {
            try
            {
                PID = data.PID;
                Icon = data.Icon;
                Code = data.Code;
                Name = data.Name;
                Description = data.Description;
                EstimatedStartDate = data.EstimatedStartDate;
                EstimatedEndDate = data.EstimatedEndDate;
                ActualStartDate = data.ActualStartDate;
                ActualEndDate = data.ActualEndDate;
                Folder = data.Folder;
                Caption = data.Caption;
                URL = data.URL;
                Organization = data.Organization;

                Tasks = data.Tasks;

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
                if (dr["pid"] != System.DBNull.Value) { PID = new Guid(dr["pid"].ToString()); }
                if (dr["ProjIcon"] != System.DBNull.Value) { Icon = (String)dr["ProjIcon"]; }
                if (dr["ProjCode"] != System.DBNull.Value) { Code = (String)dr["ProjCode"]; }
                if (dr["ProjName"] != System.DBNull.Value) { Name = (String)dr["ProjName"]; }
                if (dr["ProjDesc"] != System.DBNull.Value) { Description = (String)dr["ProjDesc"]; }
                if (dr["ProjEstStartDate"] != System.DBNull.Value) { EstimatedStartDate = (DateTime)dr["ProjEstStartDate"]; }
                if (dr["ProjEstEndDate"] != System.DBNull.Value) { EstimatedEndDate = (DateTime)dr["ProjEstEndDate"]; }
                if (dr["ProjActStartDate"] != System.DBNull.Value) { ActualStartDate = (DateTime)dr["ProjActStartDate"]; }
                if (dr["ProjActEndDate"] != System.DBNull.Value) { ActualEndDate = (DateTime)dr["ProjActEndDate"]; }
                if (dr["ProjFolder"] != System.DBNull.Value) { Folder = (String)dr["ProjFolder"]; }
                if (dr["ProjCaption"] != System.DBNull.Value) { Caption = (String)dr["ProjCaption"]; }
                if (dr["ProjURL"] != System.DBNull.Value) { URL = (String)dr["ProjURL"]; }
                if (dr["ProjOrg"] != System.DBNull.Value) { Organization = (String)dr["ProjOrg"]; }

                //load tasks
                //load taxonomies
                Taxonomies = Taxonomy.ListFor(pconstring, PID);

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
        /// Return Project for [pid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ppid">as projects identifier</param>
        /// <returns></returns>
        /// /// <TableName>[vw_projects]</TableName>
        public static Project getProject(string pconstring, Guid ppid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_projects "));
                        query.Append(string.Format(" WHERE pid=?pid; "));
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

                            return new Project(pconstring, ds.Tables["tbl"].Rows[0]);
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
        /// Load Project for [pid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ppid">as projects identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_projects]</TableName>
        public bool Load(string pconstring, Guid ppid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_projects "));
                        query.Append(string.Format(" WHERE pid=?pid; "));
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
        /// Load Project for [pid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcode">as projects unique cod</param>
        /// <returns></returns>
        /// <TableName>[vw_projects]</TableName>
        public bool Load(string pconstring, string pcode)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_projects "));
                        query.Append(string.Format(" WHERE ProjCode=?Code; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?Code", MySqlDbType.VarChar, 50)).Value = pcode.ToString();

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
        /// Add Project To System
        /// </summary>
        /// <param name="pconstring">as Data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addProject", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        //reset id
                        PID = Guid.NewGuid();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?picon", MySqlDbType.VarChar, 125)).Value = Icon.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcode", MySqlDbType.VarChar, 50)).Value = Code.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 125)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarString)).Value = Description.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pestStartDate", MySqlDbType.VarChar, 50)).Value = EstimatedStartDate.HasValue ? Convert.ToDateTime(EstimatedStartDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pestEndDate", MySqlDbType.VarChar, 50)).Value = EstimatedEndDate.HasValue ? Convert.ToDateTime(EstimatedEndDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pactStartDate", MySqlDbType.VarChar, 50)).Value = ActualStartDate.HasValue ? Convert.ToDateTime(ActualStartDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pactEndDate", MySqlDbType.VarChar, 50)).Value = ActualEndDate.HasValue ? Convert.ToDateTime(ActualEndDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pfolder", MySqlDbType.VarChar, 255)).Value = Folder.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcaption", MySqlDbType.VarChar, 150)).Value = Caption.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?purl", MySqlDbType.VarChar, 125)).Value = URL.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?porg", MySqlDbType.VarString, 100)).Value = Organization.Trim();


                        //if the return value is not a guid then there was an error in the stored procedures
                        string s = DBValueToString(sqlcomm.ExecuteScalar());
                        Guid g;
                        try
                        {
                            g = new Guid(s);
                            return true;
                        }
                        catch (FormatException ex)
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
        /// Update Project in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateProject", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?picon", MySqlDbType.VarChar, 125)).Value = Icon.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcode", MySqlDbType.VarChar, 50)).Value = Code.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 125)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarString)).Value = Description.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pestStartDate", MySqlDbType.VarChar, 50)).Value = EstimatedStartDate.HasValue ? Convert.ToDateTime(EstimatedStartDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pestEndDate", MySqlDbType.VarChar, 50)).Value = EstimatedEndDate.HasValue ? Convert.ToDateTime(EstimatedEndDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pactStartDate", MySqlDbType.VarChar, 50)).Value = ActualStartDate.HasValue ? Convert.ToDateTime(ActualStartDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pactEndDate", MySqlDbType.VarChar, 50)).Value = ActualEndDate.HasValue ? Convert.ToDateTime(ActualEndDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pfolder", MySqlDbType.VarChar, 255)).Value = Folder.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcaption", MySqlDbType.VarChar, 150)).Value = Caption.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?purl", MySqlDbType.VarChar, 125)).Value = URL.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?porg", MySqlDbType.VarString, 100)).Value = Organization.Trim();

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
        /// Remove Project from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects]</TableName>
        public Boolean Delete(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteProject", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
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
        /// Remove Project from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ppid">as projects's unique identifier</param>
        /// <returns></returns>
        /// <TableName>[projects]</TableName>
        public static Boolean Delete(String pconstring, Guid ppid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteProject", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = ppid.ToString();
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
        /// Return List of projects from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="orderby">optional order by column value</param>
        /// <returns></returns>
        /// <TableName>[vw_projects]</TableName>
        public static List<Project> ListFor(string pconstring, string orderby = "ProjName", SortDirection sortdirection = SortDirection.Ascending)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string direction = (sortdirection == SortDirection.Descending) ? "DESC" : "ASC";
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_projects");
                        query.Append(string.Format(" ORDER BY `{0}` {1};", orderby, direction));
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

                            List<Project> lst = new List<Project>();
                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            {
                                lst.Add(new Project(pconstring, dr));
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
        /// Return List of project from system 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="taxonomy">as classification</param>
        /// <param name="orderby">as column name to sort collection by</param>
        /// <param name="sortdirection">as sort direction</param>
        /// <returns></returns>
        /// <TableName>[vw_projects_taxonomy]</TableName>
        public static List<Project> ListFor(string pconstring, string taxonomy, string orderby = "ProjName", SortDirection sortdirection = SortDirection.Ascending)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string direction = (sortdirection == SortDirection.Descending) ? "DESC" : "ASC";
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_projects_taxonomy");
                        query.Append(" WHERE TxName = ?txname");
                        query.Append(string.Format(" ORDER BY `{0}` {1};", orderby, direction));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?txname", MySqlDbType.VarChar, 255)).Value = taxonomy.Trim();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return null;
                            }

                            List<Project> lst = new List<Project>();
                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            {
                                lst.Add(new Project(pconstring, dr));
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
        /// Return DataTable of all projects in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_projects]</TableName>
        public static DataTable dgProjects(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_projects");
                        query.Append(" ORDER BY `ProjName` ASC;");
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
        /// <TableName>[vw_projects]</TableName>
        public static DataTable dgProjects(string pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_projects");
                        query.Append(string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
                        query.Append(" ORDER BY `ProjName` ASC;");
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
        /// <summary>
        /// Return DataTable of all projects that contain taxonomy classification 
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="taxonomy">as classification </param>
        /// <returns></returns>
        /// <TableName>[vw_projects_taxonomy]</TableName>
        public static DataTable dgProjects(string pconstring, string taxonomy)
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
                        query.Append(" WHERE TxName = ?txname");
                        query.Append(" ORDER BY `ProjName` ASC;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();
                        sqlcomm.Parameters.Add(new MySqlParameter("?txname", MySqlDbType.VarChar, 255)).Value = taxonomy.Trim();

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
        /// Write Project to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of Project</param>
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

                    xmlSer = new XmlSerializer(typeof(Project));
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
        /// Create Project from File
        /// </summary>
        /// <param name="xmlpath">as source location of Project </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(Project));
                Project output = (Project)xs.Deserialize(fs);
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
        /// <param name="data">as Project</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(Project data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Project));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(Project));
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
        /// Return Project from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of Project</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Project XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Project));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                Project output = default(Project);
                output = (Project)xmlSer.Deserialize(string_reader);
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