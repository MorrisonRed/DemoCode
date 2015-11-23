using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

using CustomSecurity;

namespace democode.mvc.Models
{
    public class UserModels
    {
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "UID")]
        public Guid UID { get; set; }
        [Display(Name = "APPID")]
        public Guid AppID { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Is Anonymous")]
        public bool IsAnonymous { get; set; }
        [Display(Name = "Last Activity Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy H:mm tt}")]
        public DateTime LastActivityDate { get; set; }
        [Display(Name = "Is Authenticated")]
        public bool IsAuthenticated { get; set; }

        public UserDemographics Demographics {get; set; }
        public MembershipUser Membership { get; set; }
        public Role Role { get; set; } 
    }
}