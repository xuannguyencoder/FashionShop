using FashionShop.Models;
using FashionShop.Models.Common;
using FashionShop.Models.EF;
using FashionShop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult ProductDetail(string Alias)
        {
            ProductModel proModel = new ProductModel();
            ProductImageModel proImageModel = new ProductImageModel();
            var product = proModel.GetByAlias(Alias);
            if (product == null)
            {
                return HttpNotFound();
            }
            var Images = new List<ProductImageViewModel>();
            foreach (var item in proImageModel.ListByProductID(product.ID))
            {
                Images.Add(new ProductImageViewModel(item.ProductID, item.ColorCode, item.Image, item.DisplayOrder));
            }
            ViewBag.Images = Images;
            return View(product);
        }
        [ChildActionOnly]
        public ActionResult _ProductList(string alias ,long CateID, int pageIndex = 1, int pageSize = 9)
        {
            ProductModel proModel = new ProductModel();
            var products = proModel.ListAllByCateID(CateID).Where(x => x.Status == true && x.ProductCategory.Status == true);
            ViewBag.CategoryID = CateID;
            ViewBag.Alias = alias;

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
            if (products.Count() == 0)
            {
                TempData["Message"] = "Sản phẩm đã hết hàng";
                TempData["Status"] = "info";
            }

            return PartialView(products);
        }
        [ChildActionOnly]
        public ActionResult _RelatedProducts(long ProductID, long CategoryID)
        {
            ProductModel proModel = new ProductModel();
            var products = proModel.ListAll().Where(
                x => x.ProductCategoryID == CategoryID && x.ID != ProductID && x.Status == true).Take(4);
            return PartialView(products.ToList());
        }
        [ChildActionOnly]
        public ActionResult _ProductReview(long ProductID)
        {
            //cập nhật lại sau
            ProductReviewModel proReviewModel = new ProductReviewModel();
            var proReviews = proReviewModel.GetByProductID(ProductID);
            return PartialView(proReviews);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendReview(long ID, FormCollection form)
        {
            if (Session[Constant.CUSTOMER_SESSION] == null)
            {
                TempData["Message"] = "Đăng nhập trược khi bình luận";
                TempData["Status"] = "success";
                return RedirectToRoute("dangnhap");
            }
            ProductReviewModel productReviewModel = new ProductReviewModel();
            ProductReview productReview = new ProductReview();
            productReview.Content = form["review_content"].ToString();
            productReview.ProductID = ID;
            productReviewModel.Insert(productReview);

            ProductModel productModel = new ProductModel();
            var product = productModel.GetByID(ID);
            return RedirectToRoute("chitietsanpham", new { product.Alias });
        }
    }
}