using System;
using System.Collections.Generic;
using Android.Hardware;

namespace foosballv2s.Droid.Shared.Source.Helpers
{
    /// <summary>
    /// Defines small functions that are reused in many activities
    /// </summary>
    public static class ActivityHelper
    {
        /// <summary>
        /// From the list of all supported camera sizes chooses the best one according to the preview dimensions
        /// </summary>
        /// <param name="supportedSizes"></param>
        /// <param name="previewWidth"></param>
        /// <param name="previewHeight"></param>
        /// <returns></returns>
        public static Camera.Size GetBestPreviewSize(IList<Camera.Size> sizes, int previewWidth, int previewHeight)
        {
            double ASPECT_TOLERANCE = 0.15;
            double targetRatio;
            if (previewHeight > previewWidth)
            {
                targetRatio = (double) previewHeight / previewWidth;
            }
            else
            {
                targetRatio = (double) previewWidth / previewHeight;
            }

            if (sizes == null) return null;

            Camera.Size optimalSize = null;
            double minDiff = Double.MaxValue;

            int targetHeight = previewHeight;

            foreach (Camera.Size size in sizes) 
            {
                double ratio = (double) size.Width / size.Height;
                if (Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE)
                {
                    continue;
                }
                if (Math.Abs(size.Height - targetHeight) <= minDiff) 
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            if (optimalSize == null) 
            {
                minDiff = Double.MaxValue;
                foreach (Camera.Size size in sizes) 
                {
                    if (Math.Abs(size.Height - targetHeight) <= minDiff) 
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }
            return optimalSize;
        }
    }
}
