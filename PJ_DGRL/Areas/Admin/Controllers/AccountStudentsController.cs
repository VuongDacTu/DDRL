using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PJ_DGRL.Areas.Admin.Models;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class AccountStudentsController : BaseController
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
            _context.AccountStudents.FirstOrDefault(x => x.Id == accId).Password = "12345";
            _context.SaveChanges();
            return RedirectToAction("Index", new { msv = msv });
        }
    }
}
