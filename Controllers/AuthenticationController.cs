using Microsoft.AspNetCore.Mvc;

namespace TimeBasedPreventiveMeasures.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
