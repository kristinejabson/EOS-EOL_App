using Microsoft.AspNetCore.Mvc;

namespace TimeBasedPreventiveMeasures.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            //Example change
            return View();
        }

        public IActionResult NotificationListPartial(bool isPartial = true)
        {
            ViewData["IsPartial"] = isPartial;
            return PartialView("_NotificationListPartial");
        }
    }
}
