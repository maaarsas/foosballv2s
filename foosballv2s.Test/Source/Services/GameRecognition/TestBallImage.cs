using Android.Graphics;
using Emgu.CV.Structure;
using foosballv2s.Droid.Shared.Source.Services.GameRecognition;
using NUnit.Framework;

namespace foosballv2s.Test.Source.Services.GameRecognition
{
    [TestFixture]
    public class TestBallImage
    {
        [Test]
        public void TestGetColor()
        {
            Hsv imageColor = new Hsv(Color.Green.GetHue() / 2, Color.Green.GetSaturation() * 255,
                Color.Green.GetBrightness() * 255);
            Bitmap bitmap = Bitmap.CreateBitmap(200, 200, Bitmap.Config.Argb8888);
            bitmap.EraseColor(Color.Green);

            BallImage ballImage = new BallImage(bitmap);
            Hsv detectedColor = ballImage.getColor();

            Assert.AreEqual(imageColor.Hue, detectedColor.Hue);
            Assert.AreEqual(imageColor.Satuation, detectedColor.Satuation);
            Assert.AreEqual(imageColor.Value, detectedColor.Value);
        }
    }
}
