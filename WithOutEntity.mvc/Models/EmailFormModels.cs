using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace democode.mvc.Models
{
    public class EmailFormModels
    {
        [Required, Display(Name = "Your name")]
        public string FromName { get; set; }
        [Required, Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }
        [Required, Display(Name = "Subject")]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}