using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Agent_Management_Server.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Agent_Management_Server.Controllers;

namespace Agent_Management_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly MyServiceTarget _service;

        public static List<Target> Targets = new List<Target>();
        public TargetsController(Iservic<Target> service) 
        {
            _service = service as MyServiceTarget;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTargets([FromBody] Target newTarget)
        {
            if (newTarget == null)
            {
                return BadRequest("Vehicle data is null");
            }                 
            await _service.AddNewTarget(newTarget);            
            return StatusCode(201,new {id = newTarget.Id });            
        }


        [HttpGet]
        public async Task<IActionResult> GetTargets()
        {
            //return _DBcontext->Targets.ToList();
            var res =  await _service.GetTargets();
            return StatusCode(200, res);            
        }
        
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> PutPin(int id, [FromBody] Location Startlocation)
        {
            try
            {
                _service.PutPinTarget(id, Startlocation);
            }
            catch (Exception e)
            {
                {
                    return BadRequest(e.Message);
                }
            }
            return Ok();            
        }


        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(int id, [FromBody] Dictionary<string, string> dirction)
        {
            Target target;
            try
            {
               target =  await _service.MoveTarget(id, dirction["direction"]);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }                      
            return Ok(target);
        }
    }
}
