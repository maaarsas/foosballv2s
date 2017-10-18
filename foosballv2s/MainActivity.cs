using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using System;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Provider;
using Android.Runtime;
using Emgu.CV.Structure;
using Java.Interop;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Java.Interop;
using Xamarin.Forms;
using View = Android.Views.View;

namespace foosballv2s
{
    [Activity(
        ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait
        )]
    public class MainActivity : Activity
    {
        private AutoCompleteTextView t1, t2, team1text, team2text;
        IO instance = new IO();
        
        private Game game;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Main);
            
            DependencyService.Register<Game>();
            game = DependencyService.Get<Game>();
            
            t1 = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            t2 = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);

            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.support_simple_spinner_dropdown_item, instance.Read_Deserialize());

            t1.Adapter = adapter;
            t2.Adapter = adapter;

            /*var btnP = FindViewById<Button>(Resource.Id.prev);
            btnP.Click += BtnPrev_Click;*/
            
            //Window.SetBackgroundDrawable(Android.Resource.Id.);
        }

        /*private void BtnPrev_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PreviousGamesActivity));

            StartActivity(intent);
        }*/

        [Export("SubmitTeamNames")]
        public void SubmitTeamNames(View view)
        {
            Validator v = new Validator();
            EditText mteam1Name = (EditText)FindViewById(Resource.Id.team1Name);
            EditText mteam2Name = (EditText)FindViewById(Resource.Id.team2Name);
            if (!v.Validate(mteam1Name.Text) || !v.Validate(mteam2Name.Text))
            {
                Toast.MakeText(this, Resource.String.wrong_team_names, ToastLength.Short);
                return;
            }   
            game.Team1.TeamName = mteam1Name.Text;
            game.Team2.TeamName = mteam2Name.Text;
            
            Intent intent = new Intent(this, typeof(BallImageActivity));

            team1text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            team2text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);

            instance.Write_Serialize(team1text, team2text);

            StartActivity(intent);
        }
    }
}