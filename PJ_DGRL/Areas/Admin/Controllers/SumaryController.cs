using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PJ_DGRL.Areas.Admin.Models;
using PJ_DGRL.Models.DGRLModels;
using System.Text;
namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class SumaryController : BaseController
    {
        private readonly DbDgrlContext _context;
        public SumaryController(DbDgrlContext context)
        {
            _context = context;
        }
        public IsActive _isActive = new IsActive();
        public IsDelete _isDelete = new IsDelete();
        public IActionResult Index(int? departmentId, int? semesterId, int? classId)
        {
            if(semesterId == null)
            {
                semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsActive == _isActive.HoatDong()).Id;
            }
            if (classId == null)
            {
                classId = _context.Classes.FirstOrDefault(x => x.IsDelete == _isDelete.Hien()).Id;
            }
            if (departmentId == null)
            {
                departmentId = _context.Classes.FirstOrDefault(x => x.Id == classId).DepartmentId;
            }
            else
            {
                classId = _context.Classes.FirstOrDefault(x => x.IsDelete == _isDelete.Hien() && x.DepartmentId == departmentId).Id;

            }


            var sum = _context.SumaryOfPoints.Include(x => x.Student).Include(x => x.Class).ThenInclude(x => x.Department).Where(x => x.ClassId == classId && x.SemesterId == semesterId);
            ViewBag.Class = _context.Classes.Include(x => x.Department).Include(x => x.Students).ThenInclude(x => x.SumaryOfPoints).Where(x => x.DepartmentId == departmentId).ToList();
            ViewData["Semester"] = _context.Semesters.OrderByDescending(x => x.Id).Where(x => x.IsActive == _isActive.HoatDong()).ToList();
            ViewBag.SemesterId = semesterId;
            ViewBag.ClassId = classId;
            ViewBag.DepartmentId = departmentId;
            ViewBag.Department = _context.Departments.ToList();
            ViewBag.Student = _context.Students.Include(x => x.SumaryOfPoints).Where(x => !x.SumaryOfPoints.Where(x => x.SemesterId == semesterId).Any() && x.ClassId == classId).ToList();
            return View(sum);
        }
        [HttpPost]
        public FileResult ExportToExcel(string sumaryTable)
        {
            var utf8Bytes = Encoding.UTF8.GetBytes(sumaryTable);
            return File(utf8Bytes, "application/vnd.ms-excel","sumaryTable.xls");
        }
    }
}
