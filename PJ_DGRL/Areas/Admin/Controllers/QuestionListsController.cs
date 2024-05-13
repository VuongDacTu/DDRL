using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class QuestionListsController : BaseController
    {
        private readonly DbDgrlContext _context;

        public QuestionListsController(DbDgrlContext context)
        {
            _context = context;
        }

        // GET: Admin/QuestionLists
        public async Task<IActionResult> Index()
        {
            var dbDgrlContext = _context.QuestionLists.Include(q => q.GroupQuestion).Include(q => q.TypeQuestion);
            ViewBag.Semester = _context.Semesters.Where(x => x.IsActive == 1).ToList();
            return View(await dbDgrlContext.ToListAsync());
        }
        // GET: Admin/QuestionLists
        public async Task<IActionResult> List()
        {
            ViewBag.Semester = _context.Semesters.Where(x => x.IsActive == 1).ToList();
            var dbDgrlContext = _context.GroupQuestions.Include(q => q.QuestionLists).ThenInclude(q => q.AnswerLists);
            return View(await dbDgrlContext.ToListAsync());
        }

        // GET: Admin/QuestionLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionList = await _context.QuestionLists
                .Include(q => q.GroupQuestion)
                .Include(q => q.TypeQuestion).Include(q => q.AnswerLists)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionList == null)
            {
                return NotFound();
            }

            return View(questionList);
        }

        // GET: Admin/QuestionLists/Create
        public IActionResult Create()
        {
            ViewData["GroupQuestionId"] = new SelectList(_context.GroupQuestions, "Id", "Name");
            ViewData["TypeQuestionId"] = new SelectList(_context.TypeQuestions, "Id", "Name");
            return View();
        }

        // POST: Admin/QuestionLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContentQuestion,TypeQuestionId,GroupQuestionId,OrderBy,Status,CreateDate,UpdateDate,UpdateBy,IsEdit")] QuestionList questionList)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                int i = (int)_context.QuestionLists.OrderByDescending(x => x.OrderBy).FirstOrDefault().OrderBy;
                questionList.OrderBy = i + 1;
                questionList.CreateDate = DateTime.Now;
                questionList.UpdateBy = admin.UserName;
                _context.Add(questionList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupQuestionId"] = new SelectList(_context.GroupQuestions, "Id", "Name", questionList.GroupQuestionId);
            ViewData["TypeQuestionId"] = new SelectList(_context.TypeQuestions, "Id", "Name", questionList.TypeQuestionId);
            return View(questionList);
        }

        // GET: Admin/QuestionLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionList = await _context.QuestionLists.FindAsync(id);
            if (questionList == null)
            {
                return NotFound();
            }
            ViewData["GroupQuestionId"] = new SelectList(_context.GroupQuestions, "Id", "Name", questionList.GroupQuestionId);
            ViewData["TypeQuestionId"] = new SelectList(_context.TypeQuestions, "Id", "Name", questionList.TypeQuestionId);
            return View(questionList);
        }

        // POST: Admin/QuestionLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContentQuestion,TypeQuestionId,GroupQuestionId,OrderBy,Status,CreateDate,UpdateDate,UpdateBy,IsEdit")] QuestionList questionList)
        {
            if (id != questionList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                    int i = (int)_context.QuestionLists.OrderByDescending(x => x.OrderBy).FirstOrDefault().OrderBy;
                    questionList.OrderBy = i + 1;
                    questionList.UpdateDate = DateTime.Now;
                    questionList.UpdateBy = admin.UserName;
                    _context.Update(questionList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionListExists(questionList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupQuestionId"] = new SelectList(_context.GroupQuestions, "Id", "Name", questionList.GroupQuestionId);
            ViewData["TypeQuestionId"] = new SelectList(_context.TypeQuestions, "Id", "Name", questionList.TypeQuestionId);
            return View(questionList);
        }

        // GET: Admin/QuestionLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionList = await _context.QuestionLists
                .Include(q => q.GroupQuestion)
                .Include(q => q.TypeQuestion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionList == null)
            {
                return NotFound();
            }

            return View(questionList);
        }

        // POST: Admin/QuestionLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionList = await _context.QuestionLists.FindAsync(id);
            if (questionList != null)
            {
                var answerInQuestion = _context.AnswerLists.Where(q => q.QuestionId == id).ToList();
                foreach(var item in answerInQuestion)
                {
                    _context.AnswerLists.Remove(item);
                }
                _context.QuestionLists.Remove(questionList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionListExists(int id)
        {
            return _context.QuestionLists.Any(e => e.Id == id);
        }
        public IActionResult Set(int? questionId, bool? list)
        {
            _context.QuestionLists.FirstOrDefault(x => x.Id == questionId).Status = 1; 
            _context.SaveChanges();
            if (list == true)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult UnSet(int? questionId, bool? list)
        {
            _context.QuestionLists.FirstOrDefault(x => x.Id == questionId).Status = 0;
            _context.SaveChanges();
            if (list == true)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult SelectAll(int? semesterId, bool? list)
        {
            var questions = _context.QuestionLists.Include(x => x.QuestionHisories).ToList();
            if (semesterId != null)
            {
                foreach (var item in questions)
                {
                    item.Status = 0;
                }
                var questionHistory = _context.QuestionHisories.Include(x => x.Question).Where(x => x.SemesterId == semesterId).ToList();
                foreach(var item in questionHistory)
                {
                   item.Question.Status = 1;

                }
                _context.SaveChanges();
                if (list == true)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            foreach(var item in questions)
            {
                item.Status = 1;
            }
            _context.SaveChanges();
            if (list == true)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
