using Microsoft.AspNetCore.Mvc;

namespace PJ_DGRL.Areas.Student.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
