using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using Microsoft.WindowsAzure.Storage.Blob;

namespace ShareClasses
{
    public class LogFileStore
    {
        private static string logStorageConnectionString = CloudConfigurationManager.GetSetting("logStorageConnectionString");
        private static CloudStorageAccount storageAccount;
        private CloudBlobContainer _container;
        private string AppName;

        public LogFileStore(string appName)
        {
            AppName = appName;
            try
            {
                if (storageAccount == null)
                    storageAccount = CloudStorageAccount.Parse(logStorageConnectionString);                

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                _container = blobClient.GetContainerReference(AppName.ToLower());
                _container.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void postDeviceLogFle(string deviceID, long LogStartTimestamp, string logFilePath)
        {
            DateTime logDT = DateTimeService.GetDateTimeByTimeStamp(LogStartTimestamp);
            string blobFilePath = logDT.ToString("yyyy/MM/dd") + "/" + deviceID + "/" + LogStartTimestamp + ".txt";

            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobFilePath);

            using (var fileStream = System.IO.File.OpenRead(logFilePath))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }
    }

    public class LogStore
    {
        private static string logStorageConnectionString = CloudConfigurationManager.GetSetting("logStorageConnectionString");
        private static CloudStorageAccount storageAccount;
        private CloudTable _table;
        private string AppName;

        public LogStore(string appName)
        {
            AppName = appName;
            try
            {
                if (storageAccount == null)
                    storageAccount = CloudStorageAccount.Parse(logStorageConnectionString);

                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                _table = tableClient.GetTableReference(AppName);
                _table.CreateIfNotExists();                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }        

        public void postLog(string level, string className, string method, string message, string detail)
        {
            logEntity log = new logEntity(level, className, method, message, detail);
            TableOperation insertOperation = TableOperation.Insert(log);
            try
            {
                _table.Execute(insertOperation);
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }

        public IEnumerable<logEntity> getLog()
        {
            return getLog("info", DateTime.Today.ToUniversalTime(), DateTime.UtcNow);
        }

        public IEnumerable<logEntity> getLog(string level)
        {
            if (string.IsNullOrEmpty(level))
                level = "info";
            return getLog(level, DateTime.Today.ToUniversalTime(), DateTime.UtcNow);
        }

        public IEnumerable<logEntity> getLog(string level, DateTime fromDateTime)
        {
            return getLog(level, fromDateTime.ToUniversalTime(), DateTime.UtcNow);
        }

        public IEnumerable<logEntity> getLog(string level, DateTime fromDateTime, DateTime toDateTime)
        {
            string partitionKey = "";
            switch (level.ToLower())
            {
                case "info":
                    partitionKey = "1";
                    break;
                case "debug":
                    partitionKey = "2";
                    break;
                case "warning":
                    partitionKey = "3";
                    break;
                case "error":
                    partitionKey = "4";
                    break;
                default:
                    partitionKey = "1";
                    break;
            }

            fromDateTime = fromDateTime.ToUniversalTime();
            toDateTime = toDateTime.ToUniversalTime();

            string partitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThanOrEqual, partitionKey);
            string date1 = TableQuery.GenerateFilterConditionForDate(
                   "Timestamp", QueryComparisons.GreaterThanOrEqual,
                   fromDateTime);
            string date2 = TableQuery.GenerateFilterConditionForDate(
                               "Timestamp", QueryComparisons.LessThanOrEqual,
                               toDateTime);
            string finalFilter = TableQuery.CombineFilters(
                                    TableQuery.CombineFilters(
                                        partitionFilter,
                                        TableOperators.And,
                                        date1),
                                    TableOperators.And, date2);
            TableQuery<logEntity> rangeQuery = new TableQuery<logEntity>().Where(finalFilter);

            return _table.ExecuteQuery(rangeQuery);
        }

    }    

    public class logEntity : TableEntity
    {
        public logEntity()
        { }       

        public logEntity(string level, string className, string method, string message, string detail)
        {
            string partitionKey = "";
            switch (level.ToLower())
            {
                case "info":
                    partitionKey = "1";
                    break;
                case "debug":
                    partitionKey = "2";
                    break;
                case "warning":
                    partitionKey = "3";
                    break;
                case "error":
                    partitionKey = "4";
                    break;
                default:
                    partitionKey = "1";
                    break;
            }

            this.PartitionKey = partitionKey;
            DateTime baseTime = new DateTime(1970, 1, 1);//宣告一個GTM時間出來
            long timeStamp = Convert.ToInt64(((TimeSpan)DateTime.UtcNow.Subtract(baseTime)).TotalMilliseconds);
            this.RowKey = timeStamp.ToString();
            this.Level = level;
            this.ClassName = className;
            this.Method = method;
            this.Message = message;
            this.Detail = detail;
        }
        public string LogTimestamp
        {
            get
            {
                return this.RowKey;
            }
        }
        //public string RowKey { get; set; }
        public string Level { get; set; }
        public string ClassName { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public string Time { get; set; }
    }
}
