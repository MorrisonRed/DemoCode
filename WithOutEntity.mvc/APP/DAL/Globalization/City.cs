using System;
using System.IO;
using System.Xml;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using MySql.Data.MySqlClient; 

//==========================================================================================
//User OBJECT
//
//==========================================================================================
//<XmlRoot("city")> _
//<Serialization.XmlTypeAttribute(Namespace:="http://services.morrisonred.com"), _
//Serialization.XmlRootAttribute("morrisonred", [Namespace]:="http://services.morrisonred.com", IsNullable:=False)> _
namespace Globalization
{
    [DataObject]
    [XmlRoot("city")]
    [Serializable()]
    public class City
    {
        private Int16 _id;
        private string _countrycode;
        private string _name;
        private string _district;
        private Int32 _population;

        #region Constructors and Destructors
        /// <summary>
        /// Instanicate New City
        /// </summary>
        public City()
        {
            SetBase();
        }
        /// <summary>
        /// Set MyBase to string.empty
        /// </summary>
        private void SetBase()
        { 
        
        }
        #endregion 

        #region Functions and Sub Routines

        #endregion 
    }
}