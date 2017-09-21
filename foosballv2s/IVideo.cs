using Emgu.CV;

namespace foosballv2s
{
    interface IVideo
    {
        Mat GetFrame();
        void Dispose();
    }
}
