using MainEnvironment.Core.Enums;
using MainEnvironment.Core.Models;
using MainEnvironment.Database;
using MainEnvironment.Web.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<DownloadInstructionsModel> GetInstallInstructionsForParticipant(ParticipantModel participant)
        {
            DownloadInstructionsModel model = null;
            var participantDetails = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participant.ParticipantId && !p.Completed && p.ConsentFormAccepted);
            if (participantDetails != null && participantDetails.ExperimentId != null)
            {
                var instructions = await this.Context.DownloadInstructions.SingleOrDefaultAsync(i => i.ExperimentId == participantDetails.ExperimentId && i.EquimentType == participantDetails.EquipmentType && i.InstructionsType == InstructionsTypeEnum.Install);
                if (instructions != null)
                {
                    model = new DownloadInstructionsModel();
                    model.Instructions = instructions.Instructions;

                    if (model.Instructions.Contains("{{DownloadLink}}"))
                    {
                        //if we request the download link token in the instructions it needs to be built here (ideally elsewhere but who really cares
                        //download token is just a guid -- ideally it would be a crypto secure string
                        participantDetails.DownloadToken = Guid.NewGuid();
                        await this.Context.SaveChangesAsync();
                    

                        StringBuilder sb = new StringBuilder();
                        sb.Append("https://experimentapi.azurewebsites.net/api/download/vive/");
                        sb.Append(participantDetails.ExperimentId.ToString());
                        sb.Append("/");
                        sb.Append(participantDetails.Id.ToString());
                        sb.Append("/");
                        sb.Append(participantDetails.DownloadToken);
                        model.Instructions = model.Instructions.Replace("{{DownloadLink}}", sb.ToString());
                    }

                    if(model.Instructions.Contains("{{UniqueCode}}"))
                    {
                        model.Instructions = model.Instructions.Replace("{{UniqueCode}}", participantDetails.UniqueCode);
                    }

                }
            }
            return model;
        }

        public async Task<DownloadInstructionsModel> GetUninstallInstructionsForParticipant(ParticipantModel participant)
        {
            DownloadInstructionsModel model = null;
            var participantDetails = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participant.ParticipantId && p.Completed && p.ConsentFormAccepted);
            if (participantDetails != null && participantDetails.ExperimentId != null)
            {
                var instructions = await this.Context.DownloadInstructions.SingleOrDefaultAsync(i => i.ExperimentId == participantDetails.ExperimentId && i.EquimentType == participantDetails.EquipmentType && i.InstructionsType == InstructionsTypeEnum.Uninstall);
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
