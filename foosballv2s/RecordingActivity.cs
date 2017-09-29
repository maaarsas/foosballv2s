using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Views;
using System;
using Android.Graphics;
using Emgu.CV.Structure;

namespace foosballv2s
{
    [Activity(Label = "foosballv2s")]
    public class RecordingActivity : Activity, TextureView.ISurfaceTextureListener, Android.Hardware.Camera.IPreviewCallback
    {
        private Android.Hardware.Camera camera;
        private TextureView textureView;
        private SurfaceView surfaceView;
        private ISurfaceHolder holder;

        private MovementDetector movementDetector;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Recording);

            textureView = (TextureView)FindViewById(Resource.Id.textureView);
            textureView.SurfaceTextureListener = this;

            surfaceView = (SurfaceView)FindViewById(Resource.Id.surfaceView);
            //set to top layer
            surfaceView.SetZOrderOnTop(true);
            //set the background to transparent
            surfaceView.Holder.SetFormat(Format.Transparent);
            holder = surfaceView.Holder;
            surfaceView.Touch += OnSurfaceViewTouch;

            movementDetector = new MovementDetector();
        }

        private void OnSurfaceViewTouch(object sender, View.TouchEventArgs e)
        {
            DrawRectangle(e.Event.GetX(), e.Event.GetY());
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
                camera.SetPreviewCallback(this);
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

        private bool isFirstFrame = true;

        public void OnPreviewFrame(byte[] data, Android.Hardware.Camera camera)
        {
            //throw new NotImplementedException();
            Console.WriteLine("onPreviewFrame " + textureView.Height + "---" + textureView.Width);
            if (isFirstFrame)
            {
                isFirstFrame = false;
                movementDetector.SetupBallDetector(data, textureView.Height, textureView.Width);
            }
            else
            {
                CircleF[] circles = movementDetector.DetectBall(data, textureView.Height, textureView.Width);
            }
        }

        private void DrawRectangle(float x, float y)
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
            Rect r = new Rect((int)x, (int)y, (int)x + 100, (int)y + 100);
            canvas.DrawRect(r, mpaint);
            holder.UnlockCanvasAndPost(canvas);

        }
    }
}