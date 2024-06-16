using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class Base1Controller : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(context.HttpContext.Session.GetString("AdminLogin"));
            if (context.HttpContext.Session.GetString("AdminLogin") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { Controller = "Login", Action = "Index", Areas = "Admin" }));
            }
            if(admin.RoleId == 3)
            {
                context.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { Controller = "Status", Action = "Index", Areas = "Admin" }));
            }
            base.OnActionExecuted(context);
        }
    }
}
