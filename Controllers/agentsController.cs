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
            return StatusCode(201 , new { id = newAgent.AgentId }) ;            
        }


        [HttpGet]
        public async Task<IActionResult> Get_Agents()
        {           
            var res =  await _service.GetAgents();            
            return StatusCode(200, res);                  
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
                agent = await _service.Moveagent(id, dirction["direction"]);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(agent);
        }
    }
}
