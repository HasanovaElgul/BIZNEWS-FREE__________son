namespace BIZNEWS_FREE.Models
{
    public class Search
    {
        public List<Article> Articles { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }

    }
}
