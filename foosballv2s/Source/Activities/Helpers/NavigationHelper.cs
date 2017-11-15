using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V7.App; 
using Android.Support.V4.Widget;
using Android.Widget;
using foosballv2s.Listeners;
using V7Toolbar = Android.Support.V7.Widget.Toolbar; 

namespace foosballv2s
{
    public class NavigationHelper
    {
        public static void SetupNavigationListener(Activity activity)
        {
            NavigationView navigationView = (NavigationView) activity.FindViewById(Resource.Id.navigation);
            navigationView.SetNavigationItemSelectedListener(new NavigationItemClickListener(activity));
        }

        public static void SetActionBarNavigationText(AppCompatActivity activity, int stringId)
        {
            var toolbar = (V7Toolbar) activity.FindViewById(Resource.Id.toolbar);
            activity.SetSupportActionBar(toolbar);
            activity.SupportActionBar.Title = activity.Resources.GetString(stringId);
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            
            var v7Toolbar = activity.FindViewById<V7Toolbar>(Resource.Id.toolbar);
            
            DrawerLayout drawerLayout = (DrawerLayout) activity.FindViewById(Resource.Id.drawer_layout);
            ActionBarDrawerToggle actionBarDrawerToggle = new ActionBarDrawerToggle(
                activity, drawerLayout, v7Toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
 
            //Setting the actionbarToggle to drawer layout
            drawerLayout.AddDrawerListener(actionBarDrawerToggle);
         
            //calling sync state is necessay or else your hamburger icon wont show up
            actionBarDrawerToggle.SyncState();
        }
    }
}