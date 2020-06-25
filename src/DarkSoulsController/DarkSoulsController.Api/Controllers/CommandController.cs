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
    public class CommandController : ControllerBase
    {
        private readonly IHubContext<DarkSoulsHub> _hub;

        public CommandController(IHubContext<DarkSoulsHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("{commandName}")]
        public async Task<string> SendCommand(string commandName)
        {
            await _hub.Clients.All.SendAsync(commandName, null);
            return "Success";
        }
    }
}
