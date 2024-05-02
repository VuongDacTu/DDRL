using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Lecturer.Controllers
{
	public class StudentController : BaseController
	{
		private readonly DbDgrlContext _context;
		public StudentController(DbDgrlContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var data = JsonConvert.DeserializeObject<AccountLecturer>(HttpContext.Session.GetString("LecturerLogin"));
			var departmentId = _context.Lecturers.FirstOrDefault(x => x.Id == data.LecturerId).DepartmentId.Value;
			var Class = _context.Classes.FirstOrDefault();
			var students = _context.Students.Where(u => u.Class.DepartmentId == departmentId && u.IsActive == 1).ToList();
			return View(students);
		}
	}
}
