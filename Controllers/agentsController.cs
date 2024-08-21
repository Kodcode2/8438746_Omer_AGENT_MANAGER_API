using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Agent_Management_Server.Service;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agent_Management_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly MyServiceAgent _service;
        public static List<Agent> Agents = new List<Agent>();
        public AgentsController(Iservic<Agent> service) 
        {
            this._service = service as MyServiceAgent;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAgents([FromBody] Agent newAgent)
        {
            if (newAgent == null)
            {
                return BadRequest("Vehicle data is null");
            }            
            await _service.AddNewAgent(newAgent);
            
            return Ok();//צריך להחזיר את ID שנוצר
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
                _service.PutPinAgent(id, Startlocation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);                
            }
            return Ok();           
        }


        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(int id, [FromBody] status_enum_direction newdirection)
        {
            var Agent = Agents.FirstOrDefault(x => x.AgentId == id);
            if (Agent == null)
            {
                return BadRequest("is id is valid");
            }
            switch (newdirection)
            {
                case status_enum_direction.WEST:
                    Console.WriteLine("You chose WEST.");
                    Agent.location.x -= 1;
                    break;
                case status_enum_direction.NORTH:
                    Console.WriteLine("You chose Latte.");
                    Agent.location.y -= 1;
                    break;
                case status_enum_direction.SOUTH:
                    Console.WriteLine("You chose NORTH.");
                    Agent.location.y += 1;
                    break;
                case status_enum_direction.EAST:
                    Console.WriteLine("You chose EAST.");
                    Agent.location.x += 1;
                    break;
                default:
                    Console.WriteLine("Unknown coffee type.");
                    break;
            }
            return Ok(Agent);
        }
    }
}
