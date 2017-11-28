using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using foosballv2s.Source.Services.CredentialStorage;
using Java.Util;
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

            if (menuItem.ItemId == Resource.Id.nav_language)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(currentActivity);
                Intent intent;
                builder.SetTitle(Resource.String.choose_lang);
                builder.SetCancelable(false);
                builder.SetPositiveButton(Resource.String.item_lt, delegate
                {
                    UpdateResources(currentActivity, "lt");
                    intent = new Intent(Application.Context, intentType);
                    currentActivity.StartActivity(intent);
                    currentActivity.Finish();
                });
                builder.SetNegativeButton(Resource.String.item_en, delegate
                {
                    UpdateResources(currentActivity, "en");
                    intent = new Intent(Application.Context, intentType);
                    currentActivity.StartActivity(intent);
                    currentActivity.Finish();
                });
                AlertDialog alertDialog = builder.Create();
                alertDialog.Show();
            }
            else
            {
                Intent intent = new Intent(Application.Context, intentType);
                currentActivity.StartActivity(intent);
                currentActivity.Finish();   
            }
            return true;
        }

        private static bool UpdateResources(Context context, string language)
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