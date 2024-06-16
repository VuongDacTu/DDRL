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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using PJ_DGRL.Areas.Admin.Models;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class LecturersController : Base1Controller
    {
        private readonly DbDgrlContext _context;
        public IsDelete _isDelete;
        public LecturersController(DbDgrlContext context, IsDelete isDelete)
        {
            _context = context;
            _isDelete = isDelete;
        }
        public Status _status = new Status();
        public IsActive _isActive = new IsActive();


        // GET: Admin/Lecturers
        public async Task<IActionResult> Index(int? departmentId,string? name, bool? isDelete)
        {
            _isDelete.An();
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
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status == _status.GiangVien()), "Id", "Name");
            return View();
        }

        // POST: Admin/Lecturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,DepartmentId,PositionId,Education,IsLeader,Birthday,Email,Phone,IsActive")] Lecturers lecturer)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                AccountLecturer acc = new AccountLecturer() {
                    UserName = lecturer.Id,
                    Password = GetSHA26Hash("12345"),
                    IsActive = _isActive.HoatDong(),
                    CreateBy = admin.UserName,
                    CreateDate = DateTime.Now,
                    LecturerId = lecturer.Id
                };
                if (lecturer.IsActive != _isActive.HoatDong())
                {
                    acc.IsActive = _isActive.NgungHoatDong();
                }
                lecturer.IsDelete = _isDelete.Hien();
                var lecturers = _context.Lecturers.FirstOrDefault(x => x.Id == lecturer.Id);
                if(lecturers != null)
                {
                    ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
                    ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status == _status.GiangVien()), "Id", "Name");
                    ViewBag.Error = "Mã giảng viên đã tồn tại";
                    return View(lecturer);
                }
                    _context.AccountLecturers.Add(acc);
                    _context.Lecturers.Add(lecturer);
                    await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new {isDelete = _isDelete.Hien()});
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", lecturer.DepartmentId);
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status == _status.GiangVien()), "Id", "Name", lecturer.PositionId);
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
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status == _status.GiangVien()), "Id", "Name", lecturer.PositionId);
            ViewBag.IsDelete = isDelete;
            return View(lecturer);
        }

        // POST: Admin/Lecturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,DepartmentId,PositionId,Education,IsLeader,Birthday,Email,Phone,IsActive")] Lecturers lecturer, bool? isDelete)
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
                    if (gv.IsActive == _isActive.HoatDong())
                    {
                        _context.AccountLecturers.FirstOrDefault(x => x.LecturerId == id).IsActive = _isActive.HoatDong();
                    }
                    else
                    {
                        _context.AccountLecturers.FirstOrDefault(x => x.LecturerId == id).IsActive = _isActive.NgungHoatDong();
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
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status == _status.GiangVien()), "Id", "Name", lecturer.PositionId);
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
                lecturer.IsDelete = _isDelete.An();
                acc.IsActive = _isActive.NgungHoatDong();

            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {isDelete = _isDelete.Hien()});
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
                lecturer.IsDelete = _isDelete.Hien();
                acc.IsActive = _isActive.HoatDong();

            }
            _context.SaveChanges();
            return RedirectToAction("Index", new { isDelete = _isDelete.An()});
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
