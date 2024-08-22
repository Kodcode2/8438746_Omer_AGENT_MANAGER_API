using Agent_Management_Server.Connect;
using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Microsoft.EntityFrameworkCore;

namespace Agent_Management_Server.Service
{
    public class MyServiceTarget : Iservic<Target>
    {
        private readonly Dbcontext _dbcontext;

        public MyServiceTarget(Dbcontext dbcontext)
        {
            this._dbcontext = dbcontext;
        }
        public async Task AddNewTarget(Target NewTarget)
        {
            //  חיבור לDB ןהכנסה לטבלה
            await _dbcontext.Targets.AddAsync(NewTarget);
            await _dbcontext.SaveChangesAsync();
            //Targets.Add(NewTarget);
        }
        public async Task<List<Target>> GetTargets()
        {
            //  חיבור לDB בקשה לקבלה מה טבלה מה
            return  _dbcontext.Targets.ToList();
            //return Targets;
        }
        //public Target GetTargetById(int id)
        //{
        //    return Targets.FirstOrDefault(x => x.Id == id);
        //}
        public void PutPinTarget(int id, Location Startlocation)
        {
            var target = _dbcontext.Targets.FirstOrDefault(x => x.Id == id);
            if (target != null)
            {
                target.locationY = Startlocation.y;
                target.locationX = Startlocation.x;
                _dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("no id is valid");
            }
        }
        public async Task<Target> MoveTarget(int id , status_enum_direction newdirection)
        {
            var target = _dbcontext.Targets.FirstOrDefault(x => x.Id == id);
            if (target == null)
            {
                throw new Exception("no id is valid");
            }
            switch (newdirection)
            {
                case status_enum_direction.WEST:
                    Console.WriteLine("You chose WEST.");
                    target.locationX -= 1;                    
                    break;
                case status_enum_direction.NORTH:
                    Console.WriteLine("You chose Latte.");
                    target.locationY -= 1;
                    break;
                case status_enum_direction.SOUTH:
                    Console.WriteLine("You chose NORTH.");
                    target.locationY += 1;
                    break;
                case status_enum_direction.EAST:
                    Console.WriteLine("You chose EAST.");
                    target.locationX += 1;
                    break;
                default:
                    Console.WriteLine("Unknown  type.");
                    break;
            } 
            await _dbcontext.SaveChangesAsync();
            
            return target;
            
            
        }


    }
}
