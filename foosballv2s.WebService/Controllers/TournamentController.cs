﻿using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Controllers
{
    [Route("api/[controller]")]
    public class TournamentController : Controller
    {
        private readonly ITournamentRepository _repository;
        //private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

        public TournamentController(ITournamentRepository repository/*, Microsoft.AspNetCore.Identity.UserManager<User> userManager*/)
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

        // GET api/tournament/1
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

        // POST api/tournament/
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Tournament tournament)
        {
            if (tournament == null)
            {
                return new BadRequestResult();
            }

            Tournament newTournament = _repository.Add(tournament);
            if (newTournament == null)
            {
                return NotFound();
            }
            return new ObjectResult(newTournament);
        }

        // PUT api/tournament/1
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

        // DELETE api/tournament/1
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tournament = _repository.Get(id);
            if (tournament == null)
            {
                return new UnauthorizedResult();
            }

            if (_repository.Remove(id))
            {
                return new NoContentResult();
            }
            return NotFound();
        }

        // put api/tournament/pair/1
        [Authorize]
        [HttpPut("pair/{tournamentId}")]
        public IActionResult AddPair(int tournamentId, [FromBody] TournamentPair tournamentPair)
        {
            if (tournamentPair == null)
            {
                return new BadRequestResult();
            }

            TournamentPair newPair = _repository.AddPair(tournamentId, tournamentPair);
            if (newPair == null)
            {
                return NotFound();
            }
            return new ObjectResult(newPair);
        }
    }
}
