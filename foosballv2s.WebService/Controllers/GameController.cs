using System;
using Http = System.Web.Http;
using System.Collections.Generic;
using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace foosballv2s.WebService.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameRepository _repository;

        public GameController(IGameRepository repository)
        {
            _repository = repository;
        }
        
        // GET api/game/
        [Authorize]
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            return _repository.GetAll();
        }

        // GET api/game/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Game game = _repository.Get(id);
            if (game == null)
            {
                return NotFound();
            }
            return new ObjectResult(game);
        }

        // POST api/game
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Game game)
        {
            if (game == null)
            {
                return new BadRequestResult();
            }
            Game updatedGame =_repository.Add(game);
            if (updatedGame == null)
            {
                return NotFound();
            }
            return new ObjectResult(updatedGame);
        }

        // PUT api/game/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Game game)
        {
            if (game == null || id != game.Id)
            {
                return BadRequest();
            }
            if (_repository.Update(id, game))
            {
                return new NoContentResult();
            }
            return NotFound();
        }

        // DELETE api/game/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_repository.Remove(id))
            {    
                return new NoContentResult();
            }
            return NotFound();
        }
    }
}