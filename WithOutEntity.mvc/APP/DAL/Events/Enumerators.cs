using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace EventMessage
{
    /// <summary>
    /// Event Message Action Result
    /// </summary>
    [Flags()]
    public enum EventResult
    {
        [XmlEnum("0")] OK = 0,
        [XmlEnum("1")] Cancel = 1,
        [XmlEnum("2")] Invalid = 2,
        //[XmlEnum("2")] Warning = 2,
        //[XmlEnum("3")] Successful = 3,
        //[XmlEnum("4")] Failed = 4,
        //[XmlEnum("5")] Opened = 5,
        //[XmlEnum("6")] Closed = 6,
        //[XmlEnum("9")] Complete = 9,
        //[XmlEnum("10")] InComplete = 10,
        //[XmlEnum("14")] UnAuthorized = 14,
        //[XmlEnum("15")] Accept = 15,
        //[XmlEnum("16")] Decline = 16,
        [XmlEnum("99")] Error = 99
    }

    /// <summary>
    /// Event Message Application level
    /// </summary>
    [Flags()]
    public enum EventLevel
    {
        [XmlEnum("0")] ApplicationLayer = 0,
        [XmlEnum("1")] DataAccessLayer = 1,
        [XmlEnum("2")] SQLServer = 2,
        [XmlEnum("3")] SystemIO = 3,
        [XmlEnum("4")] ThirdPartyControl = 4
    }

    /// <summary>
    /// Event Message Action being performed
    /// </summary>
    [Flags()]
    public enum EventAction
    {
        [XmlEnum("0")] Visit = 0,
        [XmlEnum("1")] Login = 1,
        [XmlEnum("2")] Logout = 2,
        [XmlEnum("3")] Register = 3
        //[XmlEnum("1")] Insert = 0,
        //[XmlEnum("2")] Edit = 1,
        //[XmlEnum("3")] Delete = 2,
        //[XmlEnum("4")] Print = 3,
        //[XmlEnum("5")] Login = 4,
        //[XmlEnum("6")] Logout = 5,
        //[XmlEnum("7")] InternetBrowser = 6,
        //[XmlEnum("8")] Disclaimer = 7,
        //[XmlEnum("9")] View = 8,
        //[XmlEnum("10")] Load = 9,

        //[XmlEnum("20")] ContactCard = 20,
        //[XmlEnum("21")] ContactCardInsert = 21,
        //[XmlEnum("22")] ContactCardUpdate = 22,
        //[XmlEnum("23")] ContactCardDelete = 23,
    }
}