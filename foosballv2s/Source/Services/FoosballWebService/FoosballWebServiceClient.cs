using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(foosballv2s.Source.Services.FoosballWebService.FoosballWebServiceClient))]
namespace foosballv2s.Source.Services.FoosballWebService
{
    /// <summary>
    /// Makes HTTP requests and parses the responses
    /// </summary>
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

        /// <summary>
        /// Send a GET HTTP request
        /// </summary>
        /// <param name="endPointUri"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string endPointUri)
        {
            var uri = GetFullUri(endPointUri);
            var response = await client.GetAsync(uri);
            return await GetResponseReturn(response);
        }
        
        /// <summary>
        /// Send a POST HTTP request with content
        /// </summary>
        /// <param name="endPointUri"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string endPointUri, string json)
        {
            var uri = GetFullUri(endPointUri);
            var jsonParams = new StringContent (json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, jsonParams);
            return await GetResponseReturn(response);
        }
        
        /// <summary>
        /// Send a PUT HTTP request with content
        /// </summary>
        /// <param name="endPointUri"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task<string> PutAsync(string endPointUri, string json)
        {
            var uri = GetFullUri(endPointUri);
            var jsonParams = new StringContent (json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(uri, jsonParams);
            return await GetResponseReturn(response);
        }
        
        /// <summary>
        /// Send a DELETE HTTP request
        /// </summary>
        /// <param name="endPointUri"></param>
        /// <returns></returns>
        public async Task<string> DeleteAsync(string endPointUri)
        {
            var uri = GetFullUri(endPointUri);
            var response = await client.DeleteAsync(uri);
            return await GetResponseReturn(response);
        }

        /// <summary>
        /// Attach the API adress to the endpoint uri
        /// </summary>
        /// <param name="endpointUri"></param>
        /// <returns></returns>
        private Uri GetFullUri(string endpointUri)
        {
            return new Uri(webServiceUri + endpointUri);
        }

        /// <summary>
        /// Parse the response message
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
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