using IronDomeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IronDomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefenseController : ControllerBase
    {
        //Defines the amount of defense missiles and the types of defense missiles available.
        // Sample Request JSON : { "missileCount": 20, "missileTypes": ["INTERCEPTOR"] }
        [HttpPut("missiles")]
        public IActionResult DefineDefenseMissiles(Defense defense)
        {
            Defense myDdefense = DbService.defense;
            myDdefense.missileCount = defense.missileCount;
            myDdefense.missileTypes = defense.missileTypes;
            
            
            // Sample Response JSON : { "missileCount": 20, "missileTypes": ["INTERCEPTOR"], "status": "Missiles Ready" }
            return StatusCode(StatusCodes.Status201Created, new { missileCount = myDdefense.missileCount, missileTypes = myDdefense.missileTypes });
        }

    }
}
