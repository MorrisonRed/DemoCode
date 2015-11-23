using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace CustomSecurity
{
    [Flags()]
    public enum PasswordFormat
    {
        [XmlEnum("0")]
        PBKDF2 = 0,
    }
}