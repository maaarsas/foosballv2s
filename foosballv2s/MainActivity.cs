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
        private AutoCompleteTextView t1, t2;
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

            if (!System.IO.File.Exists(path))
            {
                System.IO.FileStream fs = System.IO.File.Create(path);
                fs.Dispose();
            }
            else
            {
                data = File.ReadAllText(path);
                names = JsonConvert.DeserializeObject<List<String>>(data);

                //Need help with this IF clause. Need to recreate list if file exists and is empty. Solutions?

                if (names == null)
                {
                    names = new List<String>();
                    names.Add("Team 1");
                    names.Add("Team 2");
                }
            }

            t1 = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            t2 = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);

            var dataRead = names.ToArray();

            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.support_simple_spinner_dropdown_item, dataRead);

            t1.Adapter = adapter;
            t2.Adapter = adapter;

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

            team1text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            team2text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);
            team1name = team1text.Text;
            team2name = team2text.Text;

            int flag1 = 0;
            int flag2 = 0;
            foreach (var name in names)
            {
                if(team1name == name)
                {
                    flag1 = 1;
                }
                if(team2name == name)
                {
                    flag2 = 1;
                }
            }

            if (flag1 == 0)
            {
                names.Add(team1name);
            }
            if (flag2 == 0)
            {
                names.Add(team2name);
            }
            

            data = JsonConvert.SerializeObject(names);
            File.WriteAllText(path, data);

            Console.WriteLine(data);

            StartActivity(intent);
        }



    }
}