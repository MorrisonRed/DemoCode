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
    public class ManagementController : Controllers.BaseController
    {
        // GET: Management
        [Route("Management")]
        public ActionResult Index()
        {
            //check if user is logged in and authorized
            if (CurrentUser == null || !CurrentUser.Role.IsSystem)
            {
                return RedirectToAction("", "Home");
            }

            return View();
        }



        #region Helpers

        #endregion
    }
}