using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;

namespace foosballv2s.Source.Activities.Fragments
{
    public class GameListFragment : Fragment
    {
        public ListView GameListView { get; private set; }
        
        public GameListFragment() {
        }
 
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Inflate the layout for this fragment
            View v = inflater.Inflate(Resource.Layout.fragment_game_list, container, false);
            GameListView = (Android.Widget.ListView) v.FindViewById(Resource.Id.game_list_view);
            return v;
        }
    }
}