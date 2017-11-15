using System;
using System.IO;
using Emgu.CV;

namespace foosballv2s.Source.Services.GameRecognition
{
    public class VideoFile : IVideo
    {
        public bool IsOpened { get; set; } = false;
        public VideoCapture capture { get; }

        public VideoFile(String file)
        {
            this.capture = new VideoCapture(file);
            if (!capture.IsOpened)
            {
                throw new FileNotFoundException(file);
            }
            IsOpened = true;
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