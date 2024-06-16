using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class GroupQuestionsController : Base1Controller
    {
        private readonly DbDgrlContext _context;

        public GroupQuestionsController(DbDgrlContext context)
        {
            _context = context;
        }

        // GET: Admin/GroupQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.GroupQuestions.ToListAsync());
        }

        // GET: Admin/GroupQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupQuestion = await _context.GroupQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupQuestion == null)
            {
                return NotFound();
            }

            return View(groupQuestion);
        }

        // GET: Admin/GroupQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/GroupQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] GroupQuestion groupQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupQuestion);
        }

        // GET: Admin/GroupQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupQuestion = await _context.GroupQuestions.FindAsync(id);
            if (groupQuestion == null)
            {
                return NotFound();
            }
            return View(groupQuestion);
        }

        // POST: Admin/GroupQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] GroupQuestion groupQuestion)
        {
            if (id != groupQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupQuestionExists(groupQuestion.Id))
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
            return View(groupQuestion);
        }

        // GET: Admin/GroupQuestions/Delete/5
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupQuestion = await _context.GroupQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupQuestion == null)
            {
                return NotFound();
            }

            return View(groupQuestion);
        }

        // POST: Admin/GroupQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupQuestion = await _context.GroupQuestions.FindAsync(id);
            if (groupQuestion != null)
            {
                _context.GroupQuestions.Remove(groupQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupQuestionExists(int id)
        {
            return _context.GroupQuestions.Any(e => e.Id == id);
        }
    }
}
