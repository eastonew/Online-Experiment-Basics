using MainEnvironment.Core.Interfaces;
using MainEnvironment.Core.Models;
using MainEnvironment.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogRepo LogRepo;

        public LogController(ILogRepo logRepo)
        {
            this.LogRepo = logRepo;
        }

        [HttpPost]
        public async Task<ActionResult> Post(LogModel log)
        {
            bool success = await this.LogRepo.AddLog(log);
            if (success)
            {
                return StatusCode((int)HttpStatusCode.Created);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }
    }
}
