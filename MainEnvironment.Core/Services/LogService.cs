using MainEnvironment.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MainEnvironment.Core.Services
{

    public class LogService : ILogService
    {
        private readonly string ApiHost;
        public LogService(string host)
        {
            this.ApiHost = host ?? throw new ArgumentNullException("Services cannot be instantiated without an host e.g. https://test-service");
        }

        public async Task<bool> LogDetails(ILogModel logModel)
        {
            bool success = false;
            if (logModel != null)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string url = $"{ApiHost}/api/log";
                        string json = JsonConvert.SerializeObject(logModel, Formatting.Indented);
                        var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            //fallback method of writing logs to a file 
                        }
                        else if(response.StatusCode == HttpStatusCode.Created)
                        {
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                   //TODO - find another way of storing this exception as obviously the Logging Api is not working
                }
            }
            return success;
        }
    }
}
