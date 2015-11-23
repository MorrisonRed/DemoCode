using System;
using System.IO;
using System.Data;
using System.Net;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using democode.mvc.Models;

namespace democode.mvc.Controllers
{
    public class EventMessageController : Controllers.BaseController
    {
        List<democode.mvc.Models.EventMessage> messages = new List<democode.mvc.Models.EventMessage>();

        private democode.mvc.Models.EventMessage convertToEventMessage(DataRow dr)
        {
            democode.mvc.Models.EventMessage msg = new democode.mvc.Models.EventMessage();
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
                if (dr["EventTime"] != System.DBNull.Value) { msg.EventDateTime = (DateTime)dr["EventTime"]; }
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
        private democode.mvc.Models.EventMessage convertToEventMessage(EventMessage.EventMessage data)
        {
            democode.mvc.Models.EventMessage msg = new democode.mvc.Models.EventMessage();
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
                msg.EventDateTime = data.EventDateTime;
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
        private EventMessage.EventMessage convertFromEventMessage(democode.mvc.Models.EventMessage data)
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


        // GET: /EventMessage/
        public ActionResult Index()
        {
            //check if user is logged in and authorized
            if (CurrentUser == null || !CurrentUser.Role.IsSystem)
            {
                return RedirectToAction("", "Home");
            }

            messages = new List<democode.mvc.Models.EventMessage>();
            DataTable dt = EventMessage.EventMessage.dgEvents(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());

            if (dt == null && dt.Rows.Count == 0) return View(messages);

            int totalRows = (dt.Rows.Count < 100) ? dt.Rows.Count - 1 : 100; //correct for off by one error

            for (int i = 0; i <= totalRows; i++)
            //foreach (DataRow dr in dt.Rows)
            {
                DataRow dr = dt.Rows[i];
                var ev = convertToEventMessage(dr);
                if (ev != null) messages.Add(ev);
            }

            return View(messages);
        }
        // GET: /EventMessage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EventMessage.EventMessage em = new EventMessage.EventMessage(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), Convert.ToInt32(id));
            
            
            return View(convertToEventMessage(em));
        }

        // GET: /EventMessage/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: /EventMessage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: /EventMessage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventMessage.EventMessage em = new EventMessage.EventMessage(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), Convert.ToInt32(id));

            if (em == null)
            {
                HttpNotFound();
            }

            return View(convertToEventMessage(em));
        }
        // POST: E/ventMessage/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "EventID,Level,Result,Application,ApplicationVersion,OperationCode,Keywords,EventDateTime,UID,IP,URL")] 
            democode.mvc.Models.EventMessage em)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                EventMessage.EventMessage ee = convertFromEventMessage(em);
                //ee.Update(ConfigurationManager.ConnectionStrings["SystemDS"].ToString());

                return RedirectToAction("Index");
            }
            return View(em);
        }

        // GET: EventMessage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: EventMessage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
