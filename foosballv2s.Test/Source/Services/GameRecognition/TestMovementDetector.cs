using System;
using NUnit.Framework;

namespace foosballv2s.Test.Source.Services.GameRecognition
{
    [TestFixture]
    public class MovementDetectorTests
    {
        String projectRootDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\..\\..\\";
        
        [Test]
        public void DetectBall_Successful()
        {
            VideoFile videoFile =
                new VideoFile(projectRootDirectory + "\\testData\\20170914_121625.mp4");
            MovementDetector movementDetector = new MovementDetector(videoFile);
            Assert.True(movementDetector.Video.IsOpened);
        }
    }
}
