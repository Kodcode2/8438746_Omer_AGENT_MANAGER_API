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
        //public Agent GetAgentById(int id)
        //{
        //    return Agents.FirstOrDefault(x => x.AgentId == id);
        //}
        public async Task<Agent> PutPinAgent(int id, Location Startlocation)
        {
            var agent = _DBcontext.Agents.FirstOrDefault(x => x.AgentId == id);
            if (agent != null)
            {
                agent.locationY = Startlocation.y;
                agent.locationX = Startlocation.x;
                Location location_ = new Location() { y = Startlocation.y,x = Startlocation.x };
                agent.location = location_;
                await _DBcontext.SaveChangesAsync();
                _service_Mission.Get_options_agent(agent);
            }
            else
            {
                throw new Exception("no id is valid");
            }
            return agent;
        }
        public async Task<Agent> MoveTarget(int id,string newdirection)
        {
            var agent = _DBcontext.Agents.FirstOrDefault(x => x.AgentId == id);
            if (agent == null)
            {
                throw new Exception("no id is valid");
            }
            var res = _service_Mission.My_dict2[newdirection];
            if (res != null)
            {
                agent.locationX += res.x;
                agent.locationY += res.y;
            }
            //switch (dirction)
            //{
            //    case status_enum_direction.nw:
            //        Console.WriteLine("You chose WEST.");
            //        agent.locationX -= 1;
            //        agent.locationY -= 1;

            //        break;
            //    case status_enum_direction.n:
            //        Console.WriteLine("You chose Latte.");
            //        agent.locationX += 0;
            //        agent.locationY -= 1;
            //        break;
            //    case status_enum_direction.ne:
            //        Console.WriteLine("You chose NORTH.");
            //        agent.locationX += 1;
            //        agent.locationY -= 1;
            //        break;
            //    case status_enum_direction.w:
            //        Console.WriteLine("You chose EAST.");
            //        agent.locationX -= 1;
            //        agent.locationY += 0;
            //        break;
            //    case status_enum_direction.e:
            //        Console.WriteLine("You chose EAST.");
            //        agent.locationX += 1;
            //        agent.locationY += 0;
            //        break;
            //    case status_enum_direction.s:
            //        Console.WriteLine("You chose EAST.");
            //        agent.locationX -= 0;
            //        agent.locationY += 1;
            //        break;
            //    case status_enum_direction.sw:
            //        Console.WriteLine("You chose EAST.");
            //        agent.locationX -= 1;
            //        agent.locationY += 1;
            //        break;
            //    case status_enum_direction.se:
            //        Console.WriteLine("You chose EAST.");
            //        agent.locationX += 1;
            //        agent.locationY += 1;
            //        break;
            //    default:
            //        Console.WriteLine("Unknown  type.");
            //        break;
            //}
            await _DBcontext.SaveChangesAsync();
            return agent;


        }
    }
}
