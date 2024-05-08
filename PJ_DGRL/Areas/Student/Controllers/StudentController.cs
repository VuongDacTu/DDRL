using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.Controllers
{
    public class StudentController : BaseController
    {
        private readonly DbDgrlContext _context;
        public StudentController(DbDgrlContext context)
        {
            _context = context;
        }
        public IActionResult Index(string? name)
        {
            var student = HttpContext.Session.GetString("LTLogin");
            var LTLogin = _context.Students.FirstOrDefault(x => x.Id == student);
            int semesterId = _context.Semesters.FirstOrDefault(x => x.DateEndClass >= DateTime.Now)?.Id ?? 0;
            var students = _context.Students.Include(x => x.SumaryOfPoints.Where(x => x.SemesterId == semesterId)).Where(u => u.ClassId == LTLogin.ClassId).ToList();
            if (!name.IsNullOrEmpty())
            {
                students = _context.Students.Include(x => x.SumaryOfPoints.Where(x => x.SemesterId == semesterId)).Where(u => u.ClassId == LTLogin.ClassId && (u.FullName.Contains(name) || u.Id.Contains(name))).ToList();
            }
            ViewBag.Name = name;
            return View(students);
        }
    }
}
