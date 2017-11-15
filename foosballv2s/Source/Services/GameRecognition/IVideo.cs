using Emgu.CV;

namespace foosballv2s.Source.Services.GameRecognition
{
    public interface IVideo
    {
        bool IsOpened { get; set; }
        
        Mat GetFrame();
        void Dispose();
    }
}