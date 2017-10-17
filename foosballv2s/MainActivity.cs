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
        private EditText mteam1Name;
        private EditText mteam2Name;

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
            Validator v = new Validator();
            mteam1Name = (EditText)FindViewById(Resource.Id.team1Name);
            mteam2Name = (EditText)FindViewById(Resource.Id.team2Name);
            if (!v.Validate(mteam1Name.Text) || !v.Validate(mteam2Name.Text))
            {
                SetContentView(Resource.Layout.Main);
                //change hint text that team name format is not correct
            }
            else
            {
                Team team1 = new Team();
                Team team2 = new Team();
                team1.teamName = mteam1Name.Text;
                team2.teamName = mteam2Name.Text;
                Intent intent = new Intent(this, typeof(BallImageActivity));
                StartActivity(intent);
            }          
        }
    }
}