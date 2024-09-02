using BIZNEWS_FREE.Data;
using BIZNEWS_FREE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIZNEWS_FREE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;


        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var featuredArticles = _context.Articles
                .Include(x => x.Category)
                .Where(x => x.IsActive == true && x.IsFeature == false)
                 .OrderByDescending(x => x.ViewCount)
                .Take(7).ToList();

            var articles = _context.Articles
                .Include(x => x.Category)
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.UpdatedDate)
                .ToList();

            var popular = _context.Articles                          //saytda sağ tərəfdə yeni blok yaradmaq ucun bit=rinci burda bunu yazırıq
                .OrderByDescending(x => x.ViewCount).Take(3).ToList();


            var categories = _context.Categories.ToList();



            HomeVM homeVM = new()
            {
                FeaturedArticles = featuredArticles,
                Articles = articles,
                PopularArticles = popular,
                Categories = categories
            };
            return View(homeVM);
        }

        public IActionResult Search(string query)
        {
            var results = _context.Articles
                .Where(a => a.Title.Contains(query) || a.Content.Contains(query))
                .ToList();
            return View(results); // Передаем List<Article> в представление
        }




    }
}
