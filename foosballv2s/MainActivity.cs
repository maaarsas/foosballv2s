using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using System;
using Android;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Provider;
using Android.Runtime;
using Emgu.CV.Structure;
using Java.Interop;
using Xamarin.Forms;
using View = Android.Views.View;

namespace foosballv2s
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            
            DependencyService.Register<Game>();
            
            //Window.SetBackgroundDrawable(Android.Resource.Id.);
        }

        [Export("SubmitTeamNames")]
        public void SubmitTeamNames(View view)
        {
            //TODO: validate team names with Regex
            Intent intent = new Intent(this, typeof(BallImageActivity));
            StartActivity(intent);
        }

    }
}