using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.Controllers
{
    public class StudentController : LTBaseController
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
            var students = _context.Students.Where(u => u.ClassId == LTLogin.ClassId && u.IsActive == 1).ToList();
            if (!name.IsNullOrEmpty())
            {
                students = _context.Students.Where(u => u.ClassId == LTLogin.ClassId && u.IsActive == 1 && u.FullName.Contains(name)).ToList();
            }
            int semesterId = _context.Semesters.FirstOrDefault(x => x.IsActive >= 1)?.Id ?? 0;
            ViewBag.Check = _context.SumaryOfPoints.Where(x => x.SemesterId == semesterId).ToList();
            return View(students);
        }
    }
}
