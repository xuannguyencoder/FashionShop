using FashionShop.Models;
using FashionShop.Models.EF;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        private MenuModel menuModel = new MenuModel();
        private MenuTypeModel menuTypeModel = new MenuTypeModel();

        public ActionResult Index()
        {
            var model = MenuListDisplayOrder();
            return View(model);
        }

        public ActionResult Create()
        {
            var menuType = menuTypeModel.FirstOrDefault();
            if (menuType != null)
            {
                ViewBag.MenuList = DDLMenu(menuType.ID);
            }
            else
            {
                ViewBag.MenuList = DDLMenu(0);
            }
            ViewBag.MenuTypeList = new SelectList(menuTypeModel.ListAll(), "ID", "Name").ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form, Menu menu)
        {
            if (ModelState.IsValid)
            {
                if (menu.Type != "url")
                    menu.Link = menu.Type + "/" + form["CategoryID"].ToString();
                if (CheckAlias(menu.Alias, menu.ID))
                {
                    var result = menuModel.Insert(menu);
                    if (result > 0)
                    {
                        TempData["Message"] = "Thêm mục menu thành công";
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
            ViewBag.MenuList = DDLMenu(menu.MenuTypeID);
            ViewBag.MenuTypeList = new SelectList(menuTypeModel.ListAll(), "ID", "Name").ToList();
            return View();
        }

        public ActionResult Edit(long? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var menu = menuModel.GetByID(ID);
            if (menu == null)
            {
                return View("_Error404");
            }

            ViewBag.MenuList = DDLMenu(menu.MenuTypeID);
            ViewBag.MenuTypeList = new SelectList(menuTypeModel.ListAll(), "ID", "Name").ToList();

            var menus = new SelectList(menuModel.ListOrderByDisplayOrder().Where(x => x.ParentID == menu.ParentID && x.MenuTypeID == menu.MenuTypeID), "DisplayOrder", "Name").ToList();
            menus.Insert(0, (new SelectListItem { Text = "Vị trí đầu", Value = "0" }));
            ViewBag.MenuDisplayOrderList = menus;
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form, Menu menu)
        {
            if (ModelState.IsValid)
            {
                if (menu.ParentID != menu.ID)
                {
                    if (menu.Type != "url")
                        menu.Link = menu.Type + "/" + form["CategoryID"].ToString();
                    if (CheckAlias(menu.Alias, menu.ID))
                    {
                        var result = menuModel.Update(menu);
                        var result2 = menuModel.UpdateLevel(menu.ID);
                        if (result && result2)
                        {
                            TempData["Message"] = "Cập nhật mục menu thành công";
                            TempData["Status"] = "success";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Alias", "Alias này đã tồn tại");
                    }
                }
                else
                {
                    ModelState.AddModelError("ParentID", "Không được chọn danh mục cha là chính nó");
                }
            }
            ViewBag.MenuList = DDLMenu(menu.MenuTypeID);
            ViewBag.MenuTypeList = new SelectList(menuTypeModel.ListAll(), "ID", "Name").ToList();

            var menus = new SelectList(menuModel.ListOrderByDisplayOrder().Where(x => x.ParentID == menu.ParentID && x.MenuTypeID == menu.MenuTypeID), "DisplayOrder", "Name").ToList();
            menus.Insert(0, (new SelectListItem { Text = "Vị trí đầu", Value = "0" }));
            ViewBag.MenuDisplayOrderList = menus;
            return View(menu);
        }

        private List<Menu> MenuListDisplayOrder()
        {
            List<Menu> menus = new List<Menu>();
            foreach (var item in menuModel.ListAllOrderBy())
            {
                string name = "";
                for (int i = 1; i <= item.Level; i++)
                {
                    name += " - ";
                }
                var item2 = item;
                item2.Name = name + item.Name;
                if (item.Level == 1)
                {
                    menus.Insert(0, item2);
                }
                else
                {
                    var menu = menuModel.GetByID(item.ParentID);
                    var index = menus.FindIndex(x => x.ID == menu.ID) + 1;
                    menus.Insert(index, item);
                }
            }
            return menus;
        }

        private List<SelectListItem> DDLMenu(int? TypeID)
        {
            var menuList = new SelectList(new List<SelectListItem>()).ToList();
            foreach (var item in menuModel.ListAllOrderBy().Where(x => x.MenuTypeID == TypeID))
            {
                string name = "";
                for (int i = 1; i <= item.Level; i++)
                {
                    name += " - ";
                }
                if (item.Level == 1)
                {
                    menuList.Insert(0, new SelectListItem { Text = name + item.Name, Value = item.ID.ToString() });
                }
                else
                {
                    var menu = menuModel.GetByID(item.ParentID);
                    var index = menuList.FindIndex(x => x.Value == menu.ID.ToString()) + 1;
                    menuList.Insert(index, new SelectListItem { Text = name + item.Name, Value = item.ID.ToString() });
                }
            }
            menuList.Insert(0, (new SelectListItem { Text = "Đây là mục menu chính", Value = "0" }));
            return menuList;
        }

        public ActionResult UpdateStatus(int? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var result = menuModel.UpdateStatus(ID);
            if (result)
            {
                TempData["Message"] = "Cập nhật trạng thái thành công";
                TempData["Status"] = "success";
                return RedirectToAction("Index");
            }
            else
                return View("_Error404");
        }

        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return View("_Error400");
            }
            var result = menuModel.Delete(ID);
            if (result)
            {
                TempData["Message"] = "Xóa mục thành công";
                TempData["Status"] = "success";
                return RedirectToAction("Index");
            }
            else
                return View("_Error404");
        }

        [HttpGet]
        public JsonResult GetMenusByMenuType(int TypeID)
        {
            var data = DDLMenu(TypeID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetList(string Type, string Link)
        {
            long selected = 0;
            try
            {
                if (Link != null)
                {
                    string[] arrListStr = Link.Split('/');
                    selected = long.Parse(arrListStr[1]);
                }
            }
            catch
            {
            }
            List<SelectListItem> data = new List<SelectListItem>();
            if (Type == "danhmucsanpham")
            {
                ProductCategoryModel proCateModel = new ProductCategoryModel();
                data = new SelectList(proCateModel.ListAll(), "ID", "Name", selected).ToList();
            }
            else if (Type == "danhmucbaiviet")
            {
                ArticleCategoryModel articleCateModel = new ArticleCategoryModel();
                data = new SelectList(articleCateModel.ListAll(), "ID", "Name", selected).ToList();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public bool CheckAlias(string alias, int menuID)
        {
            var flag = true;
            if (alias == null)
            {
                flag = !menuModel.CheckAlias(alias, menuID);
            }
            return flag;
        }
    }
}