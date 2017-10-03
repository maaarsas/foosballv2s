using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Reflection;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Media;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using Java.Lang;
using Java.Nio;
using Byte = System.Byte;
using CameraError = Android.Hardware.Camera2.CameraError;
using String = System.String;

namespace foosballv2s
{
    [Activity()]
    public class BallImageActivity : Activity, ISurfaceHolderCallback, Handler.ICallback
    {
        const String TAG = "CamTest";
        const int MY_PERMISSIONS_REQUEST_CAMERA = 1242;
        private const int MSG_CAMERA_OPENED = 1;
        private const int MSG_SURFACE_READY = 2;
        private static Handler mHandler;
        SurfaceView mSurfaceView;
        ISurfaceHolder mSurfaceHolder;
        CameraManager mCameraManager;
        String[] mCameraIDsList;
        static CameraDevice.StateCallback mCameraStateCB;
        static CameraDevice mCameraDevice;
        static CameraCaptureSession mCaptureSession;
        bool mSurfaceCreated = false;
        bool mIsCameraConfigured = false;
        static Surface mCameraSurface;
        private static ImageReader mReader;
        private static SimpleImageListener mListener = null;
        private static Handler mImageHandler;
        private static Surface mImageReaderSurface;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BallImage);

            mSurfaceView = (SurfaceView) FindViewById(Resource.Id.cameraSurfaceView);
            //mSurfaceView.SetZOrderOnTop(true);
            mSurfaceHolder = this.mSurfaceView.Holder;
            mSurfaceHolder.AddCallback(this);
            mHandler = new Handler(this);
            mCameraManager = (CameraManager) this.GetSystemService(Context.CameraService);
            try
            {
                mCameraIDsList = this.mCameraManager.GetCameraIdList();
            }
            catch (CameraAccessException e) { }
            CameraCharacteristics characteristics = mCameraManager.GetCameraCharacteristics(mCameraIDsList[0]);
            StreamConfigurationMap streamConfigs =
                (StreamConfigurationMap) characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
            Size[] jpegSizes = streamConfigs.GetOutputSizes((int) ImageFormatType.Jpeg);
            
            
            mCameraStateCB = new CameraStateCallback();
            
            mImageHandler = new Handler();
            
            
            mReader = ImageReader.NewInstance(jpegSizes[0].Width, jpegSizes[0].Height, ImageFormatType.Jpeg, 1);
            Log.Debug(TAG, mReader.ToString());
            Log.Debug(TAG, (new SimpleImageListener()).ToString());
            Log.Debug(TAG, mReader.ToString());
            ImageReader.IOnImageAvailableListener imageAvailListener = new SimpleImageListener();
            mReader.SetOnImageAvailableListener(imageAvailListener, null);
            mReader.SetOnImageAvailableListener(imageAvailListener, null);
            mImageReaderSurface = mReader.Surface;
        }
    
        protected override void OnStart() {
            base.OnStart();
            Permission permissionCheck = ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera);
            if (permissionCheck != Permission.Granted) {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.Camera)) {
    
                } else {
                    ActivityCompat.RequestPermissions(this, new String[]{Manifest.Permission.Camera}, MY_PERMISSIONS_REQUEST_CAMERA);
                    }
            } else {
                try {
                    mCameraManager.OpenCamera(mCameraIDsList[0], mCameraStateCB, new Handler());
                } catch (CameraAccessException e)
                {
                    Log.Error("ERROR", e.StackTrace);
                }
            }
        }
    
        protected override void OnStop() {
            base.OnStop();
            try {
                if (mCaptureSession != null) {
                    mCaptureSession.StopRepeating();
                    mCaptureSession.Close();
                    mCaptureSession = null;
                }
                mIsCameraConfigured = false;
            } 
            catch (CameraAccessException e) { } 
            catch (IllegalStateException e2) { } 
            finally {
                if (mCameraDevice != null) {
                    mCameraDevice.Close();
                    mCameraDevice = null;
                    mCaptureSession = null;
                }
            }
        }
    
        public bool HandleMessage(Message msg) {
            switch (msg.What) {
                case MSG_CAMERA_OPENED:
                case MSG_SURFACE_READY:
                    if (mSurfaceCreated && (mCameraDevice != null)
                            && !mIsCameraConfigured) {
                        ConfigureCamera();
                    }
                    break;
            }
    
            return true;
        }
    
        private void ConfigureCamera() {
            List<Surface> surfaceList = new List<Surface> {mCameraSurface, mImageReaderSurface};
            try {
                mCameraDevice.CreateCaptureSession(surfaceList,
                        new CaptureSessionListener(), null);
            } catch (CameraAccessException e) {
                Log.Error("ERROR", e.StackTrace);
            }
            mIsCameraConfigured = true;
        }
    
        public override void OnRequestPermissionsResult(int requestCode, String[] permissions, Permission[] grantResults) {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            switch (requestCode) {
                case MY_PERMISSIONS_REQUEST_CAMERA:
                    if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.Camera) 
                        == Permission.Granted)
                        try {
                            mCameraManager.OpenCamera(mCameraIDsList[0], mCameraStateCB, new Handler());
                        } catch (CameraAccessException e) {
                            Log.Error("ERROR", e.StackTrace);
                        }
                    break;
            }
        }
    
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            mCameraSurface = holder.Surface;
        }
    
        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height) {
            mCameraSurface = holder.Surface;
            mSurfaceCreated = true;
            mHandler.SendEmptyMessage(MSG_SURFACE_READY);
        }
    
        public void SurfaceDestroyed(ISurfaceHolder holder) {
            mSurfaceCreated = false;
        }
    
        private class CaptureSessionListener : CameraCaptureSession.StateCallback {
            
            public override void OnConfigureFailed(CameraCaptureSession session) { }
    
            public override void OnConfigured(CameraCaptureSession session) {
                
                mCaptureSession = session;
    
                try
                {
                    CaptureRequest.Builder previewRequestBuilder = mCameraDevice
                            .CreateCaptureRequest(CameraTemplate.Preview);
                    previewRequestBuilder.AddTarget(mCameraSurface);
                    previewRequestBuilder.AddTarget(mImageReaderSurface);
                    mCaptureSession.SetRepeatingRequest(previewRequestBuilder.Build(), null, null);
                } catch (CameraAccessException e) { }
            }
        }
        class CameraStateCallback : CameraDevice.StateCallback 
        {
            public override void OnOpened(CameraDevice camera)
            {
                mCameraDevice = camera;
                mHandler.SendEmptyMessage(MSG_CAMERA_OPENED);
            }
            
            public override void OnDisconnected(CameraDevice camera) { }
            
            public override void OnError(CameraDevice camera, CameraError error) { }
        }
        
        class SimpleImageListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener {
            
            public void OnImageAvailable(ImageReader reader)
            {
                Image image = reader.AcquireLatestImage();
                ByteBuffer buffer = image.GetPlanes()[0].Buffer;
                byte[] bytes = new byte[buffer.Capacity()];
                buffer.Get(bytes);
                Bitmap bitmapImage = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, null);

                BallImage ballImage = new BallImage(bitmapImage);
                Hsv hsvColor = ballImage.getColor();

                Log.Debug(TAG + "ddd", hsvColor.Hue.ToString() + " "  + hsvColor.Satuation.ToString() + " " + hsvColor.Value.ToString());
                image.Close();
            }
        }
    }
}