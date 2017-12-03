using Android;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
<<<<<<< HEAD
using foosballv2s.Droid.Shared;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
=======
using Android.Widget;
using foosballv2s.Source.Services.CredentialStorage;
using Java.Util;
using System;
>>>>>>> 5ac87cd210ad821d5197c8e764172d5e832ed9f2
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