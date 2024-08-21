using Agent_Management_Server.Interface;
using Agent_Management_Server.models;

namespace Agent_Management_Server.Service
{
    public class MyServiceTarget : Iservic<Target>
    {
        public static List<Target> Targets = new List<Target>();
        public async Task AddNewTarget(Target NewTarget)
        {
            //  חיבור לDB ןהכנסה לטבלה
            //await _DBcontext.Agents.AddAsync(newVehicle);
            //_DBcontext.SaveChangesAsync();
            Targets.Add(NewTarget);
        }
        public async Task<List<Target>> GetTargets()
        {
            //  חיבור לDB בקשה לקבלה מה טבלה מה
            //return _DBcontext._Vehicles.ToList();
            return Targets;
        }
        public Target GetTargetById(int id)
        {
            return Targets.FirstOrDefault(x => x.Id == id);
        }
        public void PutPinTarget(int id, Location Startlocation)
        {
            var Target = Targets.FirstOrDefault(x => x.Id == id);
            if (Target != null)
            {
                Target.location.y = Startlocation.y;
                Target.location.x = Startlocation.x;
            }
            else
            {
                throw new Exception("no id is valid");
            }
        }


        
        




        
    }
}
