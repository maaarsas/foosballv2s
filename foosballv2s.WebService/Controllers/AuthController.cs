using System.Collections.Generic;
using System.Threading.Tasks;
using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace foosballv2s.WebService.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repository;

        public AuthController(IAuthRepository repository)
        {
            _repository = repository;
        }
        
        
        public async Task<> Register([FromBody] RegisterViewModel)
        // GET api/team/
//        [HttpGet]
//        public IEnumerable<Team> Get()
//        {
//            return _repository.GetAll();
//        }
//
//        // GET api/team/5
//        [HttpGet("{id}")]
//        public IActionResult Get(int id)
//        {
//            Team team = _repository.Get(id);
//            if (team == null)
//            {
//                return NotFound();
//            }
//            return new ObjectResult(team);
//        }
//
//        // POST api/team
//        [HttpPost]
//        public IActionResult Post([FromBody] Team team)
//        {
//            if (team == null)
//            {
//                return new BadRequestResult();
//            }
//            Team updatedTeam =_repository.Add(team);
//            return new ObjectResult(updatedTeam);
//        }
//
//        // PUT api/team/5
//        [HttpPut("{id}")]
//        public IActionResult Put(int id, [FromBody] Team team)
//        {
//            if (team == null || id != team.Id)
//            {
//                return BadRequest();
//            }
//            if (_repository.Update(id, team))
//            {
//                return new NoContentResult();
//            }
//            return NotFound();
//        }
//
//        // DELETE api/team/5
//        [HttpDelete("{id}")]
//        public IActionResult Delete(int id)
//        {
//            if (_repository.Remove(id))
//            {    
//                return new NoContentResult();
//            }
//            return NotFound();
//        }
    }
}