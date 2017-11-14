using foosballv2s.WebService.Models;
using foosballv2s.WebService.Validators;
using NUnit.Framework;

namespace foosballv2s.WebService.Test.Validators
{
    [TestFixture]
    public class TestTeamValidator
    {
        public static object[] validationTeamCases =
        {
            new object[] {new Team {TeamName = "Te"}, true},
            new object[] {new Team {TeamName = " T"}, false},
            new object[] {new Team {TeamName = "0a"}, false},
            new object[] {new Team {TeamName = "L"}, false},
            new object[] {new Team {TeamName = "TestTestTestTestTest"}, false},
            new object[] {new Team {TeamName = "TestTestTestTestTes"}, true},
            new object[] {new Team {TeamName = "TestTest;Test"}, false},
            new object[] {new Team {TeamName = "Test0999"}, true},
            new object[] {new Team {TeamName = "234Abc456"}, true},
            new object[] {new Team {TeamName = ""}, false},
        };
        
        [Test]
        [TestCaseSource("validationTeamCases")]
        public void TestValidate(Team team, bool expectedResult)
        {
            IValidator teamValidator = new TeamValidator(team);

            bool result = teamValidator.Validate();

            Assert.AreEqual(expectedResult, result);
        }
        
        
        
    }
}