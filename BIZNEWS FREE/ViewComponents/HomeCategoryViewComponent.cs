using BIZNEWS_FREE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIZNEWS_FREE.ViewComponents
{
    public class HomeCategoryViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HomeCategoryViewComponent(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()


        {

            var categories = await _context.Categories.Include(x => x.Article).ToListAsync();
            return View("HomeCategory", categories);

        }
    }
}
