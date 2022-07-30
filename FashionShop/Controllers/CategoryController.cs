using FashionShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Controllers
{
    public class CategoryController : Controller
    {
        public ActionResult Index(string Alias, int? pageIndex = 1)
        {
            MenuModel menuModel = new MenuModel();
            var menu = menuModel.GetByAlias(Alias);
            if (menu == null)
            {
                return HttpNotFound();
            }
            string[] arrListStr = menu.Link.Split('/');
            string view = arrListStr[0];
            long ID = long.Parse(arrListStr[1]);
            ViewBag.Type = view;
            ViewBag.ID = ID;
            ViewBag.pageIndex = pageIndex;
            ViewBag.Alias = Alias;
            return View();
        }
    }
}