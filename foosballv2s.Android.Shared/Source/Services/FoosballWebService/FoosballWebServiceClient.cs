using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using foosballv2s.Droid.Shared.Source.Helpers;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage.Models;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService;
using ModernHttpClient;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(FoosballWebServiceClient))]
namespace foosballv2s.Droid.Shared.Source.Services.FoosballWebService
{
    /// <summary>
    /// Makes HTTP requests and parses the responses
    /// </summary>
    public class FoosballWebServiceClient : IWebServiceClient
    {
        private string webServiceUri = "";
        private readonly string emptyJson = "{}";
        private readonly string authScheme = "Bearer";
        private HttpClient client;
        private ICredentialStorage _credentialStorage;

        public FoosballWebServiceClient()
        {
            webServiceUri = ReadWebServiceUri();
            _credentialStorage = DependencyService.Get<ICredentialStorage>();
            client = new HttpClient(new NativeMessageHandler());
            client.MaxResponseContentBufferSize = 256000;
            AddAuthorizationHeader();
        }

        /// <summary>
        /// Send a GET HTTP request
        /// </summary>
        /// <param name="endPointUri"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string endPointUri)
        {
            var uri = GetFullUri(endPointUri);
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(uri);
            }
            catch (Exception e)
            {
                response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
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
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(uri, jsonParams);
            }
            catch (Exception e)
            {
                response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
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
            HttpResponseMessage response;
            try
            {
                response = await client.PutAsync(uri, jsonParams);
            }
            catch (Exception e)
            {
                response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
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
            HttpResponseMessage response;
            try
            {
                response = await client.DeleteAsync(uri);
            }
            catch (Exception e)
            {
                response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
            return await GetResponseReturn(response);
        }

        private string ReadWebServiceUri()
        {
            return ConfigHelper.GetConfigData(Application.Context, "api_url");
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
    }
}