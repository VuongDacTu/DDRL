using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PJ_DGRL.Areas.Admin.Models;
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
        public Status _status = new Status();
        public IsActive _isActive = new IsActive();
        public IsDelete _isDelete = new IsDelete(); 

        // GET: Admin/Students
        public async Task<IActionResult> Index(string? name,int? classId, bool? isDelete, int? departmentId)
        {
            if(isDelete == null)
            {
                isDelete = false;
            }
            ViewBag.IsDelete = isDelete;
            var students = _context.Students.Include(s => s.Class).ThenInclude(s => s.Department).Include(s => s.Position);
            var dbDgrlContext = students.Where(x => x.IsDelete == isDelete && x.Class.DepartmentId == departmentId);
            if (classId != null)
            {
                dbDgrlContext = students.Where(x => x.IsDelete == isDelete && x.ClassId == classId);
                if (!name.IsNullOrEmpty())
                {
                    dbDgrlContext = students.Where(x => x.IsDelete == isDelete && x.Class.DepartmentId == departmentId && x.ClassId == classId && x.FullName.Contains(name));
                }
            }else if (!name.IsNullOrEmpty())
            {
                    dbDgrlContext = students.Where(x => x.IsDelete == isDelete && x.Class.DepartmentId == departmentId && x.FullName.Contains(name));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes.Where(x => x.DepartmentId == departmentId), "Id", "Name",classId);
            ViewBag.DepartmentId = departmentId;
            
            return View(await dbDgrlContext.ToListAsync());
        }

        // GET: Admin/Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Admin/Students/Create
        public IActionResult Create(int? departmentId)
        {
            ViewData["ClassId"] = new SelectList(_context.Classes.Where(x => x.IsDelete == _isDelete.Hien()), "Id", "Name");
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status != _status.GiangVien()), "Id", "Name");
            ViewBag.Student = _context.Students.ToList();
            ViewBag.DepartmentId = departmentId;
            return View();
        }

        // POST: Admin/Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Birthday,Email,Phone,Gender,ClassId,PositionId,IsActive")] Students student, int?departmentId)
        {

            if (ModelState.IsValid)
            {
                var students = _context.Students.FirstOrDefault(x => x.Id == student.Id);

                var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                AccountStudent accountStudent = new AccountStudent() {
                    UserName = student.Id,
                    Password = GetSHA26Hash("12345"),
                    CreateBy = admin.UserName,
                    CreateDate = DateTime.Now,
                    IsActive = _isActive.HoatDong(),
                    StudentId = student.Id                    
                };
                if(student.IsActive != _isActive.HoatDong())
                {
                    accountStudent.IsActive = _isActive.NgungHoatDong();
                }
                student.IsDelete = false;
 

                if (students != null)
                {
                    ViewBag.Error = "Mã sinh viên đã tồn tại !";
                    ViewData["ClassId"] = new SelectList(_context.Classes.Where(x => x.IsDelete == _isDelete.Hien()), "Id", "Name");
                    ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status != _status.GiangVien()), "Id", "Name");
                    ViewBag.Student = _context.Students.ToList();
                    ViewBag.DepartmentId = departmentId;
                    return View(student);
                }
                _context.AccountStudents.Add(accountStudent);
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                //return Json("Thêm mới thành công");
                return RedirectToAction(nameof(Index), new { departmentId = departmentId, isDelete = _isDelete.Hien() });
            }
            ViewData["ClassId"] = new SelectList(_context.Classes.Where(x => x.IsDelete == _isDelete.Hien()), "Id", "Name", student.ClassId);
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status != _status.GiangVien()), "Id", "Name", student.PositionId);
            return View(student);
            

        }

        // GET: Admin/Students/Edit/5
        public async Task<IActionResult> Edit(string id, int? departmentId, bool? isDelete)
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
            ViewData["ClassId"] = new SelectList(_context.Classes.Where(x => x.IsDelete == _isDelete.Hien()), "Id", "Name", student.ClassId);
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status != _status.GiangVien()), "Id", "Name", student.PositionId);
            ViewBag.DepartmentId = departmentId;
            ViewBag.IsDelete=isDelete;
            return View(student);
        }

        // POST: Admin/Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,Birthday,Email,Phone,Gender,ClassId,PositionId,IsActive")] Students student, int? departmentId, bool? isDelete)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    student.IsDelete = (bool)isDelete;
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    var st = _context.Students.FirstOrDefault(x => x.Id == id);
                    var acc = _context.AccountStudents.FirstOrDefault(x => x.StudentId == id);
                    if(st.IsActive == _isActive.HoatDong())
                    {
                        acc.IsActive = _isActive.HoatDong();
                    }
                    else
                    {
                        acc.IsActive = _isActive.NgungHoatDong();
                    }
                    _context.SaveChanges();
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
                return RedirectToAction(nameof(Index), new {departmentId = departmentId, isDelete = isDelete });
            }
            ViewData["ClassId"] = new SelectList(_context.Classes.Where(x => x.IsDelete == false), "Id", "Name", student.ClassId);
            ViewData["PositionId"] = new SelectList(_context.Positions.Where(x => x.Status != _status.GiangVien()), "Id", "Name", student.PositionId);
            return RedirectToAction(nameof(Index), new { departmentId = departmentId, isDelete = isDelete });
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
                student.IsDelete = _isDelete.An();
                acc.IsActive = _isActive.NgungHoatDong();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { departmentId = departmentId, isDelete = _isDelete.Hien()});
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
        public IActionResult Show(string? id, int? departmentId)
        {
            var student =  _context.Students.Find(id);
            var acc = _context.AccountStudents.Where(x => x.StudentId == id).FirstOrDefault();
            if (student != null)
            {
                student.IsDelete = _isDelete.Hien();
                acc.IsActive = _isActive.HoatDong();
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { isDelete = _isDelete.An(), departmentId = departmentId });
        }
        public async Task<IActionResult> Remove(string id, int? departmentId)
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
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(string id, int? departmentId)
        {
            var student = await _context.Students.FindAsync(id);
            var acc = _context.AccountStudents.Where(x => x.StudentId == id).FirstOrDefault();
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.AccountStudents.Remove(acc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { departmentId = departmentId, isDelete = _isDelete.An() });
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