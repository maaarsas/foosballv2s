using Android.OS;
using Android.Views;
using Xamarin.Forms;
using Fragment = Android.Support.V4.App.Fragment;
using ListView = Android.Widget.ListView;
using View = Android.Views.View;

namespace foosballv2s.Source.Activities.Fragments
{
    public class TeamListFragment : Fragment
    {
        public ListView TeamListView { get; private set; }
        
        public TeamListFragment() {
        }
 
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Inflate the layout for this fragment
            View v =  inflater.Inflate(Resource.Layout.fragment_team_list, container, false);
            TeamListView = (Android.Widget.ListView) v.FindViewById(Resource.Id.team_list_view);
            return v;
        }
    }
}