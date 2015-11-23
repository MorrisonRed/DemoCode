using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace democode.mvc.Controllers
{
    public class BaseController : Controller
    {
        private CustomSecurity.User _user;

        //Set Session Variables
        #region Public Properties
        public CustomSecurity.User CurrentUser
        {
            get
            {
                _user = (CustomSecurity.User)Session["user"];
                return _user;
            }
            set
            {
                _user = value;
                Session["user"] = value;
            }
        }
        #endregion
    }
}