using NUnit.Framework;
using System;
using System.IO;

namespace foosballv2s.Test
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
