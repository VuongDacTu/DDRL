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
    public class AccountLecturersController : BaseController
    {
        private readonly DbDgrlContext _context;

        public AccountLecturersController(DbDgrlContext context)
        {
            _context = context;
        }
        public IsActive isActive = new IsActive();

        // GET: Admin/AccountLecturers
        public async Task<IActionResult> Index(string? lecturerId)
        {
            var dbDgrlContext = _context.AccountLecturers.Include(a => a.Lecturer);

            if (!lecturerId.IsNullOrEmpty())
            {
                dbDgrlContext = _context.AccountLecturers.Where(x => x.LecturerId.Equals(lecturerId)).Include(a => a.Lecturer);
            }
            return View(await dbDgrlContext.ToListAsync());
        }

        // GET: Admin/AccountLecturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountLecturer = await _context.AccountLecturers
                .Include(a => a.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountLecturer == null)
            {
                return NotFound();
            }

            return View(accountLecturer);
        }
        public IActionResult Active(string? mgv,int? accId)
        {
            _context.AccountLecturers.FirstOrDefault(x => x.Id == accId).IsActive = isActive.HoatDong();
            _context.SaveChanges();
            return RedirectToAction("Index", new {lecturerId = mgv});
        }
        public IActionResult Passive(string? mgv,int? accId)
        {
            _context.AccountLecturers.FirstOrDefault(x => x.Id == accId).IsActive = isActive.NgungHoatDong();
            _context.SaveChanges();
            return RedirectToAction("Index", new { lecturerId = mgv });
        }
        public IActionResult ResetPassword(string? mgv, int? accId)
        {
            _context.AccountLecturers.FirstOrDefault(x => x.Id == accId).Password = "12345";
            _context.SaveChanges();
            return RedirectToAction("Index", new { lecturerId = mgv });
        }
    }
}
