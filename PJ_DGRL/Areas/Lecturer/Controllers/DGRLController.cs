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
		public IActionResult Index(string? studentId)
		{
            int semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsActive == 3)?.Id ?? 0;
            var student = _context.Students.FirstOrDefault(u => u.Id == studentId);
			ViewBag.StudentId = studentId;
			ViewBag.semesterId = semesterId;
			return View(student);
		}
		[HttpPost]
		public IActionResult Submit(string? studentId, int LecturerPoint)
		{
			if (ModelState.IsValid)
			{
                var lecturer = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("LecturerLogin"));

                int semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsActive == 3)?.Id ?? 0;
                var point = _context.SumaryOfPoints.Where(x => x.SemesterId == semesterId).FirstOrDefault(x => x.StudentId == studentId);
				if(point != null)
				{
					point.UserLecturer = lecturer.UserName;
					point.LecturerPoint = LecturerPoint;
					point.UpdateDate = DateTime.Now;
                    float avg = (float)((point.SelfPoint + point.ClassPoint * 2 + point.LecturerPoint * 3) / 6);
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
