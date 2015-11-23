namespace WebAPI.Models
{
    public class Project
    {
        public string PID { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public string Caption { get; set; }
        public string EstimatedStartDate { get; set; }
        public string EstimatedEndDate { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualEndDate { get; set; }
        public string URL { get; set; }
        public string Organization { get; set; } 
    }
}