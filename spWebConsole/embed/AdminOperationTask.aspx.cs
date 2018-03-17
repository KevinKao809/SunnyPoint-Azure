using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using ShareClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace spWebConsole.embed
{
    public partial class AdminOperationTask : System.Web.UI.Page
    {
        public static spServiceBus SPServiceBus = new spServiceBus(Global.sbconnectionString, Global.sbOperationQueueName);
        public string _submitTasksMessage = "";
        public string _logAppName = "", _runButtonVisibility = "collapse", _taskName = "", _taskCommand = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["command"] != null)
            {
                _taskCommand = Request.QueryString["command"];
                _taskCommand = _taskCommand.ToLower();

                switch (_taskCommand)
                {
                    case "incompletetrip":
                        _taskName = "Incomplete Trip";
                        break;
                    case "cleanfailtrip":
                        _taskName = "Clean Fail Trip";
                        break;
                    case "rescoring":
                        _taskName = "ReScoring";
                        break;
                    case "accountstatistic":
                        _taskName = "Account Statistic";
                        break;
                    case "updatedashboard":
                        _taskName = "Update Dashboard";
                        break;
                }

                _logAppName = "spServiceBusQueueComsumer";

                string run = Request.QueryString["run"];
                if (!string.IsNullOrEmpty(run) && run.ToLower().Equals("true"))
                {
                    try
                    {                        
                        spServiceBusMessage spMessage = new spServiceBusMessage(_taskCommand, "");
                        var message = new BrokeredMessage(JsonConvert.SerializeObject(spMessage));
                        SPServiceBus.SendMessage(spMessage);
                        _submitTasksMessage = "Task: " + _taskCommand + " Submit. Task logs will auto refresh in every 15 seconds.";
                    }
                    catch (Exception ex)
                    {
                        _submitTasksMessage = ex.Message;
                    }
                }
                else
                {
                    _runButtonVisibility = "visible";
                }
            }
        }
    }
}