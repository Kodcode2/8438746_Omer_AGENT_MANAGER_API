using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Agent_Management_Server.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Agent_Management_Server.Controllers;

namespace Agent_Management_Server.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> CreateAgents([FromBody] Target newTarget)
        {
            if (newTarget == null)
            {
                return BadRequest("Vehicle data is null");
            }     
            
            await _service.AddNewTarget(newTarget);
            
            return StatusCode(201);// i need to reyren id new
            //return CreatedAtAction(nameof(GetById), new { id = newVehicle.Id, type = "vehicle" }, newVehicle);
        }
        [HttpGet]
        public async Task<IActionResult> GetTargets()
        {
            //return _DBcontext._Vehicles.ToList();
            var res =  await _service.GetTargets();
            return StatusCode(200, res);
            //Vehicles.Add(newVehicle);
            //return CreatedAtAction(nameof(GetById), new { id = newVehicle.Id, type = "vehicle" }, newVehicle);
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
        public async Task<IActionResult> Move(int id, [FromBody] status_enum_direction newdirection)
        {
            Target target;
            try
            {
               target =  await _service.MoveTarget(id, newdirection);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }                      
            return Ok(target);
        }
        //הפונקציה הזאת צריכה להיות בסרביס אחר
        //public string decima() 
        //{
        //   double t = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2))
        //    return t.ToString();
        //}
        //public int dir() 
        //{
        //    var gg = decima()
            
        //}

    }
}
