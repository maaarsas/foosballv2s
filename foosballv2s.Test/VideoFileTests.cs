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
        [Test]
        public void Cannot_Create_With_Bad_Path()
        {
            Assert.Throws<FileNotFoundException>(() => new VideoFile("/bad/path/to/file"));    
        }
    }
}
