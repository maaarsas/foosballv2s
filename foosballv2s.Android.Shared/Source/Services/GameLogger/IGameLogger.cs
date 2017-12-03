using foosballv2s.Source.Activities.Events;

namespace foosballv2s.Source.Services.GameLogger
{
    public interface IGameLogger
    {
        void LogStart(object sender, GameEventArgs args);
        void LogGoal(object sender, GameEventArgs args);
        void LogEnd(object sender, GameEventArgs args);
    }
}