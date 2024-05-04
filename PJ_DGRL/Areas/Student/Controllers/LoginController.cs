using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.Controllers
{
    [Area("Student")]
    public class LoginController : Controller
    {
        private readonly DbDgrlContext _context;
        public LoginController(DbDgrlContext context)
        {
            _context = context;
        }
        public IActionResult Index(string url)
        {
            if (HttpContext.Session.GetString("StudentLogin") != null)
            {
                var dataLogin = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
                ViewBag.Student = dataLogin;
            }
            ViewBag.UrlAction = url;
            return View();
        }
        [HttpPost]
        public IActionResult Index(AccountStudent model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // trả về trạng thái lỗi
            }
            //đăng nhập cho sinh viên
            //var pass = GetSHA26Hash(model.Password);
            var dataLogin = _context.AccountStudents.Where(x => x.UserName.Equals(model.UserName)).FirstOrDefault(x => x.Password.Equals(model.Password));
           
            if (dataLogin != null)
            {
                // Lưu lại session khi đăng nhập thành công
                HttpContext.Session.SetString("StudentLogin", dataLogin.ToJson());
                var lTLogin = _context.Students.Where(x => x.Id.Equals(dataLogin.StudentId)).FirstOrDefault(x => x.PositionId.Equals("LT"));
                if (lTLogin != null)
                {
					HttpContext.Session.SetString("LTLogin", dataLogin.UserName);
				}
				return RedirectToAction("Index", "Home");
            }
            TempData["errorLogin"] = "Mã sinh viên hoặc mật khẩu không đúng";
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("StudentLogin");
			HttpContext.Session.Remove("LTLogin");
            // huỷ session với key đã lưu trước đó
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        //static string GetSHA26Hash(string input)
        //{
        //    string hash = "";
        //    using (var sha256 = new SHA256Managed())
        //    {
        //        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        //        hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        //    }
        //    return hash;
        //}
    }
}
