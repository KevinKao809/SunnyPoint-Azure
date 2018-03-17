using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace spWebConsole.embed
{
    public partial class Dashboard : System.Web.UI.Page
    {
        public string _message;
        protected void Page_Load(object sender, EventArgs e)
        {
            _message = "ABC";
        }
    }
}