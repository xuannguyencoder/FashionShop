using FashionShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            OrderModel orderModel = new OrderModel();
            var orders = orderModel.ListALL();

            return View(orders);
        }
        public ActionResult OrderDetail(long? ID)
        {
            OrderModel orderModel = new OrderModel();
            if (ID == null)
            {
                return View("_Error400");
            }
            var order = orderModel.GetByID(ID);
            if (order == null)
            {
                return View("_Error404");
            }
            OrderDetailModel orderDetailModel = new OrderDetailModel();
            var orderDetails = orderDetailModel.ListByOrderID(order.ID);

            ViewBag.Order = order;
            return View(orderDetails);
        }

        public ActionResult OrderConfirmed(long OrderID)
        {
            OrderModel orderModel = new OrderModel();
            var result = orderModel.UpdateStatus(OrderID, 2); //Status: Đã xác nhận
            if (result)
            {
                TempData["Message"] = "Xác nhận đơn hàng thành công";
                TempData["Status"] = "success";
            }
            return RedirectToAction("Index");
        }
        public ActionResult OrderCancelled(long OrderID)
        {
            OrderModel orderModel = new OrderModel();
            OrderDetailModel orderDetailModel = new OrderDetailModel();
            ProductDetailModel proDetailModel = new ProductDetailModel();
            var result = orderModel.UpdateStatus(OrderID, 3); //Status: Đã hủy
            if (result)
            {
                var orderDetail = orderDetailModel.ListByOrderID(OrderID);
                //update product quantity before canceling
                foreach (var item in orderDetail)
                {
                    var productDetail = proDetailModel.GetByID(item.ProductDetailID);
                    int quantity = productDetail.Quantity.GetValueOrDefault(0) + item.Quantity.GetValueOrDefault(0);
                    proDetailModel.UpdateQuantity(productDetail.ID, quantity);
                }
                TempData["Message"] = "Hủy đơn hàng thành công";
                TempData["Status"] = "success";
            }
            return RedirectToAction("Index");
        }
    }
}