using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Lecturer.ViewComponents
{
    public class InforStudentsViewComponent : ViewComponent
    {
        private readonly DbDgrlContext _context;
        public InforStudentsViewComponent(DbDgrlContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? id)
        {
            var student = _context.Students.Include(x => x.SumaryOfPoints).FirstOrDefault();
            if (!id.IsNullOrEmpty())
            {
                student = _context.Students.Include(x => x.SumaryOfPoints).FirstOrDefault(x => x.Id == id);
            }
            return View(student);
        }

    }
}
