using Agent_Management_Server.Connect;
using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

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



        public async Task<List<Mission>> GetAllMission()
        {
            //  חיבור לDB בקשה לקבלה מה טבלה מה
            return await _dbcontext.Mission.ToListAsync();
            //return Mission;
        }
        public async Task<Mission> Get_Mission_by_id(int id)
        {
            var res = await _dbcontext.Mission.FirstOrDefaultAsync(a => a.agentID == id);
            //  חיבור לDB בקשה לקבלה מה טבלה מה
            return res;
            //return Mission by id;
        }

        //  הפונקציה שיוצרת כרטיס משימות עבור הסוכנים הרלוונטים-> 
        public async void Get_options_agent(Agent agent)
        {
            if (agent.status == status_enum_agent.Active )
            {
                Console.WriteLine("agent alredy");
                throw new Exception("agent alredy");
            }
            var option = this._dbcontext.Targets.ToList();          
            foreach (var target in option) 
            {
                    var r = _dbcontext.Mission.FirstOrDefault(a => a.targetID == target.Id && a.agentID == agent.AgentId);
                    if (r == null)
                    {
                        var res = Math.Sqrt(Math.Pow(target.locationX - agent.locationX, 2) + Math.Pow(target.locationY - agent.locationY, 2));
                        Console.WriteLine(res);
                        double restime = res / 5;
                        int timeremaining = (int)restime;
                        if (res <= 200 ) 
                        // יוצר כרטיס משימה->
                        {
                            Mission mission = new Mission()
                                {
                                    agentID = agent.AgentId,
                                    targetID = target.Id,
                                    Timeremaining = timeremaining,
                                    status = status_enum_mission.Waiting_for_the_command
                                };
                            this._dbcontext.Mission.Add(mission);
                            target.status = status_enum_target.busy;
                            _dbcontext.Targets.Update(target);
                            agent.status = status_enum_agent.busy;
                            _dbcontext.Agents.Update(agent);
                        }                   
                        
                    }
            }
            _dbcontext.SaveChanges();        
        }

        public List<ModelMissin> createModelMissin() 
        {
            List<ModelMissin> modelMissins = new List<ModelMissin>();
            var resMissions = _dbcontext.Mission.Where(a => a.status != status_enum_mission.false_).ToList();
            foreach (var mission in resMissions)
            {
                Agent? responsforAgent = _dbcontext.Agents.FirstOrDefault(a => a.AgentId == mission.agentID);
                Target? resforTarget = _dbcontext.Targets.FirstOrDefault(a => a.Id == mission.targetID);
                ModelMissin modelMissin = new ModelMissin()
                {
                    mission = mission,
                    agent = responsforAgent,
                    target = resforTarget
                };
                modelMissins.Add(modelMissin);
            }
            return modelMissins;
        }
        public void Culculet_to_Timeremaining(Mission mission , Agent agent, Target target)
        {
            var res = Math.Sqrt(Math.Pow(target.locationX - agent.locationX, 2) + Math.Pow(target.locationY - agent.locationY, 2));
            Console.WriteLine(res);
            double restime = res / 5;
            int timeremaining = (int)restime;
            mission.Timeremaining = timeremaining;
            if (timeremaining <= 0) 
            {
                mission.status = status_enum_mission.false_;
                agent.status = status_enum_agent.Dormant;
                target.status = status_enum_target.eliminated;
            }
            _dbcontext.Update(mission);
            _dbcontext.Update(agent);
            _dbcontext.Update(target);
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

        public void create_active_mission(int id) 
        {
            var resMissions = _dbcontext.Mission.FirstOrDefault(a => a.id == id );
            resMissions.status = status_enum_mission.Active;
            _dbcontext.Update(resMissions);
            _dbcontext.SaveChanges();

            var agent = _dbcontext.Agents.FirstOrDefault(a => a.AgentId == resMissions.agentID);
            if (agent != null) 
            {
                agent.status = status_enum_agent.Active;
                _dbcontext.Update(agent);
                _dbcontext.SaveChanges();
            }
            var target = _dbcontext.Targets.FirstOrDefault(a => a.Id == resMissions.targetID);
            if (target != null) 
            {
                target.status = status_enum_target.busy;
                _dbcontext.Update(target);
                _dbcontext.SaveChanges();
            }
            _dbcontext.SaveChanges();
        }



        //service for genral viwe -> MVC 

        public  int  Sum_all_agent() 
        {
            int resAgent = _dbcontext.Agents.ToList().Count;
            return resAgent;
            
        }
        public int Sum_all_agent_active()
        {
            int res_agent_active = _dbcontext.Agents.Where(a => a.status == status_enum_agent.Active).ToList().Count;
            return res_agent_active;
        }
        public int Sum_all_agent_busy()
        {            
            int res_agent_busy = _dbcontext.Agents.Where(a => a.status == status_enum_agent.busy).ToList().Count;
            return res_agent_busy;
        }
        public int Sum_all_Target()
        {
            int resTarget = _dbcontext.Targets.ToList().Count;
            return resTarget;
        }
        public int Sum_all_Target_eliminated()
        {
            int res_Target_eliminated = _dbcontext.Targets.Where(a => a.status == status_enum_target.eliminated).ToList().Count;
            return res_Target_eliminated;
        }
        public int Sum_all_mission()
        {
            int resMission = _dbcontext.Mission.ToList().Count;
            return resMission;
        }
        public int Sum_all_mission_false()
        {
            int res_Target_eliminated = _dbcontext.Mission.Where(a => a.status == status_enum_mission.false_ ).ToList().Count;
            return res_Target_eliminated;
        }
    }
}
