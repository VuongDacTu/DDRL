using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.ViewComponents
{
    public class ReportViewComponent : ViewComponent
    {
        private readonly DbDgrlContext _context;
        public ReportViewComponent(DbDgrlContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(bool isDelete)
        {
            ViewBag.IsDelete = isDelete;
            var department = await _context.Departments.ToListAsync();
            return View(department);
        }
    }
}
