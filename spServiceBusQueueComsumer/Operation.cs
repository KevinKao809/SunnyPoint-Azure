using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShareClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spServiceBusQueueComsumer
{
    public class Operation
    {
        public string _commond;
        Trips trip;
        public static LogStore logStore = new LogStore("spServiceBusQueueComsumer");
        private static QueueClient _client = QueueClient.CreateFromConnectionString(ConfigurationManager.AppSettings["SunnyPoint.ServiceBus.ConnectionString"], ConfigurationManager.AppSettings["OperationQueueName"]);

        public Operation(string messageBody)
        {
            try
            {
                DBHelper dbhelp = new DBHelper();
                TripsModel tripModel = new TripsModel();
                JObject json = JObject.Parse(messageBody);
                if (json["command"] != null)
                {
                    _commond = json["command"].ToString().ToLower();

                    switch (_commond)
                    {
                        case "trip_end":
                            if (json["tripid"] != null)
                            {
                                string tripID = json["tripid"].ToString();
                                                                
                                logStore.postLog("info", "Operation", "Recieve command", "trip_end", "tripID:" + tripID);
                                if (tripModel.isTripMessageComplete(tripID))
                                {
                                    trip = dbhelp.getTripByTripID(tripID);
                                    if (!(bool)trip.isComplete)
                                    {
                                        trip.isComplete = true;
                                        trip.UpdatedAt = DateTime.UtcNow;
                                        dbhelp.updateTrip(trip);                                        
                                        return;
                                    }
                                    else
                                    {
                                        logStore.postLog("Warning", "Operation", "Recieve command", "trip_end error", "duplicate message");
                                        throw new Exception("duplicate message");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("tripID not found at Service Bus message");
                            }
                            break;
                        case "incompletetrip":
                        case "cleanfailtrip":
                        case "accountstatistic":
                            logStore.postLog("info", "Operation", "Recieve command", _commond, "");                           
                            break;
                        case "monthstatistic":
                            break;
                        default:
                            throw new Exception("command undefined");
                    }
                }
            }

            catch (Exception ex)
            {
                logStore.postLog("Error", "Operation", "Recieve command", "Recieve command error", ex.Message);
                throw;
            }
        }

        public void ThreadProc()
        {
            try
            {
                switch (_commond)
                {
                    case "trip_end":
                        TripEndProcessor tripEndPro = new TripEndProcessor(trip);
                        logStore.postLog("info", "Operation", "TreadProc", "trip_end ThreadProc", "tripEndPro.Calculation");
                        tripEndPro.Calculation();
                        break;
                    case "incompletetrip":
                        TripOverdueProcessor tripOverDuePro = new TripOverdueProcessor();
                        logStore.postLog("info", "Operation", "TreadProc", "trip overdue ThreadProc", "tripOverDuePro.ManageIncompletedTrip");
                        tripOverDuePro.ManageIncompletedTrip();
                        break;
                    case "cleanfailtrip":
                        CleanFailTrip cleanFailTrip = new CleanFailTrip();
                        logStore.postLog("info", "Operation", "TreadProc", "clean fial trip ThreadProc", "daily.ManageIncompletedTrip");
                        cleanFailTrip.Clean();
                        break;
                    case "accountstatistic":
                        AccountStatistic accountStat = new AccountStatistic();
                        accountStat.RunStatistic();
                        break;
                    case "monthstatistic":
                        MonthlyReport monthlyReport = new MonthlyReport();
                        DailyReport dailyReport = new DailyReport();
                        logStore.postLog("info", "Operation", "TreadProc", "monthstatistic ThreadProc", "monthlyReport.Caculation");
                        monthlyReport.Caculation();
                        logStore.postLog("info", "Operation", "TreadProc", "monthstatistic ThreadProc", "dailyReport.Caculation");
                        dailyReport.Caculation();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message + ", Trip ID:" + trip.Id);
                logStore.postLog("Error", "Operation", "TreadProc", ex.Message, "Trip ID:" + trip.Id);
            }
        }

        public static void sendMsgToServiceBus(spServiceBusMessage spMessage)
        {
            BrokeredMessage message;
            try
            {
                message = new BrokeredMessage(JsonConvert.SerializeObject(spMessage));
                _client.Send(message);
            }
            catch (Exception ex)
            {
                Program.logStore.postLog("Error", "Operations", "sendMsgToServiceBus", ex.Message, spMessage.ToString());
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
