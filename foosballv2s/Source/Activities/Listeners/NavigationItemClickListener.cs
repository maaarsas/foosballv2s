using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using foosballv2s.Source.Services.CredentialStorage;
using Java.Util;
using System;
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
        private RadioGroup languages;
        private Android.Widget.Button btnSumbitLanguage;
        private Dialog dialog;

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

            if (menuItem.ItemId == Resource.Id.nav_language)
            {
                Helpers.LanguageHelper.ChangeLanguage(currentActivity, typeof(MainActivity));
            }
            else
            {
                Intent intent = new Intent(Application.Context, intentType);
                currentActivity.StartActivity(intent);
                currentActivity.Finish();
            }
            return true;
        } 
    }
}