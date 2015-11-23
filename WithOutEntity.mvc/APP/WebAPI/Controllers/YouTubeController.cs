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
    public class YouTubeController : ApiController
    {
        List<WebAPI.Models.YouTube> utubes = new List<WebAPI.Models.YouTube>();

        private WebAPI.Models.YouTube DataRowToWebApiModel(DataRow dr)
        {
            WebAPI.Models.YouTube utube = new WebAPI.Models.YouTube();
            try
            {
                if (dr["RSSID"] != System.DBNull.Value) { utube.RSSID = (String)dr["RSSID"].ToString(); }
                if (dr["RSSName"] != System.DBNull.Value) { utube.Name = (String)dr["RSSName"]; }
                if (dr["RSSIcon"] != System.DBNull.Value) { utube.Icon = (String)dr["RSSIcon"]; }
                if (dr["RSSDescription"] != System.DBNull.Value) { utube.Description = (String)dr["RSSDescription"]; }

                return utube;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private WebAPI.Models.YouTube DALObjectToWebApiModel(YouTube.RSS data)
        {
            WebAPI.Models.YouTube utube = new WebAPI.Models.YouTube();
            try
            {
                utube.RSSID = data.RSSID.ToString();
                utube.Name = data.Name;
                utube.Icon = data.Icon;
                utube.Description = data.Description;

                return utube;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private YouTube.RSS WebApiModelToDALObject(WebAPI.Models.YouTube data)
        {
            YouTube.RSS utube = new YouTube.RSS();
            try
            {
                utube.RSSID = new Guid(data.RSSID);
                utube.Name = data.Name;
                utube.Icon = data.Icon;
                utube.Description = data.Description;

                return utube;
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
        public IEnumerable<WebAPI.Models.YouTube> GetAllYouTubes()
        {
            utubes = new List<WebAPI.Models.YouTube>();
            DataTable dt = YouTube.RSS.dgRSS(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());
            if (dt == null && dt.Rows.Count == 0) return utubes;
            foreach (DataRow dr in dt.Rows)
            {
                var utube = DataRowToWebApiModel(dr);
                if (utube != null) utubes.Add(utube);
            }

            return utubes;
        }

        /// <summary>
        /// This maps to the R(Retrieve) part of the CRUD operation.  This will be sued to retrieve the 
        /// required data (representation of data) from the remote resource
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetYouTube(Guid rssid)
        {
            utubes = new List<WebAPI.Models.YouTube>();
            var utube = YouTube.RSS.getYouTube(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), rssid);
            if (utube == null)
            {
                return NotFound();
            }

            return Ok(DALObjectToWebApiModel(utube));
        }

        /// <summary>
        /// This maps to the C(Create) part of the CRUD operation.  This will create a new entry for the 
        /// current data that is being sent to the server
        /// </summary>
        /// <param name="value"></param>
        public HttpResponseMessage Post(WebAPI.Models.YouTube utube)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                YouTube.RSS r = this.WebApiModelToDALObject(utube);
                if (r.Add(ConfigurationManager.ConnectionStrings["SystemDS"].ToString()))
                {
                    return Request.CreateResponse<WebAPI.Models.YouTube>(System.Net.HttpStatusCode.Created, utube);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, YouTube.RSS.GetLastError.Message);
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
        public HttpResponseMessage Put(WebAPI.Models.YouTube utube)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                YouTube.RSS r = this.WebApiModelToDALObject(utube);
                if (r.Update(ConfigurationManager.ConnectionStrings["SystemDS"].ToString()))
                {
                    return Request.CreateResponse<WebAPI.Models.YouTube>(System.Net.HttpStatusCode.OK, utube);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, YouTube.RSS.GetLastError.Message);
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
        public HttpResponseMessage Delete(Guid rssid)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                if (YouTube.RSS.Delete(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), rssid))
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