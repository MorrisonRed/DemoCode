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
    public class RSSController : ApiController
    {
        List<WebAPI.Models.RSS> rsses = new List<WebAPI.Models.RSS>();

        private WebAPI.Models.RSS DataRowToWebApiModel(DataRow dr)
        {
            WebAPI.Models.RSS rss = new WebAPI.Models.RSS();
            try
            {
                if (dr["RSSID"] != System.DBNull.Value) { rss.RSSID = (String)dr["RSSID"].ToString(); }
                if (dr["RSSName"] != System.DBNull.Value) { rss.Name = (String)dr["RSSName"]; }
                if (dr["RSSIcon"] != System.DBNull.Value) { rss.Icon = (String)dr["RSSIcon"]; }
                if (dr["RSSDescription"] != System.DBNull.Value) { rss.Description = (String)dr["RSSDescription"]; }

                return rss;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private WebAPI.Models.RSS DALObjectToWebApiModel(RSS.RSS data)
        {
            WebAPI.Models.RSS rss = new WebAPI.Models.RSS();
            try
            {
                rss.RSSID = data.RSSID.ToString();
                rss.Name = data.Name;
                rss.Icon = data.Icon;
                rss.Description = data.Description;
               
                return rss;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private RSS.RSS WebApiModelToDALObject(WebAPI.Models.RSS data)
        {
            RSS.RSS rss = new RSS.RSS();
            try
            {
                rss.RSSID = new Guid(data.RSSID);
                rss.Name = data.Name;
                rss.Icon = data.Icon;
                rss.Description = data.Description;

                return rss;
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
        public IEnumerable<WebAPI.Models.RSS> GetAllRSS()
        {
            rsses = new List<WebAPI.Models.RSS>();
            DataTable dt = RSS.RSS.dgRSS(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());
            if (dt == null && dt.Rows.Count == 0) return rsses;
            foreach (DataRow dr in dt.Rows)
            {
                var rss = DataRowToWebApiModel(dr);
                if (rss != null) rsses.Add(rss);
            }

            return rsses;
        }

        /// <summary>
        /// This maps to the R(Retrieve) part of the CRUD operation.  This will be sued to retrieve the 
        /// required data (representation of data) from the remote resource
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetRSS(Guid rssid)
        {
            var rss = RSS.RSS.getRSSID(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), rssid);
            if (rss == null)
            {
                return NotFound();
            }

            return Ok(DALObjectToWebApiModel(rss));
        }

        /// <summary>
        /// This maps to the C(Create) part of the CRUD operation.  This will create a new entry for the 
        /// current data that is being sent to the server
        /// </summary>
        /// <param name="value"></param>
        public HttpResponseMessage Post(WebAPI.Models.RSS rss)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                RSS.RSS r = this.WebApiModelToDALObject(rss);
                if (r.Add(ConfigurationManager.ConnectionStrings["SystemDS"].ToString()))
                {
                    return Request.CreateResponse<WebAPI.Models.RSS>(System.Net.HttpStatusCode.Created, rss);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, RSS.RSS.GetLastError.Message);
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
        public HttpResponseMessage Put(WebAPI.Models.RSS rss)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                RSS.RSS r = this.WebApiModelToDALObject(rss);
                if (r.Update(ConfigurationManager.ConnectionStrings["SystemDS"].ToString()))
                {
                    return Request.CreateResponse<WebAPI.Models.RSS>(System.Net.HttpStatusCode.OK, rss);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, RSS.RSS.GetLastError.Message);
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
                if (RSS.RSS.Delete(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), rssid))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, RSS.RSS.GetLastError.Message);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}