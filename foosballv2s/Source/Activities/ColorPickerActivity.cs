using Color = Android.Graphics.Color;
using ListView = Android.Widget.ListView;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using foosballv2s.Source.Activities.Adapters;
using foosballv2s.Source.Entities;
using foosballv2s.Source.Services.FileIO;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Emgu.CV.Structure;
using Android.Content;
﻿using Android;
using Android.App;
using Android.OS;
using foosballv2s.Droid.Shared;

namespace foosballv2s.Source.Activities
{
    [Activity(Label = "PreviousGamesActivity")]
    public class ColorPickerActivity : Activity
    {
        private List<Hsv> cList;
        private ListView cListView;
        private IO instance = new IO();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ColorPicker);

            cListView = FindViewById<ListView>(Resource.Id.color_listView);

            cList = instance.Read_Deserialize_Color();

            ColorListAdapter adapter = new ColorListAdapter(this, cList);

            String test = cList[0].ToString();

            cListView.Adapter = adapter;
            cListView.ItemClick += CListView_ItemClick;
        }

        private void CListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Game game = DependencyService.Get<Game>();

            game.BallColor = cList[e.Position];

            Intent intent = new Intent(this, typeof(RecordingActivity));
            StartActivity(intent);
        }
    }
}