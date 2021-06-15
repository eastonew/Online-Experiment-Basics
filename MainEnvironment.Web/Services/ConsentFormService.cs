using MainEnvironment.Core.Models;
using MainEnvironment.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class ConsentFormService : IConsentFormService
    {
        private readonly IExperimentRepo ExperimentRepo;
        private readonly IInstructionsRepo InstructionsRepo;

        public ConsentFormService(IExperimentRepo repo, IInstructionsRepo instructionsRepo)
        {
            this.ExperimentRepo = repo;
            this.InstructionsRepo = instructionsRepo;
        }

        public async Task<ConsentFormModel> GetConsentFormModel(ParticipantModel participantDetails)
        {
            return await this.ExperimentRepo.GetConsentForm(participantDetails.ParticipantId);
        }

        public async Task<ParticipantInformationModel> GetParticipantInformation(ParticipantModel participantDetails)
        {
            //ideally this is handled elsewhere but is fine here for the moment
            //Unfortunately Prolific doesn't seem to like manual work, so I think we need to create a participant at this step, then cross check the final submissions with the people available on Prolific
            //nothing seems to stop someone from just adding themselves and completing the process 
            bool participantCreated = await this.ExperimentRepo.CreateParticipant(participantDetails.ParticipantId, participantDetails.EquipmentType, Guid.Parse("483DD908-A5BE-4DD4-A7F7-9BF042907156"));
            //bool success = await this.ExperimentRepo.UpdateParticipantEquipment(participantDetails.ParticipantId, participantDetails.EquipmentType);
            return await this.ExperimentRepo.GetParticipantInformationSheet(participantDetails.ParticipantId);
        }

        public async Task<DownloadInstructionsModel> SubmitConsentForm(ConsentFormModel consentForm)
        {
            DownloadInstructionsModel instructions = null;
            //to ensure we don't just accept someone sending something random use the stored version to check whether the user has completed the consent
            var storedConsentForm = await this.ExperimentRepo.GetConsentForm(consentForm.ParticipantId);
            if (storedConsentForm != null)
            {
                bool completed = true;

                foreach (var clause in storedConsentForm.Clauses)
                {
                    //find corresponding entry in provided model
                    var corresponding = consentForm.Clauses.FirstOrDefault(c => c.Id == clause.Id);
                    completed &= corresponding != null && corresponding.Accepted;
                }

                if (completed)
                {
                    var participant = await this.ExperimentRepo.MarkConsentFormAsAccepted(consentForm.ParticipantId);
                    if (participant != null)
                    {
                        instructions = await InstructionsRepo.GetInstallInstructionsForParticipant(participant);
                    }
                }
            }

            return instructions;
        }
    }
}
