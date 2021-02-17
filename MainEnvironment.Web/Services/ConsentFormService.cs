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
            //ideally this is handled elsewhere but is fine here for the moment
           bool success = await this.ExperimentRepo.UpdateParticipantEquipment(participantDetails.ParticipantId, participantDetails.EquipmentType);
            return await this.ExperimentRepo.GetConsentForm(participantDetails.ParticipantId);
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
                        instructions = await InstructionsRepo.GetInstructionsForParticipant(participant);
                    }
                }
            }

            return instructions;
        }
    }
}
