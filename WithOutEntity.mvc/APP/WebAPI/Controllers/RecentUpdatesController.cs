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
using System.Xml;

using MySql.Data.MySqlClient;
using System.Text;
using System.Web;

namespace WebAPI.Controllers
{
    public class RecentUpdatesController : ApiController
    {
        List<WebAPI.Models.RecentUpdate> updates = new List<WebAPI.Models.RecentUpdate>();
       

        private WebAPI.Models.RecentUpdate DataRowToWebApiModel(DataRow dr)
        {
            WebAPI.Models.RecentUpdate update = new WebAPI.Models.RecentUpdate();
            try
            {
                if (dr["RUTitle"] != null) { update.Title = (String)dr["RUTitle"].ToString(); }
                if (dr["RUIcon"] != null) { update.Icon = (String)dr["RUIcon"].ToString(); }
                if (dr["RULink"] != null) { update.Link = (String)dr["RULink"].ToString(); }
                if (dr["RUComment"] != null) { update.Comments = (String)dr["RUComment"].ToString(); }
                if (dr["RUPubDate"] != null) { update.PubDate = Convert.ToDateTime(dr["RUPubDate"]).ToString("dd MMM yyyy"); }
                //if (dr["RUDesc"] != null) { update.Description = WebUtility.HtmlEncode((String)dr["RUDesc"].ToString()); }
                if (dr["RUDesc"] != null) { update.Description = (String)dr["RUDesc"].ToString(); }

                return update;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private WebAPI.Models.RecentUpdate XmlNodeToWebApiModel(XmlNode node)
        {
            WebAPI.Models.RecentUpdate news = new WebAPI.Models.RecentUpdate();
            try
            {
                if (node["title"] != null) { news.Title = (String)node["title"].InnerText; }
                if (node["icon"] != null) { news.Icon = (String)node["icon"].InnerText; }
                if (node["link"] != null) { news.Link = (String)node["link"].InnerText; }
                if (node["comment"] != null) { news.Comments = (String)node["comment"].InnerText; }
                if (node["pubDate"] != null) { news.PubDate = (String)node["pubDate"].InnerText; }
                if (node["description"] != null) { news.Description = (String)node["description"].InnerText; }
                return news;
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
        public IEnumerable<WebAPI.Models.RecentUpdate> GetRecentUpdates()
        {
            var updates = new List<WebAPI.Models.RecentUpdate>();

            //////Create the XmlDocument.
            //XmlDocument doc = new XmlDocument();
            //doc.Load(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/RecentUpdates.xml"));
            ////get all items
            //XmlNodeList elemList = doc.GetElementsByTagName("item");
            //for (int i = 0; i < elemList.Count; i++)
            //{
            //    var u = XmlNodeToWebApiModel(elemList[i]);
            //    if (u != null) updates.Add(u);
            //} 

            //conflict with Configuration namespace --- dumb dumb ian
            using (MySqlConnection sqlconn = new MySqlConnection(ConfigurationManager.ConnectionStrings["SystemDS"].ToString()))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_recentupdates`");
                        query.Append(" ORDER BY `RUPubDate` DESC;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"] == null && ds.Tables["tbl"].Rows.Count == 0) return updates;

                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            {
                                var u = DataRowToWebApiModel(dr);
                                if (u != null) updates.Add(u);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //return updates;
                    }
                }
            }

             return updates;
        }
    }
}