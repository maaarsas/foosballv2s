using Android;
using Android.App;
using Android.OS;
using foosballv2s.Droid.Shared;

namespace foosballv2s.Source.Activities
{
    [Activity(Label = "PreviousGamesActivity")]
    public class ColorPickerActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ColorPicker);
            
        }
    }
}