using ShareClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spServiceBusQueueComsumer
{
    public class TripOverdueProcessor
    {
        List<Trips> incompletedTripList;
        int tripEndOverDueInHours = int.Parse(ConfigurationManager.AppSettings["TripEndOverDueInHours"]);
        public static LogStore logStore = new LogStore("spServiceBusQueueComsumer");

        public TripOverdueProcessor()
        {            
        }

        public void ManageIncompletedTrip()
        {            
            try
            {
                TripsModel tripsModel = new TripsModel();

                logStore.postLog("info", "TripOverdueProcessor", "ManageIncompletedTrip", "Starting", "");
                incompletedTripList = tripsModel.getIncompleteTrips();
                if (incompletedTripList != null && incompletedTripList.Count > 0)
                {
                    for (int i = 0; i < incompletedTripList.Count; i++)
                    {
                        if (tripsModel.isTripEndOverDue(incompletedTripList[i], tripEndOverDueInHours))
                        {
                            tripsModel.appendTripEndPoint(incompletedTripList[i]);

                            spServiceBusMessage spMessage = new spServiceBusMessage("trip_end", incompletedTripList[i].Id);
                            Operation.sendMsgToServiceBus(spMessage);
                        }
                    }
                }
                logStore.postLog("info", "TripOverdueProcessor", "ManageIncompletedTrip", "Completed", "");
            }
            catch (Exception ex)
            {
                logStore.postLog("Error", "TripOverdueProcessor", "ManageIncompletedTrip", "Error", ex.Message);
                throw;
            }
        }
    }
}
