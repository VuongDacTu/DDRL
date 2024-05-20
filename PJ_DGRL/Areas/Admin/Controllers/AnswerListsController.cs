using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DGRL.Areas.Admin.Models;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class AnswerListsController : BaseController
    {
        private readonly DbDgrlContext _context;

        public AnswerListsController(DbDgrlContext context)
        {
            _context = context;
        }

        public IsActive isActive = new IsActive();
        // GET: Admin/AnswerLists
        public async Task<IActionResult> Index(int? questionId)
        {
            var dbDgrlContext = _context.AnswerLists.Include(a => a.Question).Where(x => x.QuestionId == questionId);
            ViewBag.QID = questionId;
            return View(await dbDgrlContext.ToListAsync());
        }
        // GET: Admin/AnswerLists/Details/5
        public async Task<IActionResult> Details(int? id, int? questionId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerList = await _context.AnswerLists
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.QID = questionId;
            if (answerList == null)
            {
                return NotFound();
            }

            return View(answerList);
        }

        // GET: Admin/AnswerLists/Create
        public IActionResult Create(int? questionId)
        {
            ViewBag.QID = questionId;
            ViewData["QuestionId"] = new SelectList(_context.QuestionLists, "Id", "ContentQuestion");
            return View();
        }

        // POST: Admin/AnswerLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionId,ContentAnswer,AnswerScore,Status,IsEdit,CreateDate,UpdateDate,UpdateBy,Checked")] AnswerList answerList)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                answerList.CreateDate = DateTime.Now;
                answerList.QuestionId = answerList.QuestionId;
                answerList.UpdateBy = admin.UserName;
                answerList.Checked = 0;
                answerList.IsEdit = false;
                _context.AnswerLists.Add(answerList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { questionId = answerList.QuestionId });
            }
            ViewData["QuestionId"] = new SelectList(_context.QuestionLists, "Id", "ContentQuestion", answerList.QuestionId);
            return RedirectToAction("Index","AnswerLists", new {questionId = answerList.QuestionId });
        }

        // GET: Admin/AnswerLists/Edit/5
        public async Task<IActionResult> Edit(int? id, int? questionId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerList = await _context.AnswerLists.FindAsync(id);
            ViewBag.QID = questionId;
            if (answerList == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.QuestionLists, "Id", "ContentQuestion", answerList.QuestionId);
            return View(answerList);
        }

        // POST: Admin/AnswerLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionId,ContentAnswer,AnswerScore,Status,IsEdit,CreateDate,UpdateDate,UpdateBy,Checked")] AnswerList answerList, int questionId)
        {
            if (id != answerList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                    answerList.UpdateDate = DateTime.Now;
                    answerList.UpdateBy = admin.UserName;
                    _context.Update(answerList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerListExists(answerList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new {questionId = questionId });
            }
            ViewData["QuestionId"] = new SelectList(_context.QuestionLists, "Id", "ContentQuestion", answerList.QuestionId);
            return View(answerList);
        }

        // GET: Admin/AnswerLists/Delete/5
        public async Task<IActionResult> Delete(int? id,int? questionId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerList = await _context.AnswerLists
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.QID = questionId;
            if (answerList == null)
            {
                return NotFound();
            }

            return View(answerList);
        }

        // POST: Admin/AnswerLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int questionId)
        {
            var answerList = await _context.AnswerLists.FindAsync(id);
            if (answerList != null)
            {
                _context.AnswerLists.Remove(answerList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {questionId = questionId});
        }

        private bool AnswerListExists(int id)
        {
            return _context.AnswerLists.Any(e => e.Id == id);
        }
        public IActionResult Set(int? answerId,int? questionId)
        {
            _context.AnswerLists.FirstOrDefault(x => x.Id == answerId).Status = isActive.HoatDong();
            ViewBag.QID = questionId;
            _context.SaveChanges();
            return RedirectToAction("Index", new {questionId = questionId});
        }
        public IActionResult UnSet(int? answerId, int? questionId)
        {
            _context.AnswerLists.FirstOrDefault(x => x.Id == answerId).Status = isActive.NgungHoatDong();
            ViewBag.QID = questionId;
            _context.SaveChanges();
            return RedirectToAction("Index", new { questionId = questionId });
        }
    }
}
