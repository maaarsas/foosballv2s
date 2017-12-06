using Android.Speech.Tts;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Droid.Shared.Source.Services.TextToSpeech;
using Xamarin.Forms;

[assembly: Dependency(typeof(TextToSpeechImplementation))]
namespace foosballv2s.Droid.Shared.Source.Services.TextToSpeech
{

    public class TextToSpeechImplementation : Java.Lang.Object, ITextToSpeech, Android.Speech.Tts.TextToSpeech.IOnInitListener
    {
        Android.Speech.Tts.TextToSpeech speaker;
        string toSpeak;

        public void Welcome(string team1Name, string team2Name)
        {
            toSpeak = "Welcome " + team1Name + " and " + team2Name;

            CheckSpeaker(toSpeak);
        }

        public void OnGoal(string scoredTeam)
        {
            toSpeak = scoredTeam + " scored a goal";

            CheckSpeaker(toSpeak);
        }

        public void OnFinish(string winningTeam)
        {
            toSpeak = winningTeam + " has won!";

            CheckSpeaker(toSpeak);
        }

        public void CheckSpeaker(string toSpeak)
        {
            if (speaker == null && (DependencyService.Get<ICredentialStorage>().GetSavedLanguage().Equals(null) || DependencyService.Get<ICredentialStorage>().GetSavedLanguage() == "en"))
            {
                speaker = new Android.Speech.Tts.TextToSpeech(Forms.Context, this);
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
            else if (DependencyService.Get<ICredentialStorage>().GetSavedLanguage() == "en" || DependencyService.Get<ICredentialStorage>().GetSavedLanguage().Equals(null))
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }
    }
}