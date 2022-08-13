using FashionShop.Models;
using FashionShop.Models.EF;
using System.Linq;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class ArticleCategoryController : BaseController
    {
        private ArticleCategoryModel articleCateModel = new ArticleCategoryModel();

        public ActionResult Index()
        {
            var model = articleCateModel.ListAllByFormat();
            return View(model);
        }

        public ActionResult Create()
        {
            var cateList = articleCateModel.DDLArticleCate();
            cateList.Insert(0, (new SelectListItem { Text = "Đây là mục menu chính", Value = "0" }));
            ViewBag.ArticleCateList = cateList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form, Category articleCate)
        {
            if (ModelState.IsValid)
            {
                var result = articleCateModel.Insert(articleCate);
                if (result > 0)
                {
                    TempData["Message"] = "Thêm danh mục bài viết thành công";
                    TempData["Status"] = "success";
                    if (form["btnAdd"] != null)
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Create");
                }
            }
            var cateList = articleCateModel.DDLArticleCate();
            cateList.Insert(0, (new SelectListItem { Text = "Đây là mục menu chính", Value = "0" }));
            ViewBag.ArticleCateList = cateList;
            return View(articleCate);
        }

        public ActionResult Edit(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var articleCate = articleCateModel.GetByID(ID);
            if (articleCate == null)
            {
                return View("_Error404");
            }
            //dropdownlist danh mục cha
            var cateList = articleCateModel.DDLArticleCate();
            cateList.Insert(0, (new SelectListItem { Text = "Đây là mục menu chính", Value = "0" }));
            ViewBag.ArticleCateList = cateList;

            //dropdownlist cùng 1 danh mục cha
            var cates = new SelectList(articleCateModel.ListOrderByDisplayOrder()
                .Where(x => x.ParentID == articleCate.ParentID), "DisplayOrder", "Name").ToList();
            cates.Insert(0, (new SelectListItem { Text = "Vị trí đầu", Value = "0" }));
            ViewBag.MenuDisplayOrderList = cates;
            return View(articleCate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category articleCate)
        {
            if (ModelState.IsValid)
            {
                var result = articleCateModel.Update(articleCate);
                var result2 = articleCateModel.UpdateLevel(articleCate.ID);
                if (result && result2)
                {
                    TempData["Message"] = "Cập nhật danh mục thành công";
                    TempData["Status"] = "success";
                    return RedirectToAction("Index");
                }
            }

            //dropdownlist danh mục cha
            var cateList = articleCateModel.DDLArticleCate();
            cateList.Insert(0, (new SelectListItem { Text = "Đây là mục menu chính", Value = "0" }));
            ViewBag.ArticleCateList = cateList;

            //dropdownlist cùng 1 danh mục cha
            var cates = new SelectList(articleCateModel.ListOrderByDisplayOrder()
                .Where(x => x.ParentID == articleCate.ParentID), "DisplayOrder", "Name").ToList();
            cates.Insert(0, (new SelectListItem { Text = "Vị trí đầu", Value = "0" }));
            ViewBag.MenuDisplayOrderList = cates;
            return View(articleCate);
        }

        public ActionResult Delete(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var articleCate = articleCateModel.GetByID(ID);
            if (articleCate == null)
            {
                return View("_Error404");
            }
            var result = articleCateModel.Delete(articleCate.ID);
            if (result)
            {
                TempData["Message"] = "Xóa danh mục bài viết thành công";
                TempData["Status"] = "success";
            }
            else
            {
                TempData["Message"] = "Xóa danh mục bài viết không thành công";
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
            var result = articleCateModel.UpdateStatus(ID);
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