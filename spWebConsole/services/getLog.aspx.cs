using Newtonsoft.Json;
using ShareClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace spWebConsole.services
{
    public partial class getLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["appName"] != null)
            {
                string logLevel = null;
                if (Request.QueryString["logLevel"] != null)
                {
                    logLevel = Request.QueryString["logLevel"];
                }

                DateTime logFromDT = DateTime.Today.ToUniversalTime();
                if (Request.QueryString["logDateTimeRange"] != null)
                {
                    string range = Request.QueryString["logDateTimeRange"];
                    switch (range)
                    {
                        case "30min":
                            logFromDT = DateTime.UtcNow.AddMinutes(-30);
                            break;
                        case "1hour":
                            logFromDT = DateTime.UtcNow.AddHours(-1);
                            break;
                        case "today":
                            logFromDT = DateTime.Today.ToUniversalTime();
                            break;
                        case "week":
                            logFromDT = DateTime.Today.AddDays(-7).ToUniversalTime();
                            break;
                    }
                }

                string appName = Request.QueryString["appName"];
                LogStore logStore = new LogStore(appName);

                IEnumerable<logEntity> logs = logStore.getLog(logLevel, logFromDT);
                List <logEntity> outputLogs = new List<logEntity>();
                logs = logs.OrderByDescending(x => x.LogTimestamp);

                foreach (logEntity entity in logs)
                {
                    double timeStamp = Convert.ToDouble(entity.LogTimestamp) / 1000;
                    DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(timeStamp);
                    entity.Time = dt.ToString();
                    outputLogs.Add(entity);
                }

                Response.Write(JsonConvert.SerializeObject(outputLogs));
                Response.End();
            }
        }
    }
}