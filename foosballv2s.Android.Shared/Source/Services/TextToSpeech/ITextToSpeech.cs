namespace foosballv2s.Droid.Shared.Source.Services.TextToSpeech
{
    public interface ITextToSpeech
    {
        void Welcome(string team1Name, string team2Name);
        void OnGoal(string scoredTeam);
        void OnFinish(string winningTeam);
    }
}