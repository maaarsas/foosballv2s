using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Events;

namespace foosballv2s.Droid.Shared.Source.Services.GameLogger
{
    public class GameLogger : IGameLogger
    {
        private Game _game;
        
        public GameLogger(Game game)
        {
            _game = game;
        }
        
        public void LogStart(object sender, GameEventArgs args)
        {
            _game.GameEvents.Add(new GameEvent()
            {
                EventType = GameEvent.EventTypes.GameStart,
            });
        }
        
        public void LogGoal(object sender, GameEventArgs args)
        {
            _game.GameEvents.Add(new GameEvent()
            {
                Team = args.EventTeam,
                EventType = GameEvent.EventTypes.Goal,
            });
        }
        
        public void LogEnd(object sender, GameEventArgs args)
        {
            _game.GameEvents.Add(new GameEvent()
            {
                EventType = GameEvent.EventTypes.GameEnd,
            });
        }
    }
}