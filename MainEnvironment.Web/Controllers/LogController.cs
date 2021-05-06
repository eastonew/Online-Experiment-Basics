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
        private readonly ILeanLogService LeanLogService;
        private readonly IDataAnalysisLogService AnalysisLogService;

        public LogController(ILogRepo logRepo, ILeanLogService leanLogService, IDataAnalysisLogService analysisLogService)
        {
            this.LogRepo = logRepo;
            this.LeanLogService = leanLogService;
            this.AnalysisLogService = analysisLogService;
        }
        
        [HttpGet("Lean/{participantId}")]
        public async Task<ActionResult> Get(Guid participantId)
        {
            return StatusCode(200, await this.LeanLogService.GetLeanDataForParticipant(participantId));
        }

        [HttpGet("Analysis/{participantId}")]
        public async Task<ActionResult> GetAnalysis(Guid participantId)
        {
            return StatusCode(200, await this.AnalysisLogService.GetDataForParticipant(participantId));
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

        [HttpPost("bulk")]
        public async Task<ActionResult> BulkAdd(LogRequest logs)
        {
            bool success = true;
            if (logs?.Logs != null)
            {
                foreach (var log in logs?.Logs)
                {
                    var logSuccess = await this.LogRepo.AddLog(log);
                    success &= logSuccess;
                }
            }
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
