using FashionShop.Models;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FashionShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int pageIndex = 1, int pageSize = 12)
        {
            ProductModel productModel = new ProductModel();
            var products = productModel.ListAll().Where(
                x => x.Status == true && x.ProductCategory.Status == true && x.CreatedDate >= DateTime.Today.AddDays(-20));
            ProductImageModel proImageModel = new ProductImageModel();

            MenuTypeModel menuTypeModel = new MenuTypeModel();
            var menuTypes = menuTypeModel.ListAllByPosition(2).Where(x => x.Status = true).FirstOrDefault();
            if (menuTypes != null)
                ViewBag.ProductMenu = menuTypes.Menus.ToList();
            else
                ViewBag.ProductMenu = new List<Menu>();

            var totalRecord = 0;
            totalRecord = products.Count();
            products = products.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.Total = totalRecord;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageIndex;

            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            ViewBag.TotalPage = totalPage;

            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = pageIndex + 1;
            ViewBag.Prev = pageIndex - 1;

            return View(products);
        }

        [HttpPost]
        public ActionResult SearchProduct(FormCollection values)
        {
            var keyword = values["keyword"];
            if (String.IsNullOrEmpty(keyword))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("SearchProduct", new { keyword = keyword });
        }

        public ActionResult SearchProduct(string keyword, int pageIndex = 1, int pageSize = 12)
        {
            ProductModel productModel = new ProductModel();

            var products = productModel.Search(keyword);
            ViewBag.Keyword = keyword;

            ProductImageModel proImageModel = new ProductImageModel();

            var totalRecord = 0;
            totalRecord = products.Count();
            products = products.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.Total = totalRecord;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageIndex;

            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            ViewBag.TotalPage = totalPage;

            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = pageIndex + 1;
            ViewBag.Prev = pageIndex - 1;

            return View(products);
        }

        [ChildActionOnly]
        public ActionResult _NavbarMenu()
        {
            CartModel cartModel = new CartModel();
            MenuTypeModel menuTypeModel = new MenuTypeModel();
            var menuTypes = menuTypeModel.ListAllByPosition(1);
            ViewBag.CartCount = cartModel.ListAll().Count();
            return PartialView(menuTypes);
        }

        [ChildActionOnly]
        public ActionResult _Categories()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _SidebarLeft()
        {
            MenuTypeModel menuTypeModel = new MenuTypeModel();
            var menuTypes = menuTypeModel.ListAllByPosition(4).Where(x => x.Status == true).ToList();

            return PartialView(menuTypes);
        }

        [ChildActionOnly]
        public ActionResult _TrendSpad()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _BannerBottom()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _ServicesSpad()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _Instagram()
        {
            return PartialView();
        }
    }
}