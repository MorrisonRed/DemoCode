using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

using democode.mvc.Models;


namespace democode.mvc.Controllers
{
    public class HomeController : Controllers.BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "About Me";
            ViewBag.Message = "Acount the Author";

            return View();
        }

        public ActionResult Contact()
        {
            //set ViewBag Variables
            ViewBag.Messsage = "Contact the author";
            ViewBag.Title = "Contact Me";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModels model)
        {
            //set ViewBag Variables
            ViewBag.Messsage = "Contact the author";
            ViewBag.Title = "Contact Me";

            //Controller Logic
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage em = new MailMessage();
                    var body = @"
                        <p>Email from {0} ({1})</p>
                        <p>Message:</p>
                        <p>{2}</p>";

                    em.From = new MailAddress(ConfigurationManager.AppSettings["emailcomment"].ToString());
                    em.To.Add(ConfigurationManager.AppSettings["emailcomment"].ToString());
                    em.CC.Add(new MailAddress(model.FromEmail));
                    em.Subject = model.Subject;
                    em.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                    em.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(em);
                        return RedirectToAction("Sent");
                    }
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
            return View(model);
        }

        public ActionResult Sent()
        {
            return View();
        }
    }
}