using BIZNEWS_FREE.Data;
using BIZNEWS_FREE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIZNEWS_FREE.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;

        public ArticleController(AppDbContext context)
        {
            _context = context;
        }
        // ne volnuysya net problem 
        // ne mnojka da ostalas
        // da 
        public IActionResult Detail(int id)
        {
            var article = _context.Articles
                .Include(x => x.Category)
                .Include(x => x.ArticleTags)
                .ThenInclude(x => x.Tag) // Предполагается, что Tag является свойством в ArticleTags                       ChatGPT))))
                .FirstOrDefault(x => x.Id == id);

            var articles = _context.Articles
                .Include(x => x.Category)
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.UpdatedDate)
                .ToList();
            var featuredArticles = _context.Articles
                .Include(x => x.Category)
                .Where(x => x.IsActive == true && x.IsFeature == false)
                 .OrderByDescending(x => x.ViewCount)
                .Take(7).ToList();
            DetailVM detailVM = new()
            {
                Article = article,
                Articles = articles,
                FeaturedArticles = featuredArticles
            };


            return View(detailVM);

        }

    }
}
