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
    public class SculptureService
    {
        private readonly string ApiHost;
        public SculptureService(string host)
        {
            this.ApiHost = host ?? throw new ArgumentNullException("Services cannot be instantiated without an host e.g. https://test-service");
        }
        public async Task<ExportDefinition> GetSculpture(Guid sculptureId)
        {
            //TODO - Maybe allow bulk download but leave as this for now
            ExportDefinition sculpture = null;

            using (HttpClient client = new HttpClient())
            {
                string url = $"{ApiHost}/ThreeDee/{sculptureId}";
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    sculpture = JsonConvert.DeserializeObject<ExportDefinition>(await response.Content.ReadAsStringAsync());
                }
            }
            return sculpture;
        }

    }
}
