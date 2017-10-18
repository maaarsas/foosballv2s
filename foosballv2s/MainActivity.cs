using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Android.Content.PM;
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
        private IO instance = new IO();
        private Game game;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Main);
            
            DependencyService.Register<Game>();
            game = DependencyService.Get<Game>();
            
            t1 = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            t2 = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);

            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.support_simple_spinner_dropdown_item, instance.Read_Deserialize());

            t1.Adapter = adapter;
            t2.Adapter = adapter;

            var btnP = FindViewById<Android.Widget.Button>(Resource.Id.prev);
            btnP.Click += BtnPrev_Click;
            
            //Window.SetBackgroundDrawable(Android.Resource.Id.);
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ColorPickerActivity));

            StartActivity(intent);
        }

        [Export("SubmitTeamNames")]
        public void SubmitTeamNames(View view)
        {
            Validator v = new Validator();
            Intent intent = new Intent(this, typeof(BallImageActivity));

            team1text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            team2text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);

            if (!v.Validate(team1text.Text) || !v.Validate(team2text.Text))
            {
                Toast.MakeText(this, Resource.String.wrong_team_names, ToastLength.Short);
                return;
            }

            game.Team1.TeamName = team1text.Text;
            game.Team2.TeamName = team2text.Text;

            instance.Write_Serialize(team1text, team2text);

            StartActivity(intent);
        }
    }
}