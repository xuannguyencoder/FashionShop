using FashionShop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        [HttpPost]
        public JsonResult GetValue()
        {
            return Json(new { min = 20, max = 80 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Change()
        {
            return Json(new { min = 20, max = 80 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult RevenueForTheWeek()
        {
            OrderModel orderModel = new OrderModel();
            List<object> data = new List<object>();
            var date = GetFirstDayOfWeek(DateTime.Now);
            for (int i =0; i<7; i++)
            {
                var orders = orderModel.ListALL().Where(x =>
                    x.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy") == date.AddDays(i).ToString("dd/MM/yyyy"));

                decimal total = 0;
                foreach (var item in orders)
                {
                    total += item.OrderDetails.Sum(x=>x.Quantity *x.Price).GetValueOrDefault(0);
                }
                data.Add(total);
            }
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FeedbackCount()
        {
            FeedbackModel feedbackModel = new FeedbackModel();
            int feedbackcount = feedbackModel.ListAll().Count(); // tổng lượt phản hồi
            int reply = feedbackModel.ListAll().Where(x => x.Status == true).Count();
            int noreply = feedbackcount - reply;
            return Json(new { reply, noreply }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CustomerCount()
        {
            CustomerModel customerModel = new CustomerModel();
            int customercount = customerModel.ListAll().Count();
            return Json(new { customercount }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ArticleCount()
        {
            ArticleModel articleModel = new ArticleModel();
            int articlecount = articleModel.ListAll().Count();
            return Json(new { articlecount }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RevenueForDay()
        {
            OrderModel orderModel = new OrderModel();
            var orders = orderModel.ListALL().Where(x =>
                   x.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")); // tổng tiền trong ngày
            decimal total = 0;
            foreach (var item in orders)
            {
                total += item.OrderDetails.Sum(x => x.Quantity * x.Price).GetValueOrDefault(0);
            }
            return Json(new { revenueforday = total }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ProductCount()
        {
            ProductModel productModel = new ProductModel();
            int productcount = productModel.ListAll().Count(); // tổng số lượng sản phẩm
            return Json(new { productcount }, JsonRequestBehavior.AllowGet);
        }
        public DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            DayOfWeek firstDay = DayOfWeek.Monday;

            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }
            return firstDayInWeek;
        }
    }
}