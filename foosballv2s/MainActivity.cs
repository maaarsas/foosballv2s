using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Views;
using System;
using Android;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Android.Runtime;
using Emgu.CV.Structure;

namespace foosballv2s
{
    [Activity(Label = "foosballv2s", MainLauncher = true)]
    public class MainActivity : Activity
    {
        ImageView img;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            var btnNewGame = FindViewById<Button>(Resource.Id.buttonNew);
            img = FindViewById<ImageView>(Resource.Id.Review);

            btnNewGame.Click += BtnNewGame_Click;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Bitmap bmp = (Bitmap)data.Extras.Get("data");
            img.SetImageBitmap(bmp);
        }

        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }
    }
}