using Android.App;
using Android.OS;
using Android.Support.V7.App;
using foosballv2s.Source.Activities.Helpers;
using System;
using Android.Content;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for displaying all tournaments
    /// </summary>
    [Activity(ParentActivity = typeof(MainActivity))]
    public class TournamentsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Tournaments);
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_tournaments);

            var buttonSeeTournaments = FindViewById<Android.Widget.Button>(Resource.Id.see_tounaments);
            var buttonNewTournament = FindViewById<Android.Widget.Button>(Resource.Id.new_tournament);

            buttonSeeTournaments.Click += Btn_Click;
            buttonNewTournament.Click += Btn_Click;
        }

        public async void Btn_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ExistingTournamentsActivity));
            StartActivity(intent);
        }
    }
}