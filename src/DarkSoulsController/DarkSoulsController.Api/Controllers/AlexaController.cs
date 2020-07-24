using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using DarkSoulsController.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkSoulsController.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlexaController : ControllerBase
    {
        private readonly IHubContext<DarkSoulsHub> _hub;
     
        public AlexaController(IHubContext<DarkSoulsHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("HandleRequest")]
        public async Task<ActionResult> HandleRequest([FromBody]SkillRequest skillRequest)
        {
            // validate the alexa request: https://developer.amazon.com/docs/custom-skills/host-a-custom-skill-as-a-web-service.html
            //var isValid = await ValidateRequest(Request, skillRequest);
            //if (!isValid)
            //    return BadRequest("Validation errors");


            // if it's Launch request, then say hello and tell the user the commands
            if (skillRequest?.GetRequestType() == typeof(LaunchRequest))
            {
                return Ok(ResponseBuilder.Ask("Welcome to the dark souls controller, issue a command like 'roll' or 'Fireball!'. Prepare to die!", null));
            }
            // if it's an intent request, then choose what command based on the name of the intent
            else if (skillRequest?.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = skillRequest.Request as IntentRequest;
                await _hub.Clients.All.SendAsync(intentRequest.Intent.Name, intentRequest);
                switch (intentRequest.Intent.Name)
                {
                    case "RightLightIntent":
                        return Ok(ResponseBuilder.Tell("Light attack"));
                    case "RightHeavyIntent":
                        return Ok(ResponseBuilder.Tell("Heavy attack"));
                    case "LeftLightIntent":
                        return Ok(ResponseBuilder.Tell("Left Light attack"));
                    case "LeftHeavyIntent":
                        return Ok(ResponseBuilder.Tell("Left Heavy attack"));
                    case "RollIntent":
                        return Ok(ResponseBuilder.Ask("Rolling!", null));
                    case "ItemIntent":
                        return Ok(ResponseBuilder.Tell("Using item. Hopefully it isn't estus"));
                    case "SwapLeftWeaponIntent":
                        return Ok(ResponseBuilder.Tell("Swapping left"));
                    case "SwapRightWeaponIntent":
                        return Ok(ResponseBuilder.Tell("Swapping right"));
                    case "MoveForwardIntent":
                    case "MoveBackwardsIntent":
                    case "MoveLeftIntent":
                    case "MoveRightIntent":
                        return Ok(ResponseBuilder.Tell("Moving!"));
                    case "QuickQuitIntent": return Ok(ResponseBuilder.Tell("Abort!"));


                    case "AMAZON.HelpIntent": return Ok(ResponseBuilder.Ask("Happy to help! You can issue commands such as 'jump', 'reload', 'put on shields' and more! Try saying one of those.", null));
                    case "AMAZON.StopIntent":
                    case "AMAZON.CancelIntent": return Ok(ResponseBuilder.Tell("Thanks for using the DarkSouls Controller. Come back later."));

                    default: return Ok(ResponseBuilder.Tell("Executing command"));
                }
            }

            return Ok(ResponseBuilder.Ask("You said something I don't know what to do with. Try saying something like 'jump' or 'reload'.", null));
        }
    }
}
