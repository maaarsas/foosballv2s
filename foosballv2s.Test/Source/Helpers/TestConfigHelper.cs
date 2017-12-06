using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Test.Mock;
using foosballv2s.Droid.Shared.Source.Helpers;
using NUnit.Framework;
using PCLMock;

namespace foosballv2s.Test.Source.Activities.Helpers
{
    [TestFixture]
    public class TestConfigHelper
    {
        [TestCase("api_url", "127.0.0.1")]
        [TestCase("test_key", "some_value")]
        [TestCase("another_key", "another_value")]
        public void TestGetConfigData(string configName, string expected)
        {
            var a = Application.Context;
            var configString = ConfigHelper.GetConfigData(Application.Context, configName);
            
            Assert.AreEqual(expected, configString);
        }

        public static string GetFakeConfigDataByName(string name)
        {
            IList<string[]> pairs = new List<string[]>(new string[][]
            {
                new string[] {"api_url", "127.0.0.1"},
                new string[] {"api_urlasd", "127.3.0.1"},
                new string[] {"test_key", "some_value"},
                new string[] {"another_key", "another_value"},
            });
            return pairs.First(p => p[0] == name)[1];
        }

//        public class DemoContext : MockBase<Context>
//        {
//            public DemoContext(MockBehavior behavior = MockBehavior.Loose) : base(behavior)
//            {
//                this.When(x => x.PackageManager).Return(new DemoPackageManager().MockedObject);
//            }
//        }
//
//        public class DemoPackageManager : MockBase<PackageManager>
//        {
//            public DemoPackageManager(MockBehavior behavior = MockBehavior.Loose) : base(behavior)
//            {
//                this.When(x => x.GetApplicationInfo(It.IsAny<string>(), It.IsAny<PackageInfoFlags>()))
//                    .Return(new DemoAppInfo().MockedObject);
//            }
//        }
//
//        public class DemoAppInfo : MockBase<ApplicationInfo>
//        {
//            public DemoAppInfo(MockBehavior behavior = MockBehavior.Loose) : base(behavior)
//            {
//                this.When(x => x.MetaData).Return(new DemoBundle().MockedObject);
//            }
//        }
//
//        public class DemoBundle : MockBase<Bundle>
//        {
//            public override Bundle MockedObject { get; } = new DemoBundle().MockedObject;
//            
//            public DemoBundle(MockBehavior behavior = MockBehavior.Loose) : base(behavior)
//            {
//                this.When(x => x.GetString(It.IsAny<string>())).Return((string name) => GetFakeConfigDataByName(name));
//            }
//        }
        
    }
}