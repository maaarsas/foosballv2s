using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Views;
using System;
using System.Collections.Generic;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Emgu.CV.Structure;
using Java.Lang;
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
        bool mSurfaceCreated = true;
        bool mIsCameraConfigured = false;
        private static Surface mCameraSurface = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BallImage);

            mSurfaceView = (SurfaceView) FindViewById(Resource.Id.cameraSurfaceView);
            mSurfaceHolder = this.mSurfaceView.Holder;
            mSurfaceHolder.AddCallback(this);
            mHandler = new Handler(this);
            mCameraManager = (CameraManager) this.GetSystemService(Context.CameraService);

            try
            {
                mCameraIDsList = this.mCameraManager.GetCameraIdList();
                foreach (String id in mCameraIDsList)
                {
                    Log.Verbose(TAG, "CameraID: " + id);
                }
            }
            catch (CameraAccessException e)
            {
                Log.Error("ERROR", e.StackTrace);
            }

            mCameraStateCB = new CameraStateCallback();
        }
    
        protected override void OnStart() {
            base.OnStart();
    
            //requesting permission
            Permission permissionCheck = ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera);
            if (permissionCheck != Permission.Granted) {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.Camera)) {
    
                } else {
                    ActivityCompat.RequestPermissions(this, new String[]{Manifest.Permission.Camera}, MY_PERMISSIONS_REQUEST_CAMERA);
                    Toast.MakeText(ApplicationContext, "request permission", ToastLength.Short).Show();
                }
            } else {
                Toast.MakeText(ApplicationContext, "PERMISSION_ALREADY_GRANTED", ToastLength.Short).Show();
                try {
                    mCameraManager.OpenCamera(mCameraIDsList[1], mCameraStateCB, new Handler());
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
            } catch (CameraAccessException e) {
                // Doesn't matter, cloising device anyway
                Log.Error("ERROR", e.StackTrace);
            } catch (IllegalStateException e2) {
                // Doesn't matter, cloising device anyway
                Log.Error("ERROR", e2.StackTrace);
            } finally {
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
                    // if both surface is created and camera device is opened
                    // - ready to set up preview and other things
                    if (mSurfaceCreated && (mCameraDevice != null)
                            && !mIsCameraConfigured) {
                        ConfigureCamera();
                    }
                    break;
            }
    
            return true;
        }
    
        private void ConfigureCamera() {
            // prepare list of surfaces to be used in capture requests
            List<Surface> sfl = new List<Surface>();
    
            sfl.Add(mCameraSurface); // surface for viewfinder preview
    
            // configure camera with all the surfaces to be ever used
            try {
                mCameraDevice.CreateCaptureSession(sfl,
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
                            mCameraManager.OpenCamera(mCameraIDsList[1], mCameraStateCB, new Handler());
                        } catch (CameraAccessException e) {
                            Log.Error("ERROR", e.StackTrace);
                        }
                    break;
            }
        }
    
        public void SurfaceCreated(ISurfaceHolder holder) {
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
            public override void OnConfigureFailed(CameraCaptureSession session) {
                Log.Debug(TAG, "CaptureSessionConfigure failed");
            }
    
            public override void OnConfigured(CameraCaptureSession session) {
                Log.Debug(TAG, "CaptureSessionConfigure onConfigured");
                mCaptureSession = session;
    
                try {
                    CaptureRequest.Builder previewRequestBuilder = mCameraDevice
                            .CreateCaptureRequest(CameraTemplate.Preview);
                    previewRequestBuilder.AddTarget(mCameraSurface);
                    mCaptureSession.SetRepeatingRequest(previewRequestBuilder.Build(),
                            null, null);
                    Log.Debug(TAG, mCameraDevice.ToString());
                    Log.Debug(TAG, previewRequestBuilder.ToString());
                    Log.Debug(TAG, mCameraSurface.ToString());
                    Log.Debug(TAG, mCaptureSession.ToString());
                } catch (CameraAccessException e) {
                    Log.Debug(TAG, "setting up preview failed");
                    Log.Error("ERROR", e.StackTrace);
                }
            }
        }
        class CameraStateCallback : CameraDevice.StateCallback 
        {

            public override void OnOpened(CameraDevice camera)
            {
                Toast.MakeText(Application.Context, "onOpened", ToastLength.Short).Show();
                mCameraDevice = camera;
                mHandler.SendEmptyMessage(MSG_CAMERA_OPENED);
            }
            
            public override void OnDisconnected(CameraDevice camera)
            {
                Toast.MakeText(Application.Context, "onDisconected", ToastLength.Short).Show();
            }
            
            public override void OnError(CameraDevice camera, CameraError error)
            {
                Toast.MakeText(Application.Context, "onError", ToastLength.Short).Show();
            }
        }

    }
}