using MainEnvironment.Core.Enums;
using MainEnvironment.Core.Models;
using MainEnvironment.Database;
using MainEnvironment.Web.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class InstructionsRepo : IInstructionsRepo
    {
        private readonly EnvironmentContext Context;
        public InstructionsRepo(EnvironmentContext context)
        {
            this.Context = context;
        }

        public async Task<DownloadInstructionsModel> GetInstructionsForParticipant(ParticipantModel participant)
        {
            DownloadInstructionsModel model = null;
            var participantDetails = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participant.ParticipantId && !p.Completed && p.ConsentFormAccepted);
            if (participantDetails != null && participantDetails.ExperimentId != null)
            {
                var instructions = await this.Context.DownloadInstructions.SingleOrDefaultAsync(i => i.ExperimentId == participantDetails.ExperimentId && i.EquimentType == participantDetails.EquipmentType);
                if (instructions != null)
                {
                    model = new DownloadInstructionsModel();
                    model.Instructions = instructions.Instructions;
                }
            }
            return model;
        }
    }
}
