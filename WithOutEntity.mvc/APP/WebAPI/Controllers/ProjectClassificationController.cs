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
    public class ProjectClassificationController : ApiController
    {
        //session state is enabled for WebAPI in the global.ascx
        CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

        List<WebAPI.Models.ProjectClassification> taxonomy = new List<WebAPI.Models.ProjectClassification>();

        private WebAPI.Models.ProjectClassification DataRowToWebApiModel(DataRow dr)
        {
            WebAPI.Models.ProjectClassification taxonomy = new WebAPI.Models.ProjectClassification();
            try
            {
                if (dr["tid"] != System.DBNull.Value) { taxonomy.TID = Convert.ToInt16(dr["tid"]); }
                if (dr["pid"] != System.DBNull.Value) { taxonomy.PID = (String)dr["pid"]; }
                if (dr["TxName"] != System.DBNull.Value) { taxonomy.Name = (String)dr["TxName"]; }
                if (dr["TxDesc"] != System.DBNull.Value) { taxonomy.Description = (String)dr["TxDesc"]; }
                if (dr["TxWeight"] != System.DBNull.Value) { taxonomy.Weight = Convert.ToInt16(dr["TxWeight"]); }
                
                return taxonomy;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private WebAPI.Models.ProjectClassification DALObjectToWebApiModel(ProjectMgmt.Taxonomy data)
        {
            WebAPI.Models.ProjectClassification taxonomy = new WebAPI.Models.ProjectClassification();
            try
            {
                taxonomy.TID = data.TID;
                taxonomy.Name = data.Name;
                taxonomy.Description = data.Description;
                taxonomy.Weight = data.Weight;

                return taxonomy;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private ProjectMgmt.Taxonomy WebApiModelToDALObject(WebAPI.Models.ProjectClassification data)
        {
            ProjectMgmt.Taxonomy tax = new ProjectMgmt.Taxonomy();
            try
            {
                tax.TID = data.TID;
                tax.Name = data.Name;
                tax.Description = data.Description;
                tax.Weight = data.Weight;

                return tax;
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
        public IEnumerable<WebAPI.Models.ProjectClassification> GetAllProjectClassification()
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return null;

            taxonomy = new List<WebAPI.Models.ProjectClassification>();
            DataTable dt = null;
            //dt = ProjectMgmt.Taxonomy.dgProjectTaxonomy(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());

            if (dt == null || dt.Rows.Count == 0) return taxonomy;

            foreach (DataRow dr in dt.Rows)
            {
                var ev = DataRowToWebApiModel(dr);
                if (ev != null) taxonomy.Add(ev);
            }

            return taxonomy;
        }

        /// <summary>
        /// This maps to the R(Retrieve) part of the CRUD operation.  This will be sued to retrieve the 
        /// required data (representation of data) from the remote resource
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WebAPI.Models.ProjectClassification> GetProjectClassification(string pid)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return null;

            taxonomy = new List<WebAPI.Models.ProjectClassification>();
            DataTable dt = ProjectMgmt.Taxonomy.dgProjectTaxonomy(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), new Guid(pid));

            if (dt == null || dt.Rows.Count == 0) return taxonomy;

            foreach (DataRow dr in dt.Rows)
            {
                var ev = DataRowToWebApiModel(dr);
                if (ev != null) taxonomy.Add(ev);
            }

            return taxonomy;
        }
       
        /// <summary>
        /// This maps to the C(Create) part of the CRUD operation.  This will create a new entry for the 
        /// current data that is being sent to the server
        /// </summary>
        /// <param name="value"></param>
        public HttpResponseMessage Post(WebAPI.Models.ProjectClassification tax)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                ////add taxonomy classification to project
                ProjectMgmt.Taxonomy t = this.WebApiModelToDALObject(tax);
                if (ProjectMgmt.Taxonomy.AddToProject(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), tax.TID, new Guid(tax.PID)))
                {
                    return Request.CreateResponse<WebAPI.Models.ProjectClassification>(System.Net.HttpStatusCode.Created, tax);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ProjectMgmt.Taxonomy.GetLastError.Message);
                }
                //return Request.CreateResponse<WebAPI.Models.ProjectTaxonomy>(System.Net.HttpStatusCode.Created, tax);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This maps to the D(Delete) part of the CRUD operation.
        /// </summary>
        public HttpResponseMessage Delete(int ptid, string ppid)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                if (ProjectMgmt.Taxonomy.DeleteFromProject(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), ptid, new Guid(ppid)))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, ProjectMgmt.Taxonomy.GetLastError.Message);
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
        public HttpResponseMessage Delete(WebAPI.Models.ProjectClassification tax)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                if (ProjectMgmt.Taxonomy.DeleteFromProject(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), tax.TID, new Guid(tax.PID)))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, ProjectMgmt.Taxonomy.GetLastError.Message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}