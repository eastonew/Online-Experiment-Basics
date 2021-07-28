using MainEnvironment.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MainEnvironment.Core.Services
{
    public class ExperimentService
    {
        private readonly string ApiHost;
        public ExperimentService(string host)
        {
            this.ApiHost = host ?? throw new ArgumentNullException("Services cannot be instantiated without an host e.g. https://test-service");
        }
        public async Task<bool> CompleteExperiment(string participantId, string key)
        {
            bool success = false;
            if (!String.IsNullOrEmpty(participantId) && !String.IsNullOrEmpty(key))
            {
                try
                {
                    ExperimentRequest model = new ExperimentRequest()
                    {
                        ApiKey = key,
                        ParticipantId = participantId
                    };
                    using (HttpClient client = new HttpClient())
                    {
                        string url = $"{ApiHost}/api/experiment";
                        string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                        var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            success = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                catch (Exception ex)
                {
                    // _failures.Add(ex);
                }
            }
            return success;
        }

        public async Task<bool> MarkConsentAsAccepted(string participantId, string key)
        {
            bool success = false;
            if (!String.IsNullOrEmpty(participantId) && !String.IsNullOrEmpty(key))
            {
                try
                {
                    ExperimentRequest model = new ExperimentRequest()
                    {
                        ApiKey = key,
                        ParticipantId = participantId
                    };
                    using (HttpClient client = new HttpClient())
                    {
                        string url = $"{ApiHost}/api/experiment/acceptconsentform";
                        string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                        var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            success = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                catch (Exception ex)
                {
                    // _failures.Add(ex);
                }
            }
            return success;
        }

        public async Task<SceneModel> GetExperimentDetails(string participantId)
        {
            SceneModel model = null;
            if (!String.IsNullOrEmpty(participantId))
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string url = $"{ApiHost}/api/experiment?participantId={participantId}";
                        var response = await client.GetAsync(url);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            model = JsonConvert.DeserializeObject<SceneModel>(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // _failures.Add(ex);
                }
            }
            return model;
        }

        public async Task<string> GetExperimentVersion()
        {
            string version = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{ApiHost}/api/experiment/version";
                    var response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        version = result;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return version;
        }
    }
}
