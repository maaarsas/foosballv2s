using System;

namespace foosballv2s
{
    public class GameTimeHelper
    {
        public static string GetTimeString(DateTime? startTime, DateTime? endTime)
        {
            DateTime fromTime = startTime ?? DateTime.Now;
            DateTime toTime = endTime ?? DateTime.Now;
            int secondsSpan = (toTime - fromTime).Seconds;
            string timeString = secondsSpan.ToString(@"mm\:ss");
            return timeString;
        }
    }
}