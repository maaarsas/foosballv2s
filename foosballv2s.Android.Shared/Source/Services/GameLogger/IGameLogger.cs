using foosballv2s.Droid.Shared.Source.Events;

namespace foosballv2s.Droid.Shared.Source.Services.GameLogger
{
    public interface IGameLogger
    {
        void LogStart(object sender, GameEventArgs args);
        void LogGoal(object sender, GameEventArgs args);
        void LogEnd(object sender, GameEventArgs args);
    }
}