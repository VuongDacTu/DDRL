using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PJ_DGRL.Models.DGRLModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> Index(int? departmentId, string? coursesId,string? name,bool? isDelete)
        {
            var c = _context.Classes.Include(@c => @c.Course).Include(@c => @c.Department);
            var dbDgrlContext = c.Where(x => x.IsDelete == isDelete);
            if(departmentId != null)
            {
                dbDgrlContext = c.Where(x => x.IsDelete == isDelete && x.DepartmentId == departmentId);
                if(coursesId != null)
                {
                    dbDgrlContext = c.Where(x => x.IsDelete == isDelete && x.DepartmentId == departmentId && x.CourseId == coursesId);
                    if (!name.IsNullOrEmpty())
                    {
                        dbDgrlContext =c.Where(x => x.IsDelete == isDelete && x.DepartmentId == departmentId && x.CourseId == coursesId && x.Name.Contains(name));
                    }
                }
                else if (!name.IsNullOrEmpty())
                {
                    dbDgrlContext = c.Where(x => x.IsDelete == isDelete && x.DepartmentId == departmentId && x.Name.Contains(name));
                }
            }
            else if (coursesId != null)
            {
                dbDgrlContext = c.Where(x => x.IsDelete == isDelete && x.CourseId == coursesId);
                if (!name.IsNullOrEmpty())
                {
                    dbDgrlContext = c.Where(x => x.IsDelete == isDelete && x.CourseId == coursesId && x.Name.Contains(name));
                }
            }else if (!name.IsNullOrEmpty())
            {
                dbDgrlContext = c.Where(x => x.IsDelete == isDelete && x.Name.Contains(name));
            }
            ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name");
            ViewBag.CoursesId = new SelectList(_context.Courses.Where(x => x.IsDelete == false), "Id", "Id");
            ViewBag.IsDelete = isDelete;
            return View(await dbDgrlContext.ToListAsync());
        }

        // GET: Admin/Classes/Details/5
        public async Task<IActionResult> Details(int? id,bool? isDelete)
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
            ViewBag.IsDelete = isDelete;
            return View(@cclass);
        }

        // GET: Admin/Classes/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
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
                @class.IsDelete = false;
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { isDelete=false }) ;
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", @class.CourseId);
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
                var student = _context.Students.Where(x => x.ClassId == id).ToList();
                @class.IsDelete = true;
                foreach(var item in student)
                {
                    var acc = _context.AccountStudents.FirstOrDefault(x => x.StudentId == item.Id);
                    item.IsActive = 3;
                    item.IsDelete = true;
                    acc.IsActive = 0;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {isDelete=false});
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
        public IActionResult Status()
        {
            return View();
        }
        public IActionResult Passive(int? classId)
        {
            var c = _context.Classes.FirstOrDefault(x => x.Id == classId);
            var student = _context.Students.Where(x => x.ClassId == classId).ToList();
            c.IsActive = 0;
            foreach(var item in student)
            {
                _context.AccountStudents.FirstOrDefault(x => x.StudentId == item.Id).IsActive = 0;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Active(int? classId)
        {
            var c = _context.Classes.FirstOrDefault(x => x.Id == classId);
            var student = _context.Students.Where(x => x.ClassId == classId).ToList();
            c.IsActive = 1;
            foreach (var item in student)
            {
                _context.AccountStudents.FirstOrDefault(x => x.StudentId == item.Id).IsActive = 1;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Show(int? id) 
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                var student = _context.Students.Where(x => x.ClassId == id).ToList();
                @class.IsDelete = false;
                foreach (var item in student)
                {
                    var acc = _context.AccountStudents.FirstOrDefault(x => x.StudentId == item.Id);
                    item.IsActive = 1;
                    item.IsDelete = false;
                    acc.IsActive = 1;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new {isDelete=true});
        }
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
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

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                var student = _context.Students.Where(x => x.ClassId == id).ToList();

                foreach (var item in student)
                {
                    var acc = _context.AccountStudents.FirstOrDefault(x => x.StudentId == item.Id);
                    _context.Remove(acc);
                }
                _context.SaveChanges();
                _context.Students.RemoveRange(student);
                _context.SaveChanges();
                _context.Classes.Remove(@class);



            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {isDelete=true});
        }
    }
}
