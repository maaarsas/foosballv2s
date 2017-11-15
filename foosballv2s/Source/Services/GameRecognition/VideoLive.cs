using System;
using Emgu.CV;

namespace foosballv2s.Source.Services.GameRecognition
{
    public class VideoLive : IVideo
    {
        public bool IsOpened { get; set; }
        // TODO: Implement the class for getting frames from live stream
        public Mat GetFrame()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}