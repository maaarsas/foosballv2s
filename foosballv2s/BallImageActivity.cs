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
        bool mSurfaceCreated = false;
        bool mIsCameraConfigured = false;
        static Surface mCameraSurface;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BallImage);

            mSurfaceView = (SurfaceView) FindViewById(Resource.Id.cameraSurfaceView);
            mSurfaceView.SetZOrderMediaOverlay(true);
            mSurfaceView.SetZOrderOnTop(true);
            mSurfaceView.Holder.SetFormat(Format.Transparent);
            mSurfaceHolder = this.mSurfaceView.Holder;
            mSurfaceHolder.AddCallback(this);
            mHandler = new Handler(this);
            mCameraManager = (CameraManager) this.GetSystemService(Context.CameraService);
            try
            {
                mCameraIDsList = this.mCameraManager.GetCameraIdList();
            }
            catch (CameraAccessException e) { }
            mCameraStateCB = new CameraStateCallback();
        }
    
        protected override void OnStart() {
            base.OnStart();
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
            List<Surface> sfl = new List<Surface>();
            sfl.Add(mCameraSurface); 
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
    
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            mCameraSurface = holder.Surface;
            Log.Debug(TAG, holder.Surface.ToString());
        }
    
        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height) {
            mCameraSurface = holder.Surface;
            mSurfaceCreated = true;
            mHandler.SendEmptyMessage(MSG_SURFACE_READY);
            Log.Debug(TAG, width + " " + height);
        }
    
        public void SurfaceDestroyed(ISurfaceHolder holder) {
            mSurfaceCreated = false;
        }
    
        private class CaptureSessionListener : CameraCaptureSession.StateCallback {
            
            public override void OnConfigureFailed(CameraCaptureSession session) { }
    
            public override void OnConfigured(CameraCaptureSession session) {
                mCaptureSession = session;
    
                try {
                    CaptureRequest.Builder previewRequestBuilder = mCameraDevice
                            .CreateCaptureRequest(CameraTemplate.Preview);
                    previewRequestBuilder.AddTarget(mCameraSurface);
                    Log.Debug(TAG, mCameraSurface.ToString());
                    mCaptureSession.SetRepeatingRequest(previewRequestBuilder.Build(),
                            null, null);
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
    }
}