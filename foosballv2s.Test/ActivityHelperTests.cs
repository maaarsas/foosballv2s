using NUnit.Framework;
using NUnit.Mocks;
using System;
using System.Collections.Generic;
using Android.Hardware;
using Android.Util;

namespace foosballv2s.Test
{
    [TestFixture]
    public class ActivityHelperTests
    {
        private int[][] previewSizes = new int[][]
        {
            new int[] {400, 240},
            new int[] {480, 320},
            new int[] {576, 432},
            new int[] {640, 480},
            new int[] {720, 480},
            new int[] {768, 432},
            new int[] {800, 480},
            new int[] {1280, 720}
        };
        
//        [TestCase(480, 720, )]
//        [TestCase(720, 1080,  )]
        public void GetBestPreviewSizeTest(int previewWidth, int previewHeight, int expectedWidth, int expectedHeight)
        {
            var cameraParameters = new DynamicMock(typeof(Camera.Parameters));
            List<Camera.Size> testPreviewSizes = new List<Camera.Size>();
            foreach (int[] size in previewSizes)
            {
                testPreviewSizes.Add(new Camera.Size(null, size[0], size[1]));
            }
            cameraParameters.SetReturnValue("SupportedPreviewSizes", testPreviewSizes);

            Xamarin.Forms.Size returnedSize = ActivityHelper.GetBestPreviewSize(
                (Camera.Parameters) cameraParameters.MockInstance, 
                previewWidth, previewHeight);
            
            Assert.True(returnedSize.Width == expectedWidth && returnedSize.Height == expectedHeight);
        }
    }
}
