using Agent_Management_Server.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agent_Management_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class targetsController : ControllerBase
    {

        public static List<Target> Targets = new List<Target>();
        
    }
}
