using System.Web.Mvc;

namespace FashionShop.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //ContactControllers
            context.MapRoute(
                name: "thongtinlienhe",
                url: "admin/thong-tin-lien-he",
                defaults: new { controller = "Contact", action = "Index" }
            );
            context.MapRoute(
                name: "chinhsuathongtinlienhe",
                url: "admin/thong-tin-lien-he/chinh-sua-{ID}",
                defaults: new { controller = "Contact", action = "Edit" }
            );
            //FeedbackControllers
            context.MapRoute(
                name: "dsphanhoi",
                url: "admin/email/phan-hoi",
                defaults: new { controller = "Feedback", action = "Index" }
            );
            //EmailControllers
            context.MapRoute(
                name: "soanthu",
                url: "admin/email/soan-thu",
                defaults: new { controller = "Email", action = "Compose" }
            );
            context.MapRoute(
                name: "hopthu",
                url: "admin/email/hop-thu",
                defaults: new { controller = "Email", action = "Index" }
            );
            context.MapRoute(
                name: "docemail",
                url: "admin/email/doc-thu/{MessID}",
                defaults: new { controller = "Email", action = "ReadEmail", id = UrlParameter.Optional }
            );
            //ProductCategoryControllers
            context.MapRoute(
                name: "dsdanhmucbaiviet",
                url: "admin/bai-viet/danh-muc",
                defaults: new { controller = "ArticleCategory", action = "Index" }
            );
            context.MapRoute(
                name: "themdanhmucbaiviet",
                url: "admin/bai-viet/danh-muc/them",
                defaults: new { controller = "ArticleCategory", action = "Create" }
            );
            context.MapRoute(
                name: "csdanhmucbaiviet",
                url: "admin/bai-viet/danh-muc/chinh-sua-{ID}",
                defaults: new { controller = "ArticleCategory", action = "Edit", id = UrlParameter.Optional }
            );
            //ArticleController
            context.MapRoute(
                name: "dsbaiviet",
                url: "admin/bai-viet",
                defaults: new { controller = "Article", action = "Index" }
            );
            context.MapRoute(
                name: "thembaiviet",
                url: "admin/bai-viet/them",
                defaults: new { controller = "Article", action = "Create" }
            );
            context.MapRoute(
                name: "chinhsuabaiviet",
                url: "admin/bai-viet/chinh-sua/{Alias}-{ID}",
                defaults: new { controller = "Article", action = "Edit" }
            );
            //OrderController
            context.MapRoute(
                name: "chitietdonhang",
                url: "admin/don-hang/ma-don-hang-{ID}",
                defaults: new { controller = "Order", action = "OrderDetail" }
            );
            context.MapRoute(
                name: "danhsachdonhang",
                url: "admin/don-hang",
                defaults: new { controller = "Order", action = "Index" }
            );
            //AccountController
            context.MapRoute(
                name: "dang nhap admin",
                url: "admin/dang-nhap",
                defaults: new { controller = "Account", action = "Login" }
            );
            context.MapRoute(
                name: "dang ky admin",
                url: "admin/dang-ky",
                defaults: new { controller = "Account", action = "Register" }
            );
            context.MapRoute(
                name: "dangxuatadmin",
                url: "admin/dang-xuat",
                defaults: new { controller = "Account", action = "Logout" }
            );
            context.MapRoute(
                name: "quanmatkhau",
                url: "admin/quen-mat-khau",
                defaults: new { controller = "Account", action = "Forget" }
            );
            //ProductDetailController
            context.MapRoute(
                name: "Danh sach chi tiet san pham",
                url: "admin/san-pham/chi-tiet",
                defaults: new { controller = "ProductDetail", action = "Index" }
            );
            context.MapRoute(
                name: "Them chi tiet san pham",
                url: "admin/san-pham/chi-tiet/them-{ID}",
                defaults: new { controller = "ProductDetail", action = "Create" }
            );
            context.MapRoute(
                name: "Chinh sua chi tiet san pham",
                url: "admin/san-pham/chi-tiet/chinh-sua/{Alias}-{ID}",
                defaults: new { controller = "ProductDetail", action = "Edit", id = UrlParameter.Optional }
            );

            //Product
            context.MapRoute(
                name: "Danh sach san pham",
                url: "admin/san-pham",
                defaults: new { controller = "Product", action = "Index" }
            );
            context.MapRoute(
                name: "Them san pham",
                url: "admin/san-pham/them",
                defaults: new { controller = "Product", action = "Create" }
            );
            context.MapRoute(
                name: "Chinh sua san pham",
                url: "admin/san-pham/chinh-sua/{Alias}-{ID}",
                defaults: new { controller = "Product", action = "Edit", id = UrlParameter.Optional }
            );

            //ProductCategoryControllers
            context.MapRoute(
                name: "Danh sach danh muc san pham",
                url: "admin/san-pham/danh-muc",
                defaults: new { controller = "ProductCategory", action = "Index" }
            );
            context.MapRoute(
                name: "Them danh muc san pham",
                url: "admin/san-pham/danh-muc/them",
                defaults: new { controller = "ProductCategory", action = "Create" }
            );
            context.MapRoute(
                name: "Chinh sua danh muc san pham",
                url: "admin/san-pham/danh-muc/chinh-sua-{ID}",
                defaults: new { controller = "ProductCategory", action = "Edit", id = UrlParameter.Optional }
            );
            //MenuControllers
            context.MapRoute(
                name: "Danh sanh menu",
                url: "admin/menu",
                defaults: new { controller = "MenuType", action = "Index" }
            );
            context.MapRoute(
                name: "Them menu",
                url: "admin/menu/them",
                defaults: new { controller = "MenuType", action = "Create" }
            );
            context.MapRoute(
                name: "Chinh sua menu",
                url: "admin/menu/chinh-sua-{ID}",
                defaults: new { controller = "MenuType", action = "Edit", id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Danh sach muc menu",
                url: "admin/muc-menu",
                defaults: new { controller = "Menu", action = "Index" }
            );
            context.MapRoute(
                name: "Them muc menu",
                url: "admin/muc-menu/them",
                defaults: new { controller = "Menu", action = "Create" }
            );
            context.MapRoute(
                name: "Chinh sua muc menu",
                url: "admin/muc-menu/chinh-sua-{ID}",
                defaults: new { controller = "Menu", action = "Edit", id = UrlParameter.Optional }
            );
            //Default
            context.MapRoute(
                "Admin_Home",
                "admin/",
                new { controller = "Home", action = "Index"}
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}