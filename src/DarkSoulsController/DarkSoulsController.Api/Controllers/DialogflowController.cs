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
    public class DialogflowController : ControllerBase
    {
        private readonly IHubContext<DarkSoulsHub> _hub;

        public DialogflowController(IHubContext<DarkSoulsHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("HandleRequest")]
        public async Task<IActionResult> HandleDialogflowRequest([FromBody]dynamic dialogflowRequest)
        {
            var intentName = dialogflowRequest?.queryResult?.action?.ToString();
            dynamic response = null;

            if (intentName == "input.welcome")
            {
                response = Ask("Welcome to the dark souls controller, issue a command like 'roll' or 'Fireball!'. Prepare to die!");
            }
            await _hub.Clients.All.SendAsync(intentName as string, (IntentRequest)null);
            switch (intentName)
            {
                case "RightLightIntent":
                    return new JsonResult(Ask("Light attack"));
                case "RightHeavyIntent":
                    return new JsonResult(Ask("Heavy attack"));
                case "LeftLightIntent":
                    return new JsonResult(Ask("Left Light attack"));
                case "LeftHeavyIntent":
                    return new JsonResult(Ask("Left Heavy attack"));
                case "RollIntent":
                    return new JsonResult(Ask("Rolling!"));
                case "ItemIntent":
                    return new JsonResult(Ask("Using item. Hopefully it isn't estus"));
                case "SwapLeftWeaponIntent":
                    return new JsonResult(Ask("Swapping left"));
                case "SwapRightWeaponIntent":
                    return new JsonResult(Ask("Swapping right"));
                case "MoveForwardIntent":
                case "MoveBackwardsIntent":
                case "MoveLeftIntent":
                case "MoveRightIntent":
                    return new JsonResult(Ask("Moving!"));
                case "QuickQuitIntent": return new JsonResult(Ask("Abort!"));
            }
            if (response is null)
                response = Ask("Executing command");

            return new JsonResult(response);

        }


        private dynamic Ask(string outputText, string displayTextOverride = null)
        {

            dynamic response = new
            {
                FulfillmentText = displayTextOverride ?? outputText,
                Payload = new
                {
                    Google = new
                    {
                        ExpectUserResponse = true,
                        RichResponse = new
                        {
                            Items = new dynamic[]
                            {
                                new
                                {
                                    SimpleResponse = new
                                    {
                                        TextToSpeech = outputText
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return response;

        }

    }
}
