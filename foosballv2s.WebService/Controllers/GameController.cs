using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace foosballv2s.WebService.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        
        private readonly GameContext _context;

        public GameController(GameContext context)
        {
            _context = context;
        }

        // GET api/game/
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            List<Game> games = _context.Games.ToList();
            return games;
        }

        // GET api/game/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/game
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/game/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/game/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}