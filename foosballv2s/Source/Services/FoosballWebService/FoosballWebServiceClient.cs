using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Org.Apache.Http.Client.Params;
using Org.Json;

namespace foosballv2s.Source.Services.FoosballWebService
{
    public class FoosballWebServiceClient
    {
        private HttpClient client;

        public FoosballWebServiceClient()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<string> GetAsync(Uri uri)
        {
            var response = await client.GetAsync(uri);
            return await GetResponseReturn(response);
        }
        
        public async Task<string> PostAsync(Uri uri, string json)
        {
            var jsonParams = new StringContent (json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, jsonParams);
            return await GetResponseReturn(response);
        }
        
        public async Task<string> PutAsync(Uri uri, string json)
        {
            var jsonParams = new StringContent (json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(uri, jsonParams);
            return await GetResponseReturn(response);
        }
        
        public async Task<string> DeleteAsync(Uri uri)
        {
            var response = await client.DeleteAsync(uri);
            return await GetResponseReturn(response);
        }

        private async Task<string> GetResponseReturn(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return null;
        }
    }
}