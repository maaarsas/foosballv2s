using System.Threading.Tasks;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Helpers;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Repository;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(TournamentRepository))]
namespace foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Repository
{
    /// <summary>
    /// A class for forming and fetching the requests to the web service about tournament
    /// </summary>
    public class TournamentRepository
    {
        private readonly string endpointUrl = "/tournament";
        private IWebServiceClient client;
        private ICredentialStorage storage;

        public TournamentRepository()
        {
            client = DependencyService.Get<IWebServiceClient>();
            storage = DependencyService.Get<ICredentialStorage>();
        }

        /// <summary>
        /// Fetches all tournaments
        /// </summary>
        /// <returns></returns>
        public async Task<Tournament[]> GetAll(string urlParams = null)
        {
            var response = await client.GetAsync(endpointUrl + "/?" + urlParams);
            Tournament[] tournaments = FoosballJsonConvert.DeserializeObject<Tournament[]>(response);
            if (tournaments == null)
            {
                tournaments = new Tournament[]{};
            }
            return tournaments;
        }

        /// <summary>
        /// Fetches the tournament by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Tournament> GetById(int id)
        {
            var response = await client.GetAsync(endpointUrl + "/" + id);
            Tournament tournament = FoosballJsonConvert.DeserializeObject<Tournament>(response);
            return tournament;
        }

        /// <summary>
        /// Creates a tournament
        /// </summary>
        /// <param name="tournament"></param>
        /// <returns></returns>
        public async Task<Tournament> Create(Tournament tournament)
        {
            var tournamentJsonString = JsonConvert.SerializeObject(tournament);
            var response = await client.PostAsync(endpointUrl, tournamentJsonString);
            Tournament createdTournament = FoosballJsonConvert.DeserializeObject<Tournament>(response);
            return createdTournament;
        }

        /// <summary>
        /// Updates a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tournament"></param>
        /// <returns></returns>
        public async Task<Tournament> Update(int id, Tournament tournament)
        {
            var tournamentJsonString = JsonConvert.SerializeObject(tournament);
            var response = await client.PutAsync(endpointUrl + "/" + id , tournamentJsonString);
            Tournament updatedTournament = FoosballJsonConvert.DeserializeObject<Tournament>(response);
            return updatedTournament;
        }

        /// <summary>
        /// Deletes a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Tournament> Delete(int id)
        {
            var response = await client.DeleteAsync(endpointUrl + "/" + id);
            Tournament updatedTournament = FoosballJsonConvert.DeserializeObject<Tournament>(response);
            return updatedTournament;
        }
    }
}