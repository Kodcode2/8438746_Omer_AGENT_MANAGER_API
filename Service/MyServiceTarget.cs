using Agent_Management_Server.Connect;
using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Microsoft.EntityFrameworkCore;

namespace Agent_Management_Server.Service
{
    public class MyServiceTarget : Iservic<Target>
    {
        private readonly Mission_Menager_service _service_Mission;
        private readonly Dbcontext _dbcontext;

        public MyServiceTarget(Dbcontext dbcontext, Iservic<Mission> service_Mission)
        {
            this._dbcontext = dbcontext;
            this._service_Mission = service_Mission as Mission_Menager_service;
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
        public async Task<List<Target>> GetTarget_active()
        {
            //  חיבור לDB בקשה לקבלה מה טבלה מה
            return _dbcontext.Targets.Where(a => a.status == status_enum_target.busy).ToList();
            //return Agents;
        }
        //public Target GetTargetById(int id)
        //{
        //    return Targets.FirstOrDefault(x => x.Id == id);
        //}
        public async Task<Target> PutPinTarget(int id, Location Startlocation)
        {
            var target = _dbcontext.Targets.FirstOrDefault(x => x.Id == id);
            if (target != null)
            {
                Location location_ = new Location()
                {
                    x = Startlocation.x,
                    y = Startlocation.y
                };
                target.locationY = Startlocation.y;
                target.locationX = Startlocation.x;
                target.location = location_;
                await _dbcontext.SaveChangesAsync();
                //_service_Mission.Get_options_target(target);
            }
            else
            {
                throw new Exception("no id is valid");
            }
            return target;
        }
        public async Task<Target> MoveTarget(int id , string newdirection)
        {

            var target = _dbcontext.Targets.FirstOrDefault(x => x.Id == id);
            if (target == null)
            {
                throw new Exception("no id is valid");
            }
            var res =  _service_Mission.My_dict2[newdirection];
            if (res != null) 
            {
                target.locationX += res.x;
                target.locationY += res.y;
            }
            //switch (newdirection)
            //{
            //    case status_enum_direction.nw:
            //        Console.WriteLine("You chose WEST.");
            //        target.locationX -= 1; 
            //        target.locationY -= 1;

            //        break;
            //    case status_enum_direction.n:
            //        Console.WriteLine("You chose Latte.");
            //        target.locationX += 0;
            //        target.locationY -= 1;
            //        break;
            //    case status_enum_direction.ne:
            //        Console.WriteLine("You chose NORTH.");
            //        target.locationX += 1;
            //        target.locationY -= 1;
            //        break;
            //    case status_enum_direction.w:
            //        Console.WriteLine("You chose EAST.");
            //        target.locationX -= 1;
            //        target.locationY += 0;
            //        break;
            // case status_enum_direction.e:
            //        Console.WriteLine("You chose EAST.");
            //        target.locationX += 1;
            //        target.locationY += 0;
            //        break;
            // case status_enum_direction.s:
            //        Console.WriteLine("You chose EAST.");
            //        target.locationX -= 0;
            //        target.locationY += 1;
            //        break;
            // case status_enum_direction.sw:
            //        Console.WriteLine("You chose EAST.");
            //        target.locationX -= 1;
            //        target.locationY += 1;
            //        break;
            // case status_enum_direction.se:
            //        Console.WriteLine("You chose EAST.");
            //        target.locationX += 1;
            //        target.locationY += 1;
            //        break;
            //    default:
            //        Console.WriteLine("Unknown  type.");
            //        break;
            //} 
            await _dbcontext.SaveChangesAsync();
            
            return target;           
            
        }


    }
}
