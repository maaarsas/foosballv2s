using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Views;
using System.Collections.Generic;
using Android;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Emgu.CV.Structure;
using Java.IO;
using Java.Lang;
using Camera = Android.Hardware.Camera;
using Console = System.Console;
using Exception = System.Exception;

namespace foosballv2s
{
    public class BallImagePreview// : SurfaceView, ISurfaceHolderCallback
    {
        public static Bitmap mBitmap;
        private ISurfaceHolder holder;
        private static CameraManager mCameraManager;

        /*public BallImagePreview(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            holder = this.Holder;
            holder.AddCallback(this);
        }

        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
        {
            Android.Hardware.Camera.Parameters parameters = mCamera.GetParameters();
            parameters.SupportedPreviewSizes.Add(
                new Android.Hardware.Camera.Size(mCamera, width, height
            ));
            mCamera.SetParameters(parameters);
            mCamera.StartPreview();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try {
                mCamera = Camera.Open();
                mCamera.SetPreviewDisplay(holder);
            } catch (Exception e) {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            mCamera.StopPreview();
            mCamera.Release();
        }
        **/
        /*public static void TakePicture(){  

            Camera.IPictureCallback mPictureCallback = new PictureCall() {
                @Override
                public void onPictureTaken(byte[] data, Camera camera) {

                BitmapFactory.Options options = new BitmapFactory.Options();
                mBitmap = BitmapFactory.decodeByteArray(data, 0, data.length, options);
            }
            };
            mCamera.takePicture(null, null, mPictureCallback);
        }*/
    }
}