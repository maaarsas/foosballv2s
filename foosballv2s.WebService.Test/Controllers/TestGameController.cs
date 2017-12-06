using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using foosballv2s.WebService.Controllers;
using foosballv2s.WebService.Models;
using foosballv2s.WebService.Params;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;

namespace foosballv2s.WebService.Test.Controllers
{
    [TestFixture]
    public class TestGameController
    {
        private Mock<UserManager<User>> userManager = new Mock<UserManager<User>>();
        
        [Test]
        public void PostGame_ShouldReturnSameGame()
        {
            var repository = GetGameRepository();
            var controller = new GameController(repository, userManager.Object);
            var game = GetDemoGames()[0];
            var result = controller.Post(game) as ObjectResult;
            Game resultGame = (Game) result.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(resultGame.Team1, game.Team1);
            Assert.AreEqual(resultGame.Team2, game.Team2);
            Assert.Greater(resultGame.Id, 0);
        }
        
        [Test]
        [TestCaseSource("GetWrongDemoGames")]
        public void PostGame_ShouldFail_WrongName(Game game)
        {
            var repository = GetGameRepository();
            var controller = new GameController(repository, userManager.Object);
            var result = controller.Post(game) as BadRequestObjectResult;

            Assert.AreEqual((int) HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void PutProduct_ShouldReturnStatusCode()
        {
            var repository = GetGameRepository();
            var controller = new GameController(repository, userManager.Object);
            var game = GetDemoGames()[1];
            controller.Post(game);
            var result = controller.Put(game.Id, game) as NoContentResult;
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(NoContentResult), result);
            Assert.AreEqual((int) HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public void PutProduct_ShouldFail_WhenDifferentID()
        {
            var repository = GetGameRepository();
            var controller = new GameController(repository, userManager.Object);
            var game = GetDemoGames()[0];
            controller.Post(game);
            var badResult = controller.Put(999, game);
            
            Assert.IsInstanceOf(typeof(Microsoft.AspNetCore.Mvc.BadRequestResult), badResult);
        }

        [Test]
        public void GetProduct_ShouldReturnProductWithSameID()
        {
            var repository = GetGameRepository();
            var game = GetDemoGames()[0];
            repository.Add(game);

            var controller = new GameController(repository, userManager.Object);
            var result = controller.Get(1) as ObjectResult;
            Game resultGame = (Game) result.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(game.Id, resultGame.Id);
            Assert.AreEqual(resultGame.Team1, game.Team1);
            Assert.AreEqual(resultGame.Team2, game.Team2);
        }

        [Test]
        public void GetProducts_ShouldReturnAllProducts()
        {;
            var repository = GetGameRepository();
            Game[] games = GetDemoGames();
            foreach (Game game in games)
            {
                repository.Add(game);
            }

            var controller = new GameController(repository, userManager.Object);
            var result = controller.Get(new GameParams(), new SortParams()) as IEnumerable<Game>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void DeleteProduct_ShouldReturnOK()
        {
            var repository = GetGameRepository();
            var game = GetDemoGames()[0];
            repository.Add(game);

            var controller = new GameController(repository, userManager.Object);
            var result = controller.Delete(1) as NoContentResult;

            Assert.IsNotNull(result);
            Assert.AreEqual((int) HttpStatusCode.NoContent, result.StatusCode);
        }
        
        [Test]
        public void DeleteProduct_ShouldFail_NotExisting()
        {
            var repository = GetGameRepository();
            var game = GetDemoGames()[0];
            repository.Add(game);

            var controller = new GameController(repository, userManager.Object);
            var result = controller.Delete(2) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
            Assert.AreEqual((int) HttpStatusCode.NotFound, result.StatusCode);
        }

        public static Game[] GetDemoGames()
        {
            return new Game[]
            {
                new Game()
                {
                    Id = 1, 
                    Team1 = TestTeamController.GetDemoTeams()[0], 
                    Team2 = TestTeamController.GetDemoTeams()[1], 
                    Team1Score = 4, 
                    Team2Score = 7, 
                    StartTime = new DateTime(2017, 1, 1, 2, 2, 2), 
                    EndTime = new DateTime(2017, 1, 1, 2, 3, 3),
                },
                new Game()
                {
                    Id = 2, 
                    Team1 = TestTeamController.GetDemoTeams()[1], 
                    Team2 = TestTeamController.GetDemoTeams()[2], 
                    Team1Score = 0, 
                    Team2Score = 7, 
                    StartTime = new DateTime(2017, 2, 1, 2, 3, 2), 
                    EndTime = new DateTime(2017, 1, 1, 2, 3, 3),
                },
                new Game()
                {
                    Id = 3, 
                    Team1 = TestTeamController.GetDemoTeams()[0], 
                    Team2 = TestTeamController.GetDemoTeams()[2], 
                    Team1Score = 7, 
                    Team2Score = 2, 
                    StartTime = new DateTime(1998, 12, 1, 23, 59, 2), 
                    EndTime = new DateTime(1998, 12, 2, 0, 3, 3),
                },
            };
        }
        
        public static Game[] GetWrongDemoGames()
        {
            return new Game[]
            {
                new Game() // wrong score
                {
                    Id = 1, 
                    Team1 = TestTeamController.GetDemoTeams()[0], 
                    Team2 = TestTeamController.GetDemoTeams()[1], 
                    Team1Score = 8, 
                    Team2Score = 7, 
                    StartTime = new DateTime(2017, 1, 1, 2, 2, 2), 
                    EndTime = new DateTime(2017, 1, 1, 2, 3, 3),
                },
                new Game() // wrong time
                {
                    Id = 2, 
                    Team1 = TestTeamController.GetDemoTeams()[1], 
                    Team2 = TestTeamController.GetDemoTeams()[2], 
                    Team1Score = 0, 
                    Team2Score = 7, 
                    StartTime = new DateTime(2017, 1, 1, 2, 3, 4), 
                    EndTime = new DateTime(2017, 1, 1, 2, 3, 3),
                },
                new Game() // same team
                {
                    Id = 3, 
                    Team1 = TestTeamController.GetDemoTeams()[0], 
                    Team2 = TestTeamController.GetDemoTeams()[0], 
                    Team1Score = 7, 
                    Team2Score = 2, 
                    StartTime = new DateTime(1998, 12, 1, 23, 59, 2), 
                    EndTime = new DateTime(1998, 12, 2, 0, 3, 3),
                },
            };
        }

        private IGameRepository GetGameRepository()
        {
            return new GameRepository(new TestWebServiceDbContext());
        }
    }
}