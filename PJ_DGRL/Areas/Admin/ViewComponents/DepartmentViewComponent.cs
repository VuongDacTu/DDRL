using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.ViewComponents
{
    public class DepartmentViewComponent : ViewComponent
    {
        private readonly DbDgrlContext _context;
        public DepartmentViewComponent(DbDgrlContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var department = await _context.Departments.ToListAsync();
            return View(department);
        }
    }
}
