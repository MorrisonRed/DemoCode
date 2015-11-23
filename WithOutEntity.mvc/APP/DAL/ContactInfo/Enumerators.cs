using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace ContactInfo
{
    [Flags()]
    public enum EmailAddressType
    {
        [XmlEnum("0")] Business = 0,
        [XmlEnum("1")] Home = 1,
        [XmlEnum("2")] Other = 2,
        [XmlEnum("99")] Undefined = 99
    }

    [Flags()]
    public enum PhoneNumberType
    {
        [XmlEnum("0")] Business = 0,
        [XmlEnum("1")] Home = 1,
        [XmlEnum("2")] Mobile = 2,
        [XmlEnum("3")] TollFree = 3, 
        [XmlEnum("4")] Fax = 4, 
        [XmlEnum("5")] Other = 5, 
        [XmlEnum("99")] Undefined = 99
    }

    [Flags()]
    public enum AddressType
    {
        [XmlEnum("0")] Business = 0, 
        [XmlEnum("1")] Home = 1, 
        [XmlEnum("2")] Other = 2, 
        [XmlEnum("3")] Shipping = 3,
        [XmlEnum("4")] Mailing = 4, 
        [XmlEnum("99")] Undefined = 99
    }

    [Flags()]
    public enum ContactCardType
    {
        [XmlEnum("0")] Person = 0, 
        [XmlEnum("1")] External = 1, 
        [XmlEnum("2")] Client = 2, 
        [XmlEnum("3")] Supplier = 3, 
        [XmlEnum("4")] Vendor = 4, 
        [XmlEnum("5")] DistributionGroup = 5, 
        [XmlEnum("6")] SecurityGroup = 6,
        [XmlEnum("7")] Organization = 7, 
        [XmlEnum("8")] FAQ = 8, 
        [XmlEnum("99")] Undefined = 99
    }
}
