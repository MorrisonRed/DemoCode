using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace democode.mvc.Helpers
{
    public static class HtmlHelpers
    {
        public static string getFontAwesomeEventLevel(int level)
        {
            //cast to enumerator for readablility
            EventMessage.EventLevel l = (EventMessage.EventLevel)level;

            switch (l)
            {
                case EventMessage.EventLevel.ApplicationLayer:
                    return "<i class='fa fa-cog' title='Application Layer'></i>";
                case EventMessage.EventLevel.DataAccessLayer:
                    return "<i class='fa fa-bolt' title='Data Access Layer'></i>";
                case EventMessage.EventLevel.SQLServer:
                    return "<i class='fa fa-database' title='Database Layer'></i>";
                case EventMessage.EventLevel.SystemIO:
                    return "<i class='fa fa-hdd-o' title='System IO'></i>";
                case EventMessage.EventLevel.ThirdPartyControl:
                    return "<i class='fa fa-bullseye' title=''></i>";
                default:
                    return null;  //null return do not rendor in page
            }
        }
        public static string getFontAwesomeEventAction(int action)
        {
            //cast to enumerator for readablility
            EventMessage.EventAction a = (EventMessage.EventAction)action;

            switch (a)
            {
                case EventMessage.EventAction.Login:
                    return "<i class='fa-sign-in' title='Login'></i>";
                case EventMessage.EventAction.Logout:
                    return "<i class='fa-sign-out' title='Logout'></i>";
                case EventMessage.EventAction.Register:
                    return "<i class='icon-chevron-sign-up' title='Register'></i>";
                case EventMessage.EventAction.Visit:
                    return "<i class='fa fa fa-refresh' title='Visit'></i>";
                default:
                    return null;  //null return do not rendor in page
            }
        }
        public static string getFontAwesomeEventResult(int result)
        {
            //cast to enumerator for readablility
            EventMessage.EventResult r = (EventMessage.EventResult)result;

            switch (r)
            {
                case EventMessage.EventResult.OK:
                    return "<i class='fa fa-check' title='OK'></i>";
                case EventMessage.EventResult.Cancel:
                    return "<i class='fa fa-close' title='Cancel'></i>";
                case EventMessage.EventResult.Error:
                    return "<i class='fa fa-bug' title='Error'></i>";
                case EventMessage.EventResult.Invalid:
                    return "<i class='fa fa-lock' title='Invalid'></i>";
                default:
                    return null;  //null return do not rendor in page
            }
        }
    }
}
