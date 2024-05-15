using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        private readonly DbDgrlContext _context;
        public ReportController(DbDgrlContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? departmentId, int? semesterId)
        {
            if(semesterId == null)
            {
                semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsActive == 1).Id;
            }
            var @class = _context.Classes.Include(x => x.Department).Include(x => x.Students).ThenInclude(x => x.SumaryOfPoints).Where(x => x.IsDelete == false && x.DepartmentId == departmentId).ToList();
            ViewData["Semester"] = _context.Semesters.Where(x => x.IsActive == 1).ToList();
            ViewBag.Student = _context.Students.Include(x => x.SumaryOfPoints).ToList();
            ViewBag.Sum = _context.SumaryOfPoints.Include(x => x.Student).ThenInclude(x => x.Class).Where(x => x.SemesterId == semesterId).ToList();
            ViewBag.DepartmentId = departmentId;
            ViewBag.SemesterId = semesterId;
            return View(@class);
        }
    }
}
