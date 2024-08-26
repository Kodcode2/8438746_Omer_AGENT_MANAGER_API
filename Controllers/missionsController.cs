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
            var res = await _service_Mission.GetAllMission();
            return StatusCode(200, res);
        }        

        [HttpGet("{id}")]
        public async Task<IActionResult> Get_missionBY_id(int id)
        {
            var res = await _service_Mission.Get_Mission_by_id(id);
            return StatusCode(200, res);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Create_new_mission_for_active(int id, [FromBody] object obj ) 
        {
            try 
            {
                 _service_Mission.create_active_mission(id);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
            return StatusCode(200);
        }

        // הפונקציה שהיא מזיזה את הסוכנים לכיוון המטרה
         //ןבמקרה של פגיעה היא מעדכנת את הדברים הרלוונטים כמו סטטוסים וכדו
         //היה ניתן להשתמש פה בתור עבור המשימות ובכל פעם להוציא לתרד חדש ....לא עשיתי מפעת הזמן
        [HttpPost("update")]
        public async Task<IActionResult> Run()
        {           
            var res_agent =  await _service_agent.GetAgents_active();
            var resMissions =  _dbcontext.Mission.Where(a=> a.status== status_enum_mission.Active).ToList();
            foreach (var mission in resMissions) 
            {                 
                Agent? responsforAgent =  _dbcontext.Agents.FirstOrDefault(a => a.AgentId == mission.agentID);
                Target? resforTarget = _dbcontext.Targets.FirstOrDefault(a => a.Id == mission.targetID);
                var res =  _service_Mission.Culculet_to_target(responsforAgent, resforTarget);
                if (res != null) 
                {
                    responsforAgent.locationX += res.x;
                    responsforAgent.locationY += res.y;
                    _service_Mission.Culculet_to_Timeremaining(mission, responsforAgent, resforTarget);
                    Console.WriteLine($"res Changes {responsforAgent.locationX} : {responsforAgent.locationY}");
                    if (_service_Mission.The_target_was_eliminated(responsforAgent, resforTarget))
                        {
                            Console.WriteLine("boom");
                            mission.status = status_enum_mission.false_;
                            resforTarget.status = status_enum_target.eliminated;
                            responsforAgent.Amount_of_eliminations += 1;
                            responsforAgent.status = status_enum_agent.Dormant;
                            _dbcontext.Update(mission);
                            _dbcontext.Update(resforTarget);
                            _dbcontext.Update(responsforAgent);
                            _dbcontext.SaveChanges();
                    }
                    _dbcontext.Update(responsforAgent);
                    _dbcontext.SaveChanges();
                }
            }
            return StatusCode(200);           
        }


        [HttpGet("get_Missions")]
        public async Task<IActionResult> Get_aal_mission_for_view()
        {
            var res = _service_Mission.createModelMissin();
            if (res != null) 
            {
                return StatusCode(200,res);
            }
            return StatusCode(400);
        }

        [HttpGet("general")]
        public async Task<IActionResult> Get_aal_mission_general() 
        {
            int Sum_all_agent = _service_Mission.Sum_all_agent();
            int Sum_all_agent_active = _service_Mission.Sum_all_agent_active();
            int Sum_all_mission_false = _service_Mission.Sum_all_mission_false();
            int Sum_all_mission = _service_Mission.Sum_all_mission();
            int Sum_all_Targe = _service_Mission.Sum_all_Target();
            int Sum_all_Target_eliminated = _service_Mission.Sum_all_Target_eliminated();
            return StatusCode(200 , new
            {
                Sum_all_agent = Sum_all_agent,
                Sum_all_agent_active = Sum_all_agent_active,
                Sum_all_mission_false = Sum_all_mission_false,
                Sum_all_mission = Sum_all_mission,
                Sum_all_Targe = Sum_all_Targe,
                Sum_all_Target_eliminated = Sum_all_Target_eliminated
            });

        }
    }
}
