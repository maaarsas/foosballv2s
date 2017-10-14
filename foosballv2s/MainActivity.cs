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
        private String path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/previousnames.json";
        //private String file = "previousnames.json";
        private List<String> names = new List<String>();
        private String data;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            data = File.ReadAllText(path);

            if (!System.IO.File.Exists(path))
            {
                System.IO.FileStream fs = System.IO.File.Create(path);
            }
            else
            {
                names = JsonConvert.DeserializeObject<List<String>>(data);
            }

            var dataRead = names.ToArray();
            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.support_simple_spinner_dropdown_item, dataRead);

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

            //String path = folder + "/" + file;
            data = File.ReadAllText(path);

            if (!System.IO.File.Exists(path))
            {
                System.IO.FileStream fs = System.IO.File.Create(path);
            }
            else
            {
                names = JsonConvert.DeserializeObject<List<String>>(data);
            }

            team1text = (EditText)FindViewById<EditText>(Resource.Id.team1Name);
            team2text = (EditText)FindViewById<EditText>(Resource.Id.team2Name);
            team1name = team1text.Text;
            team2name = team2text.Text;

            names.Add(team1name);
            names.Add(team2name);

            data = JsonConvert.SerializeObject(names);
            File.WriteAllText(path, data);

            Console.WriteLine(data);

            StartActivity(intent);
        }



    }
}