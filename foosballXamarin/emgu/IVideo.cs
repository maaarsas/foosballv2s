namespace foosballXamarin.emgu
{
    interface IVideo
    {
        Mat GetFrame();
        void Dispose();
    }
}