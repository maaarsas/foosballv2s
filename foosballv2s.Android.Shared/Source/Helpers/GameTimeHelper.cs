using System;

namespace foosballv2s.Droid.Shared.Source.Helpers
{
    /// <summary>
    /// Game timer manipulation and setup class
    /// </summary>
    public class GameTimeHelper
    {
        /// <summary>
        /// Forms a timer string from the given start and end times
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
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