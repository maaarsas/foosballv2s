using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
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
            //_userManager = userManager;
        }

        // GET api/tournament/
        [Authorize]
        [HttpGet]
        //public IEnumerable<Game> Get(GameParams gameParams, SortParams sortParams)
        public IActionResult Get()
        {
            //var user = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.GetUserId());
            //return _repository.GetAll(gameParams, sortParams, user);
            return new ObjectResult(":DDDD");
        }
    }
}
