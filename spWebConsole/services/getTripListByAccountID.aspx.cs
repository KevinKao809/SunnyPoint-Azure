using ShareClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace spWebConsole.services
{
    public partial class getTripListByAccountID : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["aid"] != null)
            {
                string _tripListContent = "[";
                int accountID = int.Parse(Request.QueryString["aid"]);                

                DBHelper dbhelper = new DBHelper();
                List<Trips> tripList = dbhelper.getNValidTripsByAccountID(accountID, Global.maxTripsForDemo);

                AccountProfiles acct = dbhelper.getAccountProfileByID(accountID);

                if (tripList != null && tripList.Count() > 0)
                {
                    foreach (var myTrip in tripList)
                    {
                        string tripName = DateTimeService.GetDateTimeByTimeStamp(myTrip.StartTimeStamp).AddHours((int)acct.TimeZone).ToString();
                        _tripListContent = _tripListContent + "{\"tripID\":\"" + myTrip.Id + "\",\"tripName\":\"" + tripName + "\"},";
                    }
                    _tripListContent = _tripListContent.Substring(0, _tripListContent.Length - 1);
                }
                _tripListContent = _tripListContent + "]";
                Response.Write(_tripListContent);
                Response.Flush();
            }
        }
    }
}