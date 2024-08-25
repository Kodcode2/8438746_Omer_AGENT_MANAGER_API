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
    public class AgentsController : ControllerBase
    {
        private readonly MyServiceAgent _service;
        private readonly Mission_Menager_service _service_Mission;


        public static List<Agent> Agents = new List<Agent>();
        public AgentsController(Iservic<Agent> service, Iservic<Mission> _service_Mission) 
        {
            this._service = service as MyServiceAgent;
            this._service_Mission = _service_Mission as Mission_Menager_service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAgents([FromBody] Agent newAgent)
        {
            if (newAgent == null)
            {
                return BadRequest("Vehicle data is null");
            }            
            await _service.AddNewAgent(newAgent);
            
            return StatusCode(201 , new { id = newAgent.AgentId }) ;//צריך להחזיר את ID שנוצר
            //return CreatedAtAction(nameof(GetById), new { id = newVehicle.Id, type = "vehicle" }, newVehicle);
        }


        [HttpGet]
        public async Task<IActionResult> Get_Agents()
        {           
            var res =  await _service.GetAgents();
            
            return StatusCode(200, res);            
            //return CreatedAtAction(nameof(GetById), new { id = newVehicle.Id, type = "vehicle" }, newVehicle);
        }


        [HttpPut("{id}/pin")]
        public async Task<IActionResult> PutPin( int id ,[FromBody] Location Startlocation)
        {
            try 
            {
                await _service.PutPinAgent(id, Startlocation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);                
            }
            return Ok();           
        }


        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(int id, [FromBody] Dictionary<string, string> dirction)
        {
            Agent agent;
            try
            {
                agent = await _service.MoveTarget(id, dirction["direction"]);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(agent);
        }
    }
}
