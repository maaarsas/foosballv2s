using System;
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
    public class TestGameTimeHelper
    {
        [Test, TestCaseSource("GetStartEndTimes")]
        public void TestGetTimeString(DateTime? startTime, DateTime? endTime, string expected)
        {
            var timeString = GameTimeHelper.GetTimeString(startTime, endTime);
            
            Assert.AreEqual(expected, timeString);
        }

        private static object[] GetStartEndTimes =
        {
            new object[] { new DateTime(2017, 11, 22, 12, 34, 56), new DateTime(2017, 11, 22, 12, 34, 56), "00:00" },
            new object[] { new DateTime(2017, 11, 22, 12, 34, 56), new DateTime(2017, 11, 22, 12, 34, 59), "00:03" },
            new object[] { new DateTime(2017, 11, 22, 12, 34, 56), new DateTime(2017, 11, 22, 12, 35, 6), "00:10" },
            new object[] { new DateTime(2017, 11, 22, 12, 34, 56), new DateTime(2017, 11, 22, 12, 35, 55), "00:59" },
            new object[] { new DateTime(2017, 11, 22, 12, 34, 56), new DateTime(2017, 11, 22, 12, 35, 56), "01:00" },
            new object[] { new DateTime(2017, 11, 22, 12, 34, 56), new DateTime(2017, 11, 22, 12, 36, 56), "02:00" },
            new object[] { null, null, "00:00" },
        };

    }
}