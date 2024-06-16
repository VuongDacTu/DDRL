using Microsoft.AspNetCore.Mvc;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
