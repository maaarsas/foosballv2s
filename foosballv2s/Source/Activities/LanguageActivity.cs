using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using foosballv2s.Source.Activities.Helpers;
using Java.Interop;
using Java.Util;

namespace foosballv2s.Source.Activities
{
    [Activity(ParentActivity = typeof(MainActivity))]
    public class LanguageActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Language);
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_language);
            
        }
        [Export("English")]
        public void English(View view)
        {
            updateResources(this, "en");
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
        [Export("Lithuanian")]
        public void Lithuanian(View view)
        {
            updateResources(this, "lt");
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
        private static bool updateResources(Context context, string language)
        {
            Locale locale = new Locale(language);
            Locale.Default = locale;

            Resources resources = context.Resources;

            Configuration configuration = resources.Configuration;
            configuration.Locale = locale;

            resources.UpdateConfiguration(configuration, resources.DisplayMetrics);

            return true;
        }
    }
}