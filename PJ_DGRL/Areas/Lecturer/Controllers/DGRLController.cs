using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
		public IActionResult Index(string? name,int? classId)
        {
			// lấy ra kì đánh giá của giảng viên đang diễn ra
            var semester = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.DateEndClass < DateTime.Now && x.DateEndLecturer >= DateTime.Now);
			if(semester == null)
			{
                return RedirectToAction("Status");
			}
			// giảng viên trong khoa ...
            var lecturer = JsonConvert.DeserializeObject<AccountLecturer>(HttpContext.Session.GetString("LecturerLogin"));
            var lecturers = _context.Lecturers.Where(x => x.Id == lecturer.LecturerId);
			// lấy ra sinh viên của khoa
			var students = _context.Students.OrderByDescending(x => x.SumaryOfPoints.FirstOrDefault().SelfPoint).Include(x => x.SumaryOfPoints.Where(x => x.SemesterId == semester.Id)).Where(x => x.Class.DepartmentId == lecturers.FirstOrDefault().DepartmentId);
			if(classId != null)
			{
                students = _context.Students.OrderByDescending(x => x.SumaryOfPoints.FirstOrDefault().SelfPoint).Include(x => x.SumaryOfPoints.Where(x => x.SemesterId == semester.Id)).Where(x => x.ClassId == classId);
                if (!name.IsNullOrEmpty())
                {
                    students = _context.Students.OrderByDescending(x => x.SumaryOfPoints.FirstOrDefault().SelfPoint).Include(x => x.SumaryOfPoints.Where(x => x.SemesterId == semester.Id)).Where(x => x.Class.DepartmentId == classId && (x.FullName.Contains(name) || x.Id.Contains(name)));

                }

            }
            else if (!name.IsNullOrEmpty())
			{
                students = _context.Students.OrderByDescending(x => x.SumaryOfPoints.FirstOrDefault().SelfPoint).Include(x => x.SumaryOfPoints.Where(x => x.SemesterId == semester.Id)).Where(x => x.Class.DepartmentId == lecturers.FirstOrDefault().DepartmentId && (x.FullName.Contains(name) || x.Id.Contains(name)));

            }

            ViewData["Class"] = new SelectList(_context.Classes.Where(x => x.DepartmentId == lecturers.FirstOrDefault().DepartmentId), "Id", "Name");
            ViewBag.Name = name;
            return View(students);
		}
		[HttpPost]
		public IActionResult Submit(List<SumaryOfPoint> sum)
		{
			if (ModelState.IsValid)
			{
                var lecturer = JsonConvert.DeserializeObject<AccountLecturer>(HttpContext.Session.GetString("LecturerLogin"));
                int semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.DateEndClass < DateTime.Now && x.DateEndLecturer >= DateTime.Now)?.Id ?? 0;
                foreach (var s in sum) { 
					var point = _context.SumaryOfPoints.Where(x => x.SemesterId == semesterId).FirstOrDefault(x => x.StudentId == s.StudentId);
					if(point != null)
					{
						point.UserLecturer = lecturer.UserName;
						point.LecturerPoint = s.LecturerPoint;
						point.LastPoint = s.LastPoint;
						point.UpdateDate = DateTime.Now;

						point.LastPoint = (int)point.LecturerPoint;
						if (point.LastPoint >= 90)
						{
							point.Classify = "Xuất sắc";
						}else if(point.LastPoint >= 80)
						{
							point.Classify = "Tốt";
						}else if (point.LastPoint >= 70)
						{
							point.Classify = "Khá";
						}else if (point.LastPoint >= 60)
						{
							point.Classify = "Trung bình khá";
						}else if(point.LastPoint >= 50)
						{
							point.Classify = "Trung bình";
						}
						else
						{
							point.Classify = "Trượt";
						}
					}
					_context.SaveChanges();
					
				}
                return RedirectToAction("Status");
            }
            return RedirectToAction("Index");

        }
        public IActionResult Status(int? id)
		{
            // lấy ra kì đánh giá của giảng viên đang diễn ra
            var semester = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault(x => x.DateEndClass < DateTime.Now && x.DateEndLecturer >= DateTime.Now);
            if (semester == null)
            {
                ViewBag.Status = "Kì đánh giá của giảng viên chưa diễn ra !!!";
			}
			else
			{
                ViewBag.Status = "Đã lưu !!!";
            }
            return View();
		}
	}
}
