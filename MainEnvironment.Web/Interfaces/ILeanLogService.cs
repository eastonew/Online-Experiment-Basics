using MainEnvironment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface ILeanLogService
    {
        Task<List<HeadsetPositionData>> GetLeanDataForParticipant(Guid participantId);
    }
}
