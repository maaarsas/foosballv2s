using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foosballv2s.Test
{
    [TestFixture]
    public class MovementDetectorTests
    {
        String projectRootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())
                    .Parent.FullName;
        
        [Test]
        public void DetectBall_Successful()
        {
            VideoFile videoFile =
                new VideoFile(projectRootDirectory + "\\testData\\20170914_121625.mp4");
            MovementDetector movementDetector = new MovementDetector(videoFile);
            Assert.True(movementDetector.DetectBall());
        }
    }
}
