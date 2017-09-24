using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Views;
using System;
using Android.Graphics;

namespace foosballXamarin
{
    [Activity(Label = "foosballXamarin", MainLauncher = true)]
    public class MainActivity : Activity, TextureView.ISurfaceTextureListener
    {
        private Android.Hardware.Camera camera;
        private TextureView textureView;
        private SurfaceView surfaceView;
        private ISurfaceHolder holder;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            textureView = (TextureView)FindViewById(Resource.Id.textureView);
            textureView.SurfaceTextureListener = this;

            surfaceView = (SurfaceView)FindViewById(Resource.Id.surfaceView);
            //set to top layer
            surfaceView.SetZOrderOnTop(true);
            //set the background to transparent
            surfaceView.Holder.SetFormat(Format.Transparent);
            holder = surfaceView.Holder;
            surfaceView.Touch += surfaceViewTouch;
        }

        private void surfaceViewTouch(object sender, View.TouchEventArgs e)
        {
            //define the paintbrush
            Paint mpaint = new Paint();
            mpaint.Color = Color.Red;
            mpaint.SetStyle(Paint.Style.Stroke);
            mpaint.StrokeWidth = 2f;

            //draw
            Canvas canvas = holder.LockCanvas();
            //clear the paint of last time
            canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
            //draw a new one, set your ball's position to the rect here
            var x = e.Event.GetX();
            var y = e.Event.GetY();
            Rect r = new Rect((int)x, (int)y, (int)x + 100, (int)y + 100);
            canvas.DrawRect(r, mpaint);
            holder.UnlockCanvasAndPost(canvas);
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            camera.StopPreview();
            camera.Release();

            return true;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            camera = Android.Hardware.Camera.Open();

            try
            {
                camera.SetPreviewTexture(surface);
                camera.SetDisplayOrientation(90);
                camera.StartPreview();
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }
    }
}