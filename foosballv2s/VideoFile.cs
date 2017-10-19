using System;
using System.Collections.Generic;
using System.Linq;
using Emgu.CV;
using System.IO;

namespace foosballv2s
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