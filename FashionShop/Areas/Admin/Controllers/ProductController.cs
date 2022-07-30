using FashionShop.Models;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        ProductModel productModel = new ProductModel();
        ProductCategoryModel productCateModel = new ProductCategoryModel();
        ProductImageModel proImageModel = null;
        public ActionResult Index()
        {
            var model = productModel.ListAll();
            proImageModel = new ProductImageModel();
            ViewBag.ProductImages = proImageModel.ListAll();
            return View(model);
        }
        public ActionResult Create()
        {
            ViewBag.ProductCateList = DDLProductCate();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form, Product product)
        {
            if (ModelState.IsValid)
            {
                if (CheckAlias(product.Alias, 0))
                {
                    var productID = productModel.Insert(product);
                    if (productID > 0)
                    {
                        TempData["Message"] = "Thêm sản phẩm thành công";
                        TempData["Status"] = "success";
                        if (form["btnAdd"] != null)
                            return RedirectToAction("Index");
                        else
                            return RedirectToAction("Create");
                    }
                }
                else
                {
                    ModelState.AddModelError("Alias", "Alias này đã tồn tại");
                }
            }
            ViewBag.ProductCateList = DDLProductCate();
            return View(product);
        }
        public ActionResult Edit(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var product = productModel.GetByID(ID);
            if (product == null)
            {
                return View("_Error404");
            }
            ViewBag.ProductCateList = DDLProductCate();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (CheckAlias(product.Alias, product.ID))
                {
                    var result = productModel.Update(product);
                    if (result)
                    {
                        TempData["Message"] = "Cập nhật sản phẩm thành công";
                        TempData["Status"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("Alias", "Alias này đã tồn tại");
                }    
            }
            ViewBag.ProductCateList = DDLProductCate();
            return View(product);
        }
        public ActionResult Delete(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var product = productModel.GetByID(ID);
            if (product == null)
            {
                return View("_Error404");
            }
            var result = productModel.Delete(ID);
            if (result)
            {
                TempData["Message"] = "Xóa sản phẩm thành công";
                TempData["Status"] = "success";
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdateStatus(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var result = productModel.UpdateStatus(ID);
            if (result)
            {
                TempData["Message"] = "Cập nhật trạng thái thành công";
                TempData["Status"] = "success";
                return RedirectToAction("Index");
            }
            else
                return View("_Error404");
        }
        public bool CheckExtension(string extension)
        {
            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private List<SelectListItem> DDLProductCate()
        {
            var productCateList = new SelectList(new List<SelectListItem>()).ToList();
            foreach (var item in productCateModel.ListAll())
            {
                for (int i = 1; i <= item.Level; i++)
                {
                    item.Name = " - " + item.Name;
                }
                if (item.Level == 1)
                {
                    productCateList.Insert(0, new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
                }
                else
                {
                    var cate = productCateModel.GetByID(item.ParentID);
                    var index = productCateList.FindIndex(x => x.Value == cate.ID.ToString()) + 1;
                    productCateList.Insert(index, new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
                }
            }
            return productCateList;
        }

        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase uploadFiles)
        {
            string returnImagePath = string.Empty;
            string filename, extension, imageName, imageSavePath;
            if (uploadFiles.ContentLength > 0)
            {
                filename = Path.GetFileNameWithoutExtension(uploadFiles.FileName);
                extension = Path.GetExtension(uploadFiles.FileName);
                imageName = DateTime.Now.ToString("yymmssfff")+"_"+filename;
                imageSavePath = Server.MapPath("/Assets/images/uploads/summerNote/") + imageName + extension;
                uploadFiles.SaveAs(imageSavePath);
                returnImagePath = "/Assets/images/uploads/summerNote/" + imageName + extension;
            }
            return Json(Convert.ToString(returnImagePath), JsonRequestBehavior.AllowGet);
        }
        public bool CheckAlias(string alias, long productID)
        {
            var flag = true;
            if (alias == null)
            {
                flag = !productModel.CheckAlias(alias, productID);
            }
            return flag;
        }
    }
}