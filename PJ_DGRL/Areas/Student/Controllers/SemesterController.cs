using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Student.Controllers
{
    public class SemesterController : BaseController
    {
        private readonly DbDgrlContext _ddrlContext;
        public SemesterController(DbDgrlContext ddrlContext)
        {
            _ddrlContext = ddrlContext;
        }
        public async Task<IActionResult> History()
        {
            var semesters = await _ddrlContext.Semesters.ToListAsync();
            return View(semesters);
        }
    }
}