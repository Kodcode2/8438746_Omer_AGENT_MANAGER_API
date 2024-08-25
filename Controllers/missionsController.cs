using Agent_Management_Server.Connect;
using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Agent_Management_Server.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agent_Management_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class missionsController : ControllerBase
    {
        private readonly MyServiceAgent _service_agent;
        private readonly Mission_Menager_service _service_Mission;
        private readonly MyServiceTarget _service_target;
        private readonly Dbcontext _dbcontext;

        public missionsController(Dbcontext dbcontext , Iservic<Agent> service_agent, Iservic<Mission> service_Mission, Iservic<Target> service_target)
        {
            _service_agent = service_agent as MyServiceAgent;
            _service_Mission = service_Mission as Mission_Menager_service;
            _service_target = service_target as MyServiceTarget;
            this._dbcontext = dbcontext;
        }
        [HttpGet]
        public async Task<IActionResult> Get_aal_mission() 
        {
            var res =  _dbcontext.Mission.ToList();
            return StatusCode(200 , res);
        }


        [HttpPost("update")]
        public async Task<IActionResult> Run()
        {
           
            var res_agent =  await _service_agent.GetAgents_active();
            var resM =  _dbcontext.Mission.ToList();
            foreach (var mission in resM) 
            {
                Agent res =  _dbcontext.Agents.FirstOrDefault(a => a.AgentId == mission.agentID);
                Target res2 = _dbcontext.Targets.FirstOrDefault(a => a.Id == mission.targetID);
                var re = _service_Mission.Culculet_to_target(res, res2);
                if (re != null) 
                {
                    res.locationX += re.x;
                    res.locationY += re.y;
                    Console.WriteLine($"uuuuuuuuuuuuuuuuu{res.locationX} : {res.locationY} ");
                    _dbcontext.Update(res);
                    _dbcontext.SaveChanges();
                }
            }


            //var res_target = await _service_target.GetTarget_active();

            return new JsonResult(res_agent);

            //שליפה של הסוכנים שיש עליהם משימה לפי הסטוטס שלהם
            //
            //חישוב מרחק וכיוון 
            // ביצוע תזוזוה לכיון המטרה
            // עדכון זמן חיסול
          
            
        }

    }
}
