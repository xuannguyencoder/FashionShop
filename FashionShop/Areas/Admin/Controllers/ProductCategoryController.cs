using FashionShop.Models;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        ProductCategoryModel productCateModel = new ProductCategoryModel();
        public ActionResult Index()
        {
            var model = productCateModel.ListAllByFormat();
            return View(model);
        }
        public ActionResult Create()
        {
            var cateList = productCateModel.DDLProductCate();
            cateList.Insert(0, (new SelectListItem { Text = "Đây là mục menu chính", Value = "0" }));
            ViewBag.ProductCateList = cateList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form, ProductCategory productCate)
        {

            if (ModelState.IsValid)
            {
                var result = productCateModel.Insert(productCate);
                if (result > 0)
                {
                    TempData["Message"] = "Thêm danh mục sản phẩm thành công";
                    TempData["Status"] = "success";
                    if (form["btnAdd"] != null)
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Create");
                }
                
            }
            var cateList = productCateModel.DDLProductCate();
            cateList.Insert(0, (new SelectListItem { Text = "Đây là mục menu chính", Value = "0" }));
            ViewBag.ProductCateList = cateList;
            return View(productCate);
        }
        public ActionResult Edit(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var productCate = productCateModel.GetByID(ID);
            if (productCate == null)
            {
                return View("_Error404");
            }
            //dropdownlist danh mục cha
            var cateList = productCateModel.DDLProductCate();
            cateList.Insert(0, (new SelectListItem { Text = "Đây là mục menu chính", Value = "0" }));
            ViewBag.ProductCateList = cateList;

            //dropdownlist cùng 1 danh mục cha
            var cates = new SelectList(productCateModel.ListOrderByDisplayOrder()
                .Where(x => x.ParentID == productCate.ParentID), "DisplayOrder", "Name").ToList();
            cates.Insert(0, (new SelectListItem { Text = "Vị trí đầu", Value = "0" }));
            ViewBag.MenuDisplayOrderList = cates;
            return View(productCate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory productCate)
        {
            if (ModelState.IsValid)
            {
                var result = productCateModel.Update(productCate);
                var result2 = productCateModel.UpdateLevel(productCate.ID);
                if (result && result2)
                {
                    TempData["Message"] = "Cập nhật danh mục thành công";
                    TempData["Status"] = "success";
                    return RedirectToAction("Index");
                }
            }

            //dropdownlist danh mục cha
            var cateList = productCateModel.DDLProductCate();
            cateList.Insert(0, (new SelectListItem { Text = "Đây là mục menu chính", Value = "0" }));
            ViewBag.ProductCateList = cateList;

            //dropdownlist cùng 1 danh mục cha
            var cates = new SelectList(productCateModel.ListOrderByDisplayOrder()
                .Where(x => x.ParentID == productCate.ParentID), "DisplayOrder", "Name").ToList();
            cates.Insert(0, (new SelectListItem { Text = "Vị trí đầu", Value = "0" }));
            ViewBag.MenuDisplayOrderList = cates;
            return View(productCate);
        }
        
        public ActionResult Delete(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var productCate = productCateModel.GetByID(ID);
            if (productCate == null)
            {
                return View("_Error404");
            }
            var result = productCateModel.Delete(productCate.ID);
            if (result)
            {
                TempData["Message"] = "Xóa danh mục sản phẩm thành công";
                TempData["Status"] = "success";
            }
            else
            {
                TempData["Message"] = "Xóa danh mục sản phẩm không thành công";
                TempData["Status"] = "error";
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdateStatus(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var result = productCateModel.UpdateStatus(ID);
            if (result)
            {
                TempData["Message"] = "Cập nhật trạng thái thành công";
                TempData["Status"] = "success";
                return RedirectToAction("Index");
            }
            else
                return View("_Error404"); //không tìm thấy id
        }
    }
}