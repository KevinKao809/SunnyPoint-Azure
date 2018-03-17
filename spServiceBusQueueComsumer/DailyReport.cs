using ShareClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spServiceBusQueueComsumer
{
    class DailyReport
    {
        public int accountID;
        public int Trips;
        public float Mileage;
        public int hardBreaks;
        public int hardAccelerations;
        public int overSpeed;
        public int Score;
        public int NegativeEvents;

        DateTime UtcTimeNow = DateTime.UtcNow;

        public void Caculation()
        {
            try
            {           
                DBHelper dbhelp = new DBHelper();
                List<AccountProfiles> allAccount = dbhelp.getAllValidAccountProfiles();
                
                foreach (var acct in allAccount)
                {

                    DashboardCurrentData dashboardCurrentData = dbhelp.getDashboardCurrentData((int)acct.Id);
                    if (dashboardCurrentData != null)
                    {
                        //Update
                        dashboardCurrentData.AccountID = acct.Id;
                        dashboardCurrentData = updateData(dashboardCurrentData);
                        dashboardCurrentData.UpdatedAt = UtcTimeNow;
                        dbhelp.updateDashboardCurrentData(dashboardCurrentData);
                        
                    }
                    else
                    {
                        //Insert
                        dashboardCurrentData = new DashboardCurrentData();
                        dashboardCurrentData.AccountID = acct.Id;
                        dashboardCurrentData = updateData(dashboardCurrentData);
                        dashboardCurrentData.CreatedAt = UtcTimeNow;
                        dashboardCurrentData.UpdatedAt = UtcTimeNow;
                        dashboardCurrentData.Deleted = false;
                        dbhelp.insertDashboardCurrentData(dashboardCurrentData);
                    }
                }

            }
            catch
            {
                throw;
            }
        }

    
        private static string getCurrentYearMonth()
        {
            var currentTime = DateTime.UtcNow;
            string strYMD = currentTime.ToString("yyyy/MM");
            return strYMD;
        }
        private static string getPreviousYearMonth()
        {
            var currentTime = DateTime.UtcNow.AddMonths(-1);
            string strYM = currentTime.ToString("yyyy/MM");
            return strYM;
        }

        private DashboardCurrentData updateData(DashboardCurrentData dashboardCurrentData)
        {
            DashBoardCurrentDataModel dashBoardCurrentDataModel = new DashBoardCurrentDataModel();
            DailyReport dailyReport = new DailyReport();
            List<DailyReport> dailyReportList = new List<DailyReport>();
            dashboardCurrentData.Day_NegativeEvents = dashBoardCurrentDataModel.getDayNegativeEvents(dashboardCurrentData.AccountID);
            dashboardCurrentData.Midnight_NegativeEvents = dashBoardCurrentDataModel.getNightNegativeEvents(dashboardCurrentData.AccountID);
            //hardAccelerations, hardbreaks, Overspeed
            dailyReportList = dashBoardCurrentDataModel.getNegativeEvents(dashboardCurrentData.AccountID);
            if (dailyReportList != null)
            {
                foreach (var n in dailyReportList)
                {
                    dashboardCurrentData.HardAccelerations = n.hardAccelerations;
                    dashboardCurrentData.HardBreaks = n.hardBreaks;
                    dashboardCurrentData.OverSpeed = n.overSpeed;
                }
            }
            //CurrentMonthScore,CurrentMonthNegativeEvents 
            dailyReportList = dashBoardCurrentDataModel.getScoreAndNegativeEventByMonth(dashboardCurrentData.AccountID, getCurrentYearMonth());
            if (dailyReportList != null)
            {
                foreach (var n in dailyReportList)
                {
                    dashboardCurrentData.CurrentMonthScore = n.Score;
                    dashboardCurrentData.CurrentMonthNegativeEvents = n.NegativeEvents;
                }
            }
            //PreviousMonthScore,PreviousMonthNegativeEvents
            dailyReportList = dashBoardCurrentDataModel.getScoreAndNegativeEventByMonth(dashboardCurrentData.AccountID, getPreviousYearMonth());
            if (dailyReportList != null)
            {
                foreach (var n in dailyReportList)
                {
                    dashboardCurrentData.PreviousMonthScore = n.Score;
                    dashboardCurrentData.PreviousMonthNegativeEvents = n.NegativeEvents;
                }
            }
            //CurrentMonthDayTrips,CurrentMonthDayMileage
            dailyReport = dashBoardCurrentDataModel.getTripsAndMileageByMonthDay(dashboardCurrentData.AccountID, UtcTimeNow);
            if (dailyReport != null)
            {
                dashboardCurrentData.CurrentMonthDayTrips = dailyReport.Trips;
                dashboardCurrentData.CurrentMonthDayMileage = dailyReport.Mileage;
            }
            else
            {
                dashboardCurrentData.CurrentMonthDayTrips = 0;
                dashboardCurrentData.CurrentMonthDayMileage = 0;
            }
            //PreviousMonthDayTrips,PreviousMonthDayMileage
            dailyReport = dashBoardCurrentDataModel.getTripsAndMileageByMonthDay(dashboardCurrentData.AccountID, UtcTimeNow.AddMonths(-1));
            if (dailyReport != null)
            {
                dashboardCurrentData.PreviousMonthDayTrips = dailyReport.Trips;
                dashboardCurrentData.PreviousMonthDayMileage = dailyReport.Mileage;
            }
            else
            {
                dashboardCurrentData.PreviousMonthDayTrips = 0;
                dashboardCurrentData.PreviousMonthDayMileage = 0;
            }

            return dashboardCurrentData;
        }

    }
}
