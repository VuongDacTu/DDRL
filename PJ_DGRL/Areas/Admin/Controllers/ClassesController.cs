using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class ClassesController : BaseController
    {
        private readonly DbDgrlContext _context;

        public ClassesController(DbDgrlContext context)
        {
            _context = context;
        }

        // GET: Admin/Classes
        public async Task<IActionResult> Index()
        {
            var dbDgrlContext = _context.Classes.Include(@c => @c.Course).Include(@c => @c.Department);
            return View(await dbDgrlContext.ToListAsync());
        }

        // GET: Admin/Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @cclass = await _context.Classes
                .Include(@c => @c.Course)
                .Include(@c => @c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@cclass == null)
            {
                return NotFound();
            }

            return View(@cclass);
        }

        // GET: Admin/Classes/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Admin/Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CourseId,DepartmentId,IsActive")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", @class.CourseId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", @class.DepartmentId);
            return View(@class);
        }

        // GET: Admin/Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = _context.Students.FirstOrDefault(x => x.ClassId == id);
            if (student != null)
            {
                return RedirectToAction("Status");
            }
            var @class = await _context.Classes
                .Include(@c => @c.Course)
                .Include(@c => @c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Admin/Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
        public IActionResult Status()
        {
            return View();
        }
    }
}
