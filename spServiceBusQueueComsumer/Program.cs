using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using ShareClasses;


namespace spServiceBusQueueComsumer
{
    class Program
    {
        static int _maxThreads_ = int.Parse(ConfigurationManager.AppSettings["Max.Operation.Thread"]);
        public static Thread[] threadOperation = new Thread[_maxThreads_];        
        static QueueClient _OperationQueueClient = QueueClient.CreateFromConnectionString(ConfigurationManager.AppSettings["SunnyPoint.ServiceBus.ConnectionString"], ConfigurationManager.AppSettings["OperationQueueName"]);
        public static LogStore logStore = new LogStore("spServiceBusQueueComsumer");
        static int _sleepInSec = 60;

        static void Main(string[] args)
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            bool receiveMessageFlag = true;
            BrokeredMessage message = null;
            bool continueRun = true;
            Console.WriteLine("Waiting new message. (press 'h' to stop new message receive, or ESC to close). Threads:(" + GetComsumedThreadCount() + "/" + _maxThreads_ + ")");
            logStore.postLog("info", "Program", "Main", "Waiting new message", "");
            while (continueRun)
            {
                try
                {
                    if (Console.KeyAvailable)
                    {
                        cki = Console.ReadKey(true);
                        if (cki.Key.Equals(ConsoleKey.H))   //Get Key Input 'H'
                            receiveMessageFlag = false;     //Hold on message receive
                        else if (cki.Key.Equals(ConsoleKey.Escape))
                            if (GetComsumedThreadCount() > 0)
                                Console.WriteLine("There are running operation threads. You shall hold on message receive, and wait for all running threads completed.");
                            else
                                return;
                        else
                            Console.WriteLine("Waiting new message. (press 'h' to stop new message receive, or ESC to close). Threads:(" + GetComsumedThreadCount() + "/" + _maxThreads_ + ")");
                    }
                    if (receiveMessageFlag)
                    {
                        int freeThreadId = GetFreeThreadId();
                        if (freeThreadId > -1)
                        {
                            message = _OperationQueueClient.Receive(TimeSpan.FromSeconds(5));
                            string receivedMessageContent = null;
                            if (message != null)                            
                            {
                                try
                                {
                                    receivedMessageContent = message.GetBody<string>();
                                    //logStore.postLog("info", "Program", "Main", "Recieve Message:"+receivedMessageContent, "");
                                    Operation ops = new Operation(receivedMessageContent);
                                    message.Complete();
                                    //logStore.postLog("info", "Program", "Main", "Message Complete", "");
                                    
                                    Console.WriteLine("Run operation thread at ID:" + freeThreadId + "; command:" + ops._commond);
                                    threadOperation[freeThreadId] = new Thread(new ThreadStart(ops.ThreadProc));
                                    threadOperation[freeThreadId].Start();                                    
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Contains("duplicate message"))
                                    {
                                        logStore.postLog("Warning", "Program", "Main", "Duplicate message", "Message:" + receivedMessageContent);
                                        Console.WriteLine(ex.Message + ";Message:" + receivedMessageContent);
                                        message.Complete();
                                    }
                                    else if (ex.Message.Contains("trip_start not found"))
                                    {
                                        logStore.postLog("Warning", "Program", "Main", "trip_start not found", "Message:" + receivedMessageContent);
                                        Console.WriteLine(ex.Message + ";Message:" + receivedMessageContent);
                                        message.Complete();
                                    }
                                    else if (ex.Message.Contains("trip_end not found"))
                                    {
                                        logStore.postLog("Warning", "Program", "Main", "trip_end not found", "Message:" + receivedMessageContent);
                                        Console.WriteLine(ex.Message + ";Message:" + receivedMessageContent);
                                        //message.Abandon();
                                    }
                                    else if (ex.Message.Contains("command undefined"))
                                    {
                                        logStore.postLog("Warning", "Program", "Main", "command undefined", "Message:" + receivedMessageContent);
                                        Console.WriteLine(ex.Message + ";Message:" + receivedMessageContent);
                                        message.Abandon();
                                    }
                                    else
                                    {
                                        logStore.postLog("Error", "Program", "Main", ex.Message, "Message:" + receivedMessageContent);
                                        Console.WriteLine(ex.Message + ";Message:" + receivedMessageContent);
                                        message.Complete();
                                    }                                    
                                }                                
                            }
                            else
                            {
                                Console.WriteLine("No Message, Sleep {0} secconds", _sleepInSec);
                                Thread.Sleep(_sleepInSec * 1000);
                            }
                        }                        
                    }
                    else
                    {
                        Console.WriteLine("Hold. (press 'r' to enable new message receive). Threads:(" + GetComsumedThreadCount() + "/" + _maxThreads_ + ")");
                        while (Console.ReadKey().Key != ConsoleKey.R)
                        {
                            Console.WriteLine("\r");
                            Console.WriteLine("Hold. (press 'r' to enable new message receive). Threads:(" + GetComsumedThreadCount() + "/" + _maxThreads_ + ")");
                        }
                        Console.WriteLine("\r");
                        receiveMessageFlag = true;

                        Console.WriteLine("Waiting new message. (press 'h' to stop new message receive, or ESC to close). Threads:(" + GetComsumedThreadCount() + "/" + _maxThreads_ + ")");
                    }
                }
                catch (MessagingException ex)
                {
                    if (!ex.IsTransient)
                    {
                        logStore.postLog("Error", "Program", "Main", ex.Message, "");
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                    else
                    {
                        //If transient error/exception, let's back-off for N seconds and retry 
                        Console.WriteLine("Will retry after in {0} seconds", _sleepInSec);
                        Thread.Sleep(_sleepInSec*1000);
                    }
                }
            }
        }

        static int GetFreeThreadId()
        {
            for (int i = 0; i < threadOperation.Length; i++)
            {
                if (threadOperation[i] == null || threadOperation[i].ThreadState == ThreadState.Stopped)
                {
                    return i;
                }
            }
            return -1;
        }

        static int GetComsumedThreadCount()
        {
            int count = 0;
            for (int i = 0; i < threadOperation.Length; i++)
            {
                if (threadOperation[i] == null || threadOperation[i].ThreadState == ThreadState.Stopped)
                {
                    count++;
                }
            }
            return (_maxThreads_ - count);
        }
    }    
}
