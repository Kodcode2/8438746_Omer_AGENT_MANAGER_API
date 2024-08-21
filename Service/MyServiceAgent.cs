using Agent_Management_Server.Interface;
using Agent_Management_Server.models;

namespace Agent_Management_Server.Service
{
    public class MyServiceAgent : Iservic<Agent>
    {
        public static List<Agent> Agents = new List<Agent>();

        public async Task AddNewAgent(Agent NewAgent)
        {
            //  חיבור לDB ןהכנסה לטבלה
            //await _DBcontext.Agents.AddAsync(newVehicle);
            //_DBcontext.SaveChangesAsync();
            Agents.Add(NewAgent);
        }
        public async Task<List<Agent>> GetAgents()
        {
            //  חיבור לDB בקשה לקבלה מה טבלה מה
            //return _DBcontext._Vehicles.ToList();
            return Agents;
        }
        public Agent GetAgentById(int id)
        {
            return Agents.FirstOrDefault(x => x.AgentId == id);
        }
        public void PutPinAgent(int id, Location Startlocation)
        {
            var agent = Agents.FirstOrDefault(x => x.AgentId == id);
            if (agent != null)
            {
                agent.location.y = Startlocation.y;
                agent.location.x = Startlocation.x;
            }
            else
            {
                throw new Exception("no id is valid");
            }
        }
    }
}
