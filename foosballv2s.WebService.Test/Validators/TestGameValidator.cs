using System;
using foosballv2s.WebService.Models;
using foosballv2s.WebService.Validators;
using NUnit.Framework;

namespace foosballv2s.WebService.Test.Validators
{
    [TestFixture]
    public class TestGameValidator
    {
        private static Team[] teams =
        {
            new Team() {Id = 1, TeamName = "Test1"},
            new Team() {Id = 2, TeamName = "Test2"},
        };

        private static DateTime[] times =
        {
            new DateTime(2017, 11, 10, 12, 34, 56),
            new DateTime(2017, 11, 10, 12, 35, 56),
            DateTime.Now.Add(new TimeSpan(0, 15, 0)), // 15 minutes from now
        };
        
        public static object[] validationGameCases =
        {
            new object[] {new Game {Team1 = teams[0], Team2 = teams[1], Team1Score = 0, Team2Score = 7, 
                StartTime = times[0], EndTime = times[1]}, true}, // everything correct
            
            new object[] {new Game {Team1 = teams[0], Team2 = teams[0], Team1Score = 0, Team2Score = 7, 
                StartTime = times[0], EndTime = times[1]}, false}, // same teams
            
            new object[] {new Game {Team1 = null, Team2 = teams[1], Team1Score = 0, Team2Score = 7, 
                StartTime = times[0], EndTime = times[1]}, false}, // non existing teams
            
            new object[] {new Game {Team1 = teams[0], Team2 = teams[1], Team1Score = -1, Team2Score = 8, 
                StartTime = times[0], EndTime = times[1]}, false}, // score out of boundaries
            
            new object[] {new Game {Team1 = teams[0], Team2 = teams[1], Team1Score = 6, Team2Score = 6, 
                StartTime = times[0], EndTime = times[1]}, false}, // none of teams reached max score
            
            new object[] {new Game {Team1 = teams[0], Team2 = teams[1], Team1Score = 0, Team2Score = 7, 
                StartTime = times[1], EndTime = times[0]}, false}, // ending time earlier than beginning
            
            new object[] {new Game {Team1 = teams[0], Team2 = teams[1], Team1Score = 0, Team2Score = 7, 
                StartTime = times[2], EndTime = times[1]}, false}, // start time is in the future
            
            new object[] {new Game {Team1 = teams[0], Team2 = teams[1], Team1Score = 0, Team2Score = 7, 
                StartTime = times[0], EndTime = times[2]}, false}, // end time is in the future
        };
        
        [Test]
        [TestCaseSource("validationGameCases")]
        public void TestValidate(Game game, bool expectedResult)
        {
            IValidator gameValidator = new GameValidator(game);

            bool result = gameValidator.Validate();

            Assert.AreEqual(expectedResult, result);
        }
        
        
        
    }
}