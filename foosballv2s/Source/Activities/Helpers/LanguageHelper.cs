using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Java.Util;
using Android.Content.Res;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Source.Activities.Dialogs;
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
            DialogFragment dialog = new LanguageDialogFragment(currentActivity, newActivity);
            dialog.Show(currentActivity.FragmentManager, "lang");
        }

        public static bool UpdateResources(Context context, string language)
        {
            var credentialStorage = DependencyService.Get<ICredentialStorage>();
            Locale locale = new Locale(language);
            Locale.Default = locale;

            Resources resources = context.Resources;

            Configuration configuration = resources.Configuration;
            configuration.Locale = locale;

            resources.UpdateConfiguration(configuration, resources.DisplayMetrics);
            credentialStorage.SaveLanguage(language);
            return true;
        }
    }
}