using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Media.Projection;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using Java.Interop;
using Java.Lang;
using Java.Nio;
using Byte = System.Byte;
using Camera = Android.Hardware.Camera;
using CameraError = Android.Hardware.Camera2.CameraError;
using File = Java.IO.File;
using FileMode = Xamarin.Forms.Internals.FileMode;
using Stream = Android.Media.Stream;
using String = System.String;

namespace foosballv2s
{
    [Activity()]
    public class BallImageActivity : Activity, TextureView.ISurfaceTextureListener
    {
        const String TAG = "CamTest";
        
        private Camera mCamera;
        private TextureView textureView;

        private int viewWidth = 0;
        private int viewHeight = 0;

        private bool colorDetected = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BallImage);
           
            textureView = (TextureView) FindViewById(Resource.Id.cameraSurfaceView);
            textureView.SurfaceTextureListener = this;
        }

        [Export("DetectBallColor")]
        public void DetectBallColor(View view)
        {
            Bitmap b = textureView.Bitmap;
            
            BallImage ballImage = new BallImage(b);
            Hsv hsvColor = ballImage.getColor();
            
            Color detectedRGBColor = Color.HSVToColor(new float[]{
                (float) hsvColor.Hue * 2, 
                (float) hsvColor.Satuation / 255, 
                (float) hsvColor.Value / 255
            });

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
        

        [Export("SubmitBallPhoto")]
        public void SubmitBallPhoto(View view)
        {
            Intent intent = new Intent(this, typeof(RecordingActivity));
            StartActivity(intent);
        }
       
        
        public void OnSurfaceTextureAvailable(SurfaceTexture surfacetexture, int i, int j) {
            mCamera = Camera.Open();
            try {
                mCamera.SetPreviewTexture(surfacetexture);
            } catch (IOException e) {
                Log.Error(TAG + "-ERROR", e.StackTrace);
            }
            Camera.Parameters tmp = mCamera.GetParameters();
            tmp.SetPreviewSize(tmp.SupportedPreviewSizes[0].Width, tmp.SupportedPreviewSizes[0].Height);
            mCamera.SetParameters(tmp);
            mCamera.SetDisplayOrientation(90);
            mCamera.StartPreview();

        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surfacetexture) {
            if (mCamera != null) {
                mCamera.StopPreview();
                mCamera.Release();
                mCamera = null;
            }
            return false;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surfacetexture, int i, int j) {
            if (mCamera != null) {
                Camera.Parameters tmp = mCamera.GetParameters();
                tmp.SetPreviewSize(tmp.SupportedPreviewSizes[0].Width, tmp.SupportedPreviewSizes[0].Height);
                mCamera.SetParameters(tmp);
                mCamera.StartPreview();
            }
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surfacetexture) {
        }

    }
}