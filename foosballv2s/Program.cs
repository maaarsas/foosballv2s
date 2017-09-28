using System;
using System.IO;

namespace foosballv2s
{
    class Program
    {
        static void Main(string[] args)
        {
            String videoName = "\\foosballv2s\\data\\video_samples\\20170914_121625.mp4";
            String ballImageName = "\\foosballv2s\\data\\ball.jpg";
            try
            {
                String projectRootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())
                    .Parent.Parent.FullName;
                IVideo videoFile = new VideoFile(projectRootDirectory + videoName);
                BallImage ballImage = new BallImage(projectRootDirectory + ballImageName);
                MovementDetector detector = new MovementDetector(videoFile);
                detector.DetectBall(ballImage.getColor());
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine("Video file \"" + exception.Message + "\" could not be opened");
            }
            
        }
    }
}
