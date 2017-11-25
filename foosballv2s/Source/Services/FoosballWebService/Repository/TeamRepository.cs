using System.Threading.Tasks;
using foosballv2s.Source.Entities;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(foosballv2s.Source.Services.FoosballWebService.Repository.TeamRepository))]
namespace foosballv2s.Source.Services.FoosballWebService.Repository
{
    /// <summary>
    /// A class for forming and fetching requests to a web service about teams
    /// </summary>
    public class TeamRepository
    {
        private readonly string endpointUrl = "/team";
        private IWebServiceClient client;

        public TeamRepository()
        {
            client = DependencyService.Get<IWebServiceClient>();
        }
       
        /// <summary>
        /// Gets all teams
        /// </summary>
        /// <returns></returns>
        public async Task<Team[]> GetAll()
        {
            var response = await client.GetAsync(endpointUrl);
            Team[] teams = FoosballJsonConvert.DeserializeObject<Team[]>(response);
            return teams;
        }
        
        /// <summary>
        /// Gets one team by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Team> GetById(int id)
        {
            var response = await client.GetAsync(endpointUrl + "/" + id);
            Team team = FoosballJsonConvert.DeserializeObject<Team>(response);
            return team;
        }
        
        /// <summary>
        /// Creates a team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public async Task<Team> Create(Team team)
        {
            var teamJsonString = JsonConvert.SerializeObject(team);
            var response = await client.PostAsync(endpointUrl, teamJsonString);
            Team createdTeam = FoosballJsonConvert.DeserializeObject<Team>(response);
            return createdTeam;
        }
        
        /// <summary>
        /// Updates a team by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public async Task<Team> Update(int id, Team team)
        {
            var teamJsonString = JsonConvert.SerializeObject(team);
            var response = await client.PutAsync(endpointUrl + "/" + id , teamJsonString);
            Team updatedTeam = FoosballJsonConvert.DeserializeObject<Team>(response);
            return updatedTeam;
        }
        
        /// <summary>
        /// Deletes a team by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Team> Delete(int id)
        {
            var response = await client.DeleteAsync(endpointUrl + "/" + id);
            Team updatedTeam = FoosballJsonConvert.DeserializeObject<Team>(response);
            return updatedTeam;
        }
    }
}