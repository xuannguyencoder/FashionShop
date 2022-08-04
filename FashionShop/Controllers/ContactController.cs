using FashionShop.Models;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            ContactModel contactModel = new ContactModel();
            var contact = contactModel.GetActive();
            if (contact != null)
            {
                ViewBag.Contact = contact;
            }
            else
            {
                ViewBag.Contact = new Contact();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Feedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                FeedbackModel feedbackModel = new FeedbackModel();
                long result = feedbackModel.Insert(feedback);
                if (result > 0)
                {
                    TempData["Message"] = "Gửi phản hồi thành công";
                    TempData["Status"] = "success";
                    return RedirectToRoute("lienhe");
                }
            }
            ContactModel contactModel = new ContactModel();
            var contact = contactModel.GetActive();
            if (contact != null)
            {
                ViewBag.Contact = contact;
            }
            else
            {
                ViewBag.Contact = new Contact();
            }
            return View("Index", feedback );
        }
        [HttpPost]
        public JsonResult GetPoint()
        {
            ContactModel contactModel = new ContactModel();
            var contact = contactModel.GetActive();
            if (contact != null)
            {
                ViewBag.Contact = contact;
            }
            else
            {
                ViewBag.Contact = new Contact();
            }

            return Json(new { pointX = contact.PointX, pointY = contact.PointY }, JsonRequestBehavior.AllowGet);
        }

    }
}