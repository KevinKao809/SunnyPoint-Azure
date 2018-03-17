using ShareClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spServiceBusQueueComsumer
{
    public class TripEndProcessor
    {
        List<TripPoints> tripPoints;
        GoogleService gService = new GoogleService();
        int midNightFrom = int.Parse(ConfigurationManager.AppSettings["MidNightFrom"]);
        int midNightTo = int.Parse(ConfigurationManager.AppSettings["MidNightTo"]);
        DateTime midNightFromTime, midNightToTime;
        bool midNightOnFlag = false;
        double maxSpeed = 0, totalDistance = 0;
        double prelat = 0, prelng = 0;
        int hardBreak = 0, hardAcceleration = 0, overSpeed = 0;
        double maxLng = -180, minLng = 180, maxLat = -90, minLat = 90;
        Random rand = new Random();
        Trips theTrip;
        DBHelper _dbHelp = null;

        public TripEndProcessor(Trips trip)
        {
            try
            {
                _dbHelp = new DBHelper();
                theTrip = trip;
                tripPoints = _dbHelp.getTripPointsByTripID(theTrip.Id);
                if (tripPoints == null || tripPoints.Count == 0)
                    throw new Exception("tripPoints not found");

                Console.WriteLine("Time: " + DateTime.UtcNow.ToString() + ", TripEndProcessor, tripID:" + theTrip.Id);
            }
            catch (Exception ex)
            {                
                Program.logStore.postLog("Error", "TripEndProcessor", "TripEndProcessor", ex.Message, "tripID:" + trip.Id);
                throw;
            }
        }

        public void Calculation()
        {
            missingGPSHandle();

            for (int i = 0; i < tripPoints.Count; i++)
            {
                updateCountry_LocalTime(i).Wait();
                verifyMidnightDriving(i);
                overSpeedCheck(i);
                calculateDistance(i);
                mapBoundry(i);

                /* Max Speed */
                if (tripPoints[i].Speed > maxSpeed)
                    maxSpeed = (double)tripPoints[i].Speed;     //KM
                /* HardBreak / HardAcceleration */
                if (tripPoints[i].MessageType == "hard_braking")
                    hardBreak++;
                if (tripPoints[i].MessageType == "hard_acceleration")
                    hardAcceleration++;
            }

            updateTrip();
        }

        private void missingGPSHandle()
        {
            double preValidlat = 0, preValidlng = 0;
            List<int> MissingGPSLocationPoints = new List<int>();

            for (int i = 0; i < tripPoints.Count; i++)
            {
                /* Missing GPS location */
                if (tripPoints[i].Latitude == 0 && tripPoints[i].Longitude == 0)
                {
                    if (preValidlat != 0 && preValidlng != 0)
                    {   // using previouse location
                        tripPoints[i].Latitude = (decimal)preValidlat;
                        tripPoints[i].Longitude = (decimal)preValidlng;
                    }
                    else
                    {   // Missing GPS location at trip_start
                        MissingGPSLocationPoints.Add(i);
                    }
                }
                else
                {
                    preValidlat = (double)tripPoints[i].Latitude;
                    preValidlng = (double)tripPoints[i].Longitude;
                    if (MissingGPSLocationPoints.Count > 0)
                    {
                        foreach (var point in MissingGPSLocationPoints)
                        {
                            tripPoints[point].Latitude = (decimal)preValidlat;
                            tripPoints[point].Longitude = (decimal)preValidlng;
                        }
                        MissingGPSLocationPoints.Clear();
                    }
                }
            }
        }

        private async Task<bool> updateCountry_LocalTime(int item)
        {
            /* Get and Update Country and Local time of each Trip Points  */
            if (tripPoints[item].Latitude != 0 && tripPoints[item].Longitude != 0)
            {
                try
                {
                    //Google Map has API access quota limitation; So, we can't get country for each point at here.
                    tripPoints[item].Country = await gService.GetCountryByLocation(tripPoints[item].Latitude.ToString(), tripPoints[item].Longitude.ToString());
                    tripPoints[item].LocalTime = await gService.GetLocalTimeByLocation(tripPoints[item].Latitude.ToString(), tripPoints[item].Longitude.ToString(), (tripPoints[item].RecordedTimeStamp / 1000).ToString());
                    
                }
                catch (Exception ex)
                {
                    Program.logStore.postLog("Error", "TripEndProcessor", "updateCountry_LocalTime", ex.Message, "tripID:" + tripPoints[item].TripID + "; tripPointID:" + tripPoints[item].Id);
                    Console.WriteLine("Time: " + DateTime.UtcNow.ToString() + ", Error:TripEndProcessor:updateCountry_LocalTime:" + ex.Message + ":tripID:" + tripPoints[item].TripID + "; tripPointID:" + tripPoints[item].Id);
                }                
            }
            return true;
        }

        private void verifyMidnightDriving(int item)
        {
            if (tripPoints[item].Latitude == 0 && tripPoints[item].Longitude == 0)
                return;

            /* Verify MidNight Drive */
            DateTime localTime = (DateTime)tripPoints[item].LocalTime;
            if ((localTime.Hour >= midNightFrom) && (localTime.Hour < midNightTo))
            {
                tripPoints[item].MidNightDrive = true;
                if (!midNightOnFlag)
                {
                    midNightOnFlag = true;
                    midNightFromTime = localTime;
                }
            }
            else
            {
                tripPoints[item].MidNightDrive = false;
                if (midNightOnFlag)
                {
                    midNightOnFlag = false;
                    midNightToTime = localTime;
                }
            }
        }

        private void overSpeedCheck(int item)
        {
            /* Over Speed Verify */
            if (tripPoints[item].Speed > 110)
            {
                tripPoints[item].OverSpeed = true;
                overSpeed++;
                tripPoints[item].SpeedLimit = 110;
            }
            //else if ((tripPoints[item].speed > 60) && (tripPoints[item].speed <= 110))
            //{
            //    int rnd = rand.Next(1, 6);
            //    if ((rnd * tripPoints[item].speed) >= 270)
            //    {
            //        tripPoints[item].overSpeed = true;
            //        overSpeed++;
            //        tripPoints[item].speedLimit = (tripPoints[item].speed / 10) * 10;
            //    }
            //    else
            //    {
            //        tripPoints[item].overSpeed = false;
            //        tripPoints[item].speedLimit = (tripPoints[item].speed / 10) * 10;
            //    }
            //}
            else
            {
                tripPoints[item].OverSpeed = false;
                tripPoints[item].SpeedLimit = 110;
            }
        }

        private void calculateDistance(int item)
        {
            if (prelat == 0)
            {
                prelat = (double)tripPoints[item].Latitude; prelng = (double)tripPoints[item].Longitude;
            }
            else
            {
                var locA = new GeoCoordinate(prelat, prelng);
                var locB = new GeoCoordinate((double)tripPoints[item].Latitude, (double)tripPoints[item].Longitude);
                totalDistance = totalDistance + locA.GetDistanceTo(locB); // metres

                prelat = (double)tripPoints[item].Latitude;
                prelng = (double)tripPoints[item].Longitude;
            }
        }

        private void mapBoundry(int item)
        {
            /* Caculate Map boundry  */
            if ((double)tripPoints[item].Longitude > maxLng)
                maxLng = (double)tripPoints[item].Longitude;
            if ((double)tripPoints[item].Longitude < minLng)
                minLng = (double)tripPoints[item].Longitude;
            if ((double)tripPoints[item].Latitude > maxLat)
                maxLat = (double)tripPoints[item].Latitude;
            if ((double)tripPoints[item].Latitude < minLat)
                minLat = (double)tripPoints[item].Latitude;
        }

        private async void updateTrip()
        {
            if (midNightOnFlag)
            {
                midNightOnFlag = false;
                midNightToTime = (DateTime)tripPoints[tripPoints.Count - 1].LocalTime;

                theTrip.MidNightDriveInSec = (int)(midNightToTime - midNightFromTime).TotalSeconds;
            }
            else
                theTrip.MidNightDriveInSec = 0;

            theTrip.Distance = totalDistance / 1000;  /* convert to KM */
            theTrip.HardBreaks = hardBreak;
            theTrip.HardAccelerations = hardAcceleration;
            theTrip.MaxSpeed = maxSpeed;
            theTrip.OverSpeeds = overSpeed;

            theTrip.EndTimeStamp = (long)tripPoints[tripPoints.Count - 1].RecordedTimeStamp;    /* min sec */

            if (tripPoints[0].Latitude!=0 && tripPoints[0].Longitude!=0)
            {
                theTrip.StartCountry = await gService.GetCountryByLocation(tripPoints[0].Latitude.ToString(), tripPoints[0].Longitude.ToString());
                theTrip.EndCountry = await gService.GetCountryByLocation(tripPoints[tripPoints.Count - 1].Latitude.ToString(), tripPoints[tripPoints.Count - 1].Longitude.ToString());
            }
            
            theTrip.StartDateTime = DateTimeService.GetDateTimeByTimeStamp(theTrip.StartTimeStamp);
            theTrip.DriveTimeInSec = (int)((tripPoints[tripPoints.Count - 1].RecordedTimeStamp - tripPoints[0].RecordedTimeStamp) / 1000);
            theTrip.IdelingTimeInSec = (int)tripPoints[tripPoints.Count - 1].IdlingTime / 1000;  /* change min sec to sec */
            if (theTrip.DriveTimeInSec > 0)
                theTrip.AverageSpeed = (theTrip.Distance / theTrip.DriveTimeInSec) * 60 * 60;   /* KM/Hours  */
            theTrip.CenterLat = (decimal)(maxLat + minLat) / 2;
            theTrip.CenterLng = (decimal)(maxLng + minLng) / 2;
            theTrip.MaxLat = (decimal)maxLat;
            theTrip.Maxlng = (decimal)maxLng;
            theTrip.MinLat = (decimal)minLat;
            theTrip.MinLng = (decimal)minLng;
            theTrip.UpdatedAt = DateTime.UtcNow;

            /* AccountID */
            Devices spDongle = _dbHelp.getDeviceByIoTHubDeviceID(theTrip.IoTHubDeviceID);
            theTrip.AccountID = (int)spDongle.AccountID;
            theTrip.ProcessCompleteFlag = true;
            theTrip.UpdatedAt = DateTime.UtcNow;

            /* update Trip data */
            _dbHelp.updateTrip(theTrip);
            _dbHelp.updateTripPoints(tripPoints);
            Console.WriteLine("Time: " + DateTime.UtcNow.ToString() + ", Update Trip Data Completed, TripID: " + theTrip.Id);
        }
    }
}
