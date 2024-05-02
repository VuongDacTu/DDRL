using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.Controllers
{
    public class ResultDGRLController : BaseController
    {
        private readonly DbDgrlContext _context;
        public ResultDGRLController(DbDgrlContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? semesterId)
        {
            var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
            var GroupQuestion = _context.GroupQuestions.Include(x => x.QuestionLists).ThenInclude(x => x.AnswerLists).ThenInclude(x => x.SelfAnswers.Where(x => x.StudentId == student.StudentId)).ToList();
            ViewBag.SumSelfPoint = _context.SumaryOfPoints.Where(u => u.StudentId == student.UserName && u.SemesterId == semesterId).FirstOrDefault().SelfPoint;
            return View(GroupQuestion);
        }
    }
}
