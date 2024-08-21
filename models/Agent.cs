using System.ComponentModel.DataAnnotations;

namespace Agent_Management_Server.models
{
    public class Agent : Interface_Target_Agent
    {
        [Key]
        public int AgentId { get; set; }
        public string Name { get; set; }
        public string direction { get; set; }
        public status_enum_agent status { get; set; }       
        public Location? location { get; set; }
    }
}
