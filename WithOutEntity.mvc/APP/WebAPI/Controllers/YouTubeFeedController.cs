using System;
using System.Text;
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
    public class YouTubeFeedController : ApiController
    {

        /// <summary>
        /// This maps to the R(Retrieve) part of the CRUD operation.  This will be sued to retrieve the 
        /// required data (representation of data) from the remote resource
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetAllYouTubes()
        {
            return NotFound();
        }

        /// <summary>
        /// This maps to the R(Retrieve) part of the CRUD operation.  This will be sued to retrieve the 
        /// required data (representation of data) from the remote resource
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetYouTube(Guid rssid)
        {
            Guid theguid = rssid;
            StringBuilder sb = new StringBuilder();
            YouTube.RSS myyoutube = new YouTube.RSS(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), theguid);
            String imgPath = myyoutube.Icon.Replace("~", "");

            sb.Append("<header style='padding-left:25px;padding-right:25px;'>");
            sb.Append(String.Format("<img src='{0}' style='margin-top:-20px;margin-left:-20px;position:relative;float:left;' />", imgPath));
            sb.Append(string.Format("<h2>{0}</h2>", myyoutube.Name));
            sb.Append(string.Format("<h3>{0}</h3>", myyoutube.Description));
            sb.Append("</header>");
            sb.Append("<div class='rss' style='max-height:500px;overflow:auto;'>");
            //sb.Append(myyoutube.MergeChannelItemsToHTML(feedcount: 6));
            sb.Append(myyoutube.MergeChannelItemsToHTML(width: "300", feedcount: 5));
            sb.Append("</div>");

            return Ok(sb.ToString());
        }
    }
}