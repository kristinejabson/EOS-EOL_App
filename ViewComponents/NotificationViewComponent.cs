using Microsoft.AspNetCore.Mvc;

namespace TimeBasedPreventiveMeasures.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("NotificationsComponent");
        }
    }
}
