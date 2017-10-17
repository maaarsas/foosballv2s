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
        private AutoCompleteTextView t1, t2, team1text, team2text;
        IO instance = new IO();

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            t1 = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            t2 = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);

            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.support_simple_spinner_dropdown_item, instance.Read_Deserialize());

            t1.Adapter = adapter;
            t2.Adapter = adapter;

            /*var btnP = FindViewById<Button>(Resource.Id.prev);
            btnP.Click += BtnPrev_Click;*/

        }

        /*private void BtnPrev_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PreviousGamesActivity));

            StartActivity(intent);
        }*/

        [Export("SubmitTeamNames")]
        public void SubmitTeamNames(View view)
        {
            //TODO: validate team names with Regex

            Intent intent = new Intent(this, typeof(BallImageActivity));

            team1text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            team2text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);

            instance.Write_Serialize(team1text, team2text);

            StartActivity(intent);
        }
    }
}