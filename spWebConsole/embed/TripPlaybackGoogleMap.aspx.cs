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
    public partial class TripPlaybackGoogleMap : System.Web.UI.Page
    {
        /* Data */
        public int tripScore = 0, tripNegativeEvent = 0, tripAvgSpeed = 0, tripMaxSpeed = 0;
        public string tripDuration = "", tripStartDT = "", tripDistance = "0 km";

        /* Bing Map */
        public static string _GoogleAPIKey = ConfigurationManager.AppSettings["GoogleAPIKey"];
        public int _zoom;
        public string _routingPath;
        public string _mapCenter;
        public string _carStartLocation;
        public string _carIcon;
        public string _eventTitle;
        public string _eventDesc;
        public string _eventImage;
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
                Trips _trip = dbhelper.getTripByTripID(tripID);
                GoogleService googleService = new GoogleService();
                _zoom = googleService.getGoogleMapBoundsZoomLevel((double)_trip.MaxLat, (double)_trip.MinLat, (double)_trip.Maxlng, (double)_trip.MinLng, Global.mapWidth, Global.mapHeight);

                AccountProfiles acct = dbhelper.getAccountProfileByID((int)_trip.AccountID);

                tripScore = (int)_trip.Rating;
                tripDistance = Convert.ToInt32(_trip.Distance) + " km";
                tripDuration = DateTimeService.ConvertTotalSecondToString((int)_trip.DriveTimeInSec);
                tripStartDT = DateTimeService.GetDateTimeByTimeStamp(_trip.StartTimeStamp).AddHours((int)acct.TimeZone).ToString("yyyy-MM-dd hh:mm");
                tripNegativeEvent = (int)(_trip.HardBreaks + _trip.HardAccelerations + _trip.OverSpeeds);
                tripAvgSpeed = Convert.ToInt32(_trip.AverageSpeed);
                tripMaxSpeed = Convert.ToInt32(_trip.MaxSpeed);

                List<TripPoints> tripPoints = dbhelper.getTripPointsByTripID(tripID);
                int item = 0;

                foreach (var tripPoint in tripPoints)
                {
                    if (tripPoint.LocalTime == null)
                        continue;

                    _routingPath = _routingPath + "{lat:" + tripPoint.Latitude + ",lng:" + tripPoint.Longitude + "},";
                    _tripPoints = _tripPoints + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "," + tripPoint.RecordedTimeStamp / 100 + "," + tripPoint.Speed + "',";

                    string pointDesc = "Date:" + ((DateTime)tripPoint.LocalTime).ToShortDateString() + "</br>Time:" + ((DateTime)tripPoint.LocalTime).ToShortTimeString() + "</br>Speed:" + tripPoint.Speed + "</br>Speed Limite:" + tripPoint.SpeedLimit;
                    switch (tripPoint.MessageType)
                    {
                        case "trip_start":
                            _eventTitle = _eventTitle + "'TRIP START',";
                            _eventDesc = _eventDesc + "'" + pointDesc + "',";
                            _eventImage = _eventImage + "'/assets/images/carPlayback/tripStart_" + iconSize + ".png',";
                            _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                            item++;
                            break;
                        case "hard_braking":
                            _eventTitle = _eventTitle + "'HARD BREAKING',";
                            _eventDesc = _eventDesc + "'" + pointDesc + "',";
                            _eventImage = _eventImage + "'/assets/images/carPlayback/break_" + iconSize + ".png',";
                            _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                            item++;
                            break;
                        case "hard_acceleration":
                            _eventTitle = _eventTitle + "'HARD ACCELERATION',";
                            _eventDesc = _eventDesc + "'" + pointDesc + "',";
                            _eventImage = _eventImage + "'/assets/images/carPlayback/acceleration_" + iconSize + ".png',";
                            _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                            item++;
                            break;
                        case "trip_end":
                            _eventTitle = _eventTitle + "'TRIP END',";
                            _eventDesc = _eventDesc + "'" + pointDesc + "',";
                            _eventImage = _eventImage + "'/assets/images/carPlayback/tripEnd_" + iconSize + ".png',";
                            _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                            item++;
                            break;
                    }
                    if ((bool)tripPoint.MidNightDrive)
                    {
                        _eventTitle = _eventTitle + "'MIDNIGHT DRIVING',";
                        _eventDesc = _eventDesc + "'" + pointDesc + "',";
                        _eventImage = _eventImage + "'/assets/images/carPlayback/midnight_" + iconSize + ".png',";
                        _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                        item++;
                    }
                    if ((bool)tripPoint.OverSpeed)
                    {
                        _eventTitle = _eventTitle + "'OVER SPEED',";
                        _eventDesc = _eventDesc + "'" + pointDesc + "',";
                        _eventImage = _eventImage + "'/assets/images/carPlayback/over-speed_" + iconSize + ".png',";
                        _eventPoint = _eventPoint + "'" + tripPoint.Latitude + "," + tripPoint.Longitude + "',";
                        item++;
                    }
                }
                if (_routingPath.Length > 0)
                    _routingPath = _routingPath.Substring(0, _routingPath.Length - 1);
                if (_eventPoint.Length > 0)
                    _eventPoint = _eventPoint.Substring(0, _eventPoint.Length - 1);
                if (_tripPoints.Length > 0)
                    _tripPoints = _tripPoints.Substring(0, _tripPoints.Length - 1);
                if (_eventTitle.Length > 0)
                    _eventTitle = _eventTitle.Substring(0, _eventTitle.Length - 1);
                if (_eventDesc.Length > 0)
                    _eventDesc = _eventDesc.Substring(0, _eventDesc.Length - 1);

                _carStartLocation = "{lat:" + tripPoints[0].Latitude + ",lng:" + tripPoints[0].Longitude + "}";
                _carIcon = "/assets/images/carPlayback/car_" + iconSize + ".png";
                _mapCenter = "{lat:" + _trip.CenterLat + ",lng:" + _trip.CenterLng + "}";
            }

        }        
    }
}