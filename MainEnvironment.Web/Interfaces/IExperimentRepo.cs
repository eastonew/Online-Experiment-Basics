using MainEnvironment.Core;
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
        Task<bool> MarkConsentFormAsAccepted(string participantId, string key);
    }
}
