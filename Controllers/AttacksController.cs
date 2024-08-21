using IronDomeApi.Models;
using IronDomeApi.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IronDomeApi.Middlewares;
using IronDomeApi.Middlewares.Attack;

namespace IronDomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttacksController : ControllerBase
    {
        private int requestStatus;

        //Returns information about all attacks.
        [HttpGet]
        public IActionResult GetAllAttacks()
        {
            requestStatus = StatusCodes.Status200OK;// status of the request = 200
            return StatusCode(
                requestStatus,// = 200
                HttpUtils.Response(
                    requestStatus, //gives the status of the request = true
                    new {
                        attacks = DbService.AttacksList.ToArray() // gives all the attacks
                    })
                );

        }

        //create a new attack and gets an attack ID.
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult CreateAttack(Attack attack)
        {
            attack.Id = Guid.NewGuid();
            attack.attackStatus = AttackStatuses.PENDING; // The attack status is now pending
            DbService.AttacksList.Add(attack);// add the attack to the list
            requestStatus = StatusCodes.Status201Created;// status of the request = 201
            return StatusCode(
                requestStatus,//  = 200
                HttpUtils.Response(
                    requestStatus,// gives the status of the request = true
                    new {
                        attack = attack // gives the JSON of the attack
                    })
            );
        }

        //Initiates an attack using the specified attack ID, running a background task.
        [HttpPost("{id}/start")]
        public IActionResult StartAttack(Guid? id)
        {
            Attack attack = DbService.AttacksList.FirstOrDefault(att => att.Id == id);
            requestStatus = StatusCodes.Status404NotFound;
            if (attack == null) return StatusCode(requestStatus, HttpUtils.Response(requestStatus, "Attack not found"));
            if (attack.attackStatus == AttackStatuses.COMPLETED)
            {
                requestStatus = StatusCodes.Status400BadRequest;
                return StatusCode(
                    requestStatus,
                    HttpUtils.Response(requestStatus, "Cannot start an attack that has already been completed.")
                );
            }

            attack.StartedAt = DateTime.Now;
            attack.attackStatus = AttackStatuses.IN_PROGRESS;

            Task attackTask = Task.Run(() =>
            {
                Task.Delay(10000);
            });

            requestStatus = StatusCodes.Status200OK;
            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Attack Started.", TaskId = attackTask.Id }
                );
        }

        //Checks the status of an ongoing attack.
        [HttpGet("{id}/status")]
        public IActionResult CheckAttackStatus(Guid? id)
        {
            Attack attack = DbService.AttacksList.FirstOrDefault(attack => attack.Id == id);

            if (attack == null)
            {
                requestStatus = StatusCodes.Status404NotFound;
                return StatusCode(requestStatus, HttpUtils.Response(requestStatus, "attack not found"));
            }

            requestStatus = StatusCodes.Status200OK;
            return StatusCode(requestStatus, HttpUtils.Response(requestStatus, new { attack = attack }));
        }

        //Intercepting an ongoing attack using the attack ID.
        [HttpPost("{id}/intercept")]
        public async Task<IActionResult> InterceptAttack(Guid? id)
        {
            Attack attack = DbService.AttacksList.FirstOrDefault(attack => attack.Id == id);

            if (attack.attackStatus == AttackStatuses.IN_PROGRESS)
            {
                bool intercept = await IronDome.HandleAttack(attack);


                if (intercept)
                {
                    attack.interceptedStatus = InterceptedStatuses.SUCCSESS;
                }
                else
                {
                    attack.interceptedStatus = InterceptedStatuses.FAILED;
                }

                attack.attackStatus = AttackStatuses.COMPLETED;
                requestStatus = StatusCodes.Status200OK;
                return StatusCode(requestStatus, HttpUtils.Response(requestStatus, new
                {
                    message = "Attack intercepted",
                    status = attack.interceptedStatus
                }));
            }
            requestStatus = StatusCodes.Status400BadRequest;
            return StatusCode(requestStatus, "An attack that is already completed or pending cannot be intercepted");
        }


        // Defines the count of missiles and types of missiles for the attack.
        // sample request: { "missileCount": 10, "missileType": ["BALISTIC_MISSILE", "ROCKET"] }
        [HttpPut("{id}/missiles")]
        public IActionResult DefineAttackMissiles(Guid? id, Attack attack)
        {
            if (id == null)
            {
                requestStatus = StatusCodes.Status404NotFound;
                return StatusCode(requestStatus, "The Id is null");
            }
            
            Attack oldAttack = DbService.AttacksList.FirstOrDefault(attack => attack.Id == id);
            
            if (oldAttack == null)
            {
                requestStatus = StatusCodes.Status404NotFound;
                return StatusCode(requestStatus, "There is no missile with this ID number");
            }

            oldAttack.missilesTypes = attack.missilesTypes;
            oldAttack.missileCount = attack.missileCount;
            oldAttack.MissilesDefined = "Missiles Defined";

            // Sample Response JSON: { "id": 1, "missileCount": 10, "missileTypes": ["BALISTIC_MISSILE", "ROCKET"], "status": "Missiles Defined" }
            return StatusCode(StatusCodes.Status201Created,
                new { id = oldAttack.Id, missileCount = oldAttack.missileCount, missilesTypes = oldAttack.missilesTypes, status = oldAttack.MissilesDefined });
        }

        // Erases an attack as long as it hasn't started.
        [HttpDelete("{id}")]
        public IActionResult DeleteAttack(Guid? id)
        {
            if (id == null)
            {
                requestStatus = StatusCodes.Status404NotFound;
                return StatusCode(requestStatus, HttpUtils.Response(requestStatus, "The Id is null"));
            }

            Attack attack = DbService.AttacksList.FirstOrDefault(attack => attack.Id == id);
            if (attack == null)
            {
                requestStatus = StatusCodes.Status404NotFound;
                return StatusCode(requestStatus, HttpUtils.Response(requestStatus, "There is no missile with this ID number"));
            }
            if (attack.attackStatus == AttackStatuses.PENDING)
            {
                DbService.AttacksList.Remove(attack);
                requestStatus = StatusCodes.Status200OK;
                return StatusCode(requestStatus, "The attack was successfully deleted");
            }
            if (attack.attackStatus != AttackStatuses.PENDING) {
                requestStatus = StatusCodes.Status200OK;
                return StatusCode(requestStatus, "A non-pending attack cannot be deleted");
                    }
            return null;
        }


    }
}
