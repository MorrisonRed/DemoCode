using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using democode.mvc;

namespace System.Web.Mvc
{

    
    public abstract class BaseWebViewPage : WebViewPage
    {
        
        //Set Session Variables
        #region Public Properties
        private CustomSecurity.User _user;
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


        public Html5Helper Html5 { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            //Html5 = new Html5Helper<object>(base.ViewContext, this);
        }
    }

    public abstract class BaseWebViewPage<TModel> : WebViewPage<TModel>
    {
        
        //Set Session Variables
        #region Public Properties
        private CustomSecurity.User _user;
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

        public Html5Helper Html5 { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            //Html5 = new Html5Helper<object>(base.ViewContext, this);
        }
    }
}