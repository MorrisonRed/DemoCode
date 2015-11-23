using System;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public class Html5Helper
    {
        public Html5Helper(ViewContext viewContext,
          IViewDataContainer viewDataContainer)
            : this(viewContext, viewDataContainer, RouteTable.Routes)
        {

        }

        public Html5Helper(ViewContext viewContext,
           IViewDataContainer viewDataContainer, RouteCollection routeCollection)
        {
            ViewContext = viewContext;
            ViewData = new ViewDataDictionary(viewDataContainer.ViewData);
        }

        public ViewDataDictionary ViewData
        {
            get;
            private set;
        }

        public ViewContext ViewContext
        {
            get;
            private set;
        }
    }

    public static class Html5Extensions
    {
        public static IHtmlString EmailInput(this Html5Helper html, string name, string value)
        {
            var tagBuilder = new TagBuilder("input");
            tagBuilder.Attributes.Add("type", "email");
            tagBuilder.Attributes.Add("value", value);
            return new HtmlString(tagBuilder.ToString());
        }
    }
}