using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class SemestersController : BaseController
    {
        private readonly DbDgrlContext _context;

        public SemestersController(DbDgrlContext context)
        {
            _context = context;
        }

        // GET: Admin/Semesters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Semesters.Where(x => x.IsActive == 1).ToListAsync());
        }

        // GET: Admin/Semesters/Create
        public IActionResult Create()
        {
            var semester = _context.Semesters.FirstOrDefault(x => x.DateEndLecturer > DateTime.Now);
            if(semester != null)
            {
                return RedirectToAction("Status");
            }
            ViewBag.Questions = _context.QuestionLists.Include(x => x.AnswerLists.Where(x => x.Status == 1)).Where(x => x.Status == 1).ToList();
            ViewBag.SchoolYear = (DateTime.Now.Year - 1).ToString() + " - " + (DateTime.Now.Year).ToString();
            return View();
        }

        // POST: Admin/Semesters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SchoolYear,DateOpenStudent,DateEndStudent,DateEndClass,DateEndLecturer,IsActive")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                semester.IsActive = 1;
                _context.Add(semester);
                await _context.SaveChangesAsync();
                var Questions = _context.QuestionLists.Include(x => x.AnswerLists.Where(x => x.Status == 1)).Where(x => x.Status == 1).ToList();
                int a = 1;
                foreach (var item in Questions)
                {
                    QuestionHisory questionHisory = new QuestionHisory()
                    {
                        QuestionId = item.Id,
                        SemesterId = semester.Id,
                        OrderBy = a++,
                        CreateBy = admin.UserName,
                        CreateDate = DateTime.Now,

                    };
                    _context.QuestionHisories.Add(questionHisory);

                    item.IsEdit = true;
                    foreach (var item2 in item.AnswerLists)
                    {
                        item2.IsEdit = true;
                    }
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(semester);
        }

        // GET: Admin/Semesters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters.FindAsync(id);
            if (semester == null)
            {
                return NotFound();
            }
            return View(semester);
        }

        // POST: Admin/Semesters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SchoolYear,DateOpenStudent,DateEndStudent,DateEndClass,DateEndLecturer,IsActive")] Semester semester)
        {
            if (id != semester.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var s = _context.Semesters.Where(x => x.Id != id).FirstOrDefault(x => x.DateEndLecturer > DateTime.Now);
                    if (s != null)
                    {
                        return RedirectToAction("Status");
                    }
                    _context.Update(semester);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemesterExists(semester.Id))
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
            return View(semester);
        }
        private bool SemesterExists(int id)
        {
            return _context.Semesters.Any(e => e.Id == id);
        }
        public IActionResult Status()
        {
            var semester = _context.Semesters.FirstOrDefault(x => x.DateEndLecturer > DateTime.Now);
            return View(semester);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semester.DateEndLecturer > DateTime.Now)
            {
                RedirectToAction("Status");
            }
            if (semester == null)
            {
                return NotFound();
            }

            return View(semester);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var semester = await _context.Semesters.FindAsync(id);
            if (semester != null)
            {
                semester.IsActive = 0;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
