using System;
using System.IO;
using System.Text;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Data.Common;
using System.Collections;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace YouTube
{
    /// <summary>
    /// Summary description for Item
    /// </summary>
    [DataObject]
    [Serializable]
    [XmlRoot("item")]
    public class Item : IComparable
    {
        /// <summary>
        /// Summary: 
        ///   Get/Set the YouTube Item Guid
        /// 
        /// Note:
        ///   format is tag:youtube.com,2008:video:nfkYL2hfSQU
        /// </summary>
        [XmlElement(ElementName = "guid")]
        public String YouTubeGuid { get; set; }
        /// <summary>
        /// Get/Set the YouTube Item's published date
        /// </summary>
        [XmlElement(ElementName = "pubDate")]
        public string PublishedDate { get; set; }
        /// <summary>
        /// Get/Set YouTube Item Category
        /// </summary>
        [XmlElement(ElementName = "category")]
        public string Category { get; set; }
        /// <summary>
        /// Get/Set the RSS item title
        /// </summary>
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        /// <summary>
        /// Get/Set the RSS items description 
        /// </summary>
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        /// <summary>
        /// Get/Set the RSS item link
        /// </summary>
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        /// <summary>
        /// Get/Set the YouTube Item's Author
        /// </summary>
        [XmlElement(ElementName = "author")]
        public string Author { get; set; }
        /// <summary>
        /// Summary:
        ///   Get/Set the default image url for this video
        ///   
        /// Notes: 
        ///   YouTube videos are in the following format http://i.ytimg.com/vi/[[videoid]]/default.jpg
        /// </summary>
        [XmlElement(ElementName = "img")]
        public string Image { get; set; }
        /// <summary>
        /// Get the Publish date as date time
        /// </summary>
        [XmlIgnore]
        public DateTime DateTimePublished
        {
            get
            {
                // Allow a leading space in the date string. 
                // Wed, 19 Nov 2014 17:29:42 +0000
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime dateValue;

                //string x = PublishedDate.Substring(0, PublishedDate.Length - 4);
                try
                {
                    if (DateTime.TryParseExact(PublishedDate, "ddd, d MMM yyyy hh:mm:ss tt zzz", enUS,
                                            DateTimeStyles.AdjustToUniversal, out dateValue))
                    {
                        //Fri, 5 Dec 2014 12:13:28 PM EST
                        return dateValue; 
                    }
                    else if (DateTime.TryParseExact(PublishedDate, "ddd, d MMM yyyy HH:mm:ss zzz", enUS,
                                            DateTimeStyles.AdjustToUniversal, out dateValue))
                    {

                        //Fri, 5 Dec 2014 19:13:28 +0000
                        return dateValue;
                    }
                    else if (DateTime.TryParseExact(PublishedDate.Substring(0, PublishedDate.Length - 4), "ddd, d MMM yyyy HH:mm:ss", enUS,
                                            DateTimeStyles.AdjustToUniversal, out dateValue))
                    {
                        //attempt to remove string time zone (idiots at cbc)
                        //Fri, 5 Dec 2014 19:13:28 EST
                        return dateValue;
                    }
                    else
                    {
                        dateValue = Convert.ToDateTime(PublishedDate);
                        return dateValue;
                    }                   
                }
                catch
                {
                    return default(DateTime);
                }
            }
        }

        /// <summary>
        /// Get Last Error Thrown by Object
        /// </summary>
        [XmlIgnore]
        public static Exception GetLastError { get; set; }

        #region Constructors and Destructors
        /// <summary>
        /// Instanicate New RSS Item
        /// </summary>
        public Item()
        {
            SetBase();
        }
        /// <summary>
        /// Set My Base to string.Empty
        /// </summary>
        private void SetBase()
        {
            YouTubeGuid = string.Empty;
            PublishedDate = string.Empty;
            Category = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            Link = string.Empty;
            Author = string.Empty;
            Image = string.Empty;   
        }
        /// <summary>
        /// Set My Base to values of data
        /// </summary>
        /// <param name="data">as Item</param>
        /// <returns></returns>
        protected bool SetBase(Item data)
        {
            try
            {
                YouTubeGuid = data.YouTubeGuid;
                PublishedDate = data.PublishedDate;
                Category = data.Category;
                Title = data.Title;
                Description = data.Description;
                Link = data.Link;
                Author = data.Author;
                Image = data.Image;

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
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Item i = obj as Item;
            if (i != null)
                return this.DateTimePublished.CompareTo(i.DateTimePublished);
            else
                throw new ArgumentException("Object is not an item");
        }
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

            Type type = typeof(Item);
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
        /// Return the description removing unnessary tags
        /// </summary>
        /// <returns></returns>
        public string ScrubDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Description.Trim());
            sb.Replace("<p></p>", "");

            //normalize breaks
            sb.Replace("<br></br>", "<br />");
            sb.Replace("<br /><br /><br />", "<br />");
            sb.Replace("<br /><br />", "<br />");
            //remove all inline style and class tags
            string cleaned = new Regex("style=\"[^\"]*\"").Replace(sb.ToString(), "");
            cleaned = new Regex("(?<=class=\")([^\"]*)\\babc\\w*\\b([^\"]*)(?=\")").Replace(cleaned, "$1$2");

            return sb.ToString();
            //return cleaned;
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
        /// Write Item to File
        /// </summary>
        /// <param name="xmlpath">as destination path and file name of Item</param>
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

                    xmlSer = new XmlSerializer(typeof(Item));
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
        /// Create Item from File
        /// </summary>
        /// <param name="xmlpath">as source location of Item</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool XMLDeserializeFromFile(string xmlpath)
        {
            try
            {
                FileStream fs = new FileStream(xmlpath, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(Item));
                Item output = (Item)xs.Deserialize(fs);
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
        /// <param name="data">as Item</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string XMLSerializeToString(Item data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Item));
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
                XmlSerializer xmlSer = new XmlSerializer(typeof(Item));
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
        /// Return Item from XML Serialized String
        /// </summary>
        /// <param name="data">as XML Serialized String of Item</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Item XMLDeserializeFromString(string data)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Item));
                MemoryStream ms = new MemoryStream();
                StringReader strReader = default(StringReader);
                StringReader string_reader = default(StringReader);
                strReader = new StringReader(data);
                string_reader = new StringReader(data);
                Item output = default(Item);
                output = (Item)xmlSer.Deserialize(string_reader);
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