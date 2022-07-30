using FashionShop.Models;
using FashionShop.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FashionShop.Controllers
{
    public class CartController : Controller
    {
        ProductModel productModel = null;
        ProductDetailModel proDetailModel = null;
        CartModel cartModel = null;
        ProductImageModel proImageModel = null;
        public ActionResult Index()
        {
            cartModel = new CartModel();
            proImageModel = new ProductImageModel();

            var model = cartModel.ListAll();
            ViewBag.CartTotal = cartModel.getCartTotal();
            return View(model);
        }
        public ActionResult AddItem(long ID)
        {
            productModel = new ProductModel();
            proDetailModel = new ProductDetailModel();
            var proDetails = proDetailModel.GetByProductID(ID);
            var proDetail = proDetails.Where(x => x.Quantity > 0).OrderBy(x => x.SizeID).FirstOrDefault();
            if (proDetail == null)
            {
                var product = productModel.GetByID(ID);
                TempData["Message"] = "Sản phẩm đã hết hàng";
                TempData["Status"] = "success";
                return RedirectToRoute("chitietsanpham", new { product.Alias });
            }
            var cart = Session[Constant.CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.ProductDetail.ID == proDetail.ID)) // item cart có trong giỏ hàng
                {
                    foreach (var item in list)
                    {
                        if (item.ProductDetail.ID == proDetail.ID)// (số lượng thêm vào + số lượng cart)<= số lượng trong database
                        {
                            if (item.Quantity + 1 <= proDetail.Quantity)
                            {
                                item.Quantity += 1;
                            }
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Quantity = 1;
                    item.ProductDetail = proDetail;
                    list.Add(item);
                }
                Session[Constant.CartSession] = list;
            }
            else //Cart == null 
            {
                var item = new CartItem();
                item.Quantity = 1;
                item.ProductDetail = proDetail;

                // Create cart-item list
                var list = new List<CartItem>();
                list.Add(item);
                Session[Constant.CartSession] = list;
            }
            return RedirectToRoute("giohang");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem()
        {
            proDetailModel = new ProductDetailModel();
            int Quantity, SizeID;
            string Color;
            long ProductID = long.Parse(Request.Form["ProductID"]);
            try
            {
                productModel = new ProductModel();
                Quantity = int.Parse(Request.Form["quantity"]);
                SizeID = int.Parse(Request.Form["size"]);
                Color = Request.Form["color"].ToString();
            }
            catch
            {
                var product = productModel.GetByID(ProductID);
                return RedirectToRoute("chitietsanpham", new { product.Alias });
            }
            var proDetail = proDetailModel.GetByProperties(ProductID, SizeID, Color);
            if (proDetail != null)
            {
                var cart = Session[Constant.CartSession];
                if (cart != null)
                {
                    var list = (List<CartItem>)cart;   
                    if (list.Exists(x => x.ProductDetail.ID == proDetail.ID)) // item cart có trong giỏ hàng
                    {
                        foreach (var item in list)
                        {
                            // (số lượng thêm vào + số lượng product trong cart)<= số lượng trong database
                            if (item.ProductDetail.ID == proDetail.ID)
                            {
                                if (item.Quantity + Quantity <= proDetail.Quantity)
                                    item.Quantity += Quantity;
                                else
                                    item.Quantity = proDetail.Quantity.GetValueOrDefault(0); //set Maximum number of products
                            }
                        }
                    }
                    else
                    {
                        var item = new CartItem();
                        if (Quantity <= proDetail.Quantity)
                            item.Quantity = Quantity;
                        else
                            item.Quantity = proDetail.Quantity.GetValueOrDefault(0);//set Maximum number of products
                        item.ProductDetail = proDetail;
                        list.Add(item);
                    }
                    Session[Constant.CartSession] = list;
                }
                else //Cart == null
                {
                    var item = new CartItem();
                    if (Quantity <= proDetail.Quantity)
                        item.Quantity = Quantity;
                    else
                        item.Quantity = proDetail.Quantity.GetValueOrDefault(0);//set Maximum number of products
                    item.ProductDetail = proDetail;

                    // Create cart-item list
                    var list = new List<CartItem>();
                    list.Add(item);
                    Session[Constant.CartSession] = list;
                }
            }
            return RedirectToRoute("giohang");
        }

        //using ajax
        public JsonResult DeleteAll()
        {
            cartModel = new CartModel();
            cartModel.DeleteAll();
            return Json(new { status = true });
        }
        //using ajax
        public JsonResult Delete(long productDetailID)
        {
            cartModel = new CartModel();
            cartModel.Delete(productDetailID);
            return Json(new { status = true });
        }
        //using ajax
        public JsonResult Update(string cartJson)
        {
            cartModel = new CartModel();
            proDetailModel = new ProductDetailModel();
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartJson);
            var sessionCart = cartModel.ListAll();
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.ProductDetail.ID == item.ProductDetail.ID);
                if (jsonItem != null)
                {
                    var productDetail = proDetailModel.GetByID(jsonItem.ProductDetail.ID);
                    if (jsonItem.Quantity <= productDetail.Quantity)
                        item.Quantity = jsonItem.Quantity;
                    else
                        item.Quantity = productDetail.Quantity.GetValueOrDefault(0);
                }
            }
            cartModel.Update(sessionCart);
            return Json(new { status = true });
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}