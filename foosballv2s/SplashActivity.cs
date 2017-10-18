using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Views;
using System;
using Android;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Android.Runtime;
using Emgu.CV.Structure;

namespace foosballv2s
{
    [Activity(MainLauncher = true, Theme = "@style/SplashTheme")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Start home activity
            StartActivity(new Intent(this, typeof(MainActivity)));
            // close splash activity
            Finish();
        }
    }
}
