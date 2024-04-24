using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PJ_DDRL.Models.DDRLModels;

namespace PJ_DDRL.Areas.Student.Controllers
{
    public class SemesterController : BaseController
    {
        private readonly DdrlContext _ddrlContext;
        public SemesterController(DdrlContext ddrlContext)
        {
            _ddrlContext = ddrlContext;
        }

        public async Task<IActionResult> Index()
        {
            var semesters = await _ddrlContext.Semesters.ToListAsync();
            return View(semesters);
        }

    }
}
