using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MainEnvironment.Core;
using MainEnvironment.Core.Interfaces;
using MainEnvironment.Core.Models;
using MainEnvironment.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MainEnvironment.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperimentController : ControllerBase
    {
        private readonly IExperimentRepo ExperimentRepo;
        public ExperimentController(IExperimentRepo experimentRepo)
        {
            this.ExperimentRepo = experimentRepo;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string participantId)
        {
            var result = await this.ExperimentRepo.GetExperimentDetails(participantId);
            if (result != null)
            {
                return StatusCode((int)HttpStatusCode.OK, result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public async Task<ObjectResult> CompleteExperiment(ExperimentRequest model)
        {
            var result = await this.ExperimentRepo.CompleteExperiment(model.ParticipantId, model.ApiKey);
            return StatusCode((int)HttpStatusCode.OK, result);
        }

        [HttpPost("AcceptConsentForm")]
        public async Task<ActionResult> AcceptConsentForm(ExperimentRequest model)
        {
            var result = await this.ExperimentRepo.MarkConsentFormAsAccepted(model.ParticipantId, model.ApiKey);
            return StatusCode((int)HttpStatusCode.OK, result);
        }
    }
}
