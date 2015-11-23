using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace ProjectMgmt
{
    [Flags()]
    public enum SortDirection
    {
        [XmlEnum("0")]
        Default = 0,
        [XmlEnum("1")]
        Ascending = 1,
        [XmlEnum("2")]
        Descending = 2
    }

    public enum Priority
    {
        [XmlEnum("0")] Low = 0, 
        [XmlEnum("1")] Nomal = 1, 
        [XmlEnum("2")] Medium = 2, 
        [XmlEnum("3")] High = 3
    }
}