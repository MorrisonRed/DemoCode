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
    /// Project Tasks
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("task")]
    public class Task
    {
        /*
         * taskid (guid) = tasks id
         * pid (guid) = assoicated project
         * name (string) = task name
         * description (string) = task description
         * predecessors (list) = list of pedicessor tasks
         * estamatedstartdate (datetime) = date task is estimated to start
         * estimatedenddate (datetime) = date task is estimated to end
         * actualstartdate (datetime) = date task actually started
         * actualenddate (datetime) = date task acutally ended
         * milestone (bool) = whether the task is a milestone
         * priority (int) = set the priority of the task {high, medium, normal, low)
         * Progress (int) = percentage complete
         * 
         * need to do
         * resources (list) - list of resources assinged to task
         *    - additional feilds unit (int) - percentage of work load
         *    - additional feilds coordinator (bool) - owner of the task
         *    - additional fields role (int) - assoicated role for this task
         *  
         * 
        */

        #region Public Properties
        /// <summary>
        /// Get/Set the Task's unique idientifier
        /// </summary>
        [XmlElement(ElementName = "taskid")]
        public Guid TaskID { get; set; }
        /// <summary>
        /// Get/Set the assoicated project this task is assigned to
        /// </summary>
        [XmlElement(ElementName = "pid")]
        public Guid PID { get; set; }
        /// <summary>
        /// Get/Set the Name of the Task
        /// </summary>
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        /// <summary>
        /// Get/Set the description of the task
        /// </summary>
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Get/Set assoicated predecessor tasks
        /// </summary>
        [XmlArray("predecessors")]
        [XmlArrayItem("predecessor")]
        public List<Task> Predecessors { get; set; }

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
        /// Get/Set whether the task is a milestone
        /// </summary>
        [XmlElement(ElementName = "milestone")]
        public Boolean Milestone { get; set; }
        /// <summary>
        /// Get/Set the priority assinged to this task
        /// </summary>
        [XmlElement(ElementName = "priority")]
        public Priority Priority { get; set; }
        /// <summary>
        /// Get/Set the percentage competed
        /// </summary>
        [XmlElement(ElementName = "progress")]
        public int Progress { get; set; }

        /// <summary>
        /// Get/Set last error thrown by object
        /// </summary>
        [XmlIgnore]
        public static Exception GetLastError {get; set;}
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Instanicate new project task
        /// </summary>
        public Task()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate New Project task for [tskid]
        /// </summary>
        /// <param name="pconstring">as data source connections string</param>
        /// <param name="ptskid">as task's unique identifier</param>
        public Task(string pconstring, Guid ptskid)
        {
            SetBase();
            Load(pconstring, ptskid);
        }
        /// <summary>
        /// Instanicate New Task
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as datarow</param>
        /// <returns></returns>
        internal Task(string pconstring, DataRow dr)
        {
            try
            {
                if (dr["tskid"] != System.DBNull.Value) { TaskID = new Guid(dr["tskid"].ToString()); }
                if (dr["pid"] != System.DBNull.Value) { PID = new Guid(dr["pid"].ToString()); }
                if (dr["TskName"] != System.DBNull.Value) { Name = (String)dr["TskName"]; }
                if (dr["TskDesc"] != System.DBNull.Value) { Description = (String)dr["TskDesc"]; }
                if (dr["TskEstStartDate"] != System.DBNull.Value) { EstimatedStartDate = Convert.ToDateTime(dr["TskEstStartDate"]); }
                if (dr["TskEstEndDate"] != System.DBNull.Value) { EstimatedEndDate = Convert.ToDateTime(dr["TskEstEndDate"]); }
                if (dr["TskActStartDate"] != System.DBNull.Value) { ActualStartDate = Convert.ToDateTime(dr["TskActStartDate"]); }
                if (dr["TskActEndDate"] != System.DBNull.Value) { ActualEndDate = Convert.ToDateTime(dr["TskActEndDate"]); }
                if (dr["TskMilestone"] != System.DBNull.Value) { Milestone = Convert.ToBoolean(dr["TskMilestone"]); }
                if (dr["TskProgress"] != System.DBNull.Value) { Progress = Convert.ToInt16(dr["TskProgress"]); }
                if (dr["TskPriority"] != System.DBNull.Value) { Priority = (Priority)Convert.ToInt16(dr["TskPriority"]); }

                // predecessors (list) = list of pedicessor tasks
                // resources (list) = list of assigned resources 
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set My Base to values of String.Empty
        /// </summary>
        private void SetBase()
        {
            Name = string.Empty;
            Description = string.Empty;
            Predecessors = new List<Task>();               
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
                if (dr["tskid"] != System.DBNull.Value) { TaskID = new Guid(dr["tskid"].ToString()); }
                if (dr["pid"] != System.DBNull.Value) { PID = new Guid(dr["pid"].ToString()); }
                if (dr["TskName"] != System.DBNull.Value) { Name = (String)dr["TskName"]; }
                if (dr["TskDesc"] != System.DBNull.Value) { Description = (String)dr["TskDesc"]; }
                if (dr["TskEstStartDate"] != System.DBNull.Value) { EstimatedStartDate = Convert.ToDateTime(dr["TskEstStartDate"]); }
                if (dr["TskEstEndDate"] != System.DBNull.Value) { EstimatedEndDate = Convert.ToDateTime(dr["TskEstEndDate"]); }
                if (dr["TskActStartDate"] != System.DBNull.Value) { ActualStartDate = Convert.ToDateTime(dr["TskActStartDate"]); }
                if (dr["TskActEndDate"] != System.DBNull.Value) { ActualEndDate = Convert.ToDateTime(dr["TskActEndDate"]); }
                if (dr["TskMilestone"] != System.DBNull.Value) { Milestone = Convert.ToBoolean(dr["TskMilestone"]); }
                if (dr["TskProgress"] != System.DBNull.Value) { Progress = Convert.ToInt16(dr["TskProgress"]); }
                if (dr["TskPriority"] != System.DBNull.Value) { Priority = (Priority)Convert.ToInt16(dr["TskPriority"]); }

                // predecessors (list) = list of pedicessor tasks
                // resources (list) = list of assigned resources 
                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set My Base to values of [data]
        /// </summary>
        /// <param name="data">as project task</param>
        /// <returns></returns>
        internal bool SetBase(Task data)
        {
            try
            {
                TaskID = data.TaskID;
                Name = data.Name;
                Description = data.Description;

                Predecessors = data.Predecessors;
                EstimatedStartDate = data.EstimatedStartDate;
                EstimatedEndDate = data.EstimatedEndDate;
                ActualStartDate = data.ActualStartDate;
                ActualEndDate = data.ActualEndDate;

                Milestone = data.Milestone;
                Priority = data.Priority;
                Progress = data.Progress;
               
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

            Type type = typeof(Task);
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
            Type type = typeof(Task);
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
        /// Load Project Task for [tskid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ptskid">as project task identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_tasks_projects]</TableName>
        public bool Load(string pconstring, Guid ptskid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_tasks_projects "));
                        query.Append(string.Format(" WHERE tskid=?ptskid; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptskid", MySqlDbType.VarChar, 50)).Value = ptskid.ToString();

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
        /// Add Project Task To System
        /// </summary>
        /// <param name="pconstring">as Data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects_tasks]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addProjectTask", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        //reset id
                        TaskID = Guid.NewGuid();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptskid", MySqlDbType.VarChar, 50)).Value = TaskID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 125)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarString)).Value = Description.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEstStartDate", MySqlDbType.VarChar, 50)).Value = EstimatedStartDate.HasValue ? Convert.ToDateTime(EstimatedStartDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEstEndDate", MySqlDbType.VarChar, 50)).Value = EstimatedEndDate.HasValue ? Convert.ToDateTime(EstimatedEndDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pActStartDate", MySqlDbType.VarChar, 50)).Value = ActualStartDate.HasValue ? Convert.ToDateTime(ActualStartDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pActEndDate", MySqlDbType.VarChar, 50)).Value = ActualEndDate.HasValue ? Convert.ToDateTime(ActualEndDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pMilestone", MySqlDbType.Bit)).Value = (Milestone) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPriority", MySqlDbType.Int16)).Value = Convert.ToInt16(Priority);
                        sqlcomm.Parameters.Add(new MySqlParameter("?pProgress", MySqlDbType.Int16)).Value = Progress;


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
        /// Update Project Task in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects_tasks]</TableName>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateProjectTask", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptskid", MySqlDbType.VarChar, 50)).Value = TaskID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = PID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 125)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarString)).Value = Description.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEstStartDate", MySqlDbType.VarChar, 50)).Value = EstimatedStartDate.HasValue ? Convert.ToDateTime(EstimatedStartDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pEstEndDate", MySqlDbType.VarChar, 50)).Value = EstimatedEndDate.HasValue ? Convert.ToDateTime(EstimatedEndDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pActStartDate", MySqlDbType.VarChar, 50)).Value = ActualStartDate.HasValue ? Convert.ToDateTime(ActualStartDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pActEndDate", MySqlDbType.VarChar, 50)).Value = ActualEndDate.HasValue ? Convert.ToDateTime(ActualEndDate).ToString("yyyy-MM-dd") : "";
                        sqlcomm.Parameters.Add(new MySqlParameter("?pMilestone", MySqlDbType.Bit)).Value = (Milestone) ? 1 : 0;
                        sqlcomm.Parameters.Add(new MySqlParameter("?pPriority", MySqlDbType.Int16)).Value = Convert.ToInt16(Priority);
                        sqlcomm.Parameters.Add(new MySqlParameter("?pProgress", MySqlDbType.Int16)).Value = Progress;

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
        /// Remove Project task from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[projects_tasks]</TableName>
        public Boolean Delete(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteProjectTask", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptskid", MySqlDbType.VarChar, 50)).Value = TaskID.ToString();
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
        /// Remove Project task from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="proleid">as projects roles identifier</param>
        /// <returns></returns>
        /// <TableName>[projects_tasks]</TableName>
        public static Boolean Delete(String pconstring, Guid ptskid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteProjectTask", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptskid", MySqlDbType.VarChar, 50)).Value = ptskid.ToString();
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
        /// Return DataTable of all projects tasks in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_tasks_projects]</TableName>
        public static DataTable dgTasks(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_tasks_projects");
                        query.Append(" ORDER BY `TskName` ASC;");
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
        /// Return DataTable of all project tasks where [colname] contains [colval]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as value to search for</param>
        /// <returns></returns>
        /// <TableName>[vw_tasks_projects]</TableName>
        public static DataTable dgTasks(string pconstring, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_tasks_projects");
                        if (!string.IsNullOrEmpty(pcolname)) query.Append(string.Format(" WHERE `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
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

        /// <summary>
        /// Return DataTable of all projects tasks in system for [pid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ppid">as project's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_tasks_projects]</TableName>
        public static DataTable dgTasks(string pconstring, Guid ppid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_tasks_projects");
                        query.Append(" WHERE `pid`=?ppid");
                        query.Append(" ORDER BY `TskEstStartDate` ASC,`TskEstEndDate` ASC;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ppid", MySqlDbType.VarChar, 50)).Value = ppid.ToString();

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
        /// Return DataTable of all project tasks where [colname] contains [colval]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ppid">as project's identifer</param>
        /// <param name="pcolname">as column to search on</param>
        /// <param name="pcolval">as value to search for</param>
        /// <returns></returns>
        /// <TableName>[vw_tasks_projects]</TableName>
        public static DataTable dgTasks(string pconstring, Guid ppid, string pcolname, string pcolval)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_tasks_projects");
                        query.Append(" WHERE `pid`=?ppid");
                        if (!string.IsNullOrEmpty(pcolname)) query.Append(string.Format(" AND `{0}` LIKE '%{1}%'", pcolname.Trim(), pcolval.Trim()));
                        //query.Append(string.Format(" ORDER BY `{0}` ASC;", pcolname.Trim()));
                        query.Append(" ORDER BY `TskEstStartDate` ASC,`TskEstEndDate` ASC;");
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
        /// Return DataTable of all task predicessors for [tskid] tasks
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="ptskid">as project's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_tasks_predecessors]</TableName>
        public static DataTable dgTaskPredicessors(string pconstring, Guid ptskid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM vw_tasks_predecessors");
                        query.Append(" WHERE `tskid`=?ptskid");
                        query.Append(" ORDER BY `TskEstStartDate` ASC,`TskEstEndDate` ASC;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ptskid", MySqlDbType.VarChar, 50)).Value = ptskid.ToString();

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
        /// Write Task to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of Task</param>
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

                    xmlSer = new XmlSerializer(typeof(Task));
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
        /// Create Task from File
        /// </summary>
        /// <param name="xmlpath">as source location of Task </param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(Task));
                Task output = (Task)xs.Deserialize(fs);
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
        /// <param name="data">as Task</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(Project data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Task));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(Task));
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
        /// Return Task from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of Task</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Task XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Task));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                Task output = default(Task);
                output = (Task)xmlSer.Deserialize(string_reader);
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