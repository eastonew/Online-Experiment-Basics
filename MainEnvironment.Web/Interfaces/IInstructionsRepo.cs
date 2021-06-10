using MainEnvironment.Core.Enums;
using MainEnvironment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface IInstructionsRepo
    {
        Task<DownloadInstructionsModel> GetInstallInstructionsForParticipant(ParticipantModel participant);

        Task<DownloadInstructionsModel> GetUninstallInstructionsForParticipant(ParticipantModel participant);
    }
}
