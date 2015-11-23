using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace Configuration
{
    [Flags()]
    public enum ServiceProvider
    {
        [XmlEnum("0")]
        SQL = 0,
        [XmlEnum("1")]
        MySQL = 1,
    }
}