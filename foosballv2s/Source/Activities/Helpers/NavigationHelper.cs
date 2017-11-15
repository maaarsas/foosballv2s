using Android.App;
using Android.Support.Design.Widget;
using foosballv2s.Listeners;

namespace foosballv2s
{
    public class NavigationHelper
    {
        public static void SetupNavigationListener(Activity activity)
        {
            NavigationView navigationView = (NavigationView) activity.FindViewById(Resource.Id.navigation);
            navigationView.SetNavigationItemSelectedListener(new NavigationItemClickListener(activity));
        }
    }
}