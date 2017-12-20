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

namespace foosballv2s.Source.Activities.Fragments
{
    class TournamentListFragment : Fragment
    {
        public ExpandableListView TournamentListView { get; set; }

        public TournamentListFragment()
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Inflate the layout for this fragment
            View v = inflater.Inflate(Resource.Layout.fragment_tournament_list, container, false);
            TournamentListView = (Android.Widget.ExpandableListView)v.FindViewById(Resource.Id.tournament_list_view);
            return v;

        }
    }
}