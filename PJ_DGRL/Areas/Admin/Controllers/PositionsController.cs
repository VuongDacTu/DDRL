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
    public class PositionsController : BaseController
    {
        private readonly DbDgrlContext _context;

        public PositionsController(DbDgrlContext context)
        {
            _context = context;
        }

        // GET: Admin/Positions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Positions.ToListAsync());
        }

        // GET: Admin/Positions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Positions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Position position)
        {
            if (ModelState.IsValid)
            {
                _context.Add(position);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        // GET: Admin/Positions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = _context.Students.FirstOrDefault(x => x.PositionId == id);
            var lecturer = _context.Lecturers.FirstOrDefault(x => x.PositionId == id);
            if(student != null && lecturer != null)
            {
                RedirectToAction("Status");
            }
            var position = await _context.Positions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // POST: Admin/Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position != null)
            {
                _context.Positions.Remove(position);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(string id)
        {
            return _context.Positions.Any(e => e.Id == id);
        }
        public IActionResult Status() { return View(); }
    }
}
