using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class LecturersController : BaseController
    {
        private readonly DbDgrlContext _context;

        public LecturersController(DbDgrlContext context)
        {
            _context = context;
        }

        // GET: Admin/Lecturers
        public async Task<IActionResult> Index(int? departmentId,string? name, bool? isDelete)
        {
            var dbDgrlContext = _context.Lecturers.Include(l => l.Department).Include(l => l.Position).Where(x => x.IsDelete == isDelete);
            if(departmentId != null)
            {
                dbDgrlContext = _context.Lecturers.Include(l => l.Department).Include(l => l.Position).Where(x => x.IsDelete == isDelete && x.DepartmentId == departmentId);
                if (!name.IsNullOrEmpty())
                {
                    dbDgrlContext = _context.Lecturers.Include(l => l.Department).Include(l => l.Position).Where(x => x.IsDelete == isDelete && x.DepartmentId == departmentId && x.FullName.Contains(name));
                }
            }else if (!name.IsNullOrEmpty())
            {
                dbDgrlContext = _context.Lecturers.Include(l => l.Department).Include(l => l.Position).Where(x => x.IsDelete == isDelete && x.FullName.Contains(name));
            }
            ViewBag.Department = _context.Departments.ToList();
            ViewBag.Name = name;
            ViewBag.IsDelete = isDelete;
            return View(await dbDgrlContext.ToListAsync());
        }

        // GET: Admin/Lecturers/Details/5
        public async Task<IActionResult> Details(string id, bool? isDelete)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
                .Include(l => l.Department)
                .Include(l => l.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecturer == null)
            {
                return NotFound();
            }
            ViewBag.IsDelete=isDelete;
            return View(lecturer);
        }

        // GET: Admin/Lecturers/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
            return View();
        }

        // POST: Admin/Lecturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,DepartmentId,PositionId,Birthday,Email,Phone,IsActive")] Lecturers lecturer)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                AccountLecturer acc = new AccountLecturer() {
                    UserName = lecturer.Id,
                    Password = "12345",
                    IsActive = 1,
                    CreateBy = admin.UserName,
                    CreateDate = DateTime.Now,
                    LecturerId = lecturer.Id
                };
                if (lecturer.IsActive != 1)
                {
                    acc.IsActive = 0;
                }
                lecturer.IsDelete = false;
                _context.AccountLecturers.Add(acc);
                _context.Lecturers.Add(lecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {isDelete = false});
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", lecturer.DepartmentId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", lecturer.PositionId);
            return View(lecturer);
        }

        // GET: Admin/Lecturers/Edit/5
        public async Task<IActionResult> Edit(string id,bool? isDelete)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", lecturer.DepartmentId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", lecturer.PositionId);
            ViewBag.IsDelete = isDelete;
            return View(lecturer);
        }

        // POST: Admin/Lecturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,DepartmentId,PositionId,Birthday,Email,Phone,IsActive")] Lecturers lecturer, bool? isDelete)
        {
            if (id != lecturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    lecturer.IsDelete = isDelete;

                    _context.Update(lecturer);
                    await _context.SaveChangesAsync();
                    var gv = _context.Lecturers.FirstOrDefault(x => x.Id == id);
                    if (gv.IsActive == 1)
                    {
                        _context.AccountLecturers.FirstOrDefault(x => x.LecturerId == id).IsActive = 1;
                    }
                    else
                    {
                        _context.AccountLecturers.FirstOrDefault(x => x.LecturerId == id).IsActive = 0;
                    }
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new {isDelete = isDelete});
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", lecturer.DepartmentId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", lecturer.PositionId);
            return View(lecturer);
        }

        // GET: Admin/Lecturers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
                .Include(l => l.Department)
                .Include(l => l.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // POST: Admin/Lecturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer != null)
            {
                var acc = _context.AccountLecturers.FirstOrDefault(x => x.LecturerId == id);
                lecturer.IsDelete = true;
                acc.IsActive = 0;

            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {isDelete = false});
        }

        private bool LecturerExists(string id)
        {
            return _context.Lecturers.Any(e => e.Id == id);
        }
        public IActionResult Show(string id)
        {
            var lecturer = _context.Lecturers.Find(id);
            if (lecturer != null)
            {
                var acc = _context.AccountLecturers.FirstOrDefault(x => x.LecturerId == id);
                lecturer.IsDelete = false;
                acc.IsActive = 1;

            }
            _context.SaveChanges();
            return RedirectToAction("Index", new { isDelete = true});
        }
        public async Task<IActionResult> Remove(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
                .Include(l => l.Department)
                .Include(l => l.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(string id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer != null)
            {
                var acc = _context.AccountLecturers.FirstOrDefault(x => x.LecturerId == id);
                _context.Lecturers.Remove(lecturer);
                _context.AccountLecturers.Remove(acc);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { isDelete = true });
        }

    }
}
