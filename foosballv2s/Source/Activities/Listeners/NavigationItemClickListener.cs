using Android.App;
using Android.Content;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
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
            
            switch (menuItem.ItemId)
            {
                case Resource.Id.nav_teams:
                {
                    intentType = typeof(TeamsActivity);
                    break;
                }
                case Resource.Id.nav_games:
                {
                    intentType = typeof(GamesActivity);
                    break;
                }
                case Resource.Id.nav_tournaments:
                {
                    intentType = typeof(TournamentsActivity);
                    break;
                }
                case Resource.Id.nav_logout:
                {
                    ICredentialStorage storage = DependencyService.Get<ICredentialStorage>();
                    storage.Remove();
                    intentType = typeof(AuthActivity);
                    break;
                }
                default:
                {
                    intentType = typeof(MainActivity);
                    break;
                }
            }
            Intent intent = new Intent(currentActivity, intentType);
            intent.AddFlags(ActivityFlags.NoAnimation);
            currentActivity.StartActivity(intent);
            currentActivity.Finish();
            return true;
        }
    }
}