using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agent_Management_Server.models
{
    public class Agent : Interface_Target_Agent
    {
        [Key]
        public int AgentId { get; set; }
        public string Name { get; set; }
        public string direction { get; set; }
        public status_enum_agent status { get; set; }
        [NotMapped]
        public Location? location { get; set; }

        public int? locationY {  get; set; }
        public int? locationX {  get; set; }
    }
}
