using Agent_Management_Server.Connect;
using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Microsoft.EntityFrameworkCore;

namespace Agent_Management_Server.Service
{
    public class MyServiceAgent : Iservic<Agent>
    {
        private readonly Mission_Menager_service _service_Mission;
        private readonly Dbcontext _DBcontext;
       
        //public static List<Agent> Agents = new List<Agent>();

        public MyServiceAgent(Dbcontext dbcontext, Iservic<Mission> service_Mission) 
        {
            this._service_Mission = service_Mission as Mission_Menager_service;
            this._DBcontext = dbcontext;
        }

        public async Task AddNewAgent(Agent NewAgent)
        {
            //  חיבור לDB ןהכנסה לטבלה
            await _DBcontext.Agents.AddAsync(NewAgent);
            await _DBcontext.SaveChangesAsync();
            //Agents.Add(NewAgent);
        }
        public async Task<List<Agent>> GetAgents()
        {
            //  חיבור לDB בקשה לקבלה מה טבלה מה
            return  _DBcontext.Agents.ToList();
            //return Agents;
        }
        public async Task<List<Agent>> GetAgents_active()
        {
            //  חיבור לDB בקשה לקבלה מה טבלה מה
           return   _DBcontext.Agents.Where(a => a.status == status_enum_agent.busy).ToList();
            //return Agents;
        }
        public async Task<Agent> PutPinAgent(int id, Location Startlocation)
        {
            var agent = _DBcontext.Agents.FirstOrDefault(x => x.AgentId == id);
            if (agent != null || agent.status != status_enum_agent.Active)
            {
                agent.locationY = Startlocation.y;
                agent.locationX = Startlocation.x;
                Location location_ = new Location() { y = Startlocation.y,x = Startlocation.x };
                agent.location = location_;
                await _DBcontext.SaveChangesAsync();
                try 
                {
                    _service_Mission.Get_options_agent(agent);
                }
                catch (Exception e )
                {
                    Console.WriteLine(e.Message);
                }                
            }
            else
            {
                throw new Exception("no id is valid or this olredy");
            }
            return agent;
        }
        public async Task<Agent> Moveagent(int id,string newdirection)
        {
            var agent = _DBcontext.Agents.FirstOrDefault(x => x.AgentId == id);
            if (agent == null || agent.status == status_enum_agent.Active)
            {
                throw new Exception("no id is valid");
            }
            var res = _service_Mission.My_dict2[newdirection];
            if (res != null)
            {
                agent.locationX += res.x;
                agent.locationY += res.y;
            }           
            await _DBcontext.SaveChangesAsync();
            try
            {
                _service_Mission.Get_options_agent(agent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return agent;
        }
    }
}
