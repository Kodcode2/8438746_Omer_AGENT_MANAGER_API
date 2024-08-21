using Agent_Management_Server.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agent_Management_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class agentsController : ControllerBase
    {
        public static List<Agent> Agents = new List<Agent>();

        [HttpPost("agents")]
        public async Task<IActionResult> CreateAgents([FromBody] Agent newAgent)
        {
            if (newAgent == null)
            {
                return BadRequest("Vehicle data is null");
            }

            //await _DBcontext._Vehicles.AddAsync(newVehicle);

            //_DBcontext.SaveChangesAsync();
            //return _DBcontext._Vehicles.ToList();

            Agents.Add(newAgent);
            return Ok();
            //return CreatedAtAction(nameof(GetById), new { id = newVehicle.Id, type = "vehicle" }, newVehicle);
        }
        [HttpGet("agents")]
        public async Task<IActionResult> GetVehicle()
        {
            //var res = await _DBcontext._Vehicles.ToListAsync();

            //return _DBcontext._Vehicles.ToList();
            return StatusCode(200 , Agents.ToList());

            //Vehicles.Add(newVehicle);
            //return CreatedAtAction(nameof(GetById), new { id = newVehicle.Id, type = "vehicle" }, newVehicle);
        }
    }
}
