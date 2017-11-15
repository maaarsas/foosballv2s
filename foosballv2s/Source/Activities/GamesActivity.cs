using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using foosballv2s.Source.Activities.Adapters;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using Xamarin.Forms;
using ListView = Android.Widget.ListView;

namespace foosballv2s
{
    [Activity(ParentActivity=typeof(MainActivity))]
    public class GamesActivity : AppCompatActivity
    {
        private GameRepository gameRepository;
        private ListView gameListView;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Games);
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_games);

            gameRepository = DependencyService.Get<GameRepository>();
            gameListView = (ListView) FindViewById(Resource.Id.game_list_view);

            PopulateGames();
        }

        private async void PopulateGames()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.retrieving_games), true);
            
            Game[] games = await gameRepository.GetAll();

            dialog.Dismiss();
            
            GameAdapter gameAdapter = new GameAdapter(this, new List<Game>(games));
            gameListView.Adapter = gameAdapter;
        }
    }
}