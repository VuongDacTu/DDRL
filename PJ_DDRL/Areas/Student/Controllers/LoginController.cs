using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using NuGet.Protocol;
using PJ_DDRL.Models.DDRLModels;
using System.Security.Policy;
using System.Text;

namespace PJ_DDRL.Areas.Student.Controllers
{
    [Area("Student")]
    public class LoginController : Controller
    {
        private readonly DdrlContext _context;
        public LoginController(DdrlContext context)
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
            //xử lý logic phần đăng nhập tại đây

            //var pass = GetSHA26Hash(model.Password);
            var dataLogin = _context.AccountStudents.Where(x => x.UserName.Equals(model.UserName)).FirstOrDefault(x => x.Password.Equals(model.Password));

            if (dataLogin != null)
            {
                // Lưu lại session khi đăng nhập thành công
                HttpContext.Session.SetString("StudentLogin", dataLogin.ToJson());

                return RedirectToAction("Index","Semester");
            }
            TempData["errorLogin"] = "Mã sinh viên hoặc mật khẩu không đúng";
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("StudentLogin");
            // huỷ session với key đã lưu trước đó
            return RedirectToAction("Index");
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
