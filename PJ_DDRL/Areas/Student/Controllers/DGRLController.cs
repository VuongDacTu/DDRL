using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DDRL.Areas.Student.Models;
using PJ_DDRL.Models.DDRLModels;


namespace PJ_DDRL.Areas.Student.Controllers
{
    public class DGRLController : BaseController
    {
        private readonly DdrlContext _context;
        public DGRLController(DdrlContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int semesterId)
        {
            if (semesterId == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.Where(x => x.SemesterId == semesterId).Include(u => u.Answers).ToListAsync();
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Dictionary<int, int> AnswerIds, Dictionary<int, int> AnswerId)
        {
            if (ModelState.IsValid)
            {

                //var answerIds = form["AnswerIds"].ToArray();
                var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));
                // Tạo danh sách SelfAnswer từ AnswerId và StudentId
                foreach (var item in AnswerIds)
                {
                    var selfAnswer = new SelfAnswer
                    {
                        StudentId = student.UserName,
                        AnswerId = item.Value,
                    };
                    _context.SelfAnswers.Add(selfAnswer);
                }
                foreach (var item in AnswerId)
                {
                    var selfAnswer = new SelfAnswer
                    {
                        StudentId = student.UserName,
                        AnswerId = item.Value,
                    };
                    _context.SelfAnswers.Add(selfAnswer);
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index");

        }
    }
}
