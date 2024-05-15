using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class DepartmentsController : BaseController
    {
        private readonly DbDgrlContext _context;

        public DepartmentsController(DbDgrlContext context)
        {
            _context = context;
        }

        // GET: Admin/Departments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }

        // GET: Admin/Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Times")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }


        // GET: Admin/Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var lecturer = _context.Lecturers.FirstOrDefault(x => x.DepartmentId == id);
            var @class = _context.Classes.FirstOrDefault(x => x.DepartmentId == id);
            if(@class != null || lecturer != null)
            {
                return RedirectToAction("Status", new {id = id});
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Admin/Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
        public IActionResult Status(int? id)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(x => x.DepartmentId == id);
            var @class = _context.Classes.FirstOrDefault(x => x.DepartmentId == id);
            if (@class != null)
            {
                ViewBag.Status = "Lớp " + @class.Name.ToString() + " đang sử dụng chuyên ngành này, bạn phải xoá lớp học trước khi xoá chuyên ngành";
                if(lecturer != null)
                {
                    ViewBag.Status = "Lớp " + @class.Name.ToString()  + " và giảng viên có mã "+lecturer.Id.ToString()+ " đang sử dụng chuyên ngành này, bạn phải xoá lớp học và giảng viên trước khi xoá chuyên ngành";
                }
            }else if (lecturer != null)
            {
                ViewBag.Status = "Giảng viên có mã " + lecturer.Id.ToString() + " đang sử dụng chuyên ngành này, bạn phải xoá giảng viên trước khi xoá chuyên ngành";
            }
            return View();
        }
    }
}
