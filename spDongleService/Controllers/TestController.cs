using ShareClasses;
using spDongleService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace spDongleService.Controllers
{
    public class TestController : Controller
    {
        /* Call POST: test/getDevice/100 */
        [HttpPost]
        public async Task<ActionResult> GetDevice()
        {
            string lat = HttpContext.Request.Form["lat"].ToString();
            string lng = HttpContext.Request.Form["lng"].ToString();

            GoogleService gService = new GoogleService();
            string Country = await gService.GetCountryByLocation(lat, lng);
            return Json(new { country = Country }, JsonRequestBehavior.AllowGet);
        }

        /* Call POST: test/getDevice/100 */
        [HttpPost]
        public async Task<ActionResult> GetLocalTime()
        {
            string lat = HttpContext.Request.Form["lat"].ToString();
            string lng = HttpContext.Request.Form["lng"].ToString();
            string timestamp = HttpContext.Request.Form["timestamp"].ToString();

            GoogleService gService = new GoogleService();
            DateTime timeCode = await gService.GetLocalTimeByLocation(lat, lng, timestamp);
            return Json(new { time = timeCode }, JsonRequestBehavior.AllowGet);
        }
    }
}