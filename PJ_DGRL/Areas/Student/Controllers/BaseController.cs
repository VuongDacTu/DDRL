using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PJ_DGRL.Areas.Student.Controllers
{
    [Area("Student")]
    public class BaseController : Controller
    {


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Session.GetString("StudentLogin") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { Controller = "Login", Action = "Index", Areas = "Student" }));
            }
            base.OnActionExecuted(context);
        }

    }
}
