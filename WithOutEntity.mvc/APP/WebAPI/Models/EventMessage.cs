namespace WebAPI.Models
{
    public class EventMessage
    {
        public int EventID { get; set; }
        public int Level { get; set; }
        public int Action { get; set; }
        public int Result { get; set; }
        public string Application { get; set; }
        public string ApplicationVersion { get; set; }
        public string OperationCode { get; set; }
        public string Keywords { get; set; }
        public string EventDateTime { get; set; }
        public string UID { get; set; }
        public string IP { get; set; }
        public string URL { get; set; }

        //public string Data { get; set; }

    }
}