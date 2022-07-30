using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionShop.Models.Common;
using FashionShop.Models.EF;
namespace FashionShop.Models
{
    public class MenuModel
    {
        FashionShopEntities db = null;
        public MenuModel()
        {
            db = new FashionShopEntities();
        }
        public Menu GetByID(long? ID)
        {
            return db.Menus.Find(ID); 
        }
        public List<Menu> ListOrderByDisplayOrder()
        {
            return db.Menus.OrderBy(x => x.DisplayOrder).ToList();
        }
        public Menu GetByAlias(string alias)
        {
            return db.Menus.Where(x=>x.Type !="url").FirstOrDefault(x => x.Alias == alias);
        }
        public string FixAlias(string alias, int MenuID)
        {
            bool flag = true;
            int i = 1;
            string alias1 = alias;
            while (flag)
            {
                if (!CheckAlias(alias1, MenuID))
                    break;
                else
                {
                    alias1 = alias + i;
                }
                i++;
            }
            return alias1;
        }
        public bool CheckAlias(string alias, int MenuID)
        {
            if (alias != null)
            {
                var menu = db.Menus.SingleOrDefault(x => x.Alias == alias);
                if (menu == null)
                    return false;
                else if (menu.ID == MenuID)
                    return false;
                return true;
            }
            else
                return false;
        }
        public List<Menu> ListAll()
        {
            return db.Menus.ToList();
        }
        public List<Menu> ListAllOrderBy()
        {
            return db.Menus.OrderByDescending(x => x.DisplayOrder).ToList().OrderBy(x => x.Level).ToList();
        }
        public long Insert(Menu menu)
        {
            try
            {
                if (menu.ParentID == 0)
                {
                    menu.ParentID = null;
                    menu.Level = 1;
                }
                else
                {
                    var temp_menu = db.Menus.Find(menu.ParentID);
                    menu.Level = temp_menu.Level + 1;
                }
                //Alias
                ConvertData convertData = new ConvertData();
                menu.Alias = convertData.ConvertToAlias(menu.Name);
                menu.Alias = FixAlias(menu.Alias, menu.ID); //fixbug Alias lại nếu bị trùng khi thêm
                //display order
                var menus = db.Menus.Where(x => x.ParentID == menu.ParentID && x.MenuTypeID == menu.MenuTypeID).ToList();
                if (menus.Count() == 0) //trường hợp danh mục rỗng
                    menu.DisplayOrder = 1;
                else
                    menu.DisplayOrder = menus.Max(x => x.DisplayOrder) + 1;
                db.Menus.Add(menu);
                db.SaveChanges();
                return menu.ID;
            }
            catch
            {
                return 0;
            }
        }
        public bool Update(Menu menu)
        {
            try
            {
                var model = db.Menus.Find(menu.ID);
                model.Name = menu.Name;
                model.Type = menu.Type;
                model.Link = menu.Link;
                model.Status = menu.Status;
                model.MenuTypeID = menu.MenuTypeID;
                var parent_old = model.ParentID;
                if (menu.ParentID == 0)
                {
                    model.ParentID = null;
                    model.Level = 1;
                }
                else
                {
                    var temp = db.Menus.Find(menu.ParentID);
                    model.Level = temp.Level + 1;
                    model.ParentID = temp.ID;
                }
                //Alias
                ConvertData convertData = new ConvertData();
                model.Alias = convertData.ConvertToAlias(menu.Name);
                model.Alias = FixAlias(menu.Alias, menu.ID); //fixbug Alias lại nếu bị trùng khi thêm
                db.SaveChanges();
                if (menu.DisplayOrder != model.DisplayOrder && model.ParentID == parent_old) // đảm bảo vị trí thay đổi trong cùng 1 danh mục và không phải và vị trí cũ
                {
                    UpdateDisLayOrder(menu.DisplayOrder, model.DisplayOrder, model);
                }
                else if (model.ParentID != parent_old)
                {
                    var tem_menu = db.Menus.Find(model.ID);
                    var menus = db.Menus.Where(x => x.ParentID == model.ParentID && x.MenuTypeID == model.MenuTypeID && x.ID != model.ID).ToList();
                    if (menus.Count() == 0) //trường hợp danh mục rỗng
                        tem_menu.DisplayOrder = 1;
                    else
                        tem_menu.DisplayOrder = menus.Max(x => x.DisplayOrder) + 1;
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void UpdateDisLayOrder(int ptNew, int ptOul, Menu menu)
        {
            var menus = new List<Menu>();
            if (ptNew < ptOul)
            {
                menus = db.Menus.Where(x => x.ParentID == menu.ParentID & x.DisplayOrder > ptNew & x.DisplayOrder < ptOul)
                    .OrderBy(x => x.DisplayOrder).ToList(); // ds menu cùng cha với vị trí đối tượng được chọn
                var pt = ptNew; // lưu vị trí muốn thay đổi
                foreach (var item in menus)
                {
                    if (pt + 1 == item.DisplayOrder) // xét xem vị trí tiếp đến có liền nhau hay không
                    {
                        var menuType = db.Menus.Find(item.ID);
                        menuType.DisplayOrder += 1;
                        db.SaveChanges();
                        pt++;
                    }
                    else
                        break; // nếu có khoảng trống thì không cần đẩy lùi danh sách
                }
                var menu2 = db.Menus.Find(menu.ID);
                menu2.DisplayOrder = ptNew + 1;
                db.SaveChanges();
            }
            else
            {
                menus = db.Menus.Where(x => x.ParentID == menu.ParentID & x.DisplayOrder > ptOul & x.DisplayOrder <= ptNew)
                    .OrderByDescending(x => x.DisplayOrder).ToList();

                var pt = ptNew;
                foreach (var item in menus)
                {
                    if (pt == item.DisplayOrder)
                    {
                        var menuType = db.Menus.Find(item.ID);
                        menuType.DisplayOrder -= 1;
                        db.SaveChanges();
                        pt--;
                    }
                    else
                        break;// nếu có khoảng trống thì không cần đẩy lùi danh sách
                }
                var menu2 = db.Menus.Find(menu.ID);
                menu2.DisplayOrder = ptNew;
                db.SaveChanges();
            }
        }
        public bool Delete(long? ID)
        {
            try
            {
                var menu = db.Menus.Find(ID);
                db.Menus.Remove(menu);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateStatus(int? ID)
        {
            try
            {
                var menu = db.Menus.Find(ID);
                if (menu.Status == true)
                    menu.Status = false;
                else
                    menu.Status = true;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateLevel(int menuId)
        {
            try
            {
                Menu menu = new Menu();
                using (var db = new FashionShopEntities())
                {
                    menu = db.Menus.Find(menuId);
                }
                foreach (var item in db.Menus.Where(x=>x.ParentID== menu.ID))
                {

                    var tempMenu = db.Menus.Find(item.ID);
                    tempMenu.Level = menu.Level + 1;
                    tempMenu.MenuTypeID = menu.MenuTypeID;
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}