using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using foosballv2s.Source.Services.CredentialStorage;
using foosballv2s.Source.Services.CredentialStorage.Models;
using ModernHttpClient;
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
        private readonly string authScheme = "Bearer";
        private HttpClient client;
        private ICredentialStorage _credentialStorage;
        private TaskHelper<HttpResponseMessage> taskRunner;

        public FoosballWebServiceClient()
        {
            _credentialStorage = DependencyService.Get<ICredentialStorage>();
            client = new HttpClient(new NativeMessageHandler());
            client.MaxResponseContentBufferSize = 256000;
            AddAuthorizationHeader();
            ConfigureHttpTaskRunner();
        }

        /// <summary>
        /// Send a GET HTTP request
        /// </summary>
        /// <param name="endPointUri"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string endPointUri)
        {
            var uri = GetFullUri(endPointUri);
            var response = await taskRunner.RunWithRetry(() => { return client.GetAsync(uri); });
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
            var response = await taskRunner.RunWithRetry(() => { return client.PostAsync(uri, jsonParams); });
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
            var response = await taskRunner.RunWithRetry(() => { return client.PutAsync(uri, jsonParams); });
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
            var response = await taskRunner.RunWithRetry(() => { return client.DeleteAsync(uri); });
            return await GetResponseReturn(response);
        }

        /// <summary>
        /// Sets the authorization token that will be used in every sent http request
        /// </summary>
        /// <param name="token"></param>
        public void AddAuthorizationHeader()
        {
            Credential credential = _credentialStorage.Read();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authScheme, credential.Token);
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
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _credentialStorage.Remove();
            }
            return emptyJson;
        }

        private void ConfigureHttpTaskRunner()
        {
            taskRunner = new TaskHelper<HttpResponseMessage>();
            taskRunner.SetFaultTaskReturnObject(new HttpResponseMessage(HttpStatusCode.RequestTimeout));
        }
    }
}