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
                dialog = new Dialog(currentActivity);
                dialog.SetContentView(Resource.Layout.language_spinner);
                dialog.SetTitle(Resource.String.choose_lang);
                dialog.SetCancelable(false);
                dialog.Show();

                languages = (RadioGroup)dialog.FindViewById(Resource.Id.languages);
                btnSumbitLanguage = dialog.FindViewById<Android.Widget.Button>(Resource.Id.btnSumbitLanguage);
                btnSumbitLanguage.Click += btnSumbitLanguage_Click;
            }
            else
            {
                Intent intent = new Intent(Application.Context, intentType);
                currentActivity.StartActivity(intent);
                currentActivity.Finish();
            }
            return true;
        }

        private void btnSumbitLanguage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(currentActivity, typeof(MainActivity));
            switch (languages.CheckedRadioButtonId)
            {
                case Resource.Id.language1:
                    if (Locale.Default.Language.Equals("lt"))
                        break;
                    UpdateResources(currentActivity, "lt");
                    break;
                case Resource.Id.language2:
                    if (Locale.Default.Language.Equals("en"))
                        break;
                    UpdateResources(currentActivity, "en");
                    break;
            }
            dialog.Dismiss();
            currentActivity.StartActivity(intent);
            currentActivity.Finish();
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