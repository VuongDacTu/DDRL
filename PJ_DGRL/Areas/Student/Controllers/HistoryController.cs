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
            var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
            if (semesterId == null)
            {
                semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            }
            if (studentId == null)
            {
                studentId = student.StudentId;
            }
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.SelfAnswers.Where(x => x.StudentId == studentId)).ToList();
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint ?? 0;
            ViewData["Semester"] = _context.Semesters.ToList();

            return View(GroupQuestion);
        }
       
        public IActionResult Class(int? semesterId,string? studentId)
        {
            if(semesterId == null)
            {
                semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            }
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.ClassAnswers.Where(x => x.StudentId == studentId)).ToList();
            ViewBag.StudentId = studentId;
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint ?? 0;
            ViewBag.SumClassPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.ClassPoint ?? 0;

            ViewData["Semester"] = _context.Semesters.ToList();
            return View(GroupQuestion);
        }
    }
}
