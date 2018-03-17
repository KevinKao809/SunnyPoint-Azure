using ShareClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spServiceBusQueueComsumer
{
    class MonthlyReport
    {
 
        public string YearMonth=getCurrentYearMonth();
        public int Score = 89;
 
        public MonthlyReport(string insertDate)
        {
            try
            {
                DateTime dt=  DateTime.Parse(insertDate);
                this.YearMonth = dt.ToString("yyyy/MM");
            }
            catch
            {
                throw;
            }
        }

        public MonthlyReport()
        {

        }
        public void Caculation()
        {
            try
            {
                
                DBHelper dbhelp = new DBHelper();
                SunnyPointEntities spEnty = new SunnyPointEntities();
                
                DateTime dt = DateTime.Parse(YearMonth);
                var firstDayOfMonth = new DateTime(dt.Year, dt.Month, 1);
                var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
                long firstDayOfMonthTimeStamp = DateTimeService.GetTimeStampByDateTime(firstDayOfMonth);
                long firstDayOfNextMonthTimeStamp = DateTimeService.GetTimeStampByDateTime(firstDayOfNextMonth);
               
                var Accounts = from a in spEnty.Trips
                               where a.Deleted == false && a.AccountID > 0 && a.ProcessCompleteFlag == true && a.StartTimeStamp>= firstDayOfMonthTimeStamp && a.StartTimeStamp< firstDayOfNextMonthTimeStamp
                               orderby a.AccountID
                               group a by new { a.AccountID } into g
                               select new { AccountID = g.Key.AccountID, Trips = g.Count(), Mileage = g.Sum(a => a.Distance), DurationInMin = (int)g.Sum(a => (a.EndTimeStamp - a.StartTimeStamp) / 60000), NegativeEvents = g.Sum(a => a.HardBreaks) + g.Sum(a => a.HardAccelerations) + g.Sum(a => a.OverSpeeds) };

                if (Accounts != null && Accounts.Count()>0)
                {
                    foreach (var acct in Accounts)
                    {

                        Console.WriteLine("ID={0},YearMonth={1},distance={2},duration={3},NE={4},trips={5}", acct.AccountID,YearMonth, acct.Mileage, acct.DurationInMin, acct.NegativeEvents, acct.Trips);
                        DashboardMonthlyData MonthlyData = dbhelp.getDashboardMonthlyData((int)acct.AccountID, YearMonth);
                        if (MonthlyData != null)
                        {
                            MonthlyData.AccountID = (int)acct.AccountID;
                            MonthlyData.Trips = acct.Trips;
                            MonthlyData.Mileage = acct.Mileage;
                            MonthlyData.Score = this.Score;
                            MonthlyData.YearMonth = YearMonth;
                            MonthlyData.NegativeEvents = acct.NegativeEvents;
                            MonthlyData.DurationInMin = acct.DurationInMin;
                            MonthlyData.UpdatedAt = DateTime.UtcNow;
                            dbhelp.updateDashboardMonthlyData(MonthlyData);
                        }
                        else
                        {
                            MonthlyData = new DashboardMonthlyData();
                            MonthlyData.AccountID = (int)acct.AccountID;
                            MonthlyData.Trips = acct.Trips;
                            MonthlyData.Mileage = acct.Mileage;
                            MonthlyData.Score = this.Score;
                            MonthlyData.YearMonth = YearMonth;
                            MonthlyData.NegativeEvents = acct.NegativeEvents;
                            MonthlyData.DurationInMin = acct.DurationInMin;
                            dbhelp.insertDashboardMonthlyData(MonthlyData);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No match data in {0}", YearMonth);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        private static string getCurrentYearMonth()
        {
            var currentTime = DateTime.UtcNow;
            string strYM = currentTime.ToString("yyyy/MM");
            return strYM;
        }

    }

}
