using Android.App;
using Android.OS;
using Android.Content;

namespace foosballv2s
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
