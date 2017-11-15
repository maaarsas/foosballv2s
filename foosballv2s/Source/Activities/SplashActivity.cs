using Android.App;
using Android.Content;
using Android.OS;

namespace foosballv2s.Source.Activities
{
    [Activity(MainLauncher = true, Theme = "@style/SplashTheme")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Start home activity
            StartActivity(new Intent(this, typeof(MainActivity)));
            // close splash activity
            Finish();
        }
    }
}
