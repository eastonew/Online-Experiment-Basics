using MainEnvironment.Core.Interfaces;
using MainEnvironment.Database;
using MainEnvironment.Web.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class LogRepo : ILogRepo
    {
        private readonly EnvironmentContext Context;
        public LogRepo(EnvironmentContext context)
        {
            this.Context = context;
        }
        public async Task<bool> AddLog(ILogModel log)
        {
            bool success = false;
            try
            {
                var participant = await this.Context.Participants.SingleOrDefaultAsync(p => p.ExternalParticipantId == log.ParticipantId && p.ApiKey == log.ApiKey && p.KeyExpirationDate > DateTime.UtcNow);
                if (participant != null)
                {
                    if (!String.IsNullOrEmpty(log.Message) && log.LogDate != null && log.LogDate > DateTime.MinValue)
                    {
                        Log dbLog = new Log();
                        dbLog.Id = Guid.NewGuid();
                        dbLog.LogDate = log.LogDate;
                        dbLog.LogMessage = log.Message;
                        dbLog.ParticipantId = participant.Id;
                        if (log.AdditionalValues != null)
                        {
                            dbLog.AdditionalDetails = JsonConvert.SerializeObject(log.AdditionalValues);
                        }
                        var result = await this.Context.AddAsync(dbLog);
                        await this.Context.SaveChangesAsync();
                        success = true;
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public async Task<List<Log>> GetLogsForParticipant(Guid participantId)
        {
            List<Log> logs = null;
            try
            {
                logs = await Context.Logs.Where(l => l.ParticipantId == participantId).ToListAsync();
            }
            catch
            {
            }
            return logs;
        }
    }
}
