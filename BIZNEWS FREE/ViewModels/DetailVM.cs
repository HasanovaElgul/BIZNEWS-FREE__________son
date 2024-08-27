using BIZNEWS_FREE.Models;

namespace BIZNEWS_FREE.ViewModels
{
    public class DetailVM
    {
        public Article Article { get; set; }
        public List<Article> Articles { get; set; }
        public List<Article> FeaturedArticles { get; set; }


    }
}
