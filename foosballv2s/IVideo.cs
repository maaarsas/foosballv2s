using Emgu.CV;

namespace foosballv2s
{
    public interface IVideo
    {
        Mat GetFrame();
        void Dispose();
    }
}
