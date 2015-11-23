using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Web;
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

namespace RSS
{
    /// <summary>
    /// RSS Channel
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("channel")]
    public class Channel
    {

        #region Public Properties
        /// <summary>
        /// Get/Set Channel's unique identifier
        /// </summary>
        [XmlElement(ElementName = "chnlid")]
        public Guid ChannelID { get; set; }
        /// <summary>
        /// Get/Set the unique channel code routing
        /// </summary>
        [XmlElement(ElementName = "code")]
        public String Code { get; set; }
        /// <summary>
        /// Get/Set the channel's assoicated RSS Feed identifier
        /// </summary>
        [XmlElement(ElementName = "rssid")]
        public Guid RSSID { get; set; }
        /// <summary>
        /// Get/Set Channel's Title
        /// </summary>
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        /// <summary>
        /// Get/Set the Channel's associated link 
        /// </summary>
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        /// <summary>
        /// Get/Set Channels RSS feed URL
        /// </summary>
        [XmlElement(ElementName = "feed")]
        public string Feed { get; set; }
        /// <summary>
        /// Get/Set the assoicated Icon to Channel
        /// </summary>
        [XmlElement(ElementName = "icon")]
        public string Icon { get; set; }
        /// <summary>
        /// Get/Set the Channel description
        /// </summary>
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        /// <summary>
        /// Get/Set the language code that he channel is presented in
        /// </summary>
        [XmlElement(ElementName = "language")]
        public string Language { get; set; }

        /// <summary>
        /// Get/Set the Items assoicated with the RSS channel
        /// </summary>
        [XmlArray("items")]
        [XmlArrayItem("item")]
        public List<Item> Items { get; set; }

        /// <summary>
        /// Get Last Error Thrown by Object
        /// </summary>
        [XmlIgnore]
        public static Exception GetLastError {get; set;}
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Instanicate New RSS Channel
        /// </summary>
        public Channel()
        {
            SetBase();
        }
        /// <summary>
        /// Instanicate New RSS Channel
        /// </summary>
        /// <param name="pconstring">as data source connection</param>
        /// <param name="pchnlid">as channel's unique identifer</param>
        public Channel(string pconstring, Guid pchnlid)
        {
            SetBase();
            Load(pconstring, pchnlid);
        }
        /// <summary>
        /// Instanicate New RSS Channel for [code]
        /// </summary>
        /// <param name="pconstring">as data source connection</param>
        /// <param name="pcode">as channel's code</param>
        public Channel(string pconstring, String pcode)
        {
            SetBase();
            Load(pconstring, pcode);
        }
        /// <summary>
        /// Instanicate new Channel from dr
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        internal Channel(string pconstring, DataRow dr)
        {
            try
            {
                // deserialize object first as table value superceed the values in the xml
                if (dr["ChnlItemsXML"] != System.DBNull.Value && !string.IsNullOrEmpty(dr["ChnlItemsXML"].ToString())) 
                { this.XMLDeserialize((String)dr["ChnlItemsXML"]); }

                if (dr["ChnlID"] != System.DBNull.Value) { ChannelID = new Guid(dr["ChnlID"].ToString()); }
                if (dr["RSSID"] != System.DBNull.Value) { RSSID = new Guid(dr["RSSID"].ToString()); }
                if (dr["ChnlCode"] != System.DBNull.Value) { Code = (String)dr["ChnlCode"].ToString(); }
                if (dr["ChnlTitle"] != System.DBNull.Value) { Title = (String)dr["ChnlTitle"]; }
                if (dr["ChnlLink"] != System.DBNull.Value) { Link = (String)dr["ChnlLink"]; }
                if (dr["ChnlFeed"] != System.DBNull.Value) { Feed = (String)dr["ChnlFeed"]; }
                if (dr["ChnlDescription"] != System.DBNull.Value) { Description = (String)dr["ChnlDescription"]; }
                if (dr["ChnlLang"] != System.DBNull.Value) { Language = (String)dr["ChnlLang"]; }
                if (dr["ChnlIcon"] != System.DBNull.Value) { Icon = (String)dr["ChnlIcon"]; }
                
                
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set My Base to string.Empty
        /// </summary>
        private void SetBase()
        {
            Title = string.Empty;
            Link = string.Empty;
            Feed = string.Empty;
            Icon = string.Empty;
            Description = string.Empty;
        }
        /// <summary>
        /// Instanicate new Channel from dr
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="dr">as data row</param>
        internal bool SetBase(string pconstring, DataRow dr)
        {
            try
            {
                // deserialize object first as table value superceed the values in the xml
                if (dr["ChnlItemsXML"] != System.DBNull.Value && !string.IsNullOrEmpty(dr["ChnlItemsXML"].ToString()))
                { this.XMLDeserialize((String)dr["ChnlItemsXML"]); }

                if (dr["ChnlID"] != System.DBNull.Value) { ChannelID = new Guid(dr["ChnlID"].ToString()); }
                if (dr["RSSID"] != System.DBNull.Value) { RSSID = new Guid(dr["RSSID"].ToString()); }
                if (dr["ChnlCode"] != System.DBNull.Value) { Code = (String)dr["ChnlCode"].ToString(); }
                if (dr["ChnlTitle"] != System.DBNull.Value) { Title = (String)dr["ChnlTitle"]; }
                if (dr["ChnlLink"] != System.DBNull.Value) { Link = (String)dr["ChnlLink"]; }
                if (dr["ChnlFeed"] != System.DBNull.Value) { Feed = (String)dr["ChnlFeed"]; }
                if (dr["ChnlDescription"] != System.DBNull.Value) { Description = (String)dr["ChnlDescription"]; }
                if (dr["ChnlLang"] != System.DBNull.Value) { Language = (String)dr["ChnlLang"]; }
                if (dr["ChnlIcon"] != System.DBNull.Value) { Icon = (String)dr["ChnlIcon"]; }

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                throw ex;
            }
        }
        /// <summary>
        /// Set My Base to values of [data]
        /// </summary>
        /// <param name="data">as RSS channel</param>
        /// <returns></returns>
        private bool SetBase(Channel data)
        {
            try
            {
                ChannelID = data.ChannelID;
                RSSID = data.RSSID;
                Code = data.Code;
                Title = data.Title;
                Link = data.Link;
                Feed = data.Feed;
                Icon = data.Icon;
                Description = data.Description;
                Language = data.Language;

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
            return Title;
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
        /// Load Channel for [chnlid]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pchnlid">as channel's unique identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_channels_rss]</TableName>
        public bool Load(string pconstring, Guid pchnlid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_rss_channels`");
                        query.Append(" WHERE ChnlID=?ChnlID; ");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?ChnlID", MySqlDbType.VarChar, 50)).Value = pchnlid.ToString();

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
                        return true;
                    }
                }
            }
        }
        /// <summary>
        /// Load Channel for [code]
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pcode">as channel's code</param>
        /// <returns></returns>
        /// <TableName>[vw_channels_rss]</TableName>
        public bool Load(string pconstring, String pcode)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_rss_channels`");
                        query.Append(" WHERE ChnlCode=?Code; ");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?Code", MySqlDbType.VarChar, 50)).Value = pcode.ToString();

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
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Add Channel to RSS Feed
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        public bool Add(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_addRSSChannel", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        //reset rss feed id
                        ChannelID = Guid.NewGuid();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pchnlid", MySqlDbType.VarChar, 50)).Value = ChannelID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?prssid", MySqlDbType.VarChar, 50)).Value = RSSID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcode", MySqlDbType.VarChar, 50)).Value = Code.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ptitle", MySqlDbType.VarChar, 50)).Value = Title.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?plink", MySqlDbType.VarChar, 125)).Value = Link.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pfeed", MySqlDbType.VarChar, 125)).Value = Feed.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?picon", MySqlDbType.VarChar, 125)).Value = Icon.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarChar, 255)).Value = Description.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?plang", MySqlDbType.VarChar, 10)).Value = Language.ToLower().Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pXML", MySqlDbType.VarString)).Value = this.XMLSerializeToString();

                        //if the return value is not a guid then there was an error in the stored procedures
                        string s = DBValueToString(sqlcomm.ExecuteScalar());
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
        /// Update Channel for RSS Feed
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        public bool Update(string pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_updateRSSChannel", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pchnlid", MySqlDbType.VarChar, 50)).Value = ChannelID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?prssid", MySqlDbType.VarChar, 50)).Value = RSSID.ToString();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pcode", MySqlDbType.VarChar, 50)).Value = Code.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?ptitle", MySqlDbType.VarChar, 50)).Value = Title.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?plink", MySqlDbType.VarChar, 125)).Value = Link.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pfeed", MySqlDbType.VarChar, 125)).Value = Feed.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?picon", MySqlDbType.VarChar, 125)).Value = Icon.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pdesc", MySqlDbType.VarChar, 255)).Value = Description.Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?plang", MySqlDbType.VarChar, 10)).Value = Language.ToLower().Trim();
                        sqlcomm.Parameters.Add(new MySqlParameter("?pXML", MySqlDbType.VarString)).Value = this.XMLSerializeToString();

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
        /// Remove Channel from RSS feed from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[rss_channels]</TableName>
        public Boolean Delete(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteRSSChannel", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pchnlid", MySqlDbType.VarChar, 50)).Value = ChannelID.ToString();
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
        /// Remove Channel from RSS feed from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="pchnlid">as channel's unique identifier</param>
        /// <returns></returns>
        /// <TableName>[rss_channels]</TableName>
        public static Boolean Delete(String pconstring, Guid pchnlid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("usp_deleteRSSChannel", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?pchnlid", MySqlDbType.VarChar, 50)).Value = pchnlid.ToString();
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
        /// Load RSS Feed into Channel items collection based on Feed URL
        /// </summary>
        /// <param name="maxRecords">number of items to return</param>
        /// <returns></returns>
        public bool getFeed(int maxRecords = 10)
        {
            try
            {
                //Loading My site RSS FEED, Change as per your need
                WebRequest MyRssRequest = WebRequest.Create(Feed);
                WebResponse MyRssResponse = MyRssRequest.GetResponse();
                Stream MyRssStream = MyRssResponse.GetResponseStream();

                XmlDocument MyRssDocument = new XmlDocument();
                MyRssDocument.Load(MyRssStream);

                //write content to file
                //string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, @"App_Data\myrss.xml");
                //MyRssDocument.Save(filePath);

                //get items node list
                XmlNodeList rssList = MyRssDocument.SelectNodes("rss/channel/item");

                Items = new List<Item>();
                Item it;

                // Iterate/Loop through RSS Feed items
                //for (int i = 0; i < rssList.Count; i++)
                for (int i = 0; i <= maxRecords; i++)
                {
                    it = new Item();

                    XmlNode node;
                    node = rssList.Item(i).SelectSingleNode("title");
                    if (node != null)
                        it.Title = node.InnerText;
                    else
                        it.Title = "";

                    node = rssList.Item(i).SelectSingleNode("link");
                    if (node != null)
                        it.Link = node.InnerText;
                    else
                        it.Link = "";

                    node = rssList.Item(i).SelectSingleNode("description");
                    if (node != null)
                        it.Description = node.InnerText;
                    else
                    {
                        it.Description = "";
                    }

                    node = rssList.Item(i).SelectSingleNode("comments");
                    if (node != null)
                        it.Comments = node.InnerText;
                    else
                    {
                        it.Comments = "";
                    }

                    node = rssList.Item(i).SelectSingleNode("pubDate");
                    if (node != null)
                        it.PublishedDate = node.InnerText;
                    else
                    {
                        it.PublishedDate = "";
                    }

                    Items.Add(it);
                }

                return true;
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Return the collection of Items in html
        /// </summary>
        /// <returns></returns>
        public string ItemsToHTML()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (Items != null && Items.Count > 0)
            {
                foreach (Item i in Items)
                {
                    sb.Append(string.Format("<div class='rss_channel' style='display:table;'>"));
                    sb.Append(string.Format("<div class='display:table-cell;'>"));
                    sb.Append(string.Format("<a href='{0}' target='_blank'><img alt='image' style='padding:4px;' height='32' src='{1}' /></a>",
                            "#", "./images/32/social-rss.png"));
                    sb.Append(string.Format("</div>"));
                    sb.Append(string.Format("<div class='rss_item' style='display:table-cell;'>"));
                    sb.Append(string.Format("<ul style='list-style:none;list-style-image:none;padding:0;margin:0;'>"));
                    sb.Append(string.Format("<li class='title' style='display:inline-block;'><a href='{0}' target='_blank'>{1}</a></li>", i.Link, i.Title));
                    sb.Append(string.Format("<li class='pubDate' style='display:inline-block;'>{0}</li>", i.PublishedDate));
                    sb.Append(string.Format("<li class='description'><p>{0}</p></li>", i.Description));
                    sb.Append(string.Format("</ul>"));
                    sb.Append(string.Format("</div>"));
                    sb.Append(string.Format("</div>"));
                }

            }
            return sb.ToString();
        }
        /// <summary>
        /// Return the collection of Items in html
        /// </summary>
        /// <param name="lst">as list of items to convert to HTML</param>
        /// <returns></returns>
        public static string ItemsToHTML(List<Item> lst)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (lst != null && lst.Count > 0)
            {
                foreach (Item i in lst)
                {
                    sb.Append(string.Format("<div class='rss_item' style='display:block;'>"));
                    sb.Append(string.Format("<ul style='list-style:none;list-style-image:none;padding:0;margin:0;'>"));
                    sb.Append(string.Format("<li class='title' style='display:block;'><a href='{0}' target='_blank'>{1}</a></li>", i.Link, i.Title));
                    sb.Append(string.Format("<li class='pubDate' style='display:block;'>{0}</li>", i.PublishedDate));
                    sb.Append(string.Format("<li class='description' style='display:block;'><p>{0}</p></li>", i.Description));
                    sb.Append(string.Format("</ul>"));
                    sb.Append(string.Format("</div>"));
                }

            }
            return sb.ToString();
        }

        /// <summary>
        /// Return all channels assoicated with rss feed [rssid]
        /// </summary>
        /// <param name="pconstring">as data source connections string</param>
        /// <param name="prssid">as rss feed's identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_channels_rss]</TableName>
        public static List<Channel> ListFor(string pconstring, Guid prssid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_rss_channels`");
                        query.Append(" WHERE RSSID=?RSSID ");
                        query.Append(" ORDER BY `ChnlTitle` ASC;");
                        sqlcomm.CommandText = query.ToString();

                        sqlconn.Open();

                        sqlcomm.Parameters.Add(new MySqlParameter("?RSSID", MySqlDbType.VarChar, 50)).Value = prssid.ToString();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(sqlcomm))
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            da.Fill(ds, "tbl");

                            List<Channel> lst = new List<Channel>();

                            if (ds.Tables["tbl"].Rows.Count == 0)
                            {
                                GetLastError = new Exception("no data found");
                                return lst;
                            }

                            foreach (DataRow dr in ds.Tables["tbl"].Rows)
                            { 
                                lst.Add(new Channel(pconstring, dr));
                            }
                            return lst;
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
        /// Return DataTable of all rss from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <returns></returns>
        /// <TableName>[vw_rss_channels]</TableName>
        public static System.Data.DataTable dgChannels(String pconstring)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_rss_channels`");
                        query.Append(" ORDER BY `ChnlTile` ASC;");
                        sqlcomm.CommandText = query.ToString();

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
        /// <summary>
        /// Return DataTable of all rss from system
        /// </summary>
        /// <param name="pconstring">as data source connection string</param>
        /// <param name="prssid">as rss feed's unique identifier</param>
        /// <returns></returns>
        /// <TableName>[vw_rss_channels]</TableName>
        public static System.Data.DataTable dgChannels(String pconstring, Guid prssid)
        {
            using (MySqlConnection sqlconn = new MySqlConnection(pconstring))
            {
                using (MySqlCommand sqlcomm = new MySqlCommand("", sqlconn))
                {
                    sqlcomm.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append("SELECT * FROM `vw_rss_channels`");
                        query.Append(" WHERE RSSID=?RSSID ");
                        query.Append(" ORDER BY `ChnlTitle` ASC;");
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
        /// Write Channel to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of Channel</param>
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

                    xmlSer = new XmlSerializer(typeof(Channel));
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
        /// Create Channel from File
        /// </summary>
        /// <param name="xmlpath">as source location of Channel</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(Channel));
                Channel output = (Channel)xs.Deserialize(fs);
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
        /// <param name="data">as Channel</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(Channel data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Channel));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(Channel));
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
        /// Load Channel from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of Channel</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserialize(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Channel));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                Channel output = default(Channel);
                output = (Channel)xmlSer.Deserialize(string_reader);
                return SetBase(output);
            }
            catch (Exception ex)
            {
                GetLastError = ex;
                return false;
            }
        }
        /// <summary>
        /// Return Channel from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of Channel</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Channel XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Channel));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                Channel output = default(Channel);
                output = (Channel)xmlSer.Deserialize(string_reader);
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