using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Lecturer.Controllers
{
    public class ResultController : BaseController
    {
        private readonly DbDgrlContext _context;
        public ResultController(DbDgrlContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? semesterId, string? studentId)
        {
            var ss = HttpContext.Session.GetString("LecturerLogin");
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ToList();
            var semester = _context.Semesters.OrderBy(x => x.Id).Where(x => x.IsActive == 1);
            if (ss != null)
            {
                var student = JsonConvert.DeserializeObject<AccountStudent>(ss);
                //
                if (semesterId == null)
                {
                    semesterId = semester.FirstOrDefault()?.Id;
                }
                if (studentId == null)
                {
                    studentId = student.StudentId;
                }
                // set lại checked cho Answer
                var answers = _context.AnswerLists.ToList();
                ViewBag.semesterId = semesterId;
                var selfAnswers = _context.SelfAnswers.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).ToList();
                foreach (var item in answers)
                {
                    item.Checked = 0;
                }
                foreach (var item in selfAnswers)
                {
                    _context.AnswerLists.Where(u => u.Id == item.AnswerId).FirstOrDefault().Checked = 1;

                }

                // điểm tự đánh giá, nếu chưa đánh giá điểm = 0
                ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint ?? 0;
                ViewData["Semester"] = semester.ToList();
                ViewBag.Id = studentId;
                ViewBag.SemesterId = semesterId;
                return View(GroupQuestion);
            }
            return RedirectToAction("Index", "Login");
        }
        public IActionResult Class(int? semesterId, string? studentId)
        {
            var semester = _context.Semesters.OrderBy(x => x.Id).Where(x => x.IsActive == 1);
            //
            if (semesterId == null)
            {
                semesterId = semester.FirstOrDefault()?.Id;
            }
            // set lại checked cho Answer
            var answers = _context.AnswerLists.ToList();
            ViewBag.semesterId = semesterId;
            var classAnwer = _context.ClassAnswers.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).ToList();
            foreach (var item in answers)
            {
                item.Checked = 0;
            }
            foreach (var item in classAnwer)
            {
                _context.AnswerLists.Where(u => u.Id == item.AnswerId).FirstOrDefault().Checked = 1;

            }
            ViewBag.Id = studentId;
            ViewBag.SemesterId = semesterId;
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint ?? 0;
            ViewBag.SumClassPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.ClassPoint ?? 0;
            ViewData["Semester"] = semester.ToList();
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ToList();
            return View(GroupQuestion);
        }
        public IActionResult Lecturer(int? semesterId, string? studentId)
        {
            if (studentId == null)
            {
                return RedirectToAction("Status");
            }

            if (semesterId == null)
            {
                semesterId = _context.Semesters.OrderBy(x => x.Id).FirstOrDefault(x => x.IsActive == 1)?.Id;
            }
            var semester = _context.Semesters.Include(u => u.SumaryOfPoints).Where(x => x.Id == semesterId).Where(x => x.IsActive == 1).ToList();
            ViewBag.StudentId = studentId;
            ViewBag.SemesterId = semesterId;
            ViewData["Semester"] = _context.Semesters.Where(x => x.IsActive == 1).ToList();
            return View(semester);
        }
        public IActionResult Status() {  return View(); }
    }
}
