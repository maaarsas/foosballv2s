using System;
using Android.App;
using Android.Content;
using Android.OS;
using foosballv2s.Source.Activities.Helpers;
using Java.Util;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace foosballv2s.Source.Activities.Dialogs
{
    public class LanguageDialogFragment : Android.App.DialogFragment
    {
        private Activity currentActivity;
        private Type activityType;

        public LanguageDialogFragment(Activity activity, Type type)
        {
            currentActivity = activity;
            activityType = type;
        }
        
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            string[] languages = new string[]
            {
                Resources.GetString(Resource.String.lang_en),
                Resources.GetString(Resource.String.lang_lt),
            };
            
            AlertDialog.Builder builder = new AlertDialog.Builder(currentActivity);
            builder.SetTitle(Resource.String.choose_lang)
                .SetItems(languages, LanguageClickAction);
            
            return builder.Create();
        }

        private void LanguageClickAction(object sender, DialogClickEventArgs args)
        {
            Intent intent = new Intent(currentActivity, activityType);
            this.Dismiss();
            
            switch (args.Which)
            {
                case 0:
                    if (Locale.Default.Language.Equals("en"))
                        break;
                    LanguageHelper.UpdateResources(currentActivity, "en");
                    break;
                case 1:
                    if (Locale.Default.Language.Equals("lt"))
                        break;
                    LanguageHelper.UpdateResources(currentActivity, "lt");
                    break;
            }
            currentActivity.StartActivity(intent);
            currentActivity.Finish();
        }
    }
}