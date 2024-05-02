using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PJ_DGRL.Areas.Lecturer.Controllers
{
    [Area("Lecturer")]
    public class BaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Session.GetString("LecturerLogin") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { Controller = "Login", Action = "Index", Areas = "Lecturer" }));
            }
            base.OnActionExecuted(context);
        }
    }
}
