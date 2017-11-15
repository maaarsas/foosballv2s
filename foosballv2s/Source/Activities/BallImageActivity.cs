using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Emgu.CV.Structure;
using foosballv2s.Source.Activities.Helpers;
using foosballv2s.Source.Entities;
using foosballv2s.Source.Services.FileIO;
using foosballv2s.Source.Services.GameRecognition;
using Java.Interop;
using Xamarin.Forms;
using Button = Android.Widget.Button;
using Camera = Android.Hardware.Camera;
using Color = Android.Graphics.Color;
using String = System.String;
using View = Android.Views.View;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for getting a ball color from a picture
    /// </summary>
    [Activity(
        ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait
    )]
    public class BallImageActivity : Activity, TextureView.ISurfaceTextureListener
    {
        const String TAG = "CamTest";
        
        private Camera mCamera;
        private TextureView textureView;
        private Color detectedRGBColor;

        private int viewWidth = 0;
        private int viewHeight = 0;
        private Game game;

        private IO instance = new IO();

        private bool colorDetected = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.BallImage);
           
            textureView = (TextureView) FindViewById(Resource.Id.cameraSurfaceView);
            textureView.SurfaceTextureListener = this;
            
            game = DependencyService.Get<Game>();
            
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }

        /// <summary>
        /// From the current camera picture parses the ball color
        /// </summary>
        /// <param name="view"></param>
        [Export("DetectBallColor")]
        public void DetectBallColor(View view)
        {
            // TODO: Disallow the button click when a detection is already in progress
            Bitmap b = textureView.Bitmap;
            
            BallImage ballImage = new BallImage(b);
            Hsv hsvColor = ballImage.getColor();
            
            detectedRGBColor = Color.HSVToColor(new float[]{
                (float) hsvColor.Hue * 2, 
                (float) hsvColor.Satuation / 255, 
                (float) hsvColor.Value / 255
            });

            game.BallColor = hsvColor;
            colorDetected = true;
            
            ImageView colorSquare = (ImageView) FindViewById(Resource.Id.color_square);
            colorSquare.SetBackgroundColor(detectedRGBColor);
            
            // color detection button text is changed
            Button detectButton = (Button) FindViewById(Resource.Id.detect_button);
            detectButton.Text = GetString(Resource.String.detect_again);
            
            // submit button is only shown when the color is detected
            Button submitButton = (Button) FindViewById(Resource.Id.submit);
            submitButton.Visibility = ViewStates.Visible;
        }
        
        /// <summary>
        /// Submits the chosen ball color and goes to recording activity
        /// </summary>
        /// <param name="view"></param>
        [Export("SubmitBallPhoto")]
        public void SubmitBallPhoto(View view)
        {
            instance.Write_Serialize_Color(detectedRGBColor);

            Intent intent = new Intent(this, typeof(RecordingActivity));
            StartActivity(intent);
        }
       
        /// <summary>
        /// Sets up the camera when the surface view is available
        /// </summary>
        /// <param name="surfacetexture"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void OnSurfaceTextureAvailable(SurfaceTexture surfacetexture, int i, int j) {
            mCamera = Camera.Open();
            try {
                mCamera.SetPreviewTexture(surfacetexture);
            } catch (IOException e) {
                Log.Error(TAG + "-ERROR", e.StackTrace);
            }
            Camera.Parameters tmp = mCamera.GetParameters();
            Xamarin.Forms.Size bestSize = ActivityHelper.GetBestPreviewSize(mCamera.GetParameters(), textureView.Width, textureView.Height);
            tmp.SetPreviewSize((int) bestSize.Width, (int) bestSize.Height);
            tmp.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
            mCamera.SetParameters(tmp);
            mCamera.SetDisplayOrientation(90);
            mCamera.StartPreview();

        }

        /// <summary>
        /// Stops the camera when the surface view is destroyed
        /// </summary>
        /// <param name="surfacetexture"></param>
        /// <returns></returns>
        public bool OnSurfaceTextureDestroyed(SurfaceTexture surfacetexture) {
            if (mCamera != null) {
                mCamera.StopPreview();
                mCamera.Release();
                mCamera = null;
            }
            return false;
        }

        /// <summary>
        /// Sets up the preview size when the surface view size changes
        /// </summary>
        /// <param name="surfacetexture"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void OnSurfaceTextureSizeChanged(SurfaceTexture surfacetexture, int i, int j) {
            if (mCamera != null) {
                Camera.Parameters tmp = mCamera.GetParameters();
                tmp.SetPreviewSize(tmp.SupportedPreviewSizes[0].Width, tmp.SupportedPreviewSizes[0].Height);
                mCamera.SetParameters(tmp);
                mCamera.StartPreview();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="surfacetexture"></param>
        public void OnSurfaceTextureUpdated(SurfaceTexture surfacetexture) {
        }

    }
}