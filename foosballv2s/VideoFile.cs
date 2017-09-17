using Emgu.CV;
using System;
using System.IO;

namespace foosballv2s
{
    class VideoFile : IVideo
    {
        private VideoCapture capture;

        public VideoFile(String file)
        {
            this.capture = new VideoCapture(file);
            if (!capture.IsOpened)
            {
                throw new FileNotFoundException(file);
            }
        }

        public Mat GetFrame()
        {
            return this.capture.QueryFrame();
        }

        public void Dispose()
        {
            this.capture.Stop();
        }
    }
}
