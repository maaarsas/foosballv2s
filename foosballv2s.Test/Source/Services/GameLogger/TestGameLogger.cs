using System.Linq;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Events;
using foosballv2s.Droid.Shared.Source.Services.GameLogger;
using NUnit.Framework;

namespace foosballv2s.Test.Source.Services.GameLogger
{
    [TestFixture]
    public class TestGameLogger
    {
        [Test]
        public void TestLogStart()
        {
            var game = new Game();
            var logger = new Droid.Shared.Source.Services.GameLogger.GameLogger(game);
            logger.LogStart(this, new GameEventArgs(game, null));
            
            Assert.AreEqual(1, game.GameEvents.Count);
            Assert.AreEqual(GameEvent.EventTypes.GameStart, game.GameEvents.First().EventType);

        }
        
        [Test]
        public void TestLogGoal()
        {
            var game = new Game();
            var logger = new Droid.Shared.Source.Services.GameLogger.GameLogger(game);
            logger.LogGoal(this, new GameEventArgs(game, new Team()));
            
            Assert.AreEqual(1, game.GameEvents.Count);
            Assert.AreEqual(GameEvent.EventTypes.Goal, game.GameEvents.First().EventType);
        }
        
        [Test]
        public void TestLogEnd()
        {
            var game = new Game();
            var logger = new Droid.Shared.Source.Services.GameLogger.GameLogger(game);
            logger.LogEnd(this, new GameEventArgs(game, null));
            
            Assert.AreEqual(1, game.GameEvents.Count);
            Assert.AreEqual(GameEvent.EventTypes.GameEnd, game.GameEvents.First().EventType);
        }
    }
}