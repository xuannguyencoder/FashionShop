using FashionShop.Models.EF;
using System.Collections.Generic;
using System.Linq;

namespace FashionShop.Models
{
    public class MenuTypeModel
    {
        private FashionShopEntities db = null;

        public MenuTypeModel()
        {
            db = new FashionShopEntities();
        }

        public MenuType GetByID(int? ID)
        {
            return db.MenuTypes.Find(ID);
        }

        public List<MenuType> ListAll()
        {
            return db.MenuTypes.ToList();
        }

        public List<MenuType> ListAllByPosition(int PositionID)
        {
            var menuTypes = db.MenuTypes.Where(x => x.PositionID == PositionID).ToList();
            return menuTypes;
        }

        public List<MenuType> ListOrderByDisplayOrder()
        {
            return db.MenuTypes.OrderBy(x => x.DisplayOrder).ToList();
        }

        public MenuType FirstOrDefault()
        {
            return db.MenuTypes.FirstOrDefault();
        }

        public int Insert(MenuType menuType)
        {
            try
            {
                var temp = db.MenuTypes.Where(x => x.PositionID == menuType.PositionID).ToList().LastOrDefault();
                if (temp == null) //trường hợp danh mục rỗng
                    menuType.DisplayOrder = 1;
                else
                    menuType.DisplayOrder = temp.ID + 1;
                db.MenuTypes.Add(menuType);
                db.SaveChanges();
                return menuType.ID;
            }
            catch
            {
                return 0;
            }
        }

        public bool Delete(int ID)
        {
            try
            {
                MenuType menuType = db.MenuTypes.Find(ID);
                db.MenuTypes.Remove(menuType);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(MenuType menuType)
        {
            try
            {
                var model = db.MenuTypes.Find(menuType.ID);
                model.Name = menuType.Name;
                model.Status = menuType.Status;
                db.SaveChanges();
                if (menuType.DisplayOrder != model.DisplayOrder)
                {
                    UpdateDisLayOrder(menuType.DisplayOrder, model.DisplayOrder, model, menuType.PositionID);
                }
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
                var menuType = db.MenuTypes.Find(ID);
                if (menuType.Status == true)
                    menuType.Status = false;
                else
                    menuType.Status = true;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateDisLayOrder(int ptNew, int ptOul, MenuType menuTy, int positionID)
        {
            if (menuTy.PositionID == positionID) // xét trường hợp thay đổi vị trí trong cùng 1 danh mục
            {
                var menuTypes = new List<MenuType>();
                if (ptNew < ptOul)
                {
                    menuTypes = db.MenuTypes.Where(x => x.DisplayOrder > ptNew & x.DisplayOrder < ptOul & x.PositionID == positionID)
                        .OrderBy(x => x.DisplayOrder).ToList(); // ds các row đứng sau row được chọn

                    var pt = ptNew; // lưu vị trí muốn thay đổi
                    foreach (var item in menuTypes)
                    {
                        if (pt + 1 == item.DisplayOrder) // xét xem vị trí tiếp đến có liền nhau hay không
                        {
                            var menuType = db.MenuTypes.Find(item.ID);
                            menuType.DisplayOrder += 1;
                            db.SaveChanges();
                            pt++;
                        }
                        else
                            break; // nếu có khoảng trống thì không cần đẩy lùi danh sách
                    }
                    var menuTypeCurrent = db.MenuTypes.Find(menuTy.ID);
                    menuTypeCurrent.DisplayOrder = ptNew + 1;
                    db.SaveChanges();
                }
                else
                {
                    menuTypes = db.MenuTypes.Where(x => x.DisplayOrder > ptOul & x.DisplayOrder <= ptNew & x.PositionID == positionID)
                        .OrderByDescending(x => x.DisplayOrder).ToList();

                    var pt = ptNew;
                    foreach (var item in menuTypes)
                    {
                        if (pt == item.DisplayOrder)
                        {
                            var menuType = db.MenuTypes.Find(item.ID);
                            menuType.DisplayOrder -= 1;
                            db.SaveChanges();
                            pt--;
                        }
                        else
                            break;// nếu có khoảng trống thì không cần đẩy lùi danh sách
                    }

                    var menuTypeCurrent = db.MenuTypes.Find(menuTy.ID);
                    menuTypeCurrent.DisplayOrder = ptNew;
                    db.SaveChanges();
                }
            }
            else
            {
                var menuType = db.MenuTypes.Find(menuTy.ID);
                var menuTypeByPT = db.MenuTypes.Where(x => x.PositionID == menuType.PositionID).ToList()
                    .LastOrDefault(x => x.PositionID == menuType.PositionID);
                if (menuTypeByPT == null) //trường hợp dữ liệu rỗng
                    menuType.DisplayOrder = 1;
                else
                    menuType.DisplayOrder = menuTypeByPT.ID + 1;
                db.SaveChanges();
            }
        }
    }
}