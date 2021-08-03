using MainEnvironment.Core;
using MainEnvironment.Core.Enums;
using MainEnvironment.Core.Interfaces;
using MainEnvironment.Core.Models;
using MainEnvironment.Database;
using MainEnvironment.Web.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class ExperimentRepo : IExperimentRepo
    {
        private readonly EnvironmentContext Context;
        private readonly ISecureTokenService SecureTokenService;
        private readonly IInstructionsRepo instructionsRepo;
        private readonly IParticipantGroupService ParticipantGroupService;
        public ExperimentRepo(EnvironmentContext context, ISecureTokenService secureTokenService, IInstructionsRepo instructionsRepo, IParticipantGroupService participantGroupService)
        {
            this.Context = context;
            this.SecureTokenService = secureTokenService;
            this.instructionsRepo = instructionsRepo;
            this.ParticipantGroupService = participantGroupService;
        }
        public async Task<SceneModel> GetExperimentDetails(string participantId)
        {
            SceneModel details = new SceneModel();
            //if a participant has a key assigned they have tried the experiment before
            //They can attempt the experiment as many times as they like until they complete it, but only have a specified time limit to do so, otherwise they need to restart
            var participant = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participantId || p.UniqueCode.ToLower() == participantId.ToLower());
            if(participant != null && participant.ExperimentId != null)
            {
                if (!participant.Completed && participant.ConsentFormAccepted)
                {
                    //if the participant has any existing logs - delete these out of the database 
                    var existingLogs = await Context.Logs.Where(l => l.ParticipantId == participant.Id).ToListAsync();
                    foreach (var log in existingLogs)
                    {
                        Context.Remove(log);
                    }
                    await this.Context.SaveChangesAsync();

                    var experiment = await this.Context.Experiments.SingleOrDefaultAsync(e => e.Id == participant.ExperimentId && e.IsLive);
                    if (experiment != null)
                    {
                        if (experiment.ExperimentDefinition != null)
                        {
                            details = JsonConvert.DeserializeObject<SceneModel>(experiment.ExperimentDefinition);
                        }
                        else
                        {
                            details = new SceneModel();
                        }

                        int groupId = this.ParticipantGroupService.AssignGroupToParticipant(experiment.TotalGroups);
                        details.GroupId = groupId;

                        //ideally this will be a cryptographically secure key but for now just send a unique Guid
                        Guid key = Guid.NewGuid();
                        details.ApiKey = key.ToString();
                        participant.ApiKey = details.ApiKey;
                        participant.GroupId = groupId;
                        participant.KeyExpirationDate = DateTime.UtcNow.AddDays(5); // DateTime.UtcNow.AddMinutes(40); //as part of the rules once they have started they will need to 
                        Context.Update(participant);
                        await this.Context.SaveChangesAsync();
                        //what happens if they start and don't finish and then try to come back?
                    }
                }
                else
                {
                    if(participant.Completed)
                    {
                        details.ErrorMessage = "AlreadyCompleted";
                    }
                    else if(!participant.ConsentFormAccepted)
                    {
                        details.ErrorMessage = "ConsentNotAccepted";
                    }
                    else
                    {
                        details.ErrorMessage = "Unknown";
                    }
                }
            }
            else
            {
                details.ErrorMessage = "CannotFind";
            }
            return details;
        }

        public async Task<string> GetExperimentVersion()
        {
            string version = "";
            //GEt the first live experiment - not perfect but should be ok as only one experiment should be live at any time
            var experiment = await this.Context.Experiments.FirstOrDefaultAsync(e => e.IsLive);
            if (experiment != null)
            {
                version = experiment.RequiredAppVersion;
            }
            return version;
        }

        public async Task<bool> UpdateParticipantEquipment(string participantId, EquipmentTypeEnum equipment)
        {
            bool success = false;
            var participant = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participantId && !p.Completed);
            if (participant != null)
            {
                participant.EquipmentType = equipment;
                await this.Context.SaveChangesAsync();
                success = true;
            }
            return success;
        }

        public async Task<ConsentFormModel> GetConsentForm(string participantId)
        {
            ConsentFormModel details = null;
            //if a participant has a key assigned they have tried the experiment before
            var participant = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participantId && !p.Completed && !p.ConsentFormAccepted);
            if (participant != null && participant.ExperimentId != null)
            {
                var experiment = await this.Context.Experiments.SingleOrDefaultAsync(e => e.Id == participant.ExperimentId && e.IsLive);
                if (experiment != null)
                {
                    details = new ConsentFormModel();
                    details.ParticipantId = participantId;
                    details.Clauses = await Context.ConsentFormClauses.Where(c => c.ExperimentId == experiment.Id).Select(c => new ConsentClauseModel() { Clause = c.Clause, Accepted = false, Id = c.Id }).ToListAsync();
                }
            }
            return details;
        }

        public async Task<ParticipantModel> MarkConsentFormAsAccepted(string participantId)
        {
            ParticipantModel participant = null;
            try
            {
                var participantDetails = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participantId && !p.Completed);
                if (participantDetails != null && participantDetails.ExperimentId != null)
                {
                    participantDetails.ConsentFormAccepted = true;
                    participantDetails.ConsentFormAcceptedDate = DateTime.UtcNow;
                    Context.Update(participantDetails);
                    await this.Context.SaveChangesAsync();
                    participant = new ParticipantModel();
                    participant.EquipmentType = participantDetails.EquipmentType;
                    participant.ParticipantId = participantDetails.ExternalParticipantId;
                }
            }
            catch(Exception ex)
            {
            }
            return participant;
        }

        public async Task<Participant> GetParticipantDetails(Guid participantId)
        {
            return await this.Context.Participants.SingleOrDefaultAsync(p => p.Id == participantId);
        }

        public async Task<bool> CompleteExperiment(string participantId, string key)
        {
            bool success = true;
            try
            {
                var participant = await this.Context.Participants.SingleOrDefaultAsync(p => (p.ExternalParticipantId == participantId || p.UniqueCode.ToLower() == participantId.ToLower()) && !p.Completed && p.ApiKey == key);
                if (participant != null && participant.ExperimentId != null)
                {
                    participant.Completed = true;
                    participant.CompletedDate = DateTime.UtcNow;
                    participant.ApiKey = null;
                    participant.KeyExpirationDate = null;
                    Context.Update(participant);
                    await this.Context.SaveChangesAsync();
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public async Task<bool> MarkAsDownloaded(Guid participantId)
        {
            bool success = false;
            var participant = await this.GetParticipantDetails(participantId);
            if (participant != null)
            {
                participant.DownloadedEnvironment = true;
                participant.DownloadToken = null;
                await this.Context.SaveChangesAsync();
                success = true;
            }
            return success;
        }

        public async Task<ParticipantInformationModel> GetParticipantInformationSheet(string participantId)
        {
            ParticipantInformationModel details = new ParticipantInformationModel();
            //if a participant has a key assigned they have tried the experiment before
            var participant = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participantId);
            if (participant != null && participant.ExperimentId != null)
            {
                if (participant.Completed)
                {
                    details.ParticipantState = "Completed";
                }
                else if (participant.ConsentFormAccepted)
                {
                    details.ParticipantState = "Download";
                    var instructions = await this.instructionsRepo.GetInstallInstructionsForParticipant(new ParticipantModel() { ParticipantId = participantId });
                    details.Instructions = instructions.Instructions;
                }
                else
                {
                    details.ParticipantState = "Consent";
                    var experiment = await this.Context.Experiments.SingleOrDefaultAsync(e => e.Id == participant.ExperimentId && e.IsLive);
                    if (experiment != null)
                    {
                        
                        details.ParticipantId = participantId;
                        details.ParticipantInformationSheet = experiment.ParticipantInformationSheet;
                    }
                }
            }
            return details;
        }

        public async Task<bool> CreateParticipant(string participantId, EquipmentTypeEnum equipment, Guid experimentId)
        {
            bool success = false;
            try
            {
                string uniqueCode = this.SecureTokenService.GenerateSecureToken(5);
                //check for duplicates
                //add check to ensure that we aren't getting confused with the same participant on another experiment - no need to check for null as if a participant's experiment is null, this needs to completely fail
                var participant = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participantId.Trim() && p.ExperimentId.Equals(experimentId));
                if (participant == null)
                {
                    participant = new Participant();
                    participant.Id = Guid.NewGuid();
                    participant.ExternalParticipantId = participantId.Trim();
                    participant.ExperimentId = experimentId;
                    participant.Completed = false;
                    participant.CompletedDate = null;
                    participant.ApiKey = null;
                    participant.KeyExpirationDate = null;
                    participant.ConsentFormAccepted = false;
                    participant.ConsentFormAcceptedDate = null;
                    participant.DownloadToken = null;
                    participant.DownloadedEnvironment = false;
                    participant.EquipmentType = equipment;
                    participant.UniqueCode = uniqueCode;
                    this.Context.Participants.Add(participant);
                }
                else
                {
                    participant.UniqueCode = uniqueCode;
                    participant.EquipmentType = equipment;
                }

                await this.Context.SaveChangesAsync();
                success = true;
            }
            catch { }
            return success;
        }
    }
}
