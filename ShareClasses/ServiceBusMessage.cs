using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClasses
{
    public class spServiceBus
    {
        private QueueClient sbQueueClient;

        public spServiceBus(string sbconnectionString, string sbQueueName)
        {
            try
            {
                sbQueueClient = QueueClient.CreateFromConnectionString(sbconnectionString, sbQueueName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendMessage(spServiceBusMessage spMessage)
        {
            try
            {
                var message = new BrokeredMessage(JsonConvert.SerializeObject(spMessage));
                sbQueueClient.Send(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class spServiceBusMessage
    {
        public string command;
        public string tripid;

        public spServiceBusMessage(string command, string tripid)
        {
            this.command = command;
            this.tripid = tripid;
        }
    }
}
