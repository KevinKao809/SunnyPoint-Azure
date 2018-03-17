using ShareClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace spWebConsole
{
    public partial class index : System.Web.UI.Page
    {
        public string _enterpriseList, _consumerList, _tripPlaybackPage;
        protected void Page_Load(object sender, EventArgs e)
        {
            string mapProvider = Application["mapProvider"].ToString();
            if (mapProvider == "bing")
                _tripPlaybackPage = "TripPlaybackBingMap.aspx";
            else
                _tripPlaybackPage = "TripPlaybackGoogleMap.aspx";

            DBHelper dbhelp = new DBHelper();
            List<AccountProfiles> spEntAcctList = dbhelp.getEnterpriseAccountProfiles();
            List<AccountProfiles> spConAcctList = dbhelp.getConsumerAccountProfiles();

            foreach (var account in spEntAcctList)
            {
                string accountName = account.FirstName + " " + account.LastName;
                _enterpriseList = _enterpriseList + "<li><a href=\"javascript:accountSelection(" + account.Id + ",&quot;" + accountName + "&quot;,&quot;" + account.ProfilePictureUri + "&quot;)\">" + accountName + "</a></li>";
            }

            foreach (var account in spConAcctList)
            {
                string accountName = account.FirstName + " " + account.LastName;
                _consumerList = _consumerList + "<li><a href=\"javascript:accountSelection(" + account.Id + ",&quot;" + accountName + "&quot;,&quot;" + account.ProfilePictureUri + "&quot;)\">" + accountName + "</a></li>";
            }
        }
    }
}