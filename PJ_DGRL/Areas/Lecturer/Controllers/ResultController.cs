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

        public IActionResult Index(string? studentId)
        {
            int semesterId = _context.Semesters.FirstOrDefault(x => x.IsActive >= 1)?.Id ?? 0;
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.SelfAnswers.Where(x => x.StudentId == studentId)).ToList();
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint??0;
            return View(GroupQuestion);
        }
        public IActionResult Class(string? studentId)
        {
            int semesterId = _context.Semesters.FirstOrDefault(x => x.IsActive >= 1)?.Id ?? 0;
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.ClassAnswers.Where(x => x.StudentId == studentId)).ToList();
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint ?? 0;
            return View(GroupQuestion);
        }
    }
}
