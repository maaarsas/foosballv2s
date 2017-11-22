using Http = System.Web.Http;
using System.Collections.Generic;
using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace foosballv2s.WebService.Controllers
{
    [Route("api/[controller]")]
    public class TournamentController : Controller
    {
        private readonly ITournamentRepository _repository;

        public TournamentController(ITournamentRepository repository)
        {
            _repository = repository;
        }

        // GET api/tournament/
        [Authorize]
        [HttpGet]
        public IEnumerable<Tournament> Get()
        {
            return _repository.GetAll();
        }

        // GET api/tournament/0
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Tournament tournament = _repository.Get(id);
            if (tournament == null)
            {
                return NotFound();
            }
            return new ObjectResult(tournament);
        }

        // POST api/tournament
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Tournament tournament)
        {
            if (tournament == null)
            {
                return new BadRequestResult();
            }
            Tournament updatedTournament = _repository.Add(tournament);
            if (updatedTournament == null)
            {
                return NotFound();
            }
            return new ObjectResult(updatedTournament);
        }

        // PUT api/tournament/0
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Tournament tournament)
        {
            if (tournament == null || id != tournament.Id)
            {
                return BadRequest();
            }
            if (_repository.Update(id, tournament))
            {
                return new NoContentResult();
            }
            return NotFound();
        }

        // DELETE api/tournament/0
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
