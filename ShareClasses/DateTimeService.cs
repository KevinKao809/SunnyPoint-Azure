using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClasses
{
    public class DateTimeService
    {
        public static long GetTimeStampByDateTime(DateTime inputDateTime)
        {
            DateTime baseTime = new DateTime(1970, 1, 1);//宣告一個GTM時間出來
            long timeStamp = Convert.ToInt64(((TimeSpan)inputDateTime.Subtract(baseTime)).TotalMilliseconds);
            return timeStamp;
        }

        public static DateTime GetDateTimeByTimeStamp(long timeStamp)
        {
            timeStamp = timeStamp / 1000;
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timeStamp);
            return dtDateTime;

        }
        public static string ConvertTotalSecondToString(double totalSecond)
        {
            int i_duration = (int)Math.Round(totalSecond, 0);
            int hh = i_duration / 3600;
            int j_duration = i_duration - (hh * 3600);

            int mm = j_duration / 60;
            int ss = j_duration - (mm * 60);
            string s_duration = hh.ToString("00") + ":" + mm.ToString("00") + ":" + ss.ToString("00");
            return s_duration;
        }
    }
}
