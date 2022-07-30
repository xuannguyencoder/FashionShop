using FashionShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult ArticleDetails(string Alias)
        {
            ArticleModel articleModel = new ArticleModel();
            var article = articleModel.GetByAlias(Alias);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.articles = articleModel.ListAll().Where(x=>x.Status == true &&x.Category.Status == true && x.CreatedDate >= DateTime.Today.AddDays(-10)).OrderBy(x=>x.CreatedDate).Take(6).ToList();
            return View(article);
        }
        public ActionResult Tag(string tagID)
        {
            ArticleModel articleModel = new ArticleModel();
            var tag = articleModel.GetTagByID(tagID);
            if (tag==null)
            {
                return HttpNotFound();
            }
            ViewBag.TagName = tag.Name;

            var articles = articleModel.ListContentTagByTagID(tagID).Where(x => x.Status == true && x.Category.Status == true).ToList();

            return View(articles);
        }

        [ChildActionOnly]
        public ActionResult _ArticleList(long CateID)
        {
            ArticleModel articleModel = new ArticleModel();
            var articles = articleModel.ListAllByCateID(CateID).Where(x => x.Status == true && x.Category.Status == true).ToList();
            return PartialView(articles);
        }
    }
}