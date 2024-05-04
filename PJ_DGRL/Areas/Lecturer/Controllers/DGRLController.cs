using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Lecturer.Controllers
{
	public class DGRLController : BaseController
	{
		private readonly DbDgrlContext _context;
		public DGRLController(DbDgrlContext context)
		{
			_context = context;
		}
		public IActionResult Index(string? name, string? studentId)
		{
			if(studentId == null)
			{
				studentId = _context.Students.FirstOrDefault()?.Id;
			}
            int semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.DateEndClass <= DateTime.Now && x.DateEndLecturer >= DateTime.Now)?.Id ?? 0;
            var student = _context.Students.FirstOrDefault(u => u.Id == studentId);
			ViewBag.StudentId = studentId;
			ViewBag.semesterId = semesterId;
			ViewBag.SelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId).FirstOrDefault(u => u.SemesterId == semesterId)?.SelfPoint??0;
            ViewBag.ClassPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId).FirstOrDefault(u => u.SemesterId == semesterId)?.ClassPoint??0;
			ViewBag.Name = name;
            return View(student);
		}
		[HttpPost]
		public IActionResult Submit(string? studentId, int lecturerPoint)
		{
			if (ModelState.IsValid)
			{
                var lecturer = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("LecturerLogin"));

                int semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.DateEndClass <= DateTime.Now && x.DateEndLecturer >= DateTime.Now)?.Id ?? 0;
                var point = _context.SumaryOfPoints.Where(x => x.SemesterId == semesterId).FirstOrDefault(x => x.StudentId == studentId);
				if(point != null)
				{
					point.UserLecturer = lecturer.UserName;
					point.LecturerPoint = lecturerPoint;
					point.UpdateDate = DateTime.Now;
					int avg = (int)point.LecturerPoint;
                    if (avg >= 90)
					{
						point.Classify = "Xuất sắc";
					}else if(avg >= 80)
					{
                        point.Classify = "Tốt";
                    }else if (avg >= 70)
					{
						point.Classify = "Khá";
					}else if (avg >= 60)
                    {
                        point.Classify = "Trung bình khá";
                    }else if(avg >= 50)
					{
                        point.Classify = "Trung bình";
					}
					else
					{
                        point.Classify = "Trượt";
                    }
                }

				_context.SaveChanges();
                return RedirectToAction("Status");
            }
            return RedirectToAction("Index");

        }
        public IActionResult Status()
		{
			return View();
		}
	}
}
