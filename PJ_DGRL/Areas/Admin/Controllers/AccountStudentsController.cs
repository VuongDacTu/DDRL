using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PJ_DGRL.Areas.Admin.Models;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class AccountStudentsController : Base1Controller
    {
        private readonly DbDgrlContext _context;

        public AccountStudentsController(DbDgrlContext context)
        {
            _context = context;
        }
        public IsActive isActive = new IsActive();
        // GET: Admin/AccountStudents
        public async Task<IActionResult> Index(string? msv, bool? isDelete)
        {
            
            var dbDgrlContext = _context.AccountStudents.Include(a => a.Student);
            if (!msv.IsNullOrEmpty())
            {
                dbDgrlContext = _context.AccountStudents.Where(x => x.StudentId == msv).Include(a => a.Student);
            }
            ViewBag.IsDelete = isDelete;
            return View(await dbDgrlContext.OrderBy(x => x.IsActive).ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acc = await _context.AccountStudents
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acc == null)
            {
                return NotFound();
            }

            return View(acc);
        }
        private bool AccountStudentExists(int id)
        {
            return _context.AccountStudents.Any(e => e.Id == id);
        }
        public IActionResult Active(string? msv,int? accId)
        {
            _context.AccountStudents.FirstOrDefault(x => x.Id == accId).IsActive = isActive.HoatDong();
            _context.SaveChanges();
            return RedirectToAction("Index", new {msv = msv});
        }
        public IActionResult Passive(string? msv,int? accId)
        {
            _context.AccountStudents.FirstOrDefault(x => x.Id == accId).IsActive = isActive.NgungHoatDong();
            _context.SaveChanges();
            return RedirectToAction("Index", new { msv = msv });
        }
        public IActionResult ResetPassword(string? msv, int? accId)
        {
            var acc = _context.AccountStudents.FirstOrDefault(x => x.Id == accId);
            var dataLogin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            acc.Password = GetSHA26Hash("12345");
            acc.UpdateDate = DateTime.Now;
            acc.CreateBy = dataLogin.UserName;
            _context.SaveChanges();
            return RedirectToAction("Index", new { msv = msv });
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
