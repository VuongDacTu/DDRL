using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DDRL.Areas.Student.Models;
using PJ_DDRL.Models.DDRLModels;


namespace PJ_DDRL.Areas.Student.Controllers
{
    public class DDRLController : BaseController
    {
        private readonly DdrlContext _context;
        public DDRLController(DdrlContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var question = await _context.Questions.Include(u => u.Answers).ToListAsync(); 
            return View(question);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Dictionary<int, int> AnswerIds)
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

                _context.SaveChanges();
            }
            return RedirectToAction("Index");

        }
    }
}
