using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var student = HttpContext.Session.GetString("LTLogin");
            var LTLogin = _context.Students.FirstOrDefault(x => x.Id == student);
            var students = _context.Students.Where(u => u.ClassId == LTLogin.ClassId && u.IsActive == 1).ToList();
            return View(students);
        }
    }
}
