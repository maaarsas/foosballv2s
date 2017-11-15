using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Org.Apache.Http.Client.Params;
using Org.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(foosballv2s.Source.Services.FoosballWebService.FoosballWebServiceClient))]
namespace foosballv2s.Source.Services.FoosballWebService
{
    public class FoosballWebServiceClient : IWebServiceClient
    {
        private string webServiceUri = "http://18.194.122.53:5000/api";
        private readonly string emptyJson = "{}";
        private HttpClient client;

        public FoosballWebServiceClient()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<string> GetAsync(string endPointUri)
        {
            var uri = GetFullUri(endPointUri);
            var response = await client.GetAsync(uri);
            return await GetResponseReturn(response);
        }
        
        public async Task<string> PostAsync(string endPointUri, string json)
        {
            var uri = GetFullUri(endPointUri);
            var jsonParams = new StringContent (json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, jsonParams);
            return await GetResponseReturn(response);
        }
        
        public async Task<string> PutAsync(string endPointUri, string json)
        {
            var uri = GetFullUri(endPointUri);
            var jsonParams = new StringContent (json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(uri, jsonParams);
            return await GetResponseReturn(response);
        }
        
        public async Task<string> DeleteAsync(string endPointUri)
        {
            var uri = GetFullUri(endPointUri);
            var response = await client.DeleteAsync(uri);
            return await GetResponseReturn(response);
        }

        private Uri GetFullUri(string endpointUri)
        {
            return new Uri(webServiceUri + endpointUri);
        }

        private async Task<string> GetResponseReturn(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return emptyJson;
        }
    }
}