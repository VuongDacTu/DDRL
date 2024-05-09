using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;
namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class StudentsController : BaseController
    {
        private readonly DbDgrlContext _context;

        public StudentsController(DbDgrlContext context)
        {
            _context = context;
        }

        // GET: Admin/Students
        public async Task<IActionResult> Index(string? name,int? classId, int? departmentId)
        {
            var dbDgrlContext = _context.Students.Include(s => s.Class).ThenInclude(s => s.Department).Include(s => s.Position).Where(x => x.IsDelete == false && x.Class.DepartmentId == departmentId);
            if (classId != null)
            {
                dbDgrlContext = _context.Students.Include(s => s.Class).Include(s => s.Position).Where(x => x.IsDelete == false && x.ClassId == classId);
                if (!name.IsNullOrEmpty())
                {
                    dbDgrlContext = _context.Students.Include(s => s.Class).Include(s => s.Position).Where(x => x.IsDelete == false && x.Class.DepartmentId == departmentId && x.ClassId == classId && x.FullName.Contains(name));
                }
            }else if (!name.IsNullOrEmpty())
            {
                    dbDgrlContext = _context.Students.Include(s => s.Class).Include(s => s.Position).Where(x => x.IsDelete == false && x.Class.DepartmentId == departmentId && x.FullName.Contains(name));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes.Where(x => x.DepartmentId == departmentId), "Id", "Name");
            ViewBag.DepartmentId = departmentId;
            return View(await dbDgrlContext.ToListAsync());
        }

        // GET: Admin/Students/Details/5
        public async Task<IActionResult> Details(string id, int? departmentId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.DepartmentId = departmentId;
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Admin/Students/Create
        public IActionResult Create(int? departmentId)
        {
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name");
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Id != "GV"), "Id", "Name");
            ViewBag.DepartmentId = departmentId;
            return View();
        }

        // POST: Admin/Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Birthday,Email,Phone,Gender,ClassId,PositionId,IsActive")] Students student)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                AccountStudent accountStudent = new AccountStudent() {
                    UserName = student.Id,
                    Password = "12345",
                    CreateBy = admin.UserName,
                    CreateDate = DateTime.Now,
                    IsActive = 1,
                    StudentId = student.Id,
                    IsDelete = false
                    
                };
                student.IsDelete = false;
                _context.AccountStudents.Add(accountStudent);
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", student.ClassId);
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Id != "GV"), "Id", "Name", student.PositionId);
            return View(student);
        }

        // GET: Admin/Students/Edit/5
        public async Task<IActionResult> Edit(string id, int? departmentId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", student.ClassId);
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Id != "GV"), "Id", "Name", student.PositionId);
            ViewBag.DepartmentId = departmentId;

            return View(student);
        }

        // POST: Admin/Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,Birthday,Email,Phone,Gender,ClassId,PositionId,IsActive")] Students student, int? departmentId)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new {departmentId = departmentId});
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name", student.ClassId);
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Id !="GV"), "Id", "Name", student.PositionId);
            return RedirectToAction(nameof(Index), new { departmentId = departmentId });
        }

        // GET: Admin/Students/Delete/5
        public async Task<IActionResult> Delete(string id, int? departmentId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.DepartmentId = departmentId;
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Admin/Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, int? departmentId)
        {
            var student = await _context.Students.FindAsync(id);
            var acc = _context.AccountStudents.Where(x => x.StudentId == id).FirstOrDefault();
            if (student != null)
            {
                student.IsDelete = true;
                acc.IsDelete = true;
                acc.IsActive = 0;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { departmentId = departmentId });
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
