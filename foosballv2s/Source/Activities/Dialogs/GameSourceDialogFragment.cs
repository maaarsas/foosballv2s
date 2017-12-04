using System;
using Android.App;
using Android.Content;
using Android.OS;
using foosballv2s.Source.Activities.Helpers;
using Java.Util;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace foosballv2s.Source.Activities.Dialogs
{
    public class GameSourceDialogFragment : Android.App.DialogFragment
    {
        private RecordingActivity currentActivity;

        public GameSourceDialogFragment() : base()
        {
            
        }
        
        public GameSourceDialogFragment(RecordingActivity activity)
        {
            currentActivity = activity;
        }
        
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            string[] sources = new string[]
            {
                Resources.GetString(Resource.String.source_video),
                Resources.GetString(Resource.String.source_live),
            };
            
            AlertDialog.Builder builder = new AlertDialog.Builder(currentActivity);
            builder.SetTitle(Resource.String.source_choose_title)
                .SetItems(sources, SourceClickAction);
            
            return builder.Create();
        }

        private void SourceClickAction(object sender, DialogClickEventArgs args)
        {
            this.Dismiss();
            
            switch (args.Which)
            {
                case 0:
                    currentActivity.AddVideoSource();
                    break;
                case 1:
                    currentActivity.AddLiveSource();
                    break;
            }
        }
    }
}