using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace spWebConsole
{
    public class Global : System.Web.HttpApplication
    {
        public static string sbconnectionString = ConfigurationManager.AppSettings["SunnyPoint.ServiceBus.ConnectionString"];
        public static string sbOperationQueueName = ConfigurationManager.AppSettings["OperationQueueName"];
        public static int maxTripsForDemo = int.Parse(ConfigurationManager.AppSettings["MaxTripsForDemo"]);
        public static int mapWidth = int.Parse(ConfigurationManager.AppSettings["MapWidth"]);
        public static int mapHeight = int.Parse(ConfigurationManager.AppSettings["MapHeight"]);
        private static string mapProvider = ConfigurationManager.AppSettings["MapProvider"];

        protected void Application_Start(object sender, EventArgs e)
        {
            Application["mapProvider"] = mapProvider;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}