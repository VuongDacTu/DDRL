using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        // GET: Admin/AccountStudents
        public async Task<IActionResult> Index(string? msv, string stId)
        {
            var dbDgrlContext = _context.AccountStudents.Include(a => a.Student).Where(x => x.IsDelete == false);
            if(!msv.IsNullOrEmpty())
            {
                dbDgrlContext = _context.AccountStudents.Include(a => a.Student).Where(x => x.IsDelete == false && x.StudentId == msv);
            }
            if (!stId.IsNullOrEmpty())
            {
                dbDgrlContext = _context.AccountStudents.Include(a => a.Student).Where(x => x.IsDelete == false && x.StudentId.Contains(stId));
            }
            return View(await dbDgrlContext.OrderBy(x => x.IsActive).ToListAsync());
        }
        // GET: Admin/AccountStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountStudent = await _context.AccountStudents
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountStudent == null)
            {
                return NotFound();
            }

            return View(accountStudent);
        }

        // GET: Admin/AccountStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountStudent = await _context.AccountStudents
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountStudent == null)
            {
                return NotFound();
            }

            return View(accountStudent);
        }

        // POST: Admin/AccountStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountStudent = await _context.AccountStudents.FindAsync(id);

            if (accountStudent != null)
            {
                accountStudent.IsDelete = true;
                accountStudent.IsActive = 0;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool AccountStudentExists(int id)
        {
            return _context.AccountStudents.Any(e => e.Id == id);
        }
        public IActionResult Active(int? accId)
        {
            _context.AccountStudents.FirstOrDefault(x => x.Id == accId).IsActive = 1;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Passive(int? accId)
        {
            _context.AccountStudents.FirstOrDefault(x => x.Id == accId).IsActive = 0;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
