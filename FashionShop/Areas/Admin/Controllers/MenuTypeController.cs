using FashionShop.Models;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class MenuTypeController : BaseController
    {
        MenuTypeModel menuTypeModel = new MenuTypeModel();
        PositionModel positionModel = new PositionModel();
        public ActionResult Index()
        {
            return View(menuTypeModel.ListAll().OrderBy(x=>x.PositionID).ThenBy(x=>x.DisplayOrder).ToList());
        }
        public ActionResult Create()
        {
            ViewBag.PositionList = new SelectList(positionModel.ListAll(), "ID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form, MenuType menuType)
        {
            if (ModelState.IsValid)
            {
                var result = menuTypeModel.Insert(menuType);
                if (result > 0)
                {
                    TempData["Message"] = "Thêm menu thành công";
                    TempData["Status"] = "success";
                    if (form["btnAdd"] != null)
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Create");
                }
            }
            ViewBag.PositionList = new SelectList(positionModel.ListAll(), "ID", "Name");
            return View();
        }
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var menuType = menuTypeModel.GetByID(ID);
            if (menuType == null)
            {
                return View("_Error404");
            }
            
            ViewBag.PositionList = new SelectList(positionModel.ListAll(), "ID", "Name");

            var menuTypes = new SelectList(menuTypeModel.ListOrderByDisplayOrder().Where(x=>x.Position == menuType.Position), "DisplayOrder", "Name").ToList();
            menuTypes.Insert(0, (new SelectListItem { Text = "Vị trí đầu", Value = "0" }));
            ViewBag.MenuTypeList = menuTypes;

            return View(menuType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuType menuType)
        {
            if (ModelState.IsValid)
            {
                var result = menuTypeModel.Update(menuType);
                if (result)
                {
                    TempData["Message"] = "Cập nhật menu thành công";
                    TempData["Status"] = "success";
                    return RedirectToAction("Index");
                }
            }
            var menuTypes = new SelectList(menuTypeModel.ListOrderByDisplayOrder(), "DisplayOrder", "Name").ToList();
            menuTypes.Insert(0, (new SelectListItem { Text = "Vị trí đầu", Value = "0" }));
            ViewBag.MenuTypeList = menuTypes;
            return View(menuType);
        }
        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var menuType = menuTypeModel.GetByID(ID);
            if (menuType == null)
            {
                return View("_Error404");
            }
            var result = menuTypeModel.Delete(menuType.ID);
            if (result)
            {
                TempData["Message"] = "Xóa menu thành công";
                TempData["Status"] = "success";
            }
            else
            {
                TempData["Message"] = "Xóa menu không thành công";
                TempData["Status"] = "error";
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdateStatus(int? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var result = menuTypeModel.UpdateStatus(ID);
            if (result)
            {
                TempData["Message"] = "Cập nhật trạng thái thành công";
                TempData["Status"] = "success";
                return RedirectToAction("Index");
            }
            else
                return View("_Error404");
        }
    }
}