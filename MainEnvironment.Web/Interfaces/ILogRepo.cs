using MainEnvironment.Core.Interfaces;
using MainEnvironment.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface ILogRepo
    {
        Task<bool> AddLog(ILogModel log);

        Task<List<Log>> GetLogsForParticipant(Guid participantId);
    }
}
