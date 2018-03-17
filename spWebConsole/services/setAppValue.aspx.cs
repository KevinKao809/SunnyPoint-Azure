using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace spWebConsole.services
{
    public partial class setAppValue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["name"]!=null && Request.QueryString["value"]!=null)
            {
                string name = Request.QueryString["name"];
                string value = Request.QueryString["value"].ToLower();
                Application[name] = value;
            }
        }
    }
}