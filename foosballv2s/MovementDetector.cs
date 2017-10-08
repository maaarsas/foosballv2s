using Android.Graphics;
using Android.Util;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using Android.Support.V7.Util;

namespace foosballv2s
{
    public class MovementDetector
    {
        private IVideo video;

        public MovementDetector() { }

        public MovementDetector(IVideo video)
        {
            this.video = video;
        }

        // Steps:
        // 1) A frame is retrieved from a video
        // 2) The frame is converted from RGB to HSV color scheme (for more accurate detection)
        // 3) All unneeded colors are filtered out, only pixels with given color are left
        // 4) After this step the picture is in grayscale where the ball appears white and all other
        //    pixels are black
        // 5) Circles are detected in the grayscale image
        // 6) The proccessed frame with circles drawn on it is shown in a window
        private Mat frame;
        private Image<Hsv, Byte> hsvFrame;
        private Image<Gray, Byte> thresholded;
        private Hsv minHsv;
        private Hsv maxHsv;
        
        public void SetupBallDetector(int frameHeight, int frameWidth, Hsv hsvBall)
        {
            // Currently the color is hardcoded here. Needs to be detected somewhere else
            // and passed here as parameters
            //int t1min = 170, t1max = 4, t2min = 120, t2max = 255, t3min = 120, t3max = 255; // color in HSV
           double t1min = 170, t1max = 4, t2min = 120, t2max = 255, t3min = 120, t3max = 255; // color in HSV

            //VideoCapture capture = new VideoCapture();
            //Mat frame = this.video.GetFrame(); // one frame of a video
            //Bitmap bitmap = BitmapFactory.DecodeByteArray(frameInBytes, 0, frameInBytes.Length);
            //Image<Bgr, Byte> frameImage = new Image<Bgr, Byte>(frameWidth, frameHeight); //specify the width and height here
            //frameImage.Bytes = frameInBytes; //your byte array
            //frame = frameImage.Mat;
            //CvInvoke.Imdecode(frameInBytes, ImreadModes.Unchanged, frame);
            //frame = new Mat();
            //Bitmap bmp32 = bitmap.Copy(Bitmap.Config.Argb8888, true);
            frame = new Mat();
            Size size = new Size(frame.Width, frame.Height); // the size of the frame

            t1min = hsvBall.Hue - 5;
            t1max = hsvBall.Hue + 5;
            t2min = hsvBall.Satuation - 70;
            t2max = hsvBall.Satuation + 70;
            t3min = hsvBall.Value - 15;
            t3max = hsvBall.Satuation + 70;

            if (t1min < 0)
                t1min = t1min + 180;
            if (t1max > 179)
                t1max = t1max - 180;
            if (t2min < 0)
                t2min = 0;
            if (t2max > 255)
                t2max = 255;
            if (t3min < 0)
                t3min = 0;
            if (t3max > 255)
                t3max = 255;
            
            minHsv = new Hsv(t1min, t2min, t3min); // minimum color that passes
            maxHsv = new Hsv(t1max, t2max, t3max); // maximum color that passes

            hsvFrame = new Image<Hsv, byte>(new Image<Gray, Byte>[] {
                new Image<Gray, Byte>(frame.Width, frame.Height),
                new Image<Gray, Byte>(frame.Width, frame.Height),
                new Image<Gray, Byte>(frame.Width, frame.Height),
            }); // frame converted to HSV
            thresholded = new Image<Gray, byte>(frame.Width, frame.Height); // frame with filtered out colors

            // Create a window in which the captured images will be presented
            //CvInvoke.NamedWindow("Camera", NamedWindowType.KeepRatio);
            //CvInvoke.NamedWindow("HSV", NamedWindowType.KeepRatio);
            //CvInvoke.NamedWindow("Thresholded", NamedWindowType.KeepRatio);
        }
        public CircleF[] DetectBall(byte[] frameBytes, int frameHeight, int frameWidth)
        { 
            //Application.Idle += new EventHandler(delegate (object sender, EventArgs e)
            {
                //frame = this.video.GetFrame(); // Get one frame
                if (frame == null)
                {
                    Console.WriteLine("Error. A frame is empty. Skipping");
                }

                hsvFrame.Bytes = frameBytes;
                // Covert color space to HSV as it is much easier to filter colors in the HSV color-space.
                ////CvInvoke.CvtColor(frame, hsvFrame, ColorConversion.Bgr2Hsv);
                // Filter out other colors than specified
                this.FilterHsvImageColor(hsvFrame, thresholded, minHsv, maxHsv);
                // Make some smoothing for better detection results
                ////thresholded = thresholded.SmoothGaussian(5);
                // Find circles in grayscale image and draw them on the frame
                return this.DetectCirclesInImage(thresholded, frame);

                //CvInvoke.Imshow("Camera", frame); // shows the proccessed frame with circles drawn on it
                //CvInvoke.Imshow("HSV", hsvFrame);
                //CvInvoke.Imshow("Thresholded", thresholded);

                //frame.Dispose();
                //If ESC key pressed, Key=0x10001B under OpenCV 0.9.7(linux version),
                //remove higher bits using AND operator
                //if ((CvInvoke.WaitKey(1) & 255) == 27) break;
            }
            //if ((CvInvoke.WaitKey(1) & 255) == 27) Application.Exit();
            //});
            // Release the capture
        }

        /**
         * Filter out colors which are out of range in Hsv image.
         */
        private void FilterHsvImageColor(Image<Hsv, byte> hsvImage, Image<Gray, byte> resultImage, Hsv minHsv, Hsv maxHsv)
        {
            // Dealing with colors that are close to 0 or 180 by their hue (especially red)
            if (minHsv.Hue > maxHsv.Hue)
            {
                Hsv partialMax = new Hsv(180, maxHsv.Satuation, maxHsv.Value);
                Hsv partialMin = new Hsv(0, minHsv.Satuation, minHsv.Value);
                Image<Gray, Byte> thresholdedFirst = hsvImage.InRange(minHsv, partialMax);
                Image<Gray, Byte> thresholdedSecond = hsvImage.InRange(partialMin, maxHsv);
                CvInvoke.AddWeighted(thresholdedFirst, 1.0, thresholdedSecond, 1.0, 0.0, resultImage);
                //CvInvoke.GaussianBlur(thresholded, thresholded, new Size(9, 9), 2, 2);
                thresholdedFirst.Dispose();
                thresholdedSecond.Dispose();

            }
            else // Otherwise just filter out colors that are out of the given range
            {
                resultImage = hsvImage.InRange(minHsv, maxHsv);
            }
        }
        
        /**
         * Using Hough transform to detect circles in a black-white image
         */
        private CircleF[] DetectCirclesInImage(Image<Gray, byte> image, Mat outputFrame)
        {
            //CircleF[] circles = CvInvoke.HoughCircles(image, HoughType.Gradient, 1, 
            //    1000, 10, 10, 15, 60);
            return CvInvoke.HoughCircles(image, HoughType.Gradient, 2, image.Height / 4, 50, 20, 15, 60);
        }

        
    }
}