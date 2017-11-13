using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace foosballv2s.Source.Services.FoosballWebService.Repository
{
    public class TeamRepository
    {
        private readonly string endpointUrl = "/team";
        private IWebServiceClient client;

        public TeamRepository(IWebServiceClient client)
        {
            this.client = client;
        }
       
        public async Task<Team[]> GetAll()
        {
            var response = await client.GetAsync(endpointUrl);
            Team[] teams = JsonConvert.DeserializeObject<Team[]>(response);
            return teams;
        }
        
        public async Task<Team> GetById(int id)
        {
            var response = await client.GetAsync(endpointUrl + "/" + id);
            Team team = JsonConvert.DeserializeObject<Team>(response);
            return team;
        }
        
        public async Task<Team> Create(Team team)
        {
            var teamJsonString = JsonConvert.SerializeObject(team);
            var response = await client.PostAsync(endpointUrl, teamJsonString);
            Team createdTeam = JsonConvert.DeserializeObject<Team>(response);
            return createdTeam;
        }
        
        public async Task<Team> Update(int id, Team team)
        {
            var teamJsonString = JsonConvert.SerializeObject(team);
            var response = await client.PutAsync(endpointUrl + "/" + id , teamJsonString);
            Team updatedTeam = JsonConvert.DeserializeObject<Team>(response);
            return updatedTeam;
        }
        
        public async Task<Team> Delete(int id)
        {
            var response = await client.DeleteAsync(endpointUrl + "/" + id);
            Team updatedTeam = JsonConvert.DeserializeObject<Team>(response);
            return updatedTeam;
        }
    }
}