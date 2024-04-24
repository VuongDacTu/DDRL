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
        public async Task<IActionResult> Index(IFormCollection form)
        {
            if (ModelState.IsValid)
            {

                var answerIds = form["AnswerIds"].ToArray();
                var student = JsonConvert.DeserializeObject<AccountStudent>(HttpContext.Session.GetString("StudentLogin"));

                foreach (var answerId in answerIds)
                {
                    // Tạo một đối tượng SelfAnswer mới
                    var selfAnswer = new SelfAnswer
                        {
                            StudentId = student.UserName,
                            AnswerId = int.Parse(answerId),
                        };

                        _context.SelfAnswers.Add(selfAnswer);
                }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
    }
}
