using Agent_Management_Server.Connect;
using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Agent_Management_Server.Service
{
    public class Mission_Menager_service : Iservic<Mission>
    {
        public Dictionary<string, Location> My_dict2 = new Dictionary<string, Location>() {
                                  { "nw",new Location(){ x = -1 ,y = -1 } },
                                  { "n",new Location(){ x = 0 ,y = -1 } },
                                  { "ne",new Location(){ x = 1 ,y = -1 } },
                                  { "w",new Location(){ x = -1 ,y = 0 } },
                                  { "e",new Location(){ x = 1 ,y = 0 } },
                                  { "s",new Location(){ x = 0 ,y = 1 } },
                                  { "sw",new Location(){ x = -1 ,y = 1 } },
                                  { "se",new Location(){ x = 1 ,y = 1 } },
                                  { "non",new Location(){ x = 0 ,y = 0 } }};

        private readonly Dbcontext _dbcontext;

        public Mission_Menager_service(Dbcontext dbcontext)
        {
            this._dbcontext = dbcontext;
        }           

        public async void Get_options_agent(Agent agent) 
        {
            if (agent.status == status_enum_agent.Active)
            {
                Console.WriteLine("agent alredy");
                throw new Exception("agent alredy");
            }
            var option = this._dbcontext.Targets.ToList();          
            foreach (var target in option) 
            {                           
                    var res = Math.Sqrt(Math.Pow(target.locationX - agent.locationX, 2) + Math.Pow(target.locationY - agent.locationY, 2));
                    Console.WriteLine(res);
                    double restime = res / 5;
                    int timeremaining = (int)restime;
                    if (res <= 35 ) 
                    {
                        Mission mission = new Mission()
                            {
                                agentID = agent.AgentId,
                                targetID = target.Id,
                                Timeremaining = timeremaining,
                            };
                        this._dbcontext.Mission.Add(mission);
                        target.status = status_enum_target.busy;
                        _dbcontext.Targets.Update(target);
                        agent.status = status_enum_agent.busy;
                        _dbcontext.Agents.Update(agent);
                    }                    
            }
            _dbcontext.SaveChanges();        
        }

        public async void Get_options_target(Target target)
        {
            if (target.status  != status_enum_target.busy)
            {
                var option = this._dbcontext.Agents.ToList();
                foreach (var agent in option)
                {                    
                    var res = Math.Sqrt(Math.Pow(target.locationX - agent.locationX, 2) + Math.Pow(target.locationY - agent.locationY, 2));
                    Console.WriteLine(res);
                    double restime = res / 5;
                    int timeremaining = (int)restime;
                    if (res <= 200)
                    {
                        Mission mission = new Mission()
                        {
                            agentID = agent.AgentId,
                            targetID = target.Id,
                            Timeremaining = timeremaining
                        };
                        this._dbcontext.Mission.Add(mission);
                        agent.status = status_enum_agent.Active;
                        _dbcontext.Agents.Update(agent);
                    }                    
                }
                target.status = status_enum_target.busy;
                _dbcontext.Targets.Update(target);
                _dbcontext.SaveChanges();
            }
        }

        public  Location Culculet_to_target(Agent agent , Target target) 
        {
            var distanceX = target.locationX - agent.locationX;
            var distanceY = target.locationY - agent.locationY;
            if (distanceX > 0 && distanceY > 0)
            {
                return My_dict2["se"];
            }
            if (distanceX < 0 && distanceY > 0)
            {
                return My_dict2["sw"];
            }
            if (distanceX > 0 && distanceY < 0) 
            {
                return My_dict2["ne"]; 
            }
            if (distanceX < 0 && distanceY < 0) 
            {
                return My_dict2["nw"];
            }
            if (distanceX > 0 && distanceY == 0)
            {
                return My_dict2["e"];
            }
            if (distanceX == 0 && distanceY > 0)
            {
                return My_dict2["s"];
            }
            if (distanceX < 0 && distanceY == 0)
            {
                return My_dict2["w"];
            }
            if (distanceX == 0 && distanceY < 0)
            {
                return My_dict2["n"];
            }
            else 
            {
                return My_dict2["non"];
            }
        }

        public bool The_target_was_eliminated(Agent agent, Target target) 
        {
            if (target.locationX == agent.locationX && agent.locationY == target.locationY) 
            {
                return true;
            }
            return false;
        }
    }
}
