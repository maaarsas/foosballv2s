using System.Collections.Generic;
using System.Linq;
using System.Net;
using foosballv2s.WebService.Controllers;
using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;

namespace foosballv2s.WebService.Test.Controllers
{
    [TestFixture]
    public class TestTeamController
    {
        [Test]
        public void PostTeam_ShouldReturnSameTeam()
        {
            var repository = GetTeamRepository();
            var controller = new TeamController(repository);
            var team = GetDemoTeams()[0];
            var result = controller.Post(team) as ObjectResult;
            Team resultTeam = (Team) result.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(resultTeam.TeamName, team.TeamName);
            Assert.Greater(resultTeam.Id, 0);
        }
        
        [Test]
        [TestCaseSource("GetWrongDemoTeams")]
        public void PostTeam_ShouldFail_WrongName(Team team)
        {
            var repository = GetTeamRepository();
            var controller = new TeamController(repository);
            var result = controller.Post(team) as BadRequestObjectResult;

            Assert.AreEqual((int) HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void PutProduct_ShouldReturnStatusCode()
        {
            var repository = GetTeamRepository();
            var controller = new TeamController(repository);
            var team = GetDemoTeams()[1];
            controller.Post(team);
            var result = controller.Put(team.Id, team) as NoContentResult;
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(NoContentResult), result);
            Assert.AreEqual((int) HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public void PutProduct_ShouldFail_WhenDifferentID()
        {
            var repository = GetTeamRepository();
            var controller = new TeamController(repository);
            var team = GetDemoTeams()[0];
            controller.Post(team);
            var badResult = controller.Put(999, team);
            
            Assert.IsInstanceOf(typeof(Microsoft.AspNetCore.Mvc.BadRequestResult), badResult);
        }

        [Test]
        public void GetProduct_ShouldReturnProductWithSameID()
        {
            var repository = GetTeamRepository();
            var team = GetDemoTeams()[0];
            repository.Add(team);

            var controller = new TeamController(repository);
            var result = controller.Get(1) as ObjectResult;
            Team resultTeam = (Team) result.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(team.Id, resultTeam.Id);
            Assert.AreEqual(team.TeamName, resultTeam.TeamName);
        }

        [Test]
        public void GetProducts_ShouldReturnAllProducts()
        {;
            var repository = GetTeamRepository();
            Team[] teams = GetDemoTeams();
            foreach (Team team in teams)
            {
                repository.Add(team);
            }

            var controller = new TeamController(repository);
            var result = controller.Get() as IEnumerable<Team>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void DeleteProduct_ShouldReturnOK()
        {
            var repository = GetTeamRepository();
            var team = GetDemoTeams()[0];
            repository.Add(team);

            var controller = new TeamController(repository);
            var result = controller.Delete(1) as NoContentResult;

            Assert.IsNotNull(result);
            Assert.AreEqual((int) HttpStatusCode.NoContent, result.StatusCode);
        }
        
        [Test]
        public void DeleteProduct_ShouldFail_NotExisting()
        {
            var repository = GetTeamRepository();
            var team = GetDemoTeams()[0];
            repository.Add(team);

            var controller = new TeamController(repository);
            var result = controller.Delete(2) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
            Assert.AreEqual((int) HttpStatusCode.NotFound, result.StatusCode);
        }

        public static Team[] GetDemoTeams()
        {
            return new Team[]
            {
                new Team() {Id = 1, TeamName = "TestName1"},
                new Team() {Id = 2, TeamName = "TestName2"},
                new Team() {Id = 3, TeamName = "TestName3"},
            };
        }
        
        public static Team[] GetWrongDemoTeams()
        {
            return new Team[]
            {
                new Team() {Id = 1, TeamName = "a"}, // too short
                new Team() {Id = 2, TeamName = "-.;;'.';-"}, // wrong symbols
                new Team() {Id = 3, TeamName = "TestTestTestTEstTEstTestTestTEst"}, // too long
            };
        }

        private ITeamRepository GetTeamRepository()
        {
//            DbSet<Team> teams = new TestTeamDbSet();
//            var dbContextMock = new Mock<WebServiceDbContext>();        
//            dbContextMock.Setup(m => m.Teams).Returns(teams);
//            dbContextMock.Setup(m => m.SaveChanges()).Returns(1);
            return new TeamRepository(new TestWebServiceDbContext());
        }
    }
}