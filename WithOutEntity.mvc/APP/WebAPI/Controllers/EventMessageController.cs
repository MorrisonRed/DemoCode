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
    public class EventMessageController : ApiController
    {
        //session state is enabled for WebAPI in the global.ascx
        CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];
      
        List<WebAPI.Models.EventMessage> messages = new List<WebAPI.Models.EventMessage>();

		private WebAPI.Models.EventMessage convertToEventMessage(DataRow dr)
        {
            WebAPI.Models.EventMessage msg = new WebAPI.Models.EventMessage(); 
			try
            {
                if (dr["EventID"] != System.DBNull.Value) { msg.EventID = Convert.ToInt32(dr["EventID"].ToString()); }
                if (dr["EventLevel"] != System.DBNull.Value) { msg.Level = Convert.ToInt16(dr["EventLevel"].ToString()); }
                if (dr["EventAction"] != System.DBNull.Value) { msg.Action = Convert.ToInt16(dr["EventAction"].ToString()); }
                if (dr["EventResult"] != System.DBNull.Value) { msg.Result = Convert.ToInt16(dr["EventResult"].ToString()); }
                if (dr["EventApp"] != System.DBNull.Value) { msg.Application = (String)dr["EventApp"]; }
				if (dr["EventAppVer"] != System.DBNull.Value) { msg.ApplicationVersion = (String)dr["EventAppVer"]; }
				if (dr["EventOpCode"] != System.DBNull.Value) { msg.OperationCode = (String)dr["EventOpCode"]; }
				if (dr["EventKeyWords"] != System.DBNull.Value) { msg.Keywords = (String)dr["EventKeyWords"]; }
				if (dr["EventTime"] != System.DBNull.Value) { msg.EventDateTime = ((DateTime)dr["EventTime"]).ToString("yyyy-MM-dd HH:mm:ss"); }
				if (dr["EventIP"] != System.DBNull.Value) { msg.IP = (String)dr["EventIP"]; }
				if (dr["UID"] != System.DBNull.Value) { msg.UID = dr["UID"].ToString(); }
				if (dr["EventURL"] != System.DBNull.Value) { msg.URL = (String)dr["EventURL"]; }

				return msg;
            }
			catch (Exception ex)
            {
				return null;
            }
        }
		private WebAPI.Models.EventMessage convertToEventMessage(EventMessage.EventMessage data)
        {
            WebAPI.Models.EventMessage msg = new WebAPI.Models.EventMessage();
            try
            {
                msg.EventID = data.EventID;
                msg.Level = Convert.ToInt16(data.Level);
                msg.Action = Convert.ToInt16(data.Action);
                msg.Result = Convert.ToInt16(data.Result);
                msg.Application = data.Application;
                msg.ApplicationVersion = data.ApplicationVersion;
                msg.OperationCode = data.OperationCode;
                msg.Keywords = data.KeyWords;
                msg.EventDateTime = data.EventDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                msg.UID = data.UID.ToString();
                msg.IP = data.IP;
                msg.URL = data.URL;

                return msg;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private EventMessage.EventMessage convertFromEventMessage(WebAPI.Models.EventMessage data)
        {
            EventMessage.EventMessage msg = new EventMessage.EventMessage();
            try
            {
                msg.EventID = data.EventID;
                msg.Level = (EventMessage.EventLevel)Convert.ToInt16(data.Level);
                msg.Action = (EventMessage.EventAction)Convert.ToInt16(data.Action);
                msg.Result = (EventMessage.EventResult)Convert.ToInt16(data.Result);
                msg.Application = data.Application;
                msg.ApplicationVersion = data.ApplicationVersion;
                msg.OperationCode = data.OperationCode;
                msg.KeyWords = data.Keywords;
                msg.EventDateTime = Convert.ToDateTime(data.EventDateTime);
                msg.UID = (data.UID == null) ? Guid.Empty : new Guid(data.UID);
                msg.IP = data.IP;
                msg.URL = data.URL;

                return msg;
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
        public IEnumerable<WebAPI.Models.EventMessage> GetAllEventMessages()
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return null;

            messages = new List<WebAPI.Models.EventMessage>();
            DataTable dt = EventMessage.EventMessage.dgEvents(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());

            if (dt == null && dt.Rows.Count == 0) return messages;

            foreach (DataRow dr in dt.Rows)
            {
                var ev = convertToEventMessage(dr);
				if (ev != null) messages.Add(ev);
            }
            
            return messages;
        }

        /// <summary>
        /// This maps to the R(Retrieve) part of the CRUD operation.  This will be sued to retrieve the 
        /// required data (representation of data) from the remote resource
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetEventMessage(int eventid)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return NotFound();

            messages = new List<WebAPI.Models.EventMessage>();
            var ev = EventMessage.EventMessage.getMessage(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), eventid);
            if (ev == null)
            {
                return NotFound();
            }

            return Ok(convertToEventMessage(ev));
        }

        /// <summary>
        /// This maps to the C(Create) part of the CRUD operation.  This will create a new entry for the 
        /// current data that is being sent to the server
        /// </summary>
        /// <param name="value"></param>
        public HttpResponseMessage Post(WebAPI.Models.EventMessage message)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                EventMessage.EventMessage ev = this.convertFromEventMessage(message);
                if (ev.Add(ConfigurationManager.ConnectionStrings["SystemDS"].ToString()))
                {
                    return Request.CreateResponse<WebAPI.Models.EventMessage>(System.Net.HttpStatusCode.Created, message);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, EventMessage.EventMessage.GetLastError.Message);
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
        public HttpResponseMessage Put(WebAPI.Models.EventMessage message)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                //EventMessage.EventMessage ev = this.convertFromEventMessage(message);
                //if (ev.Update(ConfigurationManager.ConnectionStrings["SystemDS"].ToString()))
                //{
                //    return Request.CreateResponse<WebAPI.Models.EventMessage>(System.Net.HttpStatusCode.Created, message);
                //}
                //else
                //{
                //    return Request.CreateResponse(HttpStatusCode.InternalServerError, EventMessage.EventMessage.GetLastError.Message);
                //}
                return Request.CreateResponse<WebAPI.Models.EventMessage>(System.Net.HttpStatusCode.Created, message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// This maps to the D(Delete) part of the CRUD operation.
        /// </summary>
        public HttpResponseMessage Delete(int eventid)
        {
            //session state is enabled for WebAPI in the global.ascx
            CustomSecurity.User u = (CustomSecurity.User)HttpContext.Current.Session["user"];

            if (u == null || u.IsEmpty()) return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid user");

            try
            {
                if (EventMessage.EventMessage.Delete(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), eventid))
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