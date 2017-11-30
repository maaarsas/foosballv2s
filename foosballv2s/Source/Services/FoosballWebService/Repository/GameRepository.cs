using System;
using System.Threading.Tasks;
using foosballv2s.Source.Entities;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(foosballv2s.Source.Services.FoosballWebService.Repository.GameRepository))]
namespace foosballv2s.Source.Services.FoosballWebService.Repository
{
    /// <summary>
    /// A class for forming and fetching the requests to the web service about games
    /// </summary>
    public class GameRepository
    {
        private readonly string endpointUrl = "/game";
        private IWebServiceClient client;

        public GameRepository()
        {
            client = DependencyService.Get<IWebServiceClient>();
        }
       
        /// <summary>
        /// Fetches all games
        /// </summary>
        /// <returns></returns>
        public async Task<Game[]> GetAll()
        {
            var response = await client.GetAsync(endpointUrl);
            Game[] games = FoosballJsonConvert.DeserializeObject<Game[]>(response);
            if (games == null)
            {
                games = new Game[]{};
            }
            return games;
        }
        
        /// <summary>
        /// Fetches the game by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Game> GetById(int id)
        {
            var response = await client.GetAsync(endpointUrl + "/" + id);
            Game game = FoosballJsonConvert.DeserializeObject<Game>(response);
            return game;
        }
        
        /// <summary>
        /// Creates a game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task<Game> Create(Game game)
        {
            var gameJsonString = JsonConvert.SerializeObject(game);
            var response = await client.PostAsync(endpointUrl, gameJsonString);
            Game createdGame = FoosballJsonConvert.DeserializeObject<Game>(response);
            return createdGame;
        }
        
        /// <summary>
        /// Updates a game
        /// </summary>
        /// <param name="id"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task<Game> Update(int id, Game game)
        {
            var gameJsonString = JsonConvert.SerializeObject(game);
            var response = await client.PutAsync(endpointUrl + "/" + id , gameJsonString);
            Game updatedGame = FoosballJsonConvert.DeserializeObject<Game>(response);
            return updatedGame;
        }
        
        /// <summary>
        /// Deletes a game
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Game> Delete(int id)
        {
            var response = await client.DeleteAsync(endpointUrl + "/" + id);
            Game updatedGame = FoosballJsonConvert.DeserializeObject<Game>(response);
            return updatedGame;
        }
    }
}