using Android;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Widget;
using foosballv2s.Droid.Shared;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Source.Activities.Listeners;
using Xamarin.Forms;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using View = Android.Views.View;

namespace foosballv2s.Source.Activities.Helpers
{
    /// <summary>
    /// Defines common behaviour of the navigation for all activities
    /// </summary>
    public class NavigationHelper
    {
        private static ICredentialStorage _credentialStorage;
        
        static NavigationHelper()
        {
            _credentialStorage = DependencyService.Get<ICredentialStorage>();
        }
        /// <summary>
        /// Sets up a navigation listener for an activity
        /// </summary>
        /// <param name="activity"></param>
        public static void SetupNavigationListener(Activity activity)
        {
            NavigationView navigationView = (NavigationView) activity.FindViewById(Resource.Id.navigation);
            navigationView.SetNavigationItemSelectedListener(new NavigationItemClickListener(activity));
            
            // set up navigation header
            View headerView = navigationView.InflateHeaderView(Resource.Layout.navigation_header);
            TextView emailView = (TextView) headerView.FindViewById(Resource.Id.navigation_email);
            emailView.Text = _credentialStorage.Read().Email;
        }
        
        /// <summary>
        /// Sets up an action bar for an activity
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="stringId"></param>
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