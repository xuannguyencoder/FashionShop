using FashionShop.Models;
using FashionShop.Models.Common;
using FashionShop.Models.EF;
using FashionShop.Models.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Controllers
{
    public class PaymentController : Controller
    {
        ShipModel shipModel = null;
        OrderModel orderModel = null;
        CartModel cartModel = null;

        public ActionResult MomoPayment()
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán bằng ví điện tử MoMo";
            string returnUrl = "https://localhost:44317/ConfirmPaymentClient";
            string notifyurl = "http://ba1adf48beba.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test
            cartModel = new CartModel();
            string amount = cartModel.getCartTotal().ToString();
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            try
            {
                JObject jmessage = JObject.Parse(responseFromMomo);

                return Redirect(jmessage.GetValue("payUrl").ToString());
            }
            catch
            {
                return RedirectToAction("Checkout","Order");
            }

        }
        public ActionResult ConfirmPaymentClient()
        {
            int paymentID = (int)Session["PaymentID"];
            if (paymentID == 1 ||Request.QueryString["errorCode"].ToString() == "0")
            {
                var shipViewModel = (ShipViewModel)Session["ShipViewModel"];
                Ship ship = new Ship();
                ship.Phone = shipViewModel.Phone;
                ship.Name = shipViewModel.Name;
                ship.Address = shipViewModel.Address;
                ship.Note = shipViewModel.Note;
                shipModel = new ShipModel();
                var ShipID = shipModel.Insert(ship);

                Order order = new Order();
                order.ShipID = ShipID;
                order.PaymentID = paymentID;

                var User = (UserLogin)Session[Constant.CUSTOMER_SESSION];
                order.CustomerID = User.UserID;

                orderModel = new OrderModel();
                var orderID = orderModel.Insert(order);

                cartModel = new CartModel();
                var cart = cartModel.ListAll();
                ProductDetailModel productDetailModel = new ProductDetailModel();
                OrderDetailModel orderDetailModel = new OrderDetailModel();
                foreach (var item in cart)
                {
                    //set value for Order Detail
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductDetailID = item.ProductDetail.ID;
                    orderDetail.OrderID = orderID;
                    orderDetail.ProductName = item.ProductDetail.Product.Name;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Price = item.Price;
                    //Cập nhật số lượng product
                    int currentQuantity = item.ProductDetail.Quantity.GetValueOrDefault(0) - item.Quantity;
                    var result = productDetailModel.UpdateQuantity(item.ProductDetail.ID, currentQuantity);
                    if (result)
                    {
                        orderDetailModel.Insert(orderDetail);
                    }
                }
                string body = System.IO.File.ReadAllText(Server.MapPath("~/Assets/template/order.html"));


                body = body.Replace("{{OrderID}}", orderID.ToString());
                body = body.Replace("{{CreateDate}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                body = body.Replace("{{Email}}", User.Email);
                body = body.Replace("{{CustomerName}}", ship.Name);
                body = body.Replace("{{Address}}", ship.Address);
                body = body.Replace("{{Phone}}", ship.Phone);

                body = body.Replace("{{OrderTotal}}", cartModel.getCartTotal().ToString("N0")+" VND");
                body = body.Replace("{{Total}}", cartModel.getCartTotal().ToString("N0")+" VNĐ");
                if (paymentID == 1)
                {
                    body = body.Replace("{{PaymentStatus}}", "Chưa thanh toán");
                    body = body.Replace("{{PaymentMethod}}", "Thanh toán khi nhận hàng");
                }
                else
                {
                    body = body.Replace("{{PaymentStatus}}", "Đã thanh toán");
                    if (paymentID == 2)
                    {
                        body = body.Replace("{{PaymentMethod}}", "Ví điện tử MoMo");
                    }
                }
                string products = "";
                foreach(var item in orderDetailModel.ListByOrderID(orderID))
                {
                    var productdetails = productDetailModel.GetByID(item.ProductDetailID);
                    var total = item.Price * item.Quantity;
                    products += String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                        item.ProductName,
                        productdetails.Size.Code,
                        productdetails.Color.Name,
                        item.Price.GetValueOrDefault().ToString("N0")+" VND",
                        item.Quantity,
                        total.GetValueOrDefault().ToString("N0")+" VND");
                }
                body = body.Replace("{{Products}}", products);

                MailHelper.SendMail("Thông tin đơn hàng", body, User.Email);

                cartModel.DeleteAll();
                TempData["Message"] = "Thanh toán đơn hàng thành công. Khách hàng lòng kiểm tra email để xem thông tin thanh toán";
                TempData["Status"] = "success";
            }
            return RedirectToRoute("giohang");
        }
    }
}