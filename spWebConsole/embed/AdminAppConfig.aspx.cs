using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace spWebConsole.embed
{
    public partial class AdminAppConfig : System.Web.UI.Page
    {
        public string _googleChecked, _bingChecked;
        protected void Page_Load(object sender, EventArgs e)
        {
            string mapProvider = Application["mapProvider"].ToString();
            if (mapProvider.ToLower() == "bing")
                _bingChecked = "checked = 'checked'";
            else
                _googleChecked = "checked = 'checked'";
        }
    }
}