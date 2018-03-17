using spDongleService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.IO;

namespace spDongleService.Controllers
{
    public class DeviceController : Controller
    {
        /* Call POST: device/logFile */
        [HttpPost]
        public async Task<ActionResult> LogFile(string id)
        {
            try
            {
                string deviceId = HttpContext.Request.Form["id"];
                long logStartTS = long.Parse(HttpContext.Request.Form["startTS"]);
                string logFilePath = Path.GetTempFileName();
                HttpContext.Request.Files[0].SaveAs(logFilePath);

                /* Verify is valid Device ID */
                DeviceModels spDongle = new DeviceModels();
                if (await spDongle.GetDeviceAsync(deviceId))
                {
                    WebApiApplication.dongleAppLogStore.postDeviceLogFle(deviceId, logStartTS, logFilePath);
                    System.IO.File.Delete(logFilePath);
                    return Json(new { message = "OK" }, JsonRequestBehavior.AllowGet);
                }
                else
                    throw new Exception("DeviceNotFound");
                
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("DeviceNotFound"))
                {
                    WebApiApplication.logStore.postLog("Warning", "DeviceController", "LogFile", "Device Not Found", HttpContext.Request.Form.ToString());
                    return returnNotFound();
                }
                WebApiApplication.logStore.postLog("Error", "DeviceController", "LogFile", "Internal Server Error", HttpContext.Request.Form.ToString() + "; Error: " + ex.Message);
            }
            return returnInternalServerError();
        }

        /* Call POST: device/addDevice/100 */
        [HttpPost]
        public async Task<ActionResult> AddDevice(string id)
        {
            try
            {
                string deviceId = HttpContext.Request.Form["id"];
                string lat = HttpContext.Request.Form["lat"];
                string lng = HttpContext.Request.Form["lng"];
                string accountID = HttpContext.Request.Form["accountID"];

                if (string.IsNullOrEmpty(accountID))
                    accountID = "0";        //No Body

                DeviceModels spDongle = new DeviceModels();                
                await spDongle.AddDeviceAsync(deviceId, accountID, lat, lng);
                if (!string.IsNullOrEmpty(spDongle.IoTHubDeviceKey))
                    return returnDevice(spDongle);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("DeviceNotFound"))
                {
                    WebApiApplication.logStore.postLog("Warning", "DeviceController", "AddDevice", "Device Not Found", HttpContext.Request.Form.ToString());
                    return returnNotFound();
                }

                WebApiApplication.logStore.postLog("Error", "DeviceController", "AddDevice", "Internal Server Error", HttpContext.Request.Form.ToString() + "; Error: " + ex.Message);
            }
            return returnInternalServerError();
        }

        /* Call DELETE: device/removeDevice/100 */
        [HttpDelete]
        public async Task<ActionResult> RemoveDevice(string id)
        {
            try
            {
                DeviceModels spDongle = new DeviceModels();
                if (await spDongle.RemoveDeviceAsync(id))
                    return Json(new { message = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("DeviceNotFound"))
                {
                    WebApiApplication.logStore.postLog("Warning", "DeviceController", "RemoveDevice", "Device Not Found", "DeviceID:" + id);
                    return returnNotFound();
                }
                WebApiApplication.logStore.postLog("Error", "DeviceController", "RemoveDevice", "Internal Server Error", "DeviceID:" + id + ";Error: " + ex.Message);
            }
            return returnInternalServerError();
        }

        /* Call GET: device/getDevice/100 */
        public async Task<ActionResult> GetDevice(string id)
        {
            try
            {
                DeviceModels spDongleModel = new DeviceModels();
                await spDongleModel.GetDeviceAsync(id);
                if (!string.IsNullOrEmpty(spDongleModel.IoTHubDeviceKey))
                    return returnDevice(spDongleModel);
                else
                {
                    WebApiApplication.logStore.postLog("Warning", "DeviceController", "GetDevice", "Device Not Found", "DeviceID:" + id);
                    return returnNotFound();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("DeviceNotFound"))
                {
                    WebApiApplication.logStore.postLog("Warning", "DeviceController", "GetDevice", "Device Not Found", "DeviceID:" + id);
                    return returnNotFound();
                }
                else
                {
                    WebApiApplication.logStore.postLog("Error", "DeviceController", "GetDevice", "Internal Server Error", "DeviceID:" + id + "; Error: " + ex.Message);
                    return returnInternalServerError();
                }
            }
        }

        public ActionResult returnNotFound()
        {
            HttpContext.Response.StatusCode = 404;
            return Json(new { message = "Not Found" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult returnInternalServerError()
        {
            HttpContext.Response.StatusCode = 500;
            return Json(new { message = "Internal Server Error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult returnDevice(DeviceModels spDongle)
        {
            var DongleDevice = new
            {
                id = spDongle.IoTHubDeviceID,
                key = spDongle.IoTHubDeviceKey,
                IoTHostName = spDongle.IoTHubHostName,
                IoTHubProtocol = spDongle.IoTHubProtocol,
                AccelerationAxis = spDongle.AccelerationAxis,
                HardBreakValue = spDongle.HardBreakValue,
                HardAccelerationValue = spDongle.HardAccelerationValue,
                HeartBeatInterval = int.Parse(DeviceModels.HeartBeatInterval),
                SendMessageInterval = int.Parse(DeviceModels.SendMessageInterval)
            };
            return Json(DongleDevice, JsonRequestBehavior.AllowGet);
        }
    }
}