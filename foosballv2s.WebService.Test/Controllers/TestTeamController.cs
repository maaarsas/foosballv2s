using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;
using System.Net;
using System.Web.Http.Controllers;
using foosballv2s.WebService.Controllers;
using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using BadRequestResult = System.Web.Http.Results.BadRequestResult;
using StatusCodeResult = System.Web.Http.Results.StatusCodeResult;

namespace foosballv2s.WebService.Test
{
    [TestFixture]
    public class TestTeamController
    {
        [Test]
        public void PostTeam_ShouldReturnSameTeam()
        {
            var controller = new TeamController(new TestTeamRepository());
            var team = GetDemoTeams()[0];
            var result = controller.Post(team) as CreatedAtRouteNegotiatedContentResult<Team>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.TeamName, team.TeamName);
            Assert.Greater(result.Content.Id, 0);
        }
        
        [Test]
        [TestCaseSource("GetWrongDemoTeams")]
        public void PostTeam_ShouldFail_WrongName(Team team)
        {
            var controller = new TeamController(new TestTeamRepository());
            var result = controller.Post(team) as StatusCodeResult;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void PutProduct_ShouldReturnStatusCode()
        {
            var controller = new TeamController(new TestTeamRepository());
            var team = GetDemoTeams()[1];
            var result = controller.Put(team.Id, team) as StatusCodeResult;
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(StatusCodeResult), result);
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public void PutProduct_ShouldFail_WhenDifferentID()
        {
            var controller = new TeamController(new TestTeamRepository());
            var badResult = controller.Put(999, GetDemoTeams()[0]);
            
            Assert.IsInstanceOf(typeof(BadRequestResult), badResult);
        }

        [Test]
        public void GetProduct_ShouldReturnProductWithSameID()
        {
            var repository = new TestTeamRepository();
            var team = GetDemoTeams()[0];
            repository.Add(team);

            var controller = new TeamController(repository);
            var result = controller.Get(1) as OkNegotiatedContentResult<Team>;

            Assert.IsNotNull(result);
            Assert.AreEqual(team.Id, result.Content.Id);
            Assert.AreEqual(team.TeamName, result.Content.TeamName);
        }

        [Test]
        public void GetProducts_ShouldReturnAllProducts()
        {
            var repository = new TestTeamRepository();
            Team[] teams = GetDemoTeams();
            
            foreach (Team team in teams)
            {
                repository.Add(team);
            }

            var controller = new TeamController(repository);
            var result = controller.Get() as TestTeamDbSet;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Local.Count);
        }

        [Test]
        public void DeleteProduct_ShouldReturnOK()
        {
            var repository = new TestTeamRepository();
            var team = GetDemoTeams()[0];
            repository.Add(team);

            var controller = new TeamController(repository);
            var result = controller.Delete(1) as OkNegotiatedContentResult<Team>;

            Assert.IsNotNull(result);
            Assert.AreEqual(team.Id, result.Content.Id);
        }
        
        [Test]
        public void DeleteProduct_ShouldFail_NotExisting()
        {
            var repository = new TestTeamRepository();
            var team = GetDemoTeams()[0];
            repository.Add(team);

            var controller = new TeamController(repository);
            var result = controller.Delete(2) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(StatusCodeResult), result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        Team[] GetDemoTeams()
        {
            return new Team[]
            {
                new Team() {TeamName = "TestName1"},
                new Team() {TeamName = "TestName2"},
                new Team() {TeamName = "TestName3"},
            };
        }
        
        Team[] GetWrongDemoTeams()
        {
            return new Team[]
            {
                new Team() {TeamName = "a"}, // too short
                new Team() {TeamName = "-.;;'.';-"}, // wrong symbols
                new Team() {TeamName = "TestTestTestTEstTEstTestTestTEst"}, // too long
            };
        }
    }
}