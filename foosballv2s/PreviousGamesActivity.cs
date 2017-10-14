using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.IO;

namespace foosballv2s
{
    [Activity(Label = "PreviousGamesActivity")]
    public class PreviousGamesActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PreviousGames);

            string path = (String)System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
                + "/previousnames.json";

            string jsonObj = File.ReadAllText(path);

            List<String> names = new List<String>();
            names = JsonConvert.DeserializeObject<List<String>>(jsonObj);
        }
    }
}