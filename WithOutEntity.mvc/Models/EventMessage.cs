using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace democode.mvc.Models
{
    public class EventMessage
    {
        [Display(Name="Event ID")]
        public int EventID { get; set; }
        [Display(Name="Level")]
        public int Level { get; set; }
        [Display(Name = "Action")]
        public int Action { get; set; }
        [Display(Name = "Result")]
        public int Result { get; set; }
        [Display(Name = "Application")]
        public string Application { get; set; }
        [Display(Name = "Application Version")]
        public string ApplicationVersion { get; set; }
        [Display(Name = "Operation Code")]
        public string OperationCode { get; set; }
        [Display(Name = "Keywords")]
        public string Keywords { get; set; }
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime EventDateTime { get; set; }
        [Display(Name = "UID")]
        public string UID { get; set; }
        [Display(Name = "IP")]
        public string IP { get; set; }
        [Display(Name = "URL")]
        public string URL { get; set; }

        //public string Data { get; set; }
    }
}