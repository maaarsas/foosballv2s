using System;

namespace foosballv2s
{
    public class GameTimeHelper
    {
        public static string GetTimeString(DateTime? startTime, DateTime? endTime)
        {
            DateTime fromTime = startTime ?? DateTime.Now;
            DateTime toTime = endTime ?? DateTime.Now;
            TimeSpan secondsSpan = (toTime - fromTime);
            string timeString = secondsSpan.ToString(@"mm\:ss");
            return timeString;
        }
    }
}