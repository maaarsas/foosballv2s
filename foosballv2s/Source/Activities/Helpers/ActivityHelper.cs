using Android.Hardware;
using Xamarin.Forms;

namespace foosballv2s.Source.Activities.Helpers
{
    /// <summary>
    /// Defines small functions that are reused in many activities
    /// </summary>
    public static class ActivityHelper
    {
        /// <summary>
        /// From the list of all supported camera sizes chooses the best one according to the preview dimensions
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="previewWidth"></param>
        /// <param name="previewHeight"></param>
        /// <returns></returns>
        public static Size GetBestPreviewSize(Camera.Parameters parameters, int previewWidth, int previewHeight)
        {
            double targetRatio = (double) previewHeight / previewWidth;
            Size bestSize = new Size();

                       int i = 0;
            while (i < parameters.SupportedPreviewSizes.Count && 
                   (parameters.SupportedPreviewSizes[i].Height < previewWidth
                   || bestSize.Width / bestSize.Height < targetRatio - 0.1))
            {
                bestSize.Width = parameters.SupportedPreviewSizes[i].Width;
                bestSize.Height = parameters.SupportedPreviewSizes[i].Height;
                i++;
            }
            return bestSize;
        }
    }
}
