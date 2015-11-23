using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace YouTube
{
    public struct YouTubeGuid
    { 
        public String Tag;
        public String Video;
        public String Year;

        /// <summary>
        /// Summary:
        ///   Parse the YouTube defined Guid into it individual elements
        /// 
        /// Notes: 
        ///   format is tag:youtube.com,2008:video:nfkYL2hfSQU
        /// </summary>
        /// <param name="guid">YouTube defined guid</param>
        public void parseYouTubeGuid(string guid)
        {   
            //this a little tricky as : is the delimiter for element and values
            //so we find the first occence of the tag and video and get 
            //the substring to the next occurence of the :
            int tagindex = guid.IndexOf("tag:") + "tag:".Length;
            int videoindex = guid.IndexOf("video:") + "video:".Length;
            string tagelement = guid.Substring(tagindex, guid.Length - tagindex);
            string videoelement = guid.Substring(videoindex, guid.Length - videoindex);

            tagindex = tagelement.IndexOf(":");
            videoindex = videoelement.IndexOf(":");
            tagelement = tagelement.Substring(0, (tagindex <= 0) ? tagelement.Length : tagindex);
            videoelement = videoelement.Substring(0, (videoindex <= 0) ? videoelement.Length : videoindex);

            Tag = tagelement;
            Video = videoelement;
            Year = "";
        }
    }


    [Flags()]
    public enum YouTubeType
    {
        [XmlEnum("0")]
        Video = 0,
        [XmlEnum("1")]
        Channel = 1,
        [XmlEnum("2")]
        Playlist = 2
    }
}
