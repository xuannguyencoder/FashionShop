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
    public class ArticleController : BaseController
    {
        private ArticleModel articleModel = new ArticleModel();
        private ArticleCategoryModel articleCateModel = new ArticleCategoryModel();

        public ActionResult Index()
        {
            var model = articleModel.ListAll();
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.ArticleCateList = DDLArticleCate();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection form, HttpPostedFileBase Image, Content article)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    string filename = Path.GetFileName(Image.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + "_" + filename;
                    string path = Path.Combine(Server.MapPath("~/Assets/images/uploads/articles/"), _filename);

                    string extension = Path.GetExtension(Image.FileName);
                    if (CheckExtension(extension))
                    {
                        if (Image.ContentLength <= 2097152)
                        {
                            if (CheckAlias(article.Alias, article.ID))
                            {
                                article.Image = _filename;
                                var articleID = articleModel.Insert(article);
                                if (articleID > 0)
                                {
                                    Image.SaveAs(path);
                                    TempData["Message"] = "Thêm bài viết thành công";
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
                        else
                        {
                            ModelState.AddModelError("Image", "Kích thước ảnh lớn hơn 2MB");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Image", "Đuôi ảnh không hợp lệ");
                    }
                }
                else
                {
                    if (CheckAlias(article.Alias, article.ID))
                    {
                        var articleID = articleModel.Insert(article);
                        if (articleID > 0)
                        {
                            TempData["Message"] = "Thêm bài viết thành công";
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
            }
            ViewBag.ArticleCateList = DDLArticleCate();
            return View(article);
        }

        public bool CheckAlias(string alias, long articleID)
        {
            var flag = true;
            if (alias == null)
            {
                flag = !articleModel.CheckAlias(alias, articleID);
            }
            return flag;
        }

        public ActionResult Edit(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var article = articleModel.GetByID(ID);
            if (article == null)
            {
                return View("_Error404");
            }
            Session["ImageName"] = article.Image;
            ViewBag.ArticleCateList = DDLArticleCate();
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Content article, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    string filename = Path.GetFileName(Image.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + "_" + filename;
                    string path = Path.Combine(Server.MapPath("~/Assets/images/uploads/articles/"), _filename);

                    string extension = Path.GetExtension(Image.FileName);
                    if (CheckExtension(extension))
                    {
                        if (Image.ContentLength <= 2097152)
                        {
                            if (CheckAlias(article.Alias, article.ID))
                            {
                                article.Image = _filename;
                                var result = articleModel.Update(article);
                                if (result)
                                {
                                    Image.SaveAs(path);
                                    string oldImgPath = Request.MapPath("~/Assets/images/uploads/articles/" + Session["ImageName"]);
                                    if (System.IO.File.Exists(oldImgPath))
                                    {
                                        System.IO.File.Delete(oldImgPath);
                                    }
                                    TempData["Message"] = "Cập nhật bài viết thành công";
                                    TempData["Status"] = "success";
                                    return RedirectToAction("Index");
                                }
                            }
                            else
                                ModelState.AddModelError("Alias", "Alias này đã tồn tại");
                        }
                        else
                            ModelState.AddModelError("Image", "Kích thước ảnh lớn hơn 2MB");
                    }
                    else
                        ModelState.AddModelError("Image", "Đuôi ảnh không hợp lệ");
                }
                else
                {
                    if (CheckAlias(article.Alias, article.ID))
                    {
                        var result = articleModel.Update(article);
                        if (result)
                        {
                            TempData["Message"] = "Cập nhật bài viết thành công";
                            TempData["Status"] = "success";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                        ModelState.AddModelError("Alias", "Alias này đã tồn tại");
                }
            }
            ViewBag.ArticleCateList = DDLArticleCate();
            return View(article);
        }

        public ActionResult Delete(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var article = articleModel.GetByID(ID);
            if (article == null)
            {
                return View("_Error404");
            }
            var result = articleModel.Delete(ID);
            if (result)
            {
                string currentImg = Request.MapPath("~/Assets/images/uploads/articles/" + article.Image);
                if (System.IO.File.Exists(currentImg))
                {
                    System.IO.File.Delete(currentImg);
                }
                TempData["Message"] = "Xóa bài viết thành công";
                TempData["Status"] = "success";
                return RedirectToAction("Index");
            }
            return View(); //error
        }

        public ActionResult UpdateStatus(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var result = articleModel.UpdateStatus(ID);
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

        private List<SelectListItem> DDLArticleCate()
        {
            var articleCateList = new SelectList(new List<SelectListItem>()).ToList();
            foreach (var item in articleCateModel.ListAll())
            {
                for (int i = 1; i <= item.Level; i++)
                {
                    item.Name = " - " + item.Name;
                }
                if (item.Level == 1)
                {
                    articleCateList.Insert(0, new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
                }
                else
                {
                    var cate = articleCateModel.GetByID(item.ParentID);
                    var index = articleCateList.FindIndex(x => x.Value == cate.ID.ToString()) + 1;
                    articleCateList.Insert(index, new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
                }
            }
            return articleCateList;
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
                imageName = DateTime.Now.ToString("yymmssfff") + "_" + filename;
                imageSavePath = Server.MapPath("/Assets/images/uploads/summerNote/") + imageName + extension;
                uploadFiles.SaveAs(imageSavePath);
                returnImagePath = "/Assets/images/uploads/summerNote/" + imageName + extension;
            }
            return Json(Convert.ToString(returnImagePath), JsonRequestBehavior.AllowGet);
        }
    }
}