using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using PJ_DGRL.Models.DGRLModels;
using System.Security.Cryptography;
using System.Text;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly DbDgrlContext _context;
        public LoginController(DbDgrlContext context)
        {
            _context = context;
        }
        public IActionResult Index(string url)
        {
            if (HttpContext.Session.GetString("AdminLogin") != null)
            {
                var dataLogin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                ViewBag.Admin = dataLogin;
            }
            ViewBag.UrlAction = url;
            return View();
        }
        [HttpPost]
        public IActionResult Index(AccountAdmin model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // trả về trạng thái lỗi
            }
            //đăng nhập cho sinh viên
            var pass = GetSHA26Hash(model.Password);
            var dataLogin = _context.AccountAdmins.Where(x => x.UserName.Equals(model.UserName)).FirstOrDefault(x => x.Password.Equals(pass));
            if(dataLogin.IsActive == 1)
            {
                TempData["errorLogin"] = "Tài khoản đã bị khoá !";
                return View(model);
            }
            if (dataLogin != null)
            {
                // Lưu lại session khi đăng nhập thành công
                HttpContext.Session.SetString("AdminLogin", dataLogin.ToJson());
                return RedirectToAction("Index", "Dashboard");
            }
            TempData["errorLogin"] = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLogin");
            // huỷ session với key đã lưu trước đó
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string password)
        {
            var accadmin= JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            var account = _context.AccountAdmins.Find(accadmin.Id);
            account.Password = GetSHA26Hash(password);
            _context.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
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
