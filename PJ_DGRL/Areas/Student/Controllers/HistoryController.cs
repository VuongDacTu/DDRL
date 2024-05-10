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
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.SelfAnswers).ToList();

            if (ss != null){
                var student = JsonConvert.DeserializeObject<AccountStudent>(ss);
            
                if (semesterId == null)
                {
                    semesterId = _context.Semesters.OrderBy(x => x.Id).FirstOrDefault(x => x.IsActive == 1).Id;
                }
                else
                {
                    GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.SelfAnswers).ToList();
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

                ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint ?? 0;
                ViewData["Semester"] = _context.Semesters.Where(x => x.IsActive == 1).ToList();
                ViewBag.Id = studentId;
                ViewBag.SemesterId = semesterId;
                return View(GroupQuestion);
            }
            return RedirectToAction("Index","Login");
        }
        public IActionResult Class(int? semesterId,string? studentId)
        {
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.ClassAnswers).ToList();

            if (semesterId == null)
            {
                semesterId = _context.Semesters.OrderBy(x => x.Id).FirstOrDefault(x => x.IsActive == 1).Id;
            }else
            {
                GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.ClassAnswers).ToList();

            }
            ViewBag.Id = studentId;
            ViewBag.SemesterId = semesterId;
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint ?? 0;
            ViewBag.SumClassPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.ClassPoint ?? 0;
            ViewData["Semester"] = _context.Semesters.Where(x => x.IsActive == 1).ToList();
            return View(GroupQuestion);
        }
    }
}
