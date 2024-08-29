using BIZNEWS_FREE.Models;

namespace BIZNEWS_FREE.ViewModels
{
    public class HomeVM
    {
        public List<Article> FeaturedArticles { get; set; }
        public List<Article> Articles { get; set; }
        public List<Category> Categories { get; set; }
        public List<Article> PopularArticles { get; set; }



    }
}
