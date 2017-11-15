using Android.App;
using Android.Support.Design.Widget;
using Android.Widget;
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

        public static void SetActionBarNavigationText(Activity activity, int stringId)
        {
            var toolbar = (Toolbar) activity.FindViewById(Resource.Id.toolbar);
            activity.SetActionBar(toolbar);
            activity.ActionBar.Title = activity.Resources.GetString(stringId);
            activity.ActionBar.SetDisplayHomeAsUpEnabled(true);
        }
    }
}