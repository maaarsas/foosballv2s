using foosballv2s.Source.Activities.Events;
using foosballv2s.Source.Entities;

namespace foosballv2s.Source.Services.GameLogger
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
                Game = _game,
                EventType = GameEvent.EventTypes.GameStart,
            });
        }
        
        public void LogGoal(object sender, GameEventArgs args)
        {
            _game.GameEvents.Add(new GameEvent()
            {
                Game = _game,
                Team = args.EventTeam,
                EventType = GameEvent.EventTypes.Goal,
            });
        }
        
        public void LogEnd(object sender, GameEventArgs args)
        {
            _game.GameEvents.Add(new GameEvent()
            {
                Game = _game,
                EventType = GameEvent.EventTypes.GameEnd,
            });
        }
    }
}