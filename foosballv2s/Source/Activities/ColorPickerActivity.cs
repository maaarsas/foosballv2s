using Android.App;
using Android.OS;

namespace foosballv2s.Source.Activities
{
    [Activity(Label = "PreviousGamesActivity")]
    public class ColorPickerActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            VolumeControlStream = Android.Media.Stream.Music;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ColorPicker);
            
        }
    }
}