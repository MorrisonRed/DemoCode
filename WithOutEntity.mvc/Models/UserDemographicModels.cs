using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

using CustomSecurity;

namespace democode.mvc.Models
{
    public class UserDemographicModels
    {
        [Display(Name = "UID")]
        public Guid UID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Age")]
        public int Age { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Language")]
        public string Language { get; set; }
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Mobile Number")]
        public string MobilePhone { get; set; }
    }
}