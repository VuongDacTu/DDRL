using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.ViewComponents
{
    public class STViewComponent : ViewComponent
    {
        private readonly DbDgrlContext _context;
        public STViewComponent(DbDgrlContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? name)
        {
            var data = JsonConvert.DeserializeObject<AccountLecturer>(HttpContext.Session.GetString("LecturerLogin"));
            var departmentId = _context.Lecturers.FirstOrDefault(x => x.Id == data.LecturerId).DepartmentId.Value;
            var Class = _context.Classes.FirstOrDefault();
            int semesterId = _context.Semesters.FirstOrDefault(x => x.DateEndLecturer >= DateTime.Now)?.Id ?? 0;
            var students = _context.Students.Include(x => x.SumaryOfPoints.Where(x => x.SemesterId == semesterId)).Where(u => u.Class.DepartmentId == departmentId && u.IsActive == 1).ToList();
            if (!name.IsNullOrEmpty())
            {
                students = _context.Students.Where(u => u.Class.DepartmentId == departmentId && u.IsActive == 1 && u.FullName.Contains(name)).ToList();
            }
            return View(students);
        }
    }
}
