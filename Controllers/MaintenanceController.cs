using Microsoft.AspNetCore.Mvc;

namespace TimeBasedPreventiveMeasures.Controllers
{
    public class MaintenanceController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("History");
        }

        public IActionResult History()
        {
            return View();
        }
    }
}
