using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(foosballv2s.Source.Services.FoosballWebService.Repository.GameRepository))]
namespace foosballv2s.Source.Services.FoosballWebService.Repository
{
    public class GameRepository
    {
        private readonly string endpointUrl = "/game";
        private IWebServiceClient client;

        public GameRepository()
        {
            client = DependencyService.Get<IWebServiceClient>();
        }
       
        public async Task<Game[]> GetAll()
        {
            var response = await client.GetAsync(endpointUrl);
            Game[] games = JsonConvert.DeserializeObject<Game[]>(response);
            return games;
        }
        
        public async Task<Game> GetById(int id)
        {
            var response = await client.GetAsync(endpointUrl + "/" + id);
            Game game = JsonConvert.DeserializeObject<Game>(response);
            return game;
        }
        
        public async Task<Game> Create(Game game)
        {
            var gameJsonString = JsonConvert.SerializeObject(game);
            var response = await client.PostAsync(endpointUrl, gameJsonString);
            Game createdGame = JsonConvert.DeserializeObject<Game>(response);
            return createdGame;
        }
        
        public async Task<Game> Update(int id, Game game)
        {
            var gameJsonString = JsonConvert.SerializeObject(game);
            var response = await client.PutAsync(endpointUrl + "/" + id , gameJsonString);
            Game updatedGame = JsonConvert.DeserializeObject<Game>(response);
            return updatedGame;
        }
        
        public async Task<Game> Delete(int id)
        {
            var response = await client.DeleteAsync(endpointUrl + "/" + id);
            Game updatedGame = JsonConvert.DeserializeObject<Game>(response);
            return updatedGame;
        }
    }
}