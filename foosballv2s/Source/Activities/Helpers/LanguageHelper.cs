using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Java.Util;
using Android.Content.Res;
using foosballv2s.Source.Services.CredentialStorage;
using Xamarin.Forms;
namespace foosballv2s.Source.Activities.Helpers
{
    public class LanguageHelper
    {
        private static RadioGroup languages;
        private static Dialog dialog;
        private static Activity _currentActivity;
        private static Type _newActivity;

        public static void ChangeLanguage(Activity currentActivity, Type newActivity)
        {
            _currentActivity = currentActivity;
            _newActivity = newActivity;
            dialog = new Dialog(currentActivity);
            dialog.SetContentView(Resource.Layout.language_spinner);
            dialog.SetTitle(Resource.String.choose_lang);
            dialog.SetCancelable(false);
            dialog.Show();

            languages = (RadioGroup)dialog.FindViewById(Resource.Id.languages);
            Android.Widget.Button btnSumbitLanguage = dialog.FindViewById<Android.Widget.Button>(Resource.Id.btnSumbitLanguage);
            btnSumbitLanguage.Click += BtnSumbitLanguage_Click;
        }

        private static void BtnSumbitLanguage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(_currentActivity, _newActivity);
            dialog.Dismiss();
            var credentialStorage = DependencyService.Get<ICredentialStorage>();
            switch (languages.CheckedRadioButtonId)
            {
                case Resource.Id.language1:
                    if (Locale.Default.Language.Equals("lt"))
                        break;
                    UpdateResources(_currentActivity, "lt");
                    credentialStorage.SaveLanguage("lt");
                    break;
                case Resource.Id.language2:
                    if (Locale.Default.Language.Equals("en"))
                        break;
                    UpdateResources(_currentActivity, "en");
                    credentialStorage.SaveLanguage("en");
                    break;
            }
            _currentActivity.StartActivity(intent);
            _currentActivity.Finish();
        }

        public static bool UpdateResources(Context context, string language)
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