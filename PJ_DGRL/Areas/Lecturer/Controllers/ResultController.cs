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
            if(semesterId == null) { 
                semesterId = semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault().Id;        
            }
            var groupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.SelfAnswers.Where(x => x.StudentId == studentId)).ToList();
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint??0;
            ViewData["Semester"] = _context.Semesters.ToList();
            return View(groupQuestion);
        }
        public IActionResult Class(int? semesterId, string? studentId)
        {
            if(semesterId == null) { 
                semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            }
            var groupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.ClassAnswers.Where(x => x.StudentId == studentId)).ToList();
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.SelfPoint ?? 0;
            ViewBag.SumClassPoint = _context.SumaryOfPoints.Where(u => u.StudentId == studentId && u.SemesterId == semesterId).FirstOrDefault()?.ClassPoint ?? 0;
            ViewBag.StudentId = studentId;
            ViewData["Semester"] = _context.Semesters.ToList();
            return View(groupQuestion);
        }
        public IActionResult Lecturer(int? semesterId, string? studentId)
        {
            if (studentId == null)
            {
                return RedirectToAction("Status");
            }

            if (semesterId == null)
            {
                semesterId = _context.Semesters.OrderByDescending(x => x.Id).FirstOrDefault()?.Id;
            }
            var semester = _context.Semesters.Include(u => u.SumaryOfPoints.Where(x => x.StudentId == studentId)).Where(x => x.Id == semesterId).ToList();
            ViewBag.StudentId = studentId;
            ViewData["Semester"] = _context.Semesters.ToList();
            return View(semester);
        }
        public IActionResult Status() {  return View(); }
    }
}
