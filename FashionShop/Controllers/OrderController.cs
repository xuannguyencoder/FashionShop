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
    public class OrderController : Controller
    {
        CustomerModel cusModel = null;
        PaymentModel paymentModel = null;
        CartModel cartModel = null;
        public ActionResult Checkout()
        {
            if (Session[Constant.CartSession] == null)
            {
                TempData["Message"] = "Thêm sản phẩm vào giỏ hàng trước khi thanh toán";
                TempData["Status"] = "success";
                return RedirectToRoute("giohang");
            }
            cartModel = new CartModel();    
            var cart = cartModel.ListAll();
            if (cart.Count==0)
            {
                TempData["Message"] = "Thêm sản phẩm vào giỏ hàng trước khi thanh toán";
                TempData["Status"] = "success";
                return RedirectToRoute("giohang");
            }
            var user = (UserLogin)Session[Constant.CUSTOMER_SESSION];
            if (user == null)
            {
                TempData["Message"] = "Đăng nhập để thanh toán đơn hàng";
                TempData["Status"] = "success";
                return RedirectToRoute("dangnhap");
            }
            cusModel = new CustomerModel();
            var customer = cusModel.GetByID(user.UserID);
            ShipViewModel shipView;
            shipView = new ShipViewModel(customer.FirstName, customer.LastName, customer.Address, customer.Phone);

            paymentModel = new PaymentModel();
            ViewBag.Cart = cart;
            ViewBag.CartTotal = cartModel.getCartTotal();
            ViewBag.Payments = paymentModel.ListAll();
            return View(shipView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(FormCollection form, ShipViewModel shipViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int paymentID = int.Parse(form["payment_method"].ToString());
                    paymentModel = new PaymentModel();
                    if (paymentModel.GetByID(paymentID) ==null)
                    {
                        return View(shipViewModel);
                    }
                    switch (paymentID)
                    {
                        case 1:
                            Session["ShipViewModel"] = shipViewModel;
                            Session["PaymentID"] = 1;
                            return RedirectToAction("ConfirmPaymentClient", "Payment");
                        case 2:
                            Session["ShipViewModel"] = shipViewModel;
                            Session["PaymentID"] = 2;
                            return RedirectToAction("MomoPayment", "Payment");
                        case 3:
                            Session["PaymentID"] = 3;
                            Session["ShipViewModel"] = shipViewModel;
                            return RedirectToAction("PaypalPayment", "Payment");
                    }  
                }
                catch
                {
                    return View(shipViewModel);
                }
            }
            return View(shipViewModel);
        }
    }
}