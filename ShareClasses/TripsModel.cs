using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClasses
{
    public class TripsModel
    {
        public bool isTripMessageComplete(string tripID)
        {
            bool tripComplete = false;

            DBHelper dbhelp = new DBHelper();
            List<TripPoints> tripPoints = dbhelp.getTripPointsByTripID(tripID);
            if (tripPoints != null || tripPoints.Count > 1)
            {
                if (tripPoints[0].MessageType != "trip_start")
                    throw new Exception("trip_start not found");
                if (tripPoints[tripPoints.Count - 1].MessageType != "trip_end")
                    throw new Exception("trip_end not found");

                tripComplete = true;
            }
            return tripComplete;
        }

        public List<Trips> getDistanceZeroTrips()
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.Trips
                           where a.ProcessCompleteFlag == true && a.Distance == 0 && a.Deleted == false
                           select a;
            return L2EQuery.ToList<Trips>();
        }

        public List<Trips> getZeroMaxSpeedTrips()
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.Trips
                           where a.ProcessCompleteFlag == true && a.MaxSpeed == 0 && a.Deleted == false
                           select a;
            return L2EQuery.ToList<Trips>();
        }

        public List<TripPoints> getTripPointsWithoutStartMessage()
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();

            var innerQuery = from b in spEnty.Trips select b.Id;
            var L2EQuery = from a in spEnty.TripPoints
                           where !innerQuery.Contains(a.TripID) && a.Deleted == false
                           select a;
            return L2EQuery.ToList<TripPoints>();
        }

        public List<Trips> getIncompleteTrips()
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();
            var L2EQuery = from a in spEnty.Trips
                           where a.isComplete == false && a.Deleted == false
                           select a;
            return L2EQuery.ToList<Trips>();
        }

        public bool isTripEndOverDue(Trips trip, int overDueInHours)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();

            var L2EQuery = (from a in spEnty.TripPoints
                           where a.TripID == trip.Id && a.Deleted == false
                           orderby a.RecordedTimeStamp descending
                           select a).Take(1);

            TripPoints tripPoint = L2EQuery.FirstOrDefault<TripPoints>();
            if (tripPoint != null)
                if (DateTimeService.GetDateTimeByTimeStamp((long)tripPoint.RecordedTimeStamp).AddHours(overDueInHours) < DateTime.UtcNow)
                {
                    return true;
                }
            return false;
        }

        public void appendTripEndPoint(Trips trip)
        {
            SunnyPointEntities spEnty = new SunnyPointEntities();

            var L2EQuery = (from a in spEnty.TripPoints
                            where a.TripID == trip.Id && a.Deleted == false
                            orderby a.RecordedTimeStamp descending
                            select a).Take(1);

            TripPoints tripPoint = L2EQuery.FirstOrDefault<TripPoints>();
            if (tripPoint != null)
            {
                tripPoint.RecordedTimeStamp = tripPoint.RecordedTimeStamp + 1000;
                tripPoint.MessageType = "trip_end";
                spEnty.TripPoints.Add(tripPoint);
                spEnty.SaveChanges();
            }
        }
    }
}
