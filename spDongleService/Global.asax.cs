using ShareClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace spDongleService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static LogStore logStore;
        public static LogFileStore dongleAppLogStore;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // spDongleService initializied 
            logStore = new LogStore("spDongleService");
            dongleAppLogStore = new LogFileStore("spDongleApp");
        }
    }
}
