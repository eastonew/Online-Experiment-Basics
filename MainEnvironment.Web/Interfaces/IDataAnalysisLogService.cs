using MainEnvironment.Core.Interfaces;
using MainEnvironment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface IDataAnalysisLogService
    {
        Task<List<IAnalysisLogData>> GetDataForParticipant(Guid participantId);
    }
}
