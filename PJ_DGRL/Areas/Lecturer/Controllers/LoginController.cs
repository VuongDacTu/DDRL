using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using PJ_DGRL.Models.DGRLModels;
using System.Security.Cryptography;
using System.Text;

namespace PJ_DGRL.Areas.Lecturer.Controllers
{
    [Area("Lecturer")]
    public class LoginController : Controller
    {
        private readonly DbDgrlContext _context;
        public LoginController(DbDgrlContext context)
        {
            _context = context;
        }
        public IActionResult Index(string url)
        {
            if (HttpContext.Session.GetString("LecturerLogin") != null)
            {
                var dataLogin = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("LecturerLogin"));
                ViewBag.Lecturer = dataLogin;
            }
            ViewBag.UrlAction = url;
            return View();
        }
        [HttpPost]
        public IActionResult Index(AccountLecturer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // trả về trạng thái lỗi
            }
            //đăng nhập cho sinh viên
            var pass = GetSHA26Hash(model.Password);
            var dataLogin = _context.AccountLecturers.Where(x => x.UserName.Equals(model.UserName)).FirstOrDefault(x => x.Password.Equals(pass));

            if (dataLogin != null)
            {
                if(dataLogin.IsActive == 1)
                {
                    // Lưu lại session khi đăng nhập thành công
                    HttpContext.Session.SetString("LecturerLogin", dataLogin.ToJson());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["errorLogin"] = "Tài khoản đã bị khoá";
                    return View(model);
                }
            }
            TempData["errorLogin"] = "Mã giảng viên hoặc mật khẩu không đúng";
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LecturerLogin");
            // huỷ session với key đã lưu trước đó
            return RedirectToAction("Index","Home", new { area = "" });
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
		public IActionResult ChangePassword(string password)
		{
			var acclecturer = JsonConvert.DeserializeObject<AccountLecturer>(HttpContext.Session.GetString("LecturerLogin"));
			var account = _context.AccountLecturers.Find(acclecturer.Id);
			account.Password = GetSHA26Hash(password);
			_context.SaveChanges();
			return RedirectToAction("Index", "Home");
		}
		static string GetSHA26Hash(string input)
        {
            string hash = "";
            using (var sha256 = new SHA256Managed())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
            return hash;
        }
    }
}
