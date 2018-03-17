using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShareClasses
{
    public class GoogleService
    {
        static string GoogleTZAPIKey = "AIzaSyCM_UQPqgqmYjtTMCkaNf-G1Qtm1lkJ22M";
        static string GoogleMapAPI = "https://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&key={2}";
        static string GoogleTZAPI = "https://maps.googleapis.com/maps/api/timezone/xml?location={0},{1}&timestamp={2}&key={3}";

        DateTime previousCountryCall = new DateTime(1970, 1, 1);
        DateTime previousLocalTimeCall = new DateTime(1970, 1, 1);
        int CacheDurationInSec = 5;
        string CountryValue = "";
        string raw_offset = "";
        string dst_offset = "";

        public async Task<string> GetCountryByLocation(string lat, string lng)
        {
            if (DateTime.UtcNow > previousCountryCall.AddSeconds(CacheDurationInSec) || string.IsNullOrEmpty(CountryValue))
            {
                string requestUri = string.Format(GoogleMapAPI, lat, lng, GoogleTZAPIKey);
                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        string xmlResult = await httpClient.GetStringAsync(requestUri);
                        var xmlElm = XElement.Parse(xmlResult);
                        var status = (from elm in xmlElm.Descendants()
                                      where elm.Name == "status"
                                      select elm).FirstOrDefault();
                        if (status.Value.ToLower() == "ok")
                        {
                            var res = (from elm in xmlElm.Descendants()
                                       where elm.Name == "type" && elm.Value == "country"
                                       select elm.Parent.Element("short_name")).FirstOrDefault();
                            CountryValue = res.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + ";URL:" + requestUri);
                    }
                }
                previousCountryCall = DateTime.UtcNow;
            }
            return CountryValue;
        }

        public async Task<DateTime> GetLocalTimeByLocation(string lat, string lng, string timestamp)
        {
            if (DateTime.UtcNow > previousLocalTimeCall.AddSeconds(CacheDurationInSec) || string.IsNullOrEmpty(raw_offset))
            {
                string requestUri = string.Format(GoogleTZAPI, lat, lng, timestamp, GoogleTZAPIKey);
                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        string xmlResult = await httpClient.GetStringAsync(requestUri);
                        var xmlElm = XElement.Parse(xmlResult);
                        var status = (from elm in xmlElm.Descendants()
                                      where elm.Name == "status"
                                      select elm).FirstOrDefault();
                        if (status.Value.ToLower() == "ok")
                        {
                            raw_offset = (from elm in xmlElm.Descendants()
                                          where elm.Name == "raw_offset"
                                          select elm).FirstOrDefault().Value;
                            dst_offset = (from elm in xmlElm.Descendants()
                                          where elm.Name == "dst_offset"
                                          select elm).FirstOrDefault().Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + ";URL:" + requestUri);
                    }
                }
                previousLocalTimeCall = DateTime.UtcNow;
            }

            double localTimestamp = double.Parse(timestamp) + double.Parse(raw_offset) + double.Parse(dst_offset);

            DateTime localTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(localTimestamp);
            return localTime;
        }

        public int getGoogleMapBoundsZoomLevel(double maxLat, double minLat, double maxLng, double minLng,
                   int width, int height)
        {
            int GLOBE_WIDTH = 256; // a constant in Google's map projection  
            int ZOOM_MAX = 21;
            double latFraction = (latRad(maxLat) - latRad(minLat)) / Math.PI;
            double lngDiff = maxLng - minLng;
            double lngFraction = ((lngDiff < 0) ? (lngDiff + 360) : lngDiff) / 360;
            double latZoom = zoom(height, GLOBE_WIDTH, latFraction);
            double lngZoom = zoom(width, GLOBE_WIDTH, lngFraction);
            double bestZoom = Math.Min(Math.Min(latZoom, lngZoom), ZOOM_MAX);
            return (int)(bestZoom);
        }

        private double latRad(double lat)
        {
            double sin = Math.Sin(lat * Math.PI / 180);
            double radX2 = Math.Log((1 + sin) / (1 - sin)) / 2;
            return Math.Max(Math.Min(radX2, Math.PI), -Math.PI) / 2;
        }

        private double zoom(double mapPx, double worldPx, double fraction)
        {
            double LN2 = .693147180559945309417;
            return (Math.Log(mapPx / worldPx / fraction) / LN2);
        }
    }
}
