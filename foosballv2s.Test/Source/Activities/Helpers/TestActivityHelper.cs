using System.Collections.Generic;
using Android.Hardware;
using Android.Test.Mock;
using foosballv2s.Android.Shared.Source.Activities.Helpers;
using NUnit.Framework;

namespace foosballv2s.Test.Source.Activities.Helpers
{
    [TestFixture]
    public class TestActivityHelper
    {
        [TestCase(480, 320, 480, 320)]
        [TestCase(720, 460, 720, 480)]
        [TestCase(1080, 720, 800, 480)]
        public void GetBestPreviewSizeTest(int previewWidth, int previewHeight, int expectedWidth, int expectedHeight)
        {
            var supportedSizes = GetCameraTestSupportedSizes();
            
            var bestSize = ActivityHelper.GetBestPreviewSize(supportedSizes, previewWidth, previewHeight);
            
            Assert.AreEqual(expectedWidth, bestSize.Width);
            Assert.AreEqual(expectedHeight, bestSize.Height);
        }

        private IList<Camera.Size> GetCameraTestSupportedSizes()
        {
            return new List<Camera.Size>(new Camera.Size[]
                {
                    new Camera.Size(null, 400, 240), 
                    new Camera.Size(null, 480, 320), 
                    new Camera.Size(null, 576, 432), 
                    new Camera.Size(null, 640, 480), 
                    new Camera.Size(null, 720, 480), 
                    new Camera.Size(null, 768, 432), 
                    new Camera.Size(null, 800, 480), 
                    new Camera.Size(null, 1280, 720), 
                    new Camera.Size(null, 1920, 1080), 
                }
            );
        }
    }
}
