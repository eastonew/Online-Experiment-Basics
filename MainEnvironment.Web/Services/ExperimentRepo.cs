using MainEnvironment.Core;
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
        public ExperimentRepo(EnvironmentContext context)
        {
            this.Context = context;
        }
        public async Task<SceneModel> GetExperimentDetails(string participantId)
        {
            SceneModel details = null;
            //if a participant has a key assigned they have tried the experiment before
            var participant = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participantId && !p.Completed && (p.ApiKey == null || p.KeyExpirationDate > DateTime.UtcNow));
            if(participant != null && participant.ExperimentId != null)
            {
                //if the participant has any existing logs - delete these out of the database 
                var existingLogs = await Context.Logs.Where(l => l.ParticipantId == participant.Id).ToListAsync();
                foreach (var log in existingLogs)
                {
                    Context.Remove(log);
                }
                await this.Context.SaveChangesAsync();

                var experiment = await this.Context.Experiments.SingleOrDefaultAsync(e => e.Id == participant.ExperimentId && e.IsLive);
                if(experiment != null)
                {
                    if (experiment.ExperimentDefinition != null)
                    {
                        details = JsonConvert.DeserializeObject<SceneModel>(experiment.ExperimentDefinition);
                    }
                    else
                    {
                        details = new SceneModel();
                    }
                    details.ConsentClauses = await Context.ConsentFormClauses.Where(c => c.ExperimentId == experiment.Id).Select(c=>new ConsentClauseModel() { Clause = c.Clause }).ToArrayAsync();
                    //ideally this will be a cryptographically secure key but for now just send a unique Guid
                    Guid key = Guid.NewGuid();
                    details.ApiKey = key.ToString();
                    participant.ApiKey = details.ApiKey;
                    participant.KeyExpirationDate = DateTime.UtcNow.AddMinutes(40); //as part of the rules once they have started they will need to 
                    Context.Update(participant);
                    await this.Context.SaveChangesAsync();
                    //what happens if they start and don't finish and then try to come back?
                }
            }
            return details;
        }

        public async Task<bool> MarkConsentFormAsAccepted(string participantId, string key)
        {
            bool success = true;
            try
            {
                var participant = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participantId && !p.Completed && p.ApiKey == key);
                if (participant != null && participant.ExperimentId != null)
                {
                    participant.ConsentFormAccepted = true;
                    participant.ConsentFormAcceptedDate = DateTime.UtcNow;
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

        public async Task<bool> CompleteExperiment(string participantId, string key)
        {
            bool success = true;
            try
            {
                var participant = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == participantId && !p.Completed && p.ApiKey == key);
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
    }
}
