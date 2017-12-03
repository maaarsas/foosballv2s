using Android;
using Android.App;
using Android.Content;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using foosballv2s.Droid.Shared;
using foosballv2s.Source.Services.CredentialStorage;
using Xamarin.Forms;
using Application = Android.App.Application;

namespace foosballv2s.Source.Activities.Listeners
{
    /// <summary>
    /// A listener for reacting to navigation item's click events
    /// </summary>
    public class NavigationItemClickListener : Java.Lang.Object, NavigationView.IOnNavigationItemSelectedListener
    {
        private Activity currentActivity;
        private NavigationMenuView menu;

        public NavigationItemClickListener(Activity activity)
        {
            currentActivity = activity;
        }
        
        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            System.Type intentType;

            if (menuItem.IsChecked)
            {
                menuItem.SetChecked(false);
            }
            else
            {
                menuItem.SetChecked(true);
            }
            
            currentActivity.FindViewById<DrawerLayout>(Resource.Id.drawer_layout).CloseDrawers();
            
            if (menuItem.ItemId == Resource.Id.nav_teams)
            {
                intentType = typeof(TeamsActivity);
            }
            else if (menuItem.ItemId == Resource.Id.nav_games)
            {
                intentType = typeof(GamesActivity);
            }
            else if (menuItem.ItemId == Resource.Id.nav_tournaments)
            {
                intentType = typeof(TournamentsActivity);
            }
            else if (menuItem.ItemId == Resource.Id.nav_logout)
            {
                ICredentialStorage storage = DependencyService.Get<ICredentialStorage>();
                storage.Remove();
                intentType = typeof(AuthActivity);
            }
            else
            {
                intentType = typeof(MainActivity);
            }
            Intent intent = new Intent(currentActivity, intentType);
            intent.AddFlags(ActivityFlags.NoAnimation);
            currentActivity.StartActivity(intent);
            currentActivity.Finish();
            return true;
        }
    }
}