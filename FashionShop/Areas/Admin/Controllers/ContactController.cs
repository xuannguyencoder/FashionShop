using FashionShop.Models;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            ContactModel contactModel = new ContactModel();
            return View(contactModel.GetActive());
        }
        public ActionResult Edit(int? ID)
        {
            ContactModel contactModel = new ContactModel();
            if (ID == null)
            {
                return View("_Error400");
            }
            var contact = contactModel.GetByID(ID);
            if (contact == null)
            {
                return View("_Error404");
            }
            return View(contact);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Contact contact)
        {
            ContactModel contactModel = new ContactModel();
            if (ModelState.IsValid)
            {
                var result = contactModel.Update(contact);
                if (result)
                {
                    TempData["Message"] = "Cập nhật thông tin liên hệ thành công";
                    TempData["Status"] = "success";
                    return RedirectToAction("Index");
                }
            }
            return View(contact);
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

        [HttpPost]
        public JsonResult UpdatePoint(int ID, string pointX, string pointY)
        {
            ContactModel contactModel = new ContactModel();
            var result = contactModel.UpdatePoint(ID, pointX, pointY);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }
    }
}