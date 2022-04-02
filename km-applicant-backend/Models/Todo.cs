using System;
namespace km_applicant_backend.Models
{
    public class Todo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string descriptions { get; set; }
        public DateTime expirationDate { get; set; }
        public int percentageOfCompletion { get; set; }

    }
}
