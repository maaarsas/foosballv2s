using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using foosballv2s.Source.Activities.Adapters;
using foosballv2s.Source.Services.FileIO;
using System.Collections.Generic;

namespace foosballv2s.Source.Activities
{
    [Activity(Label = "PreviousGamesActivity")]
    public class ColorPickerActivity : Activity
    {
        private List<Color> cList;
        private ListView cListView;
        private IO instance = new IO();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ColorPicker);

            cListView = FindViewById<ListView>(Resource.Id.color_listView);

            cList = instance.Read_Deserialize_Color();

            ColorListAdapter adapter = new ColorListAdapter(this, cList);

            cListView.Adapter = adapter;
        }
    }
}