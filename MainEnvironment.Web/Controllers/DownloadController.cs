using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MainEnvironment.Core.Models;
using MainEnvironment.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace MainEnvironment.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IConsentFormService ConsentService;
        private readonly IDownloadAppService DownloadService;

        public DownloadController(IConsentFormService consentService, IDownloadAppService appDownloadService)
        {
            this.ConsentService = consentService;
            this.DownloadService = appDownloadService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var text = System.IO.File.ReadAllText("content/consent.html");
            //this needs to return a form to allow the user to enter thewir participant Id
            return Content(text, "text/html", Encoding.UTF8);
        }

        [HttpGet("Complete")]
        public IActionResult GetCompletePage()
        {
            var text = System.IO.File.ReadAllText("content/complete.html");
            //this needs to return a form to allow the user to enter thewir participant Id
            return Content(text, "text/html", Encoding.UTF8);
        }

        [HttpPost("GetConsentForm")]
        public async Task<IActionResult> GetConsentForm(ParticipantModel model)
        {
            ConsentFormModel consentDetails = null;
            if (!String.IsNullOrEmpty(model.ParticipantId))
            {
                consentDetails = await this.ConsentService.GetConsentFormModel(model);
            }
            return StatusCode((int)HttpStatusCode.OK, consentDetails);
        }

        [HttpPost("GetParticipantInformation")]
        public async Task<IActionResult> GetParticipantInformation(ParticipantModel model)
        {
            ParticipantInformationModel informationDetails = null;
            if (!String.IsNullOrEmpty(model.ParticipantId))
            {
                informationDetails = await this.ConsentService.GetParticipantInformation(model);
            }
            return StatusCode((int)HttpStatusCode.OK, informationDetails);
        }

        [HttpPost("GetCompletionDetails")]
        public async Task<IActionResult> GetCompletionDetails(ParticipantModel model)
        {
            DownloadInstructionsModel completionInstructions = null;
            if (!String.IsNullOrEmpty(model.ParticipantId))
            {
                completionInstructions = await this.DownloadService.GetCompletionInstructions(model);
            }
            return StatusCode((int)HttpStatusCode.OK, completionInstructions);
        }

        [HttpPost("SubmitConsentForm")]
        public async Task<IActionResult> SubmitConsentForm(ConsentFormModel model)
        {
            DownloadInstructionsModel downloadInstructions = null;
            if(model != null && !String.IsNullOrEmpty(model.ParticipantId)  && model.Clauses != null && model.Clauses.Count > 0)
            {
                downloadInstructions = await this.ConsentService.SubmitConsentForm(model);
            }

            return StatusCode((int)HttpStatusCode.OK, downloadInstructions);
        }

        [HttpGet("Vive/{experimentId}/{participantId}/{downloadToken}")]
        public async Task<IActionResult> DownloadViveEnvironment(Guid experimentId, Guid participantId, Guid downloadToken)
        {
            var isValid = await this.DownloadService.CheckIfUserCanDownload(experimentId, participantId, downloadToken);
            if (isValid)
            {
                var bytes = System.IO.File.ReadAllBytes($"content/{experimentId}/vive.zip");
                return File(bytes, "application/zip", "ViveEnvironment.zip");
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }
    }
}
