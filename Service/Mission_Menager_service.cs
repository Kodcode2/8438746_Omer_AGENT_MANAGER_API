using Agent_Management_Server.Connect;
using Agent_Management_Server.Interface;
using Agent_Management_Server.models;

namespace Agent_Management_Server.Service
{
    public class Mission_Menager_service : Iservic<Mission>
    {
        private readonly Dbcontext _dbcontext;

        public Mission_Menager_service(Dbcontext dbcontext)
        {
            this._dbcontext = dbcontext;
        }
        public void consoltry() 
        {
            Console.WriteLine("ggggggggggggggggggggggg");
        }
    }
}
