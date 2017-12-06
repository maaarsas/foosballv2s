using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Helpers;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Repository;
using foosballv2s.Source.Activities.Adapters;
using foosballv2s.Source.Activities.Fragments;
using foosballv2s.Source.Activities.Helpers;
using Xamarin.Forms;
using Android.Widget;
using System.Linq;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for displaying a history of games
    /// </summary>
    [Activity(ParentActivity=typeof(MainActivity), 
        ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class GamesActivity : AppCompatActivity
    {
        private TabLayout tabLayout;
        private ViewPager viewPager;
        private GameRepository gameRepository;
        private ExpandableListView gameListView;
        private GameListFragment myGamesListFragment;
        private GameListFragment allGamesListFragment;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Games);
            
            myGamesListFragment = new GameListFragment();
            allGamesListFragment = new GameListFragment();
            
            viewPager = (ViewPager) FindViewById(Resource.Id.viewpager);
            setupViewPager(viewPager);
 
            tabLayout = (TabLayout) FindViewById(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_games);

            gameRepository = DependencyService.Get<GameRepository>();
            gameListView = (ExpandableListView) FindViewById(Resource.Id.game_list_view);
        }

        protected override void OnResume()
        {
            base.OnResume();
            
            FetchUserGames();
            FetchAllGames();
        }
        
        /// <summary>
        /// Retrieves a list of games from a web service and populates a list
        /// </summary>
        private async void FetchUserGames()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.retrieving_your_games), true);
            
            var credentialStorage = DependencyService.Get<ICredentialStorage>();
            
            UrlParamsFormatter urlParams = new UrlParamsFormatter();
            urlParams.AddParam("userid", credentialStorage.Read().Id);
            urlParams.AddParam("sortby", "-EndTime");
            
            Game[] games = await gameRepository.GetAll(urlParams.UrlParams);

            Dictionary<Game, List<GameEvent>> dict = new Dictionary<Game, List<GameEvent>>();
            List<GameEvent> evList = new List<GameEvent>();

            for (int i = 0; i < games.Length; i++)
            {
                evList = games[i].GameEvents.ToList();
                dict.Add(games[i], evList);
            }

            dialog.Dismiss();
            
            ExpandableListAdapter gameAdapter = new ExpandableListAdapter(this, new List<Game>(games), dict);
            myGamesListFragment.GameListView.Adapter = gameAdapter; //FIX
        }

        /// <summary>
        /// Retrieves a list of games from a web service and populates a list
        /// </summary>
        private async void FetchAllGames()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.retrieving_all_games), true);
            
            UrlParamsFormatter urlParams = new UrlParamsFormatter();
            urlParams.AddParam("sortby", "-EndTime");
            
            Game[] games = await gameRepository.GetAll(urlParams.UrlParams);

            Dictionary<Game, List<GameEvent>> dict = new Dictionary<Game, List<GameEvent>>();
            List<GameEvent> evList = new List<GameEvent>();
            for (int i = 0; i < games.Length; i++)
            {
                evList = games[i].GameEvents.ToList();
                dict.Add(games[i], evList);
            }

            dialog.Dismiss();
            
            ExpandableListAdapter gameAdapter = new ExpandableListAdapter(this, new List<Game>(games), dict);
            allGamesListFragment.GameListView.Adapter = gameAdapter;
        }
        
        private void setupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            adapter.addFragment(myGamesListFragment, Resources.GetString(Resource.String.my_games));
            adapter.addFragment(allGamesListFragment, Resources.GetString(Resource.String.all_games));
            viewPager.Adapter = adapter;
        }
    }
}