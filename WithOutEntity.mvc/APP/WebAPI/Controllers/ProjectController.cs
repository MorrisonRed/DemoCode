using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using System.Web;

namespace WebAPI.Controllers
{
    public class ProjectController : ApiController
    {
        //session state is enabled for WebAPI in the global.ascx
        CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

        List<WebAPI.Models.Project> projects = new List<WebAPI.Models.Project>();

        private WebAPI.Models.Project DataRowToWebApiModel(DataRow dr)
        {
            WebAPI.Models.Project project = new WebAPI.Models.Project();
            try
            {
                if (dr["pid"] != System.DBNull.Value) { project.PID = (String)dr["pid"]; }
                if (dr["ProjIcon"] != System.DBNull.Value) { project.Icon = (String)dr["ProjIcon"]; }
                if (dr["ProjCode"] != System.DBNull.Value) { project.Code = (String)dr["ProjCode"]; }
                if (dr["ProjName"] != System.DBNull.Value) { project.Name = (String)dr["ProjName"]; }
                if (dr["ProjDesc"] != System.DBNull.Value) { project.Description = (String)dr["ProjDesc"]; }
                if (dr["ProjEstStartDate"] != System.DBNull.Value) { project.EstimatedStartDate = (String)dr["ProjEstStartDate"]; }
                if (dr["ProjEstEndDate"] != System.DBNull.Value) { project.EstimatedEndDate = (String)dr["ProjEstEndDate"]; }
                if (dr["ProjActStartDate"] != System.DBNull.Value) { project.ActualStartDate = (String)dr["ProjActStartDate"]; }
                if (dr["ProjActEndDate"] != System.DBNull.Value) { project.ActualEndDate = (String)dr["ProjActEndDate"]; }
                if (dr["ProjFolder"] != System.DBNull.Value) { project.Folder = (String)dr["ProjFolder"]; }
                if (dr["ProjCaption"] != System.DBNull.Value) { project.Caption = (String)dr["ProjCaption"]; }
                if (dr["ProjURL"] != System.DBNull.Value) { project.URL = (String)dr["ProjURL"]; }
                if (dr["ProjOrg"] != System.DBNull.Value) { project.Organization = (String)dr["ProjOrg"]; }

                return project;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private WebAPI.Models.Project DALObjectToWebApiModel(ProjectMgmt.Project data)
        {
            WebAPI.Models.Project project = new WebAPI.Models.Project();
            try
            {
                project.PID = data.PID.ToString();
                project.Icon = data.Icon;
                project.Code =data.Code;
                project.Name = data.Name;
                project.Description = data.Description;
                project.EstimatedStartDate = (data.EstimatedStartDate.HasValue) ? ((DateTime)data.EstimatedStartDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
                project.EstimatedEndDate = (data.EstimatedEndDate.HasValue) ? ((DateTime)data.EstimatedEndDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
                project.ActualStartDate = (data.ActualStartDate.HasValue) ? ((DateTime)data.ActualStartDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
                project.ActualEndDate = (data.ActualEndDate.HasValue) ? ((DateTime)data.ActualEndDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
                project.Folder = data.Folder;
                project.Caption = data.Caption;
                project.URL = data.URL;
                project.Organization = data.Organization;

                return project;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private ProjectMgmt.Project WebApiModelToDALObject(WebAPI.Models.Project data)
        {
            ProjectMgmt.Project dal = new ProjectMgmt.Project();
            try
            {
                dal.PID = new Guid(data.PID);
                dal.Icon = data.Icon;
                dal.Code = data.Code;
                dal.Name = data.Name;
                dal.Description = data.Description;
                dal.EstimatedStartDate = Convert.ToDateTime(data.EstimatedStartDate);
                dal.EstimatedEndDate = Convert.ToDateTime(data.EstimatedEndDate);
                dal.ActualStartDate = Convert.ToDateTime(data.ActualStartDate);
                dal.ActualEndDate = Convert.ToDateTime(data.ActualEndDate);
                dal.Folder = data.Folder;
                dal.Caption = data.Caption;
                dal.URL = data.URL;
                dal.Organization = data.Organization;

                return dal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This maps to the R(Retrieve) part of the CRUD operation.  This will be sued to retrieve the 
        /// required data (representation of data) from the remote resource
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WebAPI.Models.Project> GetAllProjects()
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return null;

            projects = new List<WebAPI.Models.Project>();
            DataTable dt = ProjectMgmt.Project.dgProjects(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());

            if (dt == null && dt.Rows.Count == 0) return projects;

            foreach (DataRow dr in dt.Rows)
            {
                var ev = DataRowToWebApiModel(dr);
                if (ev != null) projects.Add(ev);
            }

            return projects;
        }

        /// <summary>
        /// This maps to the R(Retrieve) part of the CRUD operation.  This will be sued to retrieve the 
        /// required data (representation of data) from the remote resource
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetProject(Guid pid)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return NotFound();

            projects = new List<WebAPI.Models.Project>();
            var ev = ProjectMgmt.Project.getProject(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), pid);
            if (ev == null)
            {
                return NotFound();
            }

            return Ok(DALObjectToWebApiModel(ev));
        }

        /// <summary>
        /// This maps to the C(Create) part of the CRUD operation.  This will create a new entry for the 
        /// current data that is being sent to the server
        /// </summary>
        /// <param name="value"></param>
        public HttpResponseMessage Post(WebAPI.Models.Project project)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                ProjectMgmt.Project p = this.WebApiModelToDALObject(project);
                if (p.Add(ConfigurationManager.ConnectionStrings["SystemDS"].ToString()))
                {
                    return Request.CreateResponse<WebAPI.Models.Project>(System.Net.HttpStatusCode.Created, project);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ProjectMgmt.Project.GetLastError.Message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This maps to the U(Update) part of the CRUD operation.  This protocol will update the current representation 
        /// of the data on the remote server.
        /// </summary>
        public HttpResponseMessage Put(WebAPI.Models.Project project)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                ProjectMgmt.Project p = this.WebApiModelToDALObject(project);
                if (p.Update(ConfigurationManager.ConnectionStrings["SystemDS"].ToString()))
                {
                    return Request.CreateResponse<WebAPI.Models.Project>(System.Net.HttpStatusCode.OK, project);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ProjectMgmt.Project.GetLastError.Message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This maps to the D(Delete) part of the CRUD operation.
        /// </summary>
        public HttpResponseMessage Delete(Guid pid)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                if (ProjectMgmt.Project.Delete(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), pid))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, ProjectMgmt.Project.GetLastError.Message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}