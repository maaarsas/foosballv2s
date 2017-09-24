using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Views;
using System;

namespace foosballXamarin
{
    [Activity(Label = "foosballXamarin", MainLauncher = true)]
    public class MainActivity : Activity
    {
        bool _previewing;
        Camera _camera;
        TextureView _textureView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.button1);
            _textureView = FindViewById<TextureView>(Resource.Id.textureView1);
            button.Click += delegate
            {
                try
                {
                    if (!_previewing)
                    {
                        _camera = Camera.Open();
                        _camera.SetPreviewTexture(_textureView.SurfaceTexture);
                        _camera.StartPreview();
                    }
                    else
                    {
                        _camera.StopPreview();
                        _camera.Release();
                    }
                }
                catch (Java.IO.IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    _previewing = !_previewing;
                }
            };
        }
    }
}
