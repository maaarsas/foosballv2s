using System.Threading.Tasks;
using foosballv2s.Source.Services.CredentialStorage;
using foosballv2s.Source.Services.FoosballWebService.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(foosballv2s.Source.Services.FoosballWebService.Repository.AuthRepository))]
namespace foosballv2s.Source.Services.FoosballWebService.Repository
{
    /// <summary>
    /// A class for authenticating the user
    /// </summary>
    public class AuthRepository
    {
        private readonly string endpointUrl = "/auth";

        private IWebServiceClient client;
        private ICredentialStorage _credentialStorage;

        public AuthRepository()
        {
            client = DependencyService.Get<IWebServiceClient>();
            _credentialStorage = DependencyService.Get<ICredentialStorage>();
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            var loginJson = JsonConvert.SerializeObject(model);
            var response = await client.PostAsync(endpointUrl + "/token", loginJson);
            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
            if (loginResponse.Token == null)
            {
                return false;
            }
            _credentialStorage.Save(model.Email, loginResponse.Token, loginResponse.Expiration);
            client.AddAuthorizationHeader();
            return true;
        }
        
        public async Task<bool> Register(RegisterViewModel model)
        {
            var registerJson = JsonConvert.SerializeObject(model);
            var response = await client.PostAsync(endpointUrl + "/register", registerJson);
            RegisterResponse registerResponse = JsonConvert.DeserializeObject<RegisterResponse>(response);
            return registerResponse.Succeeded;
        }
    }
}