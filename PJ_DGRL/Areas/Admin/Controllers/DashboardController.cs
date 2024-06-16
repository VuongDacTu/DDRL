using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PJ_DGRL.Areas.Admin.Controllers;

namespace PJ_DGRL.Areas.Admins.Controllers
{

    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
