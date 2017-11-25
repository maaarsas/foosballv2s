using System;
using System.IO;
using NUnit.Framework;

namespace foosballv2s.Test.Source.Services.GameRecognition
{
    [TestFixture]
    public class VideoFileTests
    {
        String projectRootDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\..\\..\\";

        [Test]
        public void Cannot_Create_With_Bad_Path()
        {
            Assert.Throws<FileNotFoundException>(() => new VideoFile("/bad/path/to/file"));    
        }
    }
}
