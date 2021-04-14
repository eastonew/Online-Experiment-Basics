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

        public DownloadAppService(IExperimentRepo repo)
        {
            this.ExperimentRepo = repo;
        }

        public async Task<bool> CheckIfUserCanDownload(Guid experimentId, Guid participantId, Guid downloadToken)
        {
            bool isValid = false;
            var participant = await this.ExperimentRepo.GetParticipantDetails(participantId);
            if (participant != null && participant.ExperimentId != null && participant.ExperimentId.Equals(experimentId))
            {
                if(participant.EquipmentType == Core.Enums.EquipmentTypeEnum.Vive && participant.ConsentFormAccepted && !participant.Completed && !participant.DownloadedEnvironment)
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
    }
}
