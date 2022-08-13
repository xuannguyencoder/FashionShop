using FashionShop.Models;
using FashionShop.Models.Common;
using FashionShop.Models.ViewModel;
using System.Web.Mvc;

namespace FashionShop.Controllers
{
    public class OrderController : Controller
    {
        private CustomerModel cusModel = new CustomerModel();
        private PaymentModel paymentModel = new PaymentModel();
        private CartModel cartModel = new CartModel();

        public ActionResult Checkout()
        {
            if (Session[Constant.CartSession] == null)
            {
                TempData["Message"] = "Thêm sản phẩm vào giỏ hàng trước khi thanh toán";
                TempData["Status"] = "success";
                return RedirectToRoute("giohang");
            }
            var cart = cartModel.ListAll();
            if (cart.Count == 0)
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

            var customer = cusModel.GetByID(user.UserID);
            ShipViewModel shipView;
            shipView = new ShipViewModel(customer.FirstName, customer.LastName, customer.Address, customer.Phone);

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
                if (form["payment_method"] != null)
                {
                    try
                    {
                        int paymentID = int.Parse(form["payment_method"].ToString());
                        if (paymentModel.GetByID(paymentID) == null)
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
                        }
                    }
                    catch
                    {
                        return View(shipViewModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("PaymentMethod", "Vui lòng chọn phương thức thanh toán");
                }
            }

            var cart = cartModel.ListAll();
            ViewBag.Cart = cart;
            ViewBag.CartTotal = cartModel.getCartTotal();
            ViewBag.Payments = paymentModel.ListAll();
            return View(shipViewModel);
        }
    }
}