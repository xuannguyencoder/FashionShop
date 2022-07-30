using FashionShop.Models;
using FashionShop.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class FeedbackController : BaseController
    {
        public ActionResult Index()
        {
            FeedbackModel feedbackModel = new FeedbackModel();
            return View(feedbackModel.ListAll());
        }
        [HttpPost]
        public JsonResult SendMail(long ID, string content)
        {
            FeedbackModel feedbackModel = new FeedbackModel();
            var fb = feedbackModel.GetByID(ID);
            if (fb!=null)
            {
                var result = MailHelper.SendMail("Trả lời phải hồi", content, fb.Email);
                if (result)
                {
                    TempData["Message"] = "Gửi email thành công";
                    TempData["Status"] = "success";
                    feedbackModel.UpdateStatus(fb.ID);
                    return Json(new { status = true });
                }
            }
            TempData["Message"] = "Gửi email thất bại";
            TempData["Status"] = "error";
            return Json(new { status = false });

        }
    }
}