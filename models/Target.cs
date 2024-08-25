
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agent_Management_Server.models
{
    public class Target : Interface_Target_Agent
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
       public string? direction { get; set; }
        public string position { get; set; }
        public string photoUrl { get; set; }
        public status_enum_target status { get; set; }
        [NotMapped]
        public Location? location { get; set; }
        public int locationY { get; set; }
        public int locationX { get; set; }
    }
}
