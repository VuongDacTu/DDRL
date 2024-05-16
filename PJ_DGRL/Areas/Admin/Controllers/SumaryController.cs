using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index(int? departmentId,int semesterId)
        {
            var sum = _context.SumaryOfPoints.Include(x => x.Class).ThenInclude(x => x.Department).Where(x => x.Class.DepartmentId == departmentId);
            ViewBag.Class = _context.Classes.Include(x => x.Department).Include(x => x.Students).ThenInclude(x => x.SumaryOfPoints).Where(x => x.DepartmentId == departmentId).ToList();
            ViewData["Semester"] = _context.Semesters.OrderByDescending(x => x.Id).Where(x => x.IsActive == 1).ToList();
            ViewBag.SemesterId = semesterId;
            return View(sum);
        }
    }
}
