using System;
using Http = System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using foosballv2s.WebService.Models;
using foosballv2s.WebService.Params;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace foosballv2s.WebService.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameRepository _repository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

        public GameController(IGameRepository repository, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        
        // GET api/game/
        [Authorize]
        [HttpGet]
        public IEnumerable<Game> Get(GameParams gameParams, SortParams sortParams)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.GetUserId());
            return _repository.GetAll(gameParams, sortParams, user);
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
            
            var user = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.GetUserId());
            if (user.Id != game.User.Id)
            {
                return new UnauthorizedResult();
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
            
            var user = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.GetUserId());
            if (user.Id != game.User.Id)
            {
                return new UnauthorizedResult();
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
            var game = _repository.Get(id);
            var user = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.GetUserId());
            if (game != null && user.Id != game.User.Id)
            {
                return new UnauthorizedResult();
            }
            
            if (_repository.Remove(id))
            {    
                return new NoContentResult();
            }
            return NotFound();
        }
    }
}