using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using Java.Interop;
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
    [Activity(
        ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Landscape
    )]
    public class RecordingActivity : Activity, TextureView.ISurfaceTextureListener, Camera.IPreviewCallback
    {
        private const string TAG = "CamTest";
        private Camera camera;
        private TextureView textureView;
        private SurfaceView surfaceView;
        private ISurfaceHolder holder;
        private MovementDetector movementDetector;

        private TextView team1ScoreView;
        private TextView team2ScoreView;

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
            
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            
            // set up displayed texts on the screen
            TextView team1NameView = (TextView) FindViewById(Resource.Id.team1_name);
            team1NameView.Text = game.Team1.TeamName;
            
            TextView team2NameView = (TextView) FindViewById(Resource.Id.team2_name);
            team2NameView.Text = game.Team2.TeamName;

            team1ScoreView = (TextView) FindViewById(Resource.Id.team1_score);
            team2ScoreView = (TextView) FindViewById(Resource.Id.team2_score);
        }

        [Export("Team1Goal")]
        public void Team1Goal(View view)
        {
            game.Team1Score++;
            team1ScoreView.Text = game.Team1Score.ToString();
            CheckGameEnd(game);
        }
        
        [Export("Team2Goal")]
        public void Team2Goal(View view)
        {
            game.Team2Score++;
            team2ScoreView.Text = game.Team2Score.ToString();
            CheckGameEnd(game);
        }

        private void CheckGameEnd(Game game)
        {
            if (game.HasEnded)
            {
                
            }
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
                Log.Error(TAG, ex.Message);
            }
            Log.Debug("camtest-setup", game.BallColor.ToString());
            Camera.Parameters tmp = camera.GetParameters();
            Size bestSize = ActivityHelper.GetBestPreviewSize(camera, textureView.Width, textureView.Height);
            tmp.SetPreviewSize((int) bestSize.Width, (int) bestSize.Height);
            //tmp.PreviewFormat = ImageFormatType.Yv12;
            tmp.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
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
            //byte[] jpegData = ConvertYuvToJpeg(data, camera);
            //Bitmap frameBitmap = BytesToBitmap(jpegData);

            //ImageView testImage = (ImageView) FindViewById(Resource.Id.test_image_view);
            //testImage.SetImageBitmap(frameBitmap);
            //var previewFormat = camera.GetParameters().PreviewFormat;

            //Log.Debug("CamTest", data.Length.ToString() + camera.GetParameters().PreviewSize.Width);

            /*Image<Hsv, System.Byte> hsvFrame = new Image<Hsv, byte>(frameBitmap);
            Bitmap bitmap = hsvFrame.Bitmap;*/
            
            //testImage.SetImageBitmap(bitmap);

            /*CircleF[] circles = movementDetector.DetectBall(hsvFrame, textureView.Height, textureView.Width);
            
            // testing
            foreach (CircleF circle in circles)
            {
                DrawRectangle(circle.Center.X, circle.Center.Y); 
                Log.Debug("Camtest" + "-circle", circle.Center.ToString());
            }*/

            //

        }

        private byte[] ConvertYuvToJpeg(byte[] yuvData, Android.Hardware.Camera camera)
        {
            Camera.Parameters cameraParameters = camera.GetParameters();
            int width = cameraParameters.PreviewSize.Width;
            int height = cameraParameters.PreviewSize.Height;
            YuvImage yuv = new YuvImage(yuvData, cameraParameters.PreviewFormat, width, height, null);   
            MemoryStream ms = new MemoryStream();
            int quality = 50;   // adjust this as needed
            yuv.CompressToJpeg(new Rect(0, 0, width, height), quality, ms);
            byte[] jpegData = ms.ToArray();

            return jpegData;
        }

        public static Bitmap BytesToBitmap(byte[] imageBytes)
        {
            int rotationAngle = 90;
            
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            Matrix matrix = new Matrix();
            matrix.PostRotate(rotationAngle);
            return Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
        }
    }
}