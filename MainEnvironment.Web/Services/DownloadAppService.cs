using MainEnvironment.Core.Models;
using MainEnvironment.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class DownloadAppService : IDownloadAppService
    {

        private readonly IExperimentRepo ExperimentRepo;
        private readonly IInstructionsRepo InstructionsRepo;

        public DownloadAppService(IExperimentRepo repo, IInstructionsRepo instructionsRepo)
        {
            this.ExperimentRepo = repo;
            this.InstructionsRepo = instructionsRepo;
        }

        public async Task<bool> CheckIfUserCanDownload(Guid experimentId, Guid participantId, Guid downloadToken)
        {
            bool isValid = false;
            var participant = await this.ExperimentRepo.GetParticipantDetails(participantId);
            if (participant != null && participant.ExperimentId != null && participant.ExperimentId.Equals(experimentId))
            {
                if(participant.EquipmentType == Core.Enums.EquipmentTypeEnum.Vive && participant.ConsentFormAccepted && !participant.Completed)
                {
                    if (participant.DownloadToken != null && participant.DownloadToken.Equals(downloadToken))
                    {
                        //final check is that we have updated the user correctly
                        isValid = await this.ExperimentRepo.MarkAsDownloaded(participantId);
                    }
                }
            }
            return isValid;
        }

        public async Task<DownloadInstructionsModel> GetCompletionInstructions(ParticipantModel participant)
        {
            var instructions = await this.InstructionsRepo.GetUninstallInstructionsForParticipant(participant);
            return instructions;
        }
    }
}
