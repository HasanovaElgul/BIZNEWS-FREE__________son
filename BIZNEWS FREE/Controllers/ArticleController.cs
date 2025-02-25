﻿using BIZNEWS_FREE.Data;
using BIZNEWS_FREE.Models;
using BIZNEWS_FREE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BIZNEWS_FREE.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ArticleController(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Detail(int id)
        {
            var article = _context.Articles
                .Include(x => x.Category)
                .Include(x => x.ArticleTags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefault(x => x.Id == id);




            var articles = _context.Articles
                .Include(x => x.Category)
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.UpdatedDate)
                .ToList();

            var currentArticle = articles.IndexOf(article);    //meqale 0 indeksdedir  
            var prev = currentArticle <= 0 ? null : articles[currentArticle - 1];   //1ci indeksde olan meqaleni gorsedecek 
            var next = currentArticle + 1 >= articles.Count ? null : articles[currentArticle + 1];    //  eger 101 varsa gorsedecek eger yoxdursa null gorsedecek


            var cookie = _contextAccessor.HttpContext.Request.Cookies["Views"];
            string[] findCookie = { "" };

            if (cookie != null)
            {
                findCookie = cookie.Split('-').ToArray();
            }

            if (!findCookie.Contains(article.Id.ToString()))
            {
                Response.Cookies.Append("Views", $"{cookie}-{article.Id}",
                   new CookieOptions
                   {
                       Secure = true,
                       HttpOnly = true,
                       Expires = DateTime.Now.AddYears(1)
                   });


            }
            article.ViewCount += 1;
            _context.Articles.Update(article);
            _context.SaveChanges();


            var similarArticles = _context.Articles.Include(x => x.Category)
      .Where(x => x.CategoryId == article.CategoryId && x.Id != article.Id)
      .Take(2).ToList();


            var featuredArticles = _context.Articles

                .Include(x => x.Category)
                .Where(x => x.IsActive == true && x.IsFeature == false)
                 .OrderByDescending(x => x.ViewCount)
                .Take(7).ToList();

            var articleComment = _context.ArticleComments
.Include(x => x.User)
.Include(x => x.Article)
.Where(x => x.ArticleId == article.Id)
.ToList();

            DetailVM detailVM = new()
            {
                Article = article,
                Articles = articles,
                FeaturedArticles = featuredArticles,
                ArticleComments = articleComment
            };
            return View(detailVM);










        }



        public async Task<IActionResult> AddComment(string content, string articleId)
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ArticleComment articleComment = new()
            {
                Content = content,
                PublishDate = DateTime.Now,
                UserId = userId,
                ArticleId = Convert.ToInt32(articleId),

            };
            await _context.ArticleComments.AddAsync(articleComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", "Article", new { Id = articleId });

        }




    }
}
