using Emgu.CV;

namespace foosballv2s.Droid.Shared.Source.Services.GameRecognition
{
    public interface IVideo
    {
        bool IsOpened { get; set; }
        
        Mat GetFrame();
        void Dispose();
    }
}