using MainEnvironment.Core;
using MainEnvironment.Core.Enums;
using MainEnvironment.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface IExperimentRepo
    {
        Task<SceneModel> GetExperimentDetails(string participantId);
        Task<bool> CompleteExperiment(string participantId, string key);
        Task<bool> UpdateParticipantEquipment(string participantId, EquipmentTypeEnum equipment);
        Task<ParticipantModel> MarkConsentFormAsAccepted(string participantId);
        Task<ConsentFormModel> GetConsentForm(string participantId);
    }
}
