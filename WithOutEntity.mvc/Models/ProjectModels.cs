using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace democode.mvc.Models
{
    public class ProjectModels
    {
        [Display(Name = "PID")]
        public Guid PID { get; set; }
        [Display(Name = "Icon")]
        public string Icon { get; set; }
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Folder")]
        public string Folder { get; set; }
        [Display(Name = "Caption")]
        public string Caption { get; set; }

        [Display(Name = "Estimated Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? EstimatedStartDate { get; set; }
        [Display(Name = "Estimated End Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? EstimatedEndDate { get; set; }
        [Display(Name = "Actual Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ActualStartDate { get; set; }
        [Display(Name = "Actual End Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ActualEndDate { get; set; }
        [Display(Name = "Url")]
        public String URL { get; set; }
        [Display(Name = "Organization")]
        public String Organization { get; set; }

        public List<String> ProjectImages { get; set; }
    }
}