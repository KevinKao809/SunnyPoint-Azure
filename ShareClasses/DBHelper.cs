using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClasses
{
    public class DBHelper
    {
        // AccountProfile Table
        public void updateAccount(AccountProfiles account)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            spEnty.AccountProfiles.Attach(account);
            spEnty.Entry(account).State = System.Data.Entity.EntityState.Modified;
            spEnty.SaveChanges();
        }

        public AccountProfiles getAccountProfileByID(int accountID)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.AccountProfiles
                           where a.Id == accountID && a.Deleted == false
                           select a;
            return L2EQuery.FirstOrDefault<AccountProfiles>();
        }

        public List<AccountProfiles> getEnterpriseAccountProfiles()
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.AccountProfiles
                           where a.isCompany==true && a.Deleted == false
                           select a;
            return L2EQuery.ToList<AccountProfiles>();
        }

        public List<AccountProfiles> getConsumerAccountProfiles()
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.AccountProfiles
                           where a.isCompany == false && a.Deleted == false
                           select a;
            return L2EQuery.ToList<AccountProfiles>();
        }
        public List<AccountProfiles> getAllValidAccountProfiles()
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.AccountProfiles
                           where  a.Deleted == false
                           select a;
            return L2EQuery.ToList<AccountProfiles>();
        }

        // Devices Table
        public List<Devices> getDeviceListByAccountID(int accountID)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.Devices
                           where a.AccountID == accountID && a.Deleted == false
                           select a;
            return L2EQuery.ToList<Devices>();
        }

        public Devices getDeviceByIoTHubDeviceID(string IoTHubDeviceID)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.Devices
                           where a.IoTHubDeviceID == IoTHubDeviceID && a.Deleted == false
                           select a;
            return L2EQuery.FirstOrDefault<Devices>();
        }

        public void updateDeiveByIoTHubDeviceID(Devices updatedDevice)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            spEnty.Devices.Attach(updatedDevice);  
            spEnty.Entry(updatedDevice).State = System.Data.Entity.EntityState.Modified;
            spEnty.SaveChanges();
        }

        // IoTHubs Table
        public IoTHubs getIoTHubByIoTHubHostName(string IoTHubHostName)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.IoTHubs
                           where a.IoTHubHostName == IoTHubHostName && a.Deleted == false
                           select a;
            return L2EQuery.FirstOrDefault<IoTHubs>();
        }

        // TripPoints Table
        public void updateTripPoints(List<TripPoints> tripPoints)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            foreach (var tripPoint in tripPoints)
            {
                spEnty.TripPoints.Attach(tripPoint);
                spEnty.Entry(tripPoint).State = System.Data.Entity.EntityState.Modified;
            }
            spEnty.SaveChanges();            
        }

        public List<TripPoints> getTripPointsByTripID(string tripID)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.TripPoints
                           where a.TripID == tripID && a.Deleted == false
                           orderby a.RecordedTimeStamp ascending
                           select a;
            return L2EQuery.ToList<TripPoints>();
        }
     

        public void deleteTripPoints(List<TripPoints> tripPoints)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            foreach (var tripPoint in tripPoints)
            {
                tripPoint.Deleted = true;
                spEnty.TripPoints.Attach(tripPoint);
                spEnty.Entry(tripPoint).State = System.Data.Entity.EntityState.Modified;
            }
            spEnty.SaveChanges();
        }

        public void deleteTripPointsByTripID(string tripID)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();

            List<TripPoints> tripPoints = getTripPointsByTripID(tripID);

            foreach (var tripPoint in tripPoints)
            {
                tripPoint.Deleted = true;
                spEnty.TripPoints.Attach(tripPoint);
                spEnty.Entry(tripPoint).State = System.Data.Entity.EntityState.Modified;
            }
            spEnty.SaveChanges();
        }

        // Trips Table
        public Trips getTripByTripID(string tripID)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.Trips
                           where a.Id == tripID && a.Deleted == false
                           select a;
            return L2EQuery.FirstOrDefault<Trips>();
        }

        public List<Trips> getAllValidTripsOrderByAccountID()
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.Trips
                           where a.Deleted == false && a.AccountID > 0 && a.ProcessCompleteFlag==true
                           orderby a.AccountID
                           select a;
            return L2EQuery.ToList<Trips>();
        }

        public List<Trips> getNValidTripsByAccountID(int accountID, int max)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = (from a in spEnty.Trips
                           where a.Deleted == false && a.AccountID == accountID && a.ProcessCompleteFlag == true && a.Distance > 0
                           orderby a.StartTimeStamp descending
                           select a).Take(max);
            return L2EQuery.ToList<Trips>();
        }

        public void updateTrip(Trips trip)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();  
            spEnty.Trips.Attach(trip);
            spEnty.Entry(trip).State = System.Data.Entity.EntityState.Modified;
            spEnty.SaveChanges();
        }          

        public void deleteTrips(List<Trips> trips)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            foreach (var trip in trips)
            {
                trip.Deleted = true;
                spEnty.Trips.Attach(trip);
                spEnty.Entry(trip).State = System.Data.Entity.EntityState.Modified;
            }
            spEnty.SaveChanges();
        }

        public DashboardMonthlyData getDashboardMonthlyData(int accountID, string date)
        {

                SunnyPointEntities spEnty = new SunnyPointEntities();
                var L2EQuery =  from a in spEnty.DashboardMonthlyData
                                where a.Deleted == false && a.AccountID == accountID && a.YearMonth == date
                                select a;

            return L2EQuery.FirstOrDefault();
        }
        public void insertDashboardMonthlyData(DashboardMonthlyData mData)
        {
            try
            {
                SunnyPointEntities spEnty = new SunnyPointEntities();
                Console.WriteLine("insert..");
                spEnty.DashboardMonthlyData.Add(mData);
                spEnty.Entry(mData).State = System.Data.Entity.EntityState.Added;
                spEnty.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public void updateDashboardMonthlyData(DashboardMonthlyData mData)
        {
            try
            {
                   SunnyPointEntities spEnty = new SunnyPointEntities();
                    Console.WriteLine("update..");
                    spEnty.DashboardMonthlyData.Attach(mData);
                    spEnty.Entry(mData).State = System.Data.Entity.EntityState.Modified;
                    spEnty.SaveChanges();

            }
            catch
            {
                throw;
            }
        }
        //DashboardCurrentData
        public DashboardCurrentData getDashboardCurrentData(int accountID)
        {

            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.DashboardCurrentData
                           where a.Deleted == false && a.AccountID == accountID 
                           select a;
            return L2EQuery.FirstOrDefault();
        }
        public void insertDashboardCurrentData(DashboardCurrentData dData)
        {
            try
            {
                SunnyPointEntities spEnty = new SunnyPointEntities();
                Console.WriteLine("insert..");
                spEnty.DashboardCurrentData.Add(dData);
                spEnty.Entry(dData).State = System.Data.Entity.EntityState.Added;
                spEnty.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public void updateDashboardCurrentData(DashboardCurrentData dData)
        {
            try
            {
                SunnyPointEntities spEnty = new SunnyPointEntities();
                Console.WriteLine("update..");
                spEnty.DashboardCurrentData.Attach(dData);
                spEnty.Entry(dData).State = System.Data.Entity.EntityState.Modified;
                spEnty.SaveChanges();

            }
            catch
            {
                throw;
            }
        }
    }
}
