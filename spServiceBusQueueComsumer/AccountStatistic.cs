using ShareClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spServiceBusQueueComsumer
{
    public class AccountStatistic
    {
        public static LogStore logStore = new LogStore("spServiceBusQueueComsumer");

        public AccountStatistic()
        {            
        }

        public void RunStatistic()
        {
            DBHelper dbhelp = new DBHelper();
            List<Trips> allTrips = dbhelp.getAllValidTripsOrderByAccountID();            

            if (allTrips!=null && allTrips.Count>0)
            {
                var accounts = from p in allTrips
                                    group p by new { p.AccountID } into g
                                    select new {accountID = g.Key.AccountID, trips = g.Count(), distance = g.Sum(p => p.Distance), driveTimeinSec = g.Sum(p=>(p.EndTimeStamp-p.StartTimeStamp)/1000), hardbreaks = g.Sum(p=>p.HardBreaks), hardAcce = g.Sum(p=>p.HardAccelerations), MaxSpeed=g.Max(p=>p.MaxSpeed), MidnighDriveInSec=g.Sum(p=>p.MidNightDriveInSec), OverSpeeds=g.Sum(p=>p.OverSpeeds) };

                foreach (var acct in accounts)
                {
                    AccountProfiles acctProfile = dbhelp.getAccountProfileByID((int)acct.accountID);
                    if (acctProfile != null)
                    {
                        acctProfile.Trips = acct.trips;
                        acctProfile.Distance = acct.distance;
                        acctProfile.DriveTimeInMin = (int)(acct.driveTimeinSec / 60);
                        acctProfile.MidnightDriveInMin = (int)(acct.MidnighDriveInSec / 60);
                        acctProfile.HardBreaks = acct.hardbreaks;
                        acctProfile.HardAccelerations = acct.hardAcce;
                        acctProfile.MaxSpeed = acct.MaxSpeed;
                        acctProfile.OverSpeeds = acct.OverSpeeds;
                        dbhelp.updateAccount(acctProfile);
                    }
                }
            }            
            logStore.postLog("info", "AccountStatistic", "RunStatistic", "Run Statistic on All Accounts Done", "");           
        }
    }
}
