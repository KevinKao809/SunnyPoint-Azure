using ShareClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spServiceBusQueueComsumer
{
    class DashBoardCurrentDataModel
    {
        public int MidnightStart = int.Parse(ConfigurationManager.AppSettings["MidNightFrom"]);
        public int MidnightEnd = int.Parse(ConfigurationManager.AppSettings["MidNightTo"]);
        public double getDayNegativeEvents(int accountId)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var Dayquery = from tp in spEnty.TripPoints
                           join t in spEnty.Trips on tp.TripID equals t.Id
                           where tp.Deleted == false && t.Deleted == false && !(tp.LocalTime.Value.Hour >= MidnightStart && tp.LocalTime.Value.Hour < MidnightEnd) && t.AccountID == accountId
                           group tp by new { t.AccountID } into g
                           select new { AccountID = g.Key.AccountID, result = Math.Round((double)(g.Count(tp => tp.MessageType.Equals("hard_acceleration")) + g.Count(tp => tp.MessageType.Equals("hard_braking")) + g.Count(tp => tp.OverSpeed.Value == true)) / g.Count(), 5) };
            if (Dayquery.Any()) return Dayquery.FirstOrDefault().result;
            else return 0;
        }

        public double getNightNegativeEvents(int accountId)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var Nightquery = from tp in spEnty.TripPoints
                             join t in spEnty.Trips on tp.TripID equals t.Id
                             where tp.Deleted == false && t.Deleted == false && (tp.LocalTime.Value.Hour >= MidnightStart && tp.LocalTime.Value.Hour< MidnightEnd) && t.AccountID == accountId
                             group tp by new { t.AccountID } into g
                             select new { AccountID = g.Key.AccountID, result = Math.Round((double)(g.Count(tp => tp.MessageType.Equals("hard_acceleration")) + g.Count(tp => tp.MessageType.Equals("hard_braking")) + g.Count(tp => tp.OverSpeed.Value == true)) / g.Count(), 5) };
            if (Nightquery.Any()) return Nightquery.FirstOrDefault().result;
            else return 0;
        }

        public List<DailyReport> getNegativeEvents(int accountId)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var NegativeEventsquery = from tp in spEnty.TripPoints
                                      join t in spEnty.Trips on tp.TripID equals t.Id
                                      where tp.Deleted == false && t.Deleted == false && t.AccountID == accountId
                                      group tp by new { t.AccountID } into g
                                      select new DailyReport() { accountID = (int)g.Key.AccountID, hardAccelerations = g.Count(tp => tp.MessageType.Equals("hard_acceleration")), hardBreaks = g.Count(tp => tp.MessageType.Equals("hard_braking")), overSpeed = g.Count(tp => tp.OverSpeed.Value == true) };
            return NegativeEventsquery.ToList();
            
        }

        public List<DailyReport> getScoreAndNegativeEventByMonth(int accountId, string YearMonth)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2Equery = from md in spEnty.DashboardMonthlyData
                           where md.Deleted == false && md.AccountID == accountId && md.YearMonth == YearMonth
                           select new DailyReport() { Score = (int)md.Score, NegativeEvents = (int)md.NegativeEvents };
            return L2Equery.ToList();
        }

        public DailyReport getTripsAndMileageByMonthDay(int accountId, DateTime NowTime)
        {
            DateTime firstDateofMonth = new DateTime(NowTime.Year, NowTime.Month, 1);
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2Equery = from t in spEnty.Trips
                           where t.Deleted == false && t.AccountID == accountId && t.StartDateTime > firstDateofMonth && t.StartDateTime < NowTime
                           group t by new { t.AccountID } into g
                           select new DailyReport { accountID = (int)g.Key.AccountID, Trips = g.Count(), Mileage = (float)g.Sum(t => t.Distance) };
            return L2Equery.FirstOrDefault();
        }
    }

}
