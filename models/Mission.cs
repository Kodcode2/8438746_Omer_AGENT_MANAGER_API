﻿using System.ComponentModel.DataAnnotations;

namespace Agent_Management_Server.models
{
    public class Mission
    {
        [Key]
        public int id {  get; set; }
        public int  targetID {  get; set; }
        public int agentID { get; set; }
    }
}
