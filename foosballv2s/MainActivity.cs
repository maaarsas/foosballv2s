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
using Java.Interop;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace foosballv2s
{
    [Activity()]
    public class MainActivity : Activity
    {
        private EditText team1text, team2text;
        private String team1name, team2name;
        private String folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); //Possibly? Android.OS.Environment.DataDirectory.AbsolutePath;
        private String file = "previousnames.json";
        private List<String> names = new List<String>();
        
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            team1text = FindViewById<EditText>(Resource.Id.team1Name);
            team2text = FindViewById<EditText>(Resource.Id.team2Name);
            team1name = team1text.ToString();
            team2name = team2text.ToString();

            names.Add(team1name);
            names.Add(team2name);

            String path = folder + "/" + file;
            String data = JsonConvert.SerializeObject(names);
            File.WriteAllText(path, data);

            var btnP = FindViewById<Button>(Resource.Id.prev);
            btnP.Click += BtnPrev_Click;

        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PreviousGamesActivity));
            StartActivity(intent);
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