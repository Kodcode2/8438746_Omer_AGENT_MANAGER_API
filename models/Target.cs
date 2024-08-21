
using System.ComponentModel.DataAnnotations;

namespace Agent_Management_Server.models
{
    public class Target : Interface_Target_Agent
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
       public string? direction { get; set; }
        public string? candidate { get; set; }
        public status_enum_target status { get; set; }
       public Location location { get; set; }
    }
}
