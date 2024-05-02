using Microsoft.AspNetCore.Mvc;

namespace PJ_DGRL.Areas.Lecturer.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
