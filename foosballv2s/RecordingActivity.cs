using System;
using System.IO;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using Java.IO;
using Java.Lang;
using Xamarin.Forms;
using Camera = Android.Hardware.Camera;
using Color = Android.Graphics.Color;
using Console = System.Console;
using Size = Xamarin.Forms.Size;
using View = Android.Views.View;

namespace foosballv2s
{
    [Activity()]
    public class RecordingActivity : Activity, TextureView.ISurfaceTextureListener, Camera.IPreviewCallback
    {
        private Camera camera;
        private TextureView textureView;
        private SurfaceView surfaceView;
        private ISurfaceHolder holder;
        private MovementDetector movementDetector;

        private Game game;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

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
            game = DependencyService.Get<Game>();
        }

        private void OnSurfaceViewTouch(object sender, View.TouchEventArgs e)
        {
            DrawRectangle(e.Event.GetX(), e.Event.GetY());
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            if (camera != null) {
                camera.StopPreview();
                camera.Release();
                camera = null;
            }
            return false;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            camera = Camera.Open();

            try
            {
                //camera.GetParameters().PreviewFormat = ImageFormatType.Rgb565;
                camera.SetPreviewTexture(surface);
                camera.SetDisplayOrientation(90);
                camera.StartPreview();
                camera.SetPreviewCallback(this);
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Camera.Parameters tmp = camera.GetParameters();
            //Size bestSize = GetBestPreviewSize(textureView.Width);
            //tmp.SetPreviewSize((int) bestSize.Width, (int) bestSize.Height);
            camera.SetParameters(tmp);
            movementDetector.SetupBallDetector(textureView.Height, textureView.Width, game.BallColor);
            Log.Debug("camtest-setup", game.BallColor.ToString());
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
           
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

        public void OnPreviewFrame(byte[] data, Camera camera)
        {
            byte[] jpegData = ConvertYuvToJpeg (data, camera);
            Bitmap frameBitmap = BytesToBitmap(jpegData);
            Image<Hsv, System.Byte> hsvFrame = new Image<Hsv, byte>(frameBitmap);
            Bitmap bitmap = hsvFrame.Bitmap;
            
            CircleF[] circles = movementDetector.DetectBall(hsvFrame, textureView.Height, textureView.Width);
            
            // testing
            foreach (CircleF circle in circles)
            {
                DrawRectangle(circle.Center.X, circle.Center.Y); 
                Log.Debug("Camtest" + "-circle", circle.Center.ToString());
            }
            
            //
           
        }

        private byte[] ConvertYuvToJpeg(byte[] yuvData, Android.Hardware.Camera camera)
        {
            Camera.Parameters cameraParameters = camera.GetParameters();
            int width = cameraParameters.PreviewSize.Width;
            int height = cameraParameters.PreviewSize.Height;
            YuvImage yuv = new YuvImage(yuvData, cameraParameters.PreviewFormat, width, height, null);   
            MemoryStream ms = new MemoryStream();
            int quality = 100;   // adjust this as needed
            yuv.CompressToJpeg(new Rect(0, 0, width, height), quality, ms);
            byte[] jpegData = ms.ToArray();

            return jpegData;
        }

        public static Bitmap BytesToBitmap(byte[] imageBytes)
        {
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            return bitmap;
        }

        private Size GetBestPreviewSize(int maxWidth)
        {
            Camera.Parameters parameters = camera.GetParameters();
            Size bestSize = new Size();
            
            int i = 0;
            do
            {
                bestSize.Width = parameters.SupportedPreviewSizes[i].Width;
                bestSize.Height = parameters.SupportedPreviewSizes[i].Height;
                i++;
            } while (parameters.SupportedPreviewSizes[i].Width <= maxWidth);
            return bestSize;
        }
    }
}