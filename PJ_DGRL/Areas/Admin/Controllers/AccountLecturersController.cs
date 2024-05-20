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

        // GET: Admin/AccountLecturers/Create
        public IActionResult Create()
        {
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "Id");
            return View();
        }

        // POST: Admin/AccountLecturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,CreateBy,CreateDate,UpdateDate,IsActive,LecturerId,IsDelete")] AccountLecturer accountLecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountLecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "Id", accountLecturer.LecturerId);
            return View(accountLecturer);
        }

        // GET: Admin/AccountLecturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountLecturer = await _context.AccountLecturers.FindAsync(id);
            if (accountLecturer == null)
            {
                return NotFound();
            }
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "Id", accountLecturer.LecturerId);
            return View(accountLecturer);
        }

        // POST: Admin/AccountLecturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,CreateBy,CreateDate,UpdateDate,IsActive,LecturerId,IsDelete")] AccountLecturer accountLecturer,string? mgv)
        {
            if (id != accountLecturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountLecturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountLecturerExists(accountLecturer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { lecturerId = mgv });
            }
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "Id", accountLecturer.LecturerId);
            return View(accountLecturer);
        }

        // GET: Admin/AccountLecturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/AccountLecturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountLecturer = await _context.AccountLecturers.FindAsync(id);
            if (accountLecturer != null)
            {
                _context.AccountLecturers.Remove(accountLecturer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountLecturerExists(int id)
        {
            return _context.AccountLecturers.Any(e => e.Id == id);
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
