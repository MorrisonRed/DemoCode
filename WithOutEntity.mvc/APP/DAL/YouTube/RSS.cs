using System;
using System.IO;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Collections;
using MySql.Data.MySqlClient; 
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Security.Cryptography;

using Google.Apis.Discovery;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTube
{
    /// <summary>
    /// RSS Feeds
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("rss")]
    public class RSS
    {

        /// <summary>
        /// Get/Set the RSS unique identifier
        /// </summary>
        [XmlElement(ElementName = "rrsid")]
        public Guid RSSID { get; set; }
        /// <summary>
        /// Get/Set the Name assigned to the RSS feed
        /// </summary>
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        /// <summary>
        /// Get/Set the url to the assoicated RSS Icon
        /// </summary>
        [XmlElement(ElementName = "icon")]
        public string Icon { get; set; }
        /// <summary>
        /// Get/Set the description for the RSS Feed
        /// </summary>
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Get/Set the list of channels assoicated with RSS Feed
        /// </summary>
        [XmlArray("channels")]
        [XmlArrayItem("channel")]
        public List<Channel> Channels { get; set; }

        /// <summary>
        /// Get last error thrown by object
        /// </summary>
        [XmlIgnore]
        public static Exception GetLastError { get; set; }

        #region Constructors and Destructors
        /// <summary>
        /// Instanicate new RSS feed
        /// </summary>
        public RSS()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate New RSS Feed
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="prssid">as RSS's unique identifier</param>
        public RSS(string pconstring, Guid prssid)
        {
            SetBase();
            Load(pconstring, prssid);
        }
        /// <summary>
        /// Instanicate new RSS Feed from [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        internal RSS(string pconstring, System.Data.DataRow dr)
        {
            try
            {
                if (dr["RSSID"] != System.DBNull.Value) { RSSID = new Guid(dr["RSSID"].ToString()); }
                if (dr["RSSName"] != System.DBNull.Value) { Name = (String)dr["RSSName"]; }
                if (dr["RSSIcon"] != System.DBNull.Value) { Icon = (String)dr["RSSIcon"]; }
                if (dr["RSSDescription"] != System.DBNull.Value) { Description = (String)dr["RSSDescription"]; }
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set My Base values to String.Empty
        /// </summary>
        private void SetBase()
        {
            Name = string.Empty;
            Icon = string.Empty;
            Description = string.Empty;
            Channels = new List<Channel>();
        }
        /// <summary>
        /// Set My Base to values of [dr]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        /// <returns></returns>
        internal Boolean SetBase(string pconstring, System.Data.DataRow dr)
        {
            try
            {
                if (dr["RSSID"] != System.DBNull.Value) { RSSID = new Guid(dr["RSSID"].ToString()); }
                if (dr["RSSName"] != System.DBNull.Value) { Name = (String)dr["RSSName"]; }
                if (dr["RSSIcon"] != System.DBNull.Value) { Icon = (String)dr["RSSIcon"]; }
                if (dr["RSSDescription"] != System.DBNull.Value) { Description = (String)dr["RSSDescription"]; }
                
                //get channels
                if (RSSID != null && RSSID != Guid.Empty) { Channels = Channel.ListFor(pconstring, RSSID); }

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set My Base to values of data
        /// </summary>
        /// <param name="data">as rss feed</param>
        /// <returns></returns>
        protected Boolean SetBase(RSS data)
        {
            try
            {
                RSSID = data.RSSID;
                Name = data.Name;
                Icon = data.Icon;
                Description = data.Description;

                Channels = data.Channels;

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        #endregion

        #region Functions and Sub Routines
        public override string ToString()
        {
            return "RSS";
        }
        /// <summary>
        /// Return true if the string is numeric
        /// </summary>
        /// <param name="stringToTest">as string to test</param>
        /// <returns></returns>
        public static bool IsNumeric(string stringToTest)
        {
            int num;
            return int.TryParse(stringToTest, out num);
        }
        /// <summary>
        /// Return true if column exists in table
        /// </summary>
        /// <param name="col">as column name</param>
        /// <param name="table">as data table</param>
        /// <returns></returns>
        private bool ColumnExists(string col, DataTable table)
        {
            bool flag;
            IEnumerator enumerator = table.Columns.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    if (((DataColumn)enumerator.Current).ColumnName != col)
                    {
                        continue;
                    }
                    flag = true;
                    return flag;
                }
                return false;
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return flag;
        }
        /// <summary>
        /// Return true if column existing in datarow
        /// </summary>
        /// <param name="col">as column name</param>
        /// <param name="dr">as data row</param>
        /// <returns></returns>
        private bool ColumnExists(string col, DataRow dr)
        {
            if (dr.Table.Columns.Contains(col))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Convert Object to type string
        /// </summary>
        /// <param name="value">value to convert to string</param>
        /// <returns></returns>
        private string DBValueToString(object value)
        {
            if (value != System.DBNull.Value) { return value.ToString(); }
            return "";
        }
        /// <summary>
        /// Return current project as Data Table
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            Type type = typeof(RSS);
            var properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                try
                {
                    info.GetValue(this, null);
                    dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                    dt.Rows[0][info.Name] = info.GetValue(this, null);
                }
                catch
                {

                }
            }

            return dt;
        }
        /// <summary>
        /// Return YouTube for [rssid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="prssid">as youtub feed's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_youtube]</TableName>
        internal static RSS getYouTube(string pconstring, Guid prssid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_youtube "));
                        query.Append(string.Format(" WHERE RSSID=?RSSID; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?RSSID", MySqlDbType.VarChar, 50)).Value = prssid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return null;
                            }

                            return new RSS(pconstring, ds.Tables["tbl"].Rows[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Load RSS Feed for [rssid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="prssid">as RSS Feed's unique identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_youtube]</TableName>
        public bool Load(string pconstring, Guid prssid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append(string.Format("SELECT * FROM vw_youtube "));
                        query.Append(string.Format(" WHERE RSSID=?RSSID; "));
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?RSSID", MySqlDbType.VarChar, 50)).Value = prssid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return false;
                            }

                            return SetBase(pconstring, ds.Tables["tbl"].Rows[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Add YouTube Feed to system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[youtube]</TableName>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addYouTube", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        //reset rss feed id
                        RSSID = Guid.NewGuid();

                        sqlcomm.Parameters.Add(new MySqlParameter("?prssid", MySqlDbType.VarChar, 50)).Value = RSSID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 50)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?picon", MySqlDbType.VarChar, 125)).Value = Icon.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarChar, 125)).Value = Description.Trim();
                        
                        //if the return value is not a guid then there was an error in the stored procedures
                        string s = sqlcomm.ExecuteScalar().ToString();
                        Guid g;
                        try
                        {
                            g = new Guid(s);
                            return true;
                        }
                        catch (FormatException ex)
                        {
                            GetLastError = new Exception(s);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Update YouTube feed in system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[youtube]</TableName>
        public Boolean Update(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateYouTube", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?prssid", MySqlDbType.VarChar, 50)).Value = RSSID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pname", MySqlDbType.VarChar, 50)).Value = Name.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?picon", MySqlDbType.VarChar, 125)).Value = Icon.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarChar, 125)).Value = Description.Trim();

                        sqlcomm.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Remove YouTube feed from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[youtube]</TableName>
        public Boolean Delete(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteYouTube", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?prssid", MySqlDbType.VarChar, 50)).Value = RSSID.ToString();
                        sqlcomm.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// Remove Youtube feed from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="prssid">as youtube's identifier</param>
        /// <returns></returns>
        /// <TableName>[youtube]</TableName>
        public static bool Delete(string pconstring, Guid prssid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteYouTube", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?prssid", MySqlDbType.VarChar, 50)).Value = prssid.ToString();
                        sqlcomm.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Return Merge Channels into a single list of Items
        /// </summary>
        /// <returns></returns>
        public List<Item> MergeChannelItemsToList()
        {
            //if there are not channels then return empty list
            //this will avoid null errors when binding
            if (Channels == null || Channels.Count == 0)
            { return new List<Item>(); }


            //create data table to hold items
            DataTable dt = new DataTable();
            List<Item> lst = new List<Item>();

            foreach (Channel c in Channels)
            {
                //load items into channel
                c.getFeed();     
                //add items to lst
                foreach (Item i in c.Items)
                {
                    lst.Add(i);
                }
            }

            //for items by pulish date descending
            lst.Sort();
            lst.Reverse();

            return lst;
        }
        /// <summary>
        /// Return Merge Channels into a single list of Items
        /// </summary>
        /// <returns></returns>
        public string MergeChannelItemsToHTML(string sortColumn = "DatePublished", string sortDirection = "Descending", string width = "", string height = "", int feedcount = 5)
        {
            System.Text.StringBuilder sb;

            //if there are not channels then return empty list
            //this will avoid null errors when binding
            if (Channels == null || Channels.Count == 0)
            { return ""; }

            //create data table to hold items
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("chnltitle", typeof(String)));
            dt.Columns.Add(new DataColumn("chnllink", typeof(String)));
            dt.Columns.Add(new DataColumn("chnlfeed", typeof(String)));
            dt.Columns.Add(new DataColumn("chnlicon", typeof(String)));
            dt.Columns.Add(new DataColumn("chnldesc", typeof(String)));
            dt.Columns.Add(new DataColumn("chnllang", typeof(String)));
            dt.Columns.Add(new DataColumn("itemguid", typeof(String)));
            dt.Columns.Add(new DataColumn("itempubdate", typeof(String)));
            dt.Columns.Add(new DataColumn("itemcategory", typeof(String))); 
            dt.Columns.Add(new DataColumn("itemtitle", typeof(String)));
            dt.Columns.Add(new DataColumn("itemdesc", typeof(String)));
            dt.Columns.Add(new DataColumn("itemlink", typeof(String)));
            dt.Columns.Add(new DataColumn("itemauthor", typeof(String)));
            dt.Columns.Add(new DataColumn("itemimage", typeof(String)));
           
            dt.Columns.Add(new DataColumn("datepublish", typeof(DateTime)));

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyC0FhNIt602GR7lqicEpEVa-ZV6Itabx2s",
                ApplicationName = this.GetType().ToString()
            });
 
            var channelListRequest = youtubeService.Search.List("snippet");
            channelListRequest.MaxResults = 10;
            SearchListResponse channelListResult;

            var playListRequest = youtubeService.PlaylistItems.List("snippet");
            playListRequest.MaxResults = 10;
            PlaylistItemListResponse playListResult;

            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();
            StringBuilder sbChannels = new StringBuilder();
            StringBuilder sbPlayLists = new StringBuilder();

            try
            {
                foreach (Channel c in Channels)
                {
                    //load items into channel
                    switch (c.Type)
                    {
                        case YouTubeType.Channel:
                            //get list of videos from channel
                            channelListRequest.ChannelId = c.YouTubeID;
                            channelListResult = channelListRequest.Execute();
                            foreach (var item in channelListResult.Items)
                            {
                                dr = dt.NewRow();
                                dr["chnltitle"] = c.Title;
                                dr["chnllink"] = c.Link;
                                dr["chnlfeed"] = c.Feed;
                                dr["chnlicon"] = c.Icon;
                                dr["chnldesc"] = c.Description;
                                dr["chnllang"] = c.Language;
                                dr["itemguid"] = item.Id.VideoId;
                                dr["itempubdate"] = item.Snippet.PublishedAtRaw;
                                dr["itemcategory"] = "";
                                dr["itemtitle"] = item.Snippet.Title;
                                dr["itemdesc"] = item.Snippet.Description;
                                dr["itemlink"] = String.Format("https://www.youtube.com/embed/{0}?wmode=transparent", item.Id.VideoId);
                                dr["itemauthor"] = "";
                                dr["itemimage"] = String.Format("<img src='{0}' alt='{1}' />", item.Snippet.Thumbnails.Medium.Url, item.Snippet.Title);

                                dr["datepublish"] = item.Snippet.PublishedAt.GetValueOrDefault();

                                dt.Rows.Add(dr);
                            }

                            break;
                        case YouTubeType.Playlist:
                            //get list of videos from channel
                            playListRequest.PlaylistId = c.YouTubeID;
                            playListResult = playListRequest.Execute();
                            foreach (var item in playListResult.Items)
                            {
                                dr = dt.NewRow();
                                dr["chnltitle"] = c.Title;
                                dr["chnllink"] = c.Link;
                                dr["chnlfeed"] = c.Feed;
                                dr["chnlicon"] = c.Icon;
                                dr["chnldesc"] = c.Description;
                                dr["chnllang"] = c.Language;
                                dr["itemguid"] = item.Snippet.ResourceId.VideoId;
                                dr["itempubdate"] = item.Snippet.PublishedAt.GetValueOrDefault().ToString("d MMM yyyy");
                                dr["itemcategory"] = "";
                                dr["itemtitle"] = item.Snippet.Title;
                                dr["itemdesc"] = item.Snippet.Description;
                                dr["itemlink"] = String.Format("https://www.youtube.com/embed/{0}?wmode=transparent", item.Snippet.ResourceId.VideoId);
                                dr["itemauthor"] = "";
                                dr["itemimage"] = String.Format("<img src='{0}' alt='{1}' />", item.Snippet.Thumbnails.Medium.Url, item.Snippet.Title);

                                dr["datepublish"] = item.Snippet.PublishedAt.GetValueOrDefault();

                                dt.Rows.Add(dr);
                            }

                            break;
                    }
                }

                //sort data table by option provided
                DataView dv = dt.DefaultView;
                dv.Sort = "datepublish desc";
                dt = dv.ToTable();

                //evaluate size parameters
                if (!string.IsNullOrEmpty(width)) width = string.Format("width='{0}'", width);
                if (!string.IsNullOrEmpty(height)) height = string.Format("height='{0}'", height);
                if (string.IsNullOrEmpty(width) && string.IsNullOrEmpty(height)) width = "width='500'";

                //create html blocks
                sb = new System.Text.StringBuilder();
                sb.Append(string.Format("<div class='rss_channel' style='display:inline-block;'>"));
                sb.Append(string.Format("<ul style='position:relative;list-style:none;list-style-image:none;padding:0;margin:0;'>"));

                int index = 0;
                foreach (DataRow row in dt.Rows)
                {
                    //exit for loop when feed count is reached
                    if (index == feedcount) break;

                    string icon = row["chnlicon"].ToString();
                    if (!string.IsNullOrEmpty(icon) && icon.StartsWith("~")) icon = "" + icon.Substring(1, icon.Length - 1);
                    string chnURL = row["chnllink"].ToString();
                    if (!chnURL.StartsWith("http://") && !chnURL.StartsWith("https://")) chnURL = "http://" + chnURL;

                    sb.Append(string.Format("<li class='title' style='display:inline-block;'>"));
                    sb.Append(string.Format("<div style='display:block;position:relative;'>"));
                    sb.Append(string.Format("<a href='{0}' style='display:inline-block;' target='_blank'>", chnURL));
                    sb.Append(string.Format("<img alt='image' style='padding:0px;float:left;' height='32' src='{0}' />", icon));
                    sb.Append(string.Format("<span style='padding:4px;display:inline-block;float:left;height:32px;'>{0}</span>", row["chnltitle"].ToString()));
                    sb.Append(string.Format("</a>"));
                    sb.Append(string.Format("</div>"));
                    sb.Append(string.Format("<a class='youtube' href='https://www.youtube.com/embed/{2}?wmode=transparent' target='_blank'><img {0} {1} alt='' src='http://i.ytimg.com/vi/{2}/mqdefault.jpg'></a>", width, height, row["itemguid"]));
                    // small image - sb.Append(string.Format("<a class='youtube' href='http://www.youtube.com/watch?v={2}&feature=youtube_gdata' target='_blank'><img {0} {1} alt='' src='http://i.ytimg.com/vi/{2}/default.jpg'></a>", width, height, ytguid.Video));
                    sb.Append(string.Format("</li>"));

                    index++;
                }

                sb.Append(string.Format("<li style='clear:both;'></li>"));
                sb.Append(string.Format("</ul>"));
                sb.Append(string.Format("</div>"));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return "";
            }
            finally
            {
                dt.Dispose();
                sb = null;
                GC.Collect();
            }
        }
        /// <summary>
        /// Return the collection of Items to an list of embedded Videos
        /// </summary>
        /// <returns></returns>
        public string MergeChannelItemsToEmbededVideos(string sortColumn = "DatePublished", string sortDirection = "Descending", string width = "", string height="", int feedcount = 5)
        {
            System.Text.StringBuilder sb;
           
            //if there are not channels then return empty list
            //this will avoid null errors when binding
            if (Channels == null || Channels.Count == 0)
            { return ""; }

            //create data table to hold items
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("chnltitle", typeof(String)));
            dt.Columns.Add(new DataColumn("chnllink", typeof(String)));
            dt.Columns.Add(new DataColumn("chnlfeed", typeof(String)));
            dt.Columns.Add(new DataColumn("chnlicon", typeof(String)));
            dt.Columns.Add(new DataColumn("chnldesc", typeof(String)));
            dt.Columns.Add(new DataColumn("chnllang", typeof(String)));
            dt.Columns.Add(new DataColumn("itemguid", typeof(String)));
            dt.Columns.Add(new DataColumn("itempubdate", typeof(String)));
            dt.Columns.Add(new DataColumn("itemcategory", typeof(String)));
            dt.Columns.Add(new DataColumn("itemtitle", typeof(String)));
            dt.Columns.Add(new DataColumn("itemdesc", typeof(String)));
            dt.Columns.Add(new DataColumn("itemlink", typeof(String)));
            dt.Columns.Add(new DataColumn("itemauthor", typeof(String)));
            dt.Columns.Add(new DataColumn("itemimage", typeof(String)));

            dt.Columns.Add(new DataColumn("datepublish", typeof(DateTime)));

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyC0FhNIt602GR7lqicEpEVa-ZV6Itabx2s",
                ApplicationName = this.GetType().ToString()
            });

            var channelListRequest = youtubeService.Search.List("snippet");
            channelListRequest.MaxResults = 10;
            SearchListResponse channelListResult;

            var playListRequest = youtubeService.PlaylistItems.List("snippet");
            playListRequest.MaxResults = 10;
            PlaylistItemListResponse playListResult;

            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();
            StringBuilder sbChannels = new StringBuilder();
            StringBuilder sbPlayLists = new StringBuilder();

            try
            {
                foreach (Channel c in Channels)
                {
                    //load items into channel
                    switch (c.Type)
                    {
                        case YouTubeType.Channel:
                            //get list of videos from channel
                            channelListRequest.ChannelId = c.YouTubeID;
                            channelListResult = channelListRequest.Execute();
                            foreach (var item in channelListResult.Items)
                            {
                                dr = dt.NewRow();
                                dr["chnltitle"] = c.Title;
                                dr["chnllink"] = c.Link;
                                dr["chnlfeed"] = c.Feed;
                                dr["chnlicon"] = c.Icon;
                                dr["chnldesc"] = c.Description;
                                dr["chnllang"] = c.Language;
                                dr["itemguid"] = item.Id.VideoId;
                                dr["itempubdate"] = item.Snippet.PublishedAt.GetValueOrDefault().ToString("d MMM yyyy");
                                dr["itemcategory"] = "";
                                dr["itemtitle"] = item.Snippet.Title;
                                dr["itemdesc"] = item.Snippet.Description;
                                dr["itemlink"] = String.Format("https://www.youtube.com/embed/{0}?wmode=transparent", item.Id.VideoId);
                                dr["itemauthor"] = "";
                                dr["itemimage"] = String.Format("<img src='{0}' alt='{1}' />", item.Snippet.Thumbnails.Medium.Url, item.Snippet.Title);

                                dr["datepublish"] = item.Snippet.PublishedAt.GetValueOrDefault();

                                dt.Rows.Add(dr);
                            }

                            break;
                        case YouTubeType.Playlist:
                            //get list of videos from channel
                            playListRequest.PlaylistId = c.YouTubeID;
                            playListResult = playListRequest.Execute();
                            foreach (var item in playListResult.Items)
                            {
                                dr = dt.NewRow();
                                dr["chnltitle"] = c.Title;
                                dr["chnllink"] = c.Link;
                                dr["chnlfeed"] = c.Feed;
                                dr["chnlicon"] = c.Icon;
                                dr["chnldesc"] = c.Description;
                                dr["chnllang"] = c.Language;
                                dr["itemguid"] = item.Snippet.ResourceId.VideoId;
                                dr["itempubdate"] = item.Snippet.PublishedAtRaw;
                                dr["itemcategory"] = "";
                                dr["itemtitle"] = item.Snippet.Title;
                                dr["itemdesc"] = item.Snippet.Description;
                                dr["itemlink"] = String.Format("https://www.youtube.com/embed/{0}?wmode=transparent", item.Snippet.ResourceId.VideoId);
                                dr["itemauthor"] = "";
                                dr["itemimage"] = String.Format("<img src='{0}' alt='{1}' />", item.Snippet.Thumbnails.Medium.Url, item.Snippet.Title);

                                dr["datepublish"] = item.Snippet.PublishedAt.GetValueOrDefault();

                                dt.Rows.Add(dr);
                            }

                            break;
                    }
                }


                //sort data table by option provided
                DataView dv = dt.DefaultView;
                dv.Sort = "datepublish desc";
                dt = dv.ToTable();

                //evaluate size parameters
                if (!string.IsNullOrEmpty(width)) width = string.Format("width='{0}'", width);
                if (!string.IsNullOrEmpty(height)) height = string.Format("height='{0}'", height);
                if (string.IsNullOrEmpty(width) && string.IsNullOrEmpty(height)) width = "width='500'";

                //create html blocks
                sb = new System.Text.StringBuilder();
                sb.Append(string.Format("<div class='rss_channel' style='display:inline-block;'>"));
                sb.Append(string.Format("<ul style='position:relative;list-style:none;list-style-image:none;padding:0;margin:0;'>"));

                //only return record for the defined feed count
                int index = 0;
                foreach (DataRow row in dt.Rows)
                {
                    //exit for loop when feed count is reached
                    if (index == feedcount) break;

                    YouTubeGuid ytguid = new YouTubeGuid();
                    ytguid.parseYouTubeGuid(row["itemguid"].ToString());

                    string icon = row["chnlicon"].ToString();
                    if (!string.IsNullOrEmpty(icon) && icon.StartsWith("~")) icon = "" + icon.Substring(1, icon.Length - 1);
                    string chnURL = row["chnllink"].ToString();
                    if (!chnURL.StartsWith("http://") && !chnURL.StartsWith("https://")) chnURL = "http://" + chnURL;

                    sb.Append(string.Format("<li class='title' style='display:inline-block;'>"));
                    sb.Append(string.Format("<div style='display:block;position:relative;'>"));
                    sb.Append(string.Format("<a href='{0}' style='display:inline-block;' target='_blank'>", chnURL));
                    sb.Append(string.Format("<img alt='image' style='padding:0px;float:left;' height='32' src='{0}' />", icon));
                    sb.Append(string.Format("<span style='padding:4px;display:inline-block;float:left;height:32px;'>{0}</span>", row["chnltitle"].ToString()));
                    sb.Append(string.Format("</a>"));
                    sb.Append(string.Format("</div>"));
                    sb.Append(string.Format("<iframe {0} {1} src='https://www.youtube.com/embed/{2}?wmode=transparent' style='z-index:1 !important;position:relative;' frameborder='0'  wmode='Opaque' allowfullscreen></iframe>", width, height, ytguid.Video));
                    //sb.Append(string.Format("<a href='http://www.youtube.com/watch?v={2}&feature=youtube_gdata' target='_blank'><img {0} {1} alt='' src='http://i.ytimg.com/vi/{2}/default.jpg'></a>", width, height, ytguid.Video));
                    sb.Append(string.Format("</li>"));

                    index++;
                }
                sb.Append(string.Format("<li style='clear:both;'></li>"));
                sb.Append(string.Format("</ul>"));
                sb.Append(string.Format("</div>"));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return "";
            }
            finally
            {
                dt.Dispose();
                sb = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// Return DataTable of all rss from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_youtube]</TableName>
        public static System.Data.DataTable dgRSS(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        string query = "";
                        query += "SELECT * FROM `vw_youtube`";
                        query += " ORDER BY `RSSName` ASC;";
                        sqlcomm.CommandText = query;

                        sqlconn.Open();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return new DataTable();
                            }

                            return ds.Tables["tbl"].Copy();
                        }
                    }
                    catch (Exception ex)
                    {
                        GetLastError = ex;
                        return null;
                    }
                }
            }
        }
        #endregion

        #region Encryption and Decryption
        public static string Left(string text, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "length must be > 0");
            else if (length == 0 || text.Length == 0)
                return "";
            else if (text.Length <= length)
                return text;
            else
                return text.Substring(0, length);
        }
        /// <summary>
        /// Decrypt Elements
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool DecryptElements()
        {
            try
            {
                //If Not String.IsNullOrEmpty(_ds) Then _ds = DecryptElement(_ds)
                //If Not String.IsNullOrEmpty(_cat) Then _cat = DecryptElement(_cat)
                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Encrypt Elements 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool EncryptElements()
        {
            try
            {
                //If Not String.IsNullOrEmpty(_ds) Then _ds = DecryptElement(_ds)
                //If Not String.IsNullOrEmpty(_cat) Then _cat = DecryptElement(_cat)
                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Returns Encrypted String using Password Encryption Key 
        /// </summary>
        /// <param name="text">plain text</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string EncryptElement(string text)
        {
            return Encrypt(text, "&%#@?,:*");
        }
        /// <summary>
        /// Returns Decrypted String using Password Encryption Key
        /// </summary>
        /// <param name="text">encrypted text</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string DecryptElement(string text)
        {
            return Decrypt(text, "&%#@?,:*");
        }
        /// <summary>
        /// Returns Encypted String [text] using specified Encryption Key [EncryKey]
        /// </summary>
        /// <param name="text">plain text</param>
        /// <param name="EncryKey">encryption key</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string Encrypt(string text, string EncryKey)
        {
            byte[] byKey = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };

            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(Left(EncryKey, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Returns Decrypted String [strWord] using specified Encryption Key [strEncryKey]
        /// </summary>
        /// <param name="text">encrypted text</param>
        /// <param name="EcryKey">decription key</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string Decrypt(string text, string EcryKey)
        {
            byte[] byKey = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
            byte[] inputByteArray = new byte[text.Length + 1];

            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(Left(EcryKey, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(text);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;

                return encoding.GetString(ms.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion

        #region XML Functions
        /// <summary>
        /// Write RSS to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of RSS</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLSerializeToFile(string xmlpath)
        {
            try
            {
                if (EncryptElements())
                {
                    FileStream fs = default(FileStream);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    XmlSerializer xmlSer = default(XmlSerializer);

                    xmlSer = new XmlSerializer(typeof(RSS));
                    fs = new FileStream(xmlpath, FileMode.Create, FileAccess.Write);
                    xmlSer.Serialize(fs, this);
                    fs.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Create RSS from File
        /// </summary>
        /// <param name="xmlpath">as source location of RSS</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(RSS));
                RSS output = (RSS)xs.Deserialize(fs);
                if (SetBase(output))
                {
                    return DecryptElements();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Return Serialized String version of [data] Object
        /// </summary>
        /// <param name="data">as RSS</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(RSS data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(RSS));
                MemoryStream ms = new MemoryStream();
                StreamReader strReader = default(StreamReader);
                string output = null;

                xmlSer.Serialize(ms, data);
                ms.Position = 0;
                strReader = new StreamReader(ms);
                output = strReader.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return null;
            }
        }
        /// <summary>
        /// Return Serialized String
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private string XMLSerializeToString()
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(RSS));
                MemoryStream ms = new MemoryStream();
                StreamReader strReader = default(StreamReader);
                string output = null;

                xmlSer.Serialize(ms, this);
                ms.Position = 0;
                strReader = new StreamReader(ms);
                output = strReader.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return null;
            }
        }
        /// <summary>
        /// Return RSS from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of RSS</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static RSS XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(RSS));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                RSS output = default(RSS);
                output = (RSS)xmlSer.Deserialize(string_reader);
                return output;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return null;
            }
        }
        #endregion
    }
}