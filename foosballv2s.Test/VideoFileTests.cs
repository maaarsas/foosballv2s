using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foosballv2s;
using System.IO;

namespace foosballv2s.Test
{
    [TestFixture]
    public class VideoFileTests
    {
        String projectRootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())
                    .Parent.FullName;

        [Test]
        public void Cannot_Create_With_Bad_Path()
        {
            Assert.Throws<FileNotFoundException>(() => new VideoFile("/bad/path/to/file"));    
        }

        //File not found
        [Test]
        public void Create_And_Check_Capture_Successful()
        {
            VideoFile videoFile = 
                new VideoFile(projectRootDirectory + "\\testData\\20170914_121625.mp4");
            Assert.True(videoFile.capture.IsOpened);
        }
    }
}
