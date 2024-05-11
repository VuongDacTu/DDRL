using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.Controllers
{
    public class HistoryController : BaseController
    {
        private readonly DbDgrlContext _context;
        public HistoryController(DbDgrlContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> History()
        {
            var semesters = await _context.Semesters.ToListAsync();
            return View(semesters);
        }
        public IActionResult Self(int? semesterId, string? studentId)
        {
            var ss = HttpContext.Session.GetString("StudentLogin");
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ToList();
            var semester = _context.Semesters.OrderBy(x => x.Id).Where(x => x.IsActive == 1);
            if (ss != null){
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
            return RedirectToAction("Index","Login");
        }
        public IActionResult Class(int? semesterId,string? studentId)
        {
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ToList();
            var semester = _context.Semesters.OrderBy(x => x.Id).Where(x => x.IsActive == 1);
            //
            if (semesterId == null)
            {
                semesterId = semester.FirstOrDefault()?.Id;
            }else
            {
                GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ToList();

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
            return View(GroupQuestion);
        }
    }
}
