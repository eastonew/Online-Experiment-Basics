using MainEnvironment.Core;
using MainEnvironment.Core.Enums;
using MainEnvironment.Core.Models;
using MainEnvironment.Database;
using System;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface IExperimentRepo
    {
        Task<SceneModel> GetExperimentDetails(string participantId);
        Task<bool> CompleteExperiment(string participantId, string key);
        Task<bool> UpdateParticipantEquipment(string participantId, EquipmentTypeEnum equipment);
        Task<bool> CreateParticipant(string participantId, EquipmentTypeEnum equipment, Guid experimentId);
        Task<ParticipantModel> MarkConsentFormAsAccepted(string participantId);
        Task<ConsentFormModel> GetConsentForm(string participantId);
        Task<ParticipantInformationModel> GetParticipantInformationSheet(string participantId);
        Task<Participant> GetParticipantDetails(Guid participantId);
        Task<bool> MarkAsDownloaded(Guid participantId);
        Task<string> GetExperimentVersion();
    }
}
