using ShareClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spServiceBusQueueComsumer
{
    public class CleanFailTrip
    {
        public static LogStore logStore = new LogStore("spServiceBusQueueComsumer");

        public CleanFailTrip()
        {            
        }

        public void Clean()
        {
            /* Clean Distance == 0 and Process Completed Trips */
            TripsModel tripsModel = new TripsModel();
            DBHelper dbhelp = new DBHelper();
            List<Trips> ZeroDistanceTrips = tripsModel.getDistanceZeroTrips();            

            if (ZeroDistanceTrips != null && ZeroDistanceTrips.Count() > 0)
            {
                foreach (var trip in ZeroDistanceTrips)
                    dbhelp.deleteTripPointsByTripID(trip.Id);

                dbhelp.deleteTrips(ZeroDistanceTrips);                
            }
            logStore.postLog("info", "CleanFailTrip", "Clean", "Clean Distance == 0 and Process Completed Trips. Done", "");

            /* Clean No Trip_Start TripPoints */
            List<TripPoints> WithoutStartMessageTripPoints = tripsModel.getTripPointsWithoutStartMessage();
            if (WithoutStartMessageTripPoints != null && WithoutStartMessageTripPoints.Count() > 0)
            {
                dbhelp.deleteTripPoints(WithoutStartMessageTripPoints);
            }
            logStore.postLog("info", "CleanFailTrip", "Clean", "Clean No Trip Start Records Done", "");

            /* Clean MaxSpeed == 0 Trip */
            List<Trips> ZeroMaxSpeedTrips = tripsModel.getZeroMaxSpeedTrips();
            if (ZeroMaxSpeedTrips != null && ZeroMaxSpeedTrips.Count() > 0)
            {
                foreach (var trip in ZeroMaxSpeedTrips)
                    dbhelp.deleteTripPointsByTripID(trip.Id);

                dbhelp.deleteTrips(ZeroMaxSpeedTrips);
            }
            logStore.postLog("info", "CleanFailTrip", "Clean", "Clean MaxSpeed == 0 Records Done", "");
        }
    }
}
