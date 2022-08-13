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
    public class ProductDetailController : BaseController
    {
        // GET: Admin/ProductDetail
        private ProductModel productModel = new ProductModel();

        private ProductDetailModel proDetailModel = new ProductDetailModel();
        private SizeModel sizeModel = new SizeModel();
        private ColorModel colorModel = new ColorModel();

        public ActionResult Index(long? ID)
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
            ProductImageModel proImageModel = new ProductImageModel();

            ViewBag.proImages = proImageModel.ListByProductID(ID);
            ViewBag.ProductID = product.ID;
            var productDetails = proDetailModel.GetByProductID(ID);
            return View(productDetails);
        }

        public ActionResult Create(long? ID)
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

            var SizeList = sizeModel.ListAll();
            ViewBag.SizeList = new SelectList(SizeList, "ID", "Code");
            ViewBag.ColorList = DDLColor();

            ViewBag.product = product;
            return View();
        }

        public ActionResult Edit(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var proDetail = proDetailModel.GetByID(ID);
            if (proDetail == null)
            {
                return View("_Error404");
            }

            var SizeList = sizeModel.ListAll();
            ViewBag.SizeList = new SelectList(SizeList, "ID", "Code");
            ViewBag.ColorList = DDLColor();
            //Show info product

            return View(proDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                bool flag = true;
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    if (file.FileName != "")
                    {
                        string extension = Path.GetExtension(file.FileName);
                        if (!CheckExtension(extension))
                        {
                            flag = false;
                            ModelState.AddModelError("Image", "Đuôi ảnh " + file.FileName + " không hợp lệ");
                        }
                        if (file.ContentLength > 2097152)
                        {
                            flag = false;
                            ModelState.AddModelError("Image", "Kích thước ảnh " + file.FileName + " lớn hơn 2MB");
                        }
                    }
                }
                if (flag)
                {
                    productDetail.Quantity = productDetail.Quantity.GetValueOrDefault(0);
                    var result = proDetailModel.UpdateQuantity(productDetail);
                    if (result)
                    {
                        InsertImage(files, productDetail.ProductID, productDetail.ColorCode);
                        TempData["Message"] = "Cập nhật chi tiết sản phẩm thành công";
                        TempData["Status"] = "success";
                        return RedirectToAction("Index", new { ID = productDetail.ProductID });
                    }
                }
            }
            ViewBag.SizeList = new SelectList(sizeModel.ListAll(), "ID", "Code");
            ViewBag.ColorList = DDLColor();
            return View(productDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form, [Bind(Include = "Quantity,ProductID, SizeID, ColorCode")] ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                bool flag = true;
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    if (file.FileName != "")
                    {
                        string extension = Path.GetExtension(file.FileName);
                        if (!CheckExtension(extension))
                        {
                            flag = false;
                            ModelState.AddModelError("Image", "Đuôi ảnh " + file.FileName + " không hợp lệ");
                        }
                        if (file.ContentLength > 2097152)
                        {
                            flag = false;
                            ModelState.AddModelError("Image", "Kích thước ảnh " + file.FileName + " lớn hơn 2MB");
                        }
                    }
                }
                if (flag)
                {
                    var proDetail = proDetailModel.GetByProperties(productDetail.ProductID, productDetail.SizeID, productDetail.ColorCode);
                    if (proDetail == null)
                    {
                        long proDetailID = proDetailModel.Insert(productDetail);
                        if (proDetailID > 0)
                        {
                            InsertImage(files, productDetail.ProductID, productDetail.ColorCode);
                            TempData["Message"] = "Thêm chi tiết sản phẩm thành công";
                            TempData["Status"] = "success";
                            if (form["btnAdd"] != null)
                                return RedirectToAction("Index", new { ID = productDetail.ProductID });
                            else
                                return RedirectToAction("Create", new { ID = productDetail.ProductID });
                        }
                    }
                    else
                    {
                        proDetail.Quantity = proDetail.Quantity.GetValueOrDefault(0) + productDetail.Quantity.GetValueOrDefault(0);
                        var result = proDetailModel.UpdateQuantity(proDetail);
                        if (result)
                        {
                            InsertImage(files, productDetail.ProductID, productDetail.ColorCode);
                            TempData["Message"] = "Thêm chi tiết sản phẩm thành công";
                            TempData["Status"] = "success";
                            if (form["btnAdd"] != null)
                                return RedirectToAction("Index", new { ID = productDetail.ProductID });
                            else
                                return RedirectToAction("Create", new { ID = productDetail.ProductID });
                        }
                    }
                }
            }
            ViewBag.SizeList = new SelectList(sizeModel.ListAll(), "ID", "Code");
            ViewBag.ColorList = DDLColor();

            var product = productModel.GetByID(productDetail.ProductID);
            ViewBag.product = product;
            return View(productDetail);
        }

        private List<SelectListItem> DDLColor()
        {
            var ColorList = from h in colorModel.ListAll()
                            select new SelectListItem
                            {
                                Value = h.Code.ToString(),
                                Text = h.Code + " - " + h.Name
                            };
            return new SelectList(ColorList, "Value", "Text").ToList();
        }

        private void InsertImage(HttpFileCollectionBase files, long productID, string colorCode)
        {
            ProductImageModel proImageModel = new ProductImageModel();
            ProductImage productImage = new ProductImage();
            productImage.ProductID = productID;
            productImage.ColorCode = colorCode;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                if (file.FileName != "")
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + "_" + filename;
                    string path = Path.Combine(Server.MapPath("~/Assets/images/uploads/products/"), _filename);
                    productImage.Image = _filename;
                    long proImageID = proImageModel.Insert(productImage);
                    if (proImageID > 0)
                    {
                        file.SaveAs(path);
                    }
                }
            }
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
    }
}