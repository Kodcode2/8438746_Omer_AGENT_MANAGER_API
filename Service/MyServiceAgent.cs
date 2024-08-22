using Agent_Management_Server.Connect;
using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Microsoft.EntityFrameworkCore;

namespace Agent_Management_Server.Service
{
    public class MyServiceAgent : Iservic<Agent>
    {
        private readonly Dbcontext _DBcontext;

        public static List<Agent> Agents = new List<Agent>();

        public MyServiceAgent(Dbcontext dbcontext) 
        {
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
        public Agent GetAgentById(int id)
        {
            return Agents.FirstOrDefault(x => x.AgentId == id);
        }
        public void PutPinAgent(int id, Location Startlocation)
        {
            var agent = _DBcontext.Agents.FirstOrDefault(x => x.AgentId == id);
            if (agent != null)
            {
                agent.locationY = Startlocation.y;
                agent.locationX = Startlocation.x;
                _DBcontext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("no id is valid");
            }
        }
        public async Task<Agent> MoveTarget(int id, status_enum_direction newdirection)
        {
            var agent = _DBcontext.Agents.FirstOrDefault(x => x.AgentId == id);
            if (agent == null)
            {
                throw new Exception("no id is valid");
            }
            switch (newdirection)
            {
                case status_enum_direction.WEST:
                    Console.WriteLine("You chose WEST.");
                    agent.locationX -= 1;
                    break;
                case status_enum_direction.NORTH:
                    Console.WriteLine("You chose Latte.");
                    agent.locationY -= 1;
                    break;
                case status_enum_direction.SOUTH:
                    Console.WriteLine("You chose NORTH.");
                    agent.locationY += 1;
                    break;
                case status_enum_direction.EAST:
                    Console.WriteLine("You chose EAST.");
                    agent.locationX += 1;
                    break;
                default:
                    Console.WriteLine("Unknown  type.");
                    break;
            }
            await _DBcontext.SaveChangesAsync();
            return agent;


        }
    }
}
