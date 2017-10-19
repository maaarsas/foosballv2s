using Android.Hardware;
using Xamarin.Forms;

namespace foosballv2s
{
    public static class ActivityHelper
    {
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