using Android.App;
using Android.OS;
using Android.Support.V7.App;
using foosballv2s.Source.Activities.Helpers;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for displaying all tournaments
    /// </summary>
    [Activity(ParentActivity=typeof(MainActivity))]
    public class TournamentsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            VolumeControlStream = Android.Media.Stream.Music;
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Tournaments);
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_tournaments);
        }
    }
}