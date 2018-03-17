using ShareClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace spWebConsole.embed
{
    public partial class DeviceManagement : System.Web.UI.Page
    {
        public int _dongleAmount = 0, _trips = 0, _tripDistance = 0, _negativeEvent = 0;
        public string _deviceTableList = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["aid"] != null)
            {
                int accountID = int.Parse(Request.QueryString["aid"]);
                DBHelper dbhelper = new DBHelper();
                List<Devices> spDeviceList = dbhelper.getDeviceListByAccountID(accountID);                
                for (int i = 0; i < spDeviceList.Count; i++)
                {
                    _deviceTableList = _deviceTableList + "<tr><td>" + spDeviceList[i].IoTHubDeviceID + "</td>";
                    _deviceTableList = _deviceTableList + "<td>" + spDeviceList[i].IoTHubHostName + "</td>";
                    _deviceTableList = _deviceTableList + "<td>" + spDeviceList[i].AccelerationAxis + "</td>";
                    _deviceTableList = _deviceTableList + "<td>" + spDeviceList[i].HardAccelerationValue + "</td>";
                    _deviceTableList = _deviceTableList + "<td>" + spDeviceList[i].HardBreakValue + "</td>";
                    _deviceTableList = _deviceTableList + "<td>" + spDeviceList[i].RegisterCountry + "</td>";
                    _deviceTableList = _deviceTableList + "<td><a href=\"#\">REMOVE</a></td></tr>";
                }

                if (string.IsNullOrEmpty(_deviceTableList))
                {
                    _deviceTableList = "<tr><td colspan='7'> No any Dongle Device</td></tr>";
                }

                _dongleAmount = spDeviceList.Count;

                AccountProfiles spAcct = dbhelper.getAccountProfileByID(accountID);       
                if (spAcct.Trips!=null && (int)spAcct.Trips>0)
                {                         
                    _trips = (int)spAcct.Trips;
                    _tripDistance = (int)spAcct.Distance;
                    _negativeEvent = (int)spAcct.HardBreaks + (int)spAcct.HardAccelerations + (int)spAcct.OverSpeeds;
                }
            }
        }
    }
}