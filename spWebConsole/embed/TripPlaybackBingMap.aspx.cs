using ShareClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace spWebConsole.embed
{
    public partial class TripPlaybackBingMap : System.Web.UI.Page
    {
        /* Data */
        public int tripScore = 0, tripNegativeEvent = 0, tripAvgSpeed = 0, tripMaxSpeed = 0;
        public string tripDuration = "", tripStartDT = "", tripDistance = "0 km";

        /* Bing Map */
        public static string _BingAPIKey = ConfigurationManager.AppSettings["BingAPIKey"];
        public string _centerLocation;
        public int _zoom;
        public string _routingPath;
        public string _carStartLocation;
        public string _carIcon;
        public string _drivingEvent;
        public string _eventTitle;
        public string _eventDesc;
        public string _eventPoint;
        public string _tripPoints;

        int iconSize = 40;
        string tripID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["tid"] != null)
            {
                tripID = Request.QueryString["tid"];

                int iconOffset = iconSize / 2;

                DBHelper dbhelper = new DBHelper();
                Trips trip = dbhelper.getTripByTripID(tripID);
                _centerLocation = trip.CenterLat + "," + trip.CenterLng;

                AccountProfiles acct = dbhelper.getAccountProfileByID((int)trip.AccountID);

                GoogleService googleService = new GoogleService();
                _zoom = googleService.getGoogleMapBoundsZoomLevel((double)trip.MaxLat, (double)trip.MinLat, (double)trip.Maxlng, (double)trip.MinLng, Global.mapWidth, Global.mapHeight);

                tripScore = (int)trip.Rating;
                tripDistance = Convert.ToInt32(trip.Distance) + " km";
                tripDuration = DateTimeService.ConvertTotalSecondToString((int)trip.DriveTimeInSec);
                tripStartDT = DateTimeService.GetDateTimeByTimeStamp(trip.StartTimeStamp).AddHours((int)acct.TimeZone).ToString("yyyy-MM-dd hh:mm");
                tripNegativeEvent = (int)(trip.HardBreaks + trip.HardAccelerations + trip.OverSpeeds);
                tripAvgSpeed = Convert.ToInt32(trip.AverageSpeed);
                tripMaxSpeed = Convert.ToInt32(trip.MaxSpeed);

                List<TripPoints> tripPoints = dbhelper.getTripPointsByTripID(tripID);
                int item = 0;

                _eventPoint = "[";
                _tripPoints = "[";
                foreach (var tripPoint in tripPoints)
                {
                    _routingPath = _routingPath + "new Microsoft.Maps.Location(" + tripPoint.Latitude + "," + tripPoint.Longitude + "),";
                    _tripPoints = _tripPoints + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "," + tripPoint.RecordedTimeStamp / 100 + "," + tripPoint.Speed + "',";

                    string pointDesc = "Date:" + ((DateTime)tripPoint.LocalTime).ToShortDateString() + "</br>Time:" + ((DateTime)tripPoint.LocalTime).ToShortTimeString() + "</br>Speed:" + tripPoint.Speed + "</br>Speed Limite:" + tripPoint.SpeedLimit;
                    switch (tripPoint.MessageType)
                    {
                        case "trip_start":
                            _eventTitle = _eventTitle + "'TRIP START',";
                            _eventDesc = _eventDesc + "'" + pointDesc + "',";
                            _drivingEvent = _drivingEvent + "new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" + tripPoint.Latitude + "," + tripPoint.Longitude + "), {title: '" + item + "', icon: '/assets/images/carPlayback/tripStart_" + iconSize + ".png', anchor: new Microsoft.Maps.Point(" + iconOffset + "," + iconOffset + ")}),";
                            _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                            item++;
                            break;
                        case "hard_braking":
                            _eventTitle = _eventTitle + "'HARD BREAKING',";
                            _eventDesc = _eventDesc + "'" + pointDesc + "',";
                            _drivingEvent = _drivingEvent + "new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" + tripPoint.Latitude + "," + tripPoint.Longitude + "), {title: '" + item + "', icon: '/assets/images/carPlayback/break_" + iconSize + ".png', anchor: new Microsoft.Maps.Point(" + iconOffset + "," + iconOffset + ")}),";
                            _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                            item++;
                            break;
                        case "hard_acceleration":
                            _eventTitle = _eventTitle + "'HARD ACCELERATION',";
                            _eventDesc = _eventDesc + "'" + pointDesc + "',";
                            _drivingEvent = _drivingEvent + "new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" + tripPoint.Latitude + "," + tripPoint.Longitude + "), {title: '" + item + "',icon: '/assets/images/carPlayback/acceleration_" + iconSize + ".png', anchor: new Microsoft.Maps.Point(" + iconOffset + "," + iconOffset + ")}),";
                            _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                            item++;
                            break;
                        case "trip_end":
                            _eventTitle = _eventTitle + "'TRIP END',";
                            _eventDesc = _eventDesc + "'" + pointDesc + "',";
                            _drivingEvent = _drivingEvent + "new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" + tripPoint.Latitude + "," + tripPoint.Longitude + "), {title: '" + item + "',icon: '/assets/images/carPlayback/tripEnd_" + iconSize + ".png', anchor: new Microsoft.Maps.Point(" + iconOffset + "," + iconOffset + ")}),";
                            _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                            item++;
                            break;
                    }
                    if ((bool)tripPoint.MidNightDrive)
                    {
                        _eventTitle = _eventTitle + "'MIDNIGHT DRIVING',";
                        _eventDesc = _eventDesc + "'" + pointDesc + "',";
                        _drivingEvent = _drivingEvent + "new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" + tripPoint.Latitude + "," + tripPoint.Longitude + "), {title: '" + item + "',icon: '/assets/images/carPlayback/midnight_" + iconSize + ".png', anchor: new Microsoft.Maps.Point(" + iconOffset + "," + iconSize + ")}),";
                        _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                        item++;
                    }
                    if ((bool)tripPoint.OverSpeed)
                    {
                        _eventTitle = _eventTitle + "'OVER SPEED',";
                        _eventDesc = _eventDesc + "'" + pointDesc + "',";
                        _drivingEvent = _drivingEvent + "new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(" + tripPoint.Latitude + "," + tripPoint.Longitude + "), {title: '" + item + "',icon: '/assets/images/carPlayback/over-speed_" + iconSize + ".png', anchor: new Microsoft.Maps.Point(" + iconSize + "," + iconOffset + ")}),";
                        _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                        item++;
                    }
                }
                _routingPath = _routingPath.Substring(0, _routingPath.Length - 1);
                _drivingEvent = _drivingEvent.Substring(0, _drivingEvent.Length - 1);
                _eventTitle = _eventTitle.Substring(0, _eventTitle.Length - 1);
                _eventDesc = _eventDesc.Substring(0, _eventDesc.Length - 1);
                _tripPoints = _tripPoints.Substring(0, _tripPoints.Length - 1) + "]";
                _carStartLocation = "new Microsoft.Maps.Location(" + tripPoints[0].Latitude + "," + tripPoints[0].Longitude + ")";
                _carIcon = "{ icon: '/assets/images/carPlayback/car_" + iconSize + ".png', anchor: new Microsoft.Maps.Point(" + iconOffset + "," + iconOffset + ") }";
                _eventPoint = _eventPoint.Substring(0, _eventPoint.Length - 1) + "]";
            }

        }
    }
}