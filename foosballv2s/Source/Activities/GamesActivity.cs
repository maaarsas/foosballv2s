using System.Collections.Generic;
using Android;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using foosballv2s.Droid.Shared;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Repository;
using foosballv2s.Source.Activities.Adapters;
using foosballv2s.Source.Activities.Helpers;
using Xamarin.Forms;
using ListView = Android.Widget.ListView;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for displaying a history of games
    /// </summary>
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

            FetchGames();
        }

        /// <summary>
        /// Retrieves a list of games from a web service and populates a list
        /// </summary>
        private async void FetchGames()
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