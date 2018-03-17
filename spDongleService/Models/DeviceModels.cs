using System;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System.Configuration;
using System.Threading.Tasks;
using ShareClasses;

namespace spDongleService.Models
{
    public class DeviceModels
    {
        public RegistryManager registryManager;

        public static string HeartBeatInterval = ConfigurationManager.AppSettings["HeartBeatInterval"];
        public static string SendMessageInterval = ConfigurationManager.AppSettings["SendMessageInterval"];

        public string IoTHubConnectionString;
        public string IoTHubHostName;
        public string IoTHubProtocol;
        public string IoTHubDeviceID;
        public string IoTHubDeviceKey;
        public int AccountID;
        public string AccelerationAxis = "X";
        public double HardBreakValue = -2;
        public double HardAccelerationValue = 2;
        public string RegisterCountry;
        public string lat;
        public string lng;
        public DateTime CreatedAt;
        public DateTime UpdatedAt;

        public DeviceModels()
        {
        }

        public async Task<bool> AddDeviceAsync(string deviceId, string accountID, string lat, string lng)
        {
            this.IoTHubDeviceID = deviceId;
            this.AccountID = int.Parse(accountID);
            this.lat = lat;
            this.lng = lng;

            DBHelper dbhelp = new DBHelper();
            Devices device = dbhelp.getDeviceByIoTHubDeviceID(IoTHubDeviceID);
            if (device == null)
                throw new Exception("DeviceNotFound");

            IoTHubs iotHub = dbhelp.getIoTHubByIoTHubHostName(device.IoTHubHostName);
            if (iotHub == null)
                throw new Exception("DeviceNotFound");

            IoTHubConnectionString = iotHub.PrimaryIoTHubConnectionString;
            IoTHubHostName = device.IoTHubHostName;
            IoTHubProtocol = device.IoTHubProtocol;
            AccelerationAxis = device.AccelerationAxis;
            HardBreakValue = (double)device.HardBreakValue;
            HardAccelerationValue = (double)device.HardAccelerationValue;

            registryManager = RegistryManager.CreateFromConnectionString(IoTHubConnectionString);

            Device deviceOnIoT;
            try
            {
                deviceOnIoT = await registryManager.AddDeviceAsync(new Device(this.IoTHubDeviceID));
                this.IoTHubDeviceKey = deviceOnIoT.Authentication.SymmetricKey.PrimaryKey;

                GoogleService gService = new GoogleService();
                device.RegisterCountry = await gService.GetCountryByLocation(this.lat, this.lng);
                device.AccountID = AccountID;
                device.isActive = true;
                device.UpdatedAt = DateTime.UtcNow;

                dbhelp.updateDeiveByIoTHubDeviceID(device);
            }
            catch (DeviceAlreadyExistsException)
            {
                deviceOnIoT = await registryManager.GetDeviceAsync(this.IoTHubDeviceID);
                this.IoTHubDeviceKey = deviceOnIoT.Authentication.SymmetricKey.PrimaryKey;
            }
            return true;
        }

        public async Task<bool> RemoveDeviceAsync(string deviceId)
        {
            try
            {
                DBHelper dbhelp = new DBHelper();
                Devices device = dbhelp.getDeviceByIoTHubDeviceID(deviceId);
                if (device == null)
                    throw new Exception("DeviceNotFound");

                IoTHubs iotHub = dbhelp.getIoTHubByIoTHubHostName(device.IoTHubHostName);
                if (iotHub == null)
                    throw new Exception("DeviceNotFound");

                registryManager = RegistryManager.CreateFromConnectionString(iotHub.PrimaryIoTHubConnectionString);

                await registryManager.RemoveDeviceAsync(deviceId);

                device.AccountID = 0;
                device.isActive = false;
                device.RegisterCountry = "";
                device.UpdatedAt = DateTime.UtcNow;
                dbhelp.updateDeiveByIoTHubDeviceID(device);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> GetDeviceAsync(string deviceId)
        {
            IoTHubDeviceID = deviceId;

            DBHelper dbhelp = new DBHelper();
            Devices device = dbhelp.getDeviceByIoTHubDeviceID(deviceId);
            if (device == null)
                throw new Exception("DeviceNotFound");

            IoTHubs iotHub = dbhelp.getIoTHubByIoTHubHostName(device.IoTHubHostName);
            if (iotHub == null)
                throw new Exception("DeviceNotFound");

            IoTHubConnectionString = iotHub.PrimaryIoTHubConnectionString;
            IoTHubHostName = device.IoTHubHostName;
            IoTHubProtocol = device.IoTHubProtocol;
            AccountID = (int)device.AccountID;
            AccelerationAxis = device.AccelerationAxis;
            HardBreakValue = (double)device.HardBreakValue;
            HardAccelerationValue = (double)device.HardAccelerationValue;
            RegisterCountry = device.RegisterCountry;

            registryManager = RegistryManager.CreateFromConnectionString(IoTHubConnectionString);

            Device deviceOnIoT;
            try
            {
                deviceOnIoT = await registryManager.GetDeviceAsync(deviceId);

                if (deviceOnIoT == null)
                    return false;
                this.IoTHubDeviceKey = deviceOnIoT.Authentication.SymmetricKey.PrimaryKey;
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
    }
}
