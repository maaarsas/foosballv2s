using Emgu.CV;

namespace foosballv2s
{
    public interface IVideo
    {
        bool IsOpened { get; set; }
        
        Mat GetFrame();
        void Dispose();
    }
}