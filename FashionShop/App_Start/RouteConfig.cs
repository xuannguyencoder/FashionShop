using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FashionShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //ContactController
            routes.MapRoute(
                name: "lienhe",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "index" },
                namespaces: new[] { "FashionShop.Controllers" }
            );
            //PaymentController
            routes.MapRoute(
                name: "ConfirmPaymentClient",
                url: "ConfirmPaymentClient",
                defaults: new { controller = "Payment", action = "ConfirmPaymentClient" },
                namespaces: new[] { "FashionShop.Controllers" }
            );
            //cartController
            routes.MapRoute(
                 name: "thanhtoandonhang",
                 url: "thanh-toan-don-hang",
                 defaults: new { controller = "Order", action = "Checkout" },
                 namespaces: new[] { "FashionShop.Controllers" }
             );
            routes.MapRoute(
                 name: "giohang",
                 url: "gio-hang",
                 defaults: new { controller = "Cart", action = "Index" },
                 namespaces: new[] { "FashionShop.Controllers" }
             );
            routes.MapRoute(
                 name: "themsanphamvaogiohang",
                 url: "them-san-pham/{ID}",
                 defaults: new { controller = "Cart", action = "AddItem" },
                 namespaces: new[] { "FashionShop.Controllers" }
             );
            //AccountController
            routes.MapRoute(
                 name: "dangnhap",
                 url: "dang-nhap",
                 defaults: new { controller = "Account", action = "Login" },
                 namespaces: new[] { "FashionShop.Controllers" }
             );
            routes.MapRoute(
                 name: "dangxuat",
                 url: "dang-xuat",
                 defaults: new { controller = "Account", action = "Logout" },
                 namespaces: new[] { "FashionShop.Controllers" }
             );
            routes.MapRoute(
                 name: "dangnhapkhac",
                 url: "dang-nhap/{Type}",
                 defaults: new { controller = "Account", action = "SignIn" },
                 namespaces: new[] { "FashionShop.Controllers" }
             );
            routes.MapRoute(
                name: "dangky",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register" },
                namespaces: new[] { "FashionShop.Controllers" }
            );
            //HomeController
            routes.MapRoute(
                 name: "trangchu",
                 url: "trang-chu",
                 defaults: new { controller = "Home", action = "Index" },
                 namespaces: new[] { "FashionShop.Controllers" }
             );
            routes.MapRoute(
                name: "phantranghome",
                url: "page={pageIndex}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "FashionShop.Controllers" }
            );
            //AritcleController
            routes.MapRoute(
                name: "chitietbaiviet",
                url: "bai-viet/{Alias}",
                defaults: new { controller = "Article", action = "ArticleDetails" },
                namespaces: new[] { "FashionShop.Controllers" }
            );
            routes.MapRoute(
                name: "tags",
                url: "tag/{tagID}",
                defaults: new { controller = "Article", action = "Tag" },
                namespaces: new[] { "FashionShop.Controllers" }
            );
            //ProductController
            routes.MapRoute(
                name: "chitietsanpham",
                url: "san-pham/{Alias}",
                defaults: new { controller = "Product", action = "ProductDetail" },
                namespaces: new[] { "FashionShop.Controllers" }
            );
            routes.MapRoute(
                name: "danhmucsanpham",
                url: "{Alias}/page={pageIndex}",
                defaults: new { controller = "Category", action = "Index" },
                namespaces: new[] { "FashionShop.Controllers" }
            );
            routes.MapRoute(
                name: "danhmucsanphamphantrang",
                url: "{Alias}",
                defaults: new { controller = "Category", action = "Index" },
                namespaces: new[] { "FashionShop.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FashionShop.Controllers" }
            );
        }
    }
}
