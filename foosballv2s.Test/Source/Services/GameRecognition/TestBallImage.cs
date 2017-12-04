using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Emgu.CV.Structure;
using foosballv2s.Droid.Shared.Source.Services.GameRecognition;
using NUnit.Framework;

namespace foosballv2s.Test.Source.Services.GameRecognition
{
    [TestFixture]
    public class TestBallImage
    {
        [Test, TestCaseSource("Bitmaps")]
        public void TestGetColor(Bitmap bitmap, Color ballColor)
        {
            Hsv imageColor = new Hsv(ballColor.GetHue() / 2,   
                ballColor.GetSaturation() * 255,  
                ballColor.GetBrightness() * 255);
            
            BallImage ballImage = new BallImage(bitmap);
            Hsv detectedColor = ballImage.getColor();

            Assert.AreEqual(imageColor.Hue, detectedColor.Hue);
            Assert.AreEqual(imageColor.Satuation, detectedColor.Satuation);
            Assert.AreEqual(imageColor.Value, detectedColor.Value);
        }

        private static object[] Bitmaps =
        {
            new object[]
            {
                GetBitmapWithColor(Color.Green, 200, 200),
                Color.Green,
            },
            new object[]
            {
                BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.ball_image_1),
                new Color(3, 169, 244),
            },
            new object[]
            {
                BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.ball_image_2),
                new Color(255, 235, 59),
            },
        };

        private static Bitmap GetBitmapWithColor(Color color, int width, int height)
        {
            var bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            bitmap.EraseColor(Color.Green);
            return bitmap;
        }


    }
}
