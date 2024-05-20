using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PJ_DGRL.Areas.Admin.Models;
using PJ_DGRL.Models.DGRLModels;

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
        public IActionResult Index(int? semesterId, int? classId)
        {
            if(semesterId == null)
            {
                semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsActive == _isActive.HoatDong()).Id;
            }
            if(classId == null)
            {
                classId = _context.Classes.FirstOrDefault(x => x.IsDelete == _isDelete.Hien()).Id;
            }

            var sum = _context.SumaryOfPoints.Include(x => x.Student).Include(x => x.Class).ThenInclude(x => x.Department).Where(x => x.ClassId == classId && x.SemesterId == semesterId);
            ViewBag.Class = _context.Classes.Include(x => x.Department).Include(x => x.Students).ThenInclude(x => x.SumaryOfPoints).ToList();
            ViewData["Semester"] = _context.Semesters.OrderByDescending(x => x.Id).Where(x => x.IsActive == _isActive.HoatDong()).ToList();
            ViewBag.SemesterId = semesterId;
            ViewBag.ClassId = classId;
            ViewBag.Student = _context.Students.Include(x => x.SumaryOfPoints).Where(x => !x.SumaryOfPoints.Where(x => x.SemesterId == semesterId).Any()).ToList();
            return View(sum);
        }
    }
}
