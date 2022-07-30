using FashionShop.Models.Common;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Models
{
    public class ArticleCategoryModel
    {
        FashionShopEntities db = null;
        HttpContext context = HttpContext.Current;
        public ArticleCategoryModel()
        {
            db = new FashionShopEntities();
        }
        public Category GetByID(long? ID)
        {
            return db.Categories.Find(ID);
        }
        public int GetLevelByID(long? ID)
        {
            return db.Categories.Find(ID).Level;
        }
        public List<Category> ListAll()
        {
            return db.Categories.ToList();
        }
        public List<Category> ListOrderByDisplayOrder()
        {
            return db.Categories.OrderBy(x => x.DisplayOrder).ToList();
        }
        public List<SelectListItem> DDLArticleCate()
        {
            var articleCateList = new SelectList(new List<SelectListItem>()).ToList();
            foreach (var item in db.Categories.OrderByDescending(x => x.DisplayOrder).ToList().OrderBy(x => x.Level).ToList())
            {
                string name = "";
                for (int i = 1; i <= item.Level; i++)
                {
                    name += " - ";
                }
                if (item.Level == 1)
                {
                    articleCateList.Insert(0, new SelectListItem { Text = name + item.Name, Value = item.ID.ToString() });
                }
                else
                {
                    var cate = db.Categories.Find(item.ParentID);
                    var index = articleCateList.FindIndex(x => x.Value == cate.ID.ToString()) + 1;
                    articleCateList.Insert(index, new SelectListItem { Text = name + item.Name, Value = item.ID.ToString() });
                }
            }
            return articleCateList;
        }

        public List<Category> ListAllByFormat()
        {
            List<Category> articleCates = new List<Category>();
            foreach (var item in db.Categories.OrderByDescending(x => x.DisplayOrder).ToList().OrderBy(x => x.Level).ToList())
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
                    articleCates.Insert(0, item2);
                }
                else
                {
                    var cate = db.Categories.Find(item.ParentID);
                    var index = articleCates.FindIndex(x => x.ID == cate.ID) + 1;
                    articleCates.Insert(index, item);
                }
            }
            return articleCates;
        }

        public List<Category> ListAllOrderByLevel()
        {
            return db.Categories.OrderBy(x => x.Level).ToList();
        }
        public List<Category> ListAllByLevel(int Level)
        {
            return db.Categories.Where(x => x.Level == Level).ToList();
        }

        public long Insert(Category articleCate)
        {
            try
            {
                if (articleCate.ParentID == 0)
                {
                    articleCate.ParentID = null;
                    articleCate.Level = 1;
                }
                else
                {
                    var temp = db.Categories.Find(articleCate.ParentID);
                    articleCate.Level = temp.Level + 1;
                }
                ConvertData convertData = new ConvertData();
                articleCate.Alias = convertData.ConvertToAlias(articleCate.Name);
                articleCate.Alias = FixAlias(articleCate.Alias); //fixbug Alias lại nếu bị trùng khi thêm
                //display order
                var cates = db.Categories.Where(x => x.ParentID == articleCate.ParentID).ToList();
                if (cates.Count() == 0) //trường hợp danh mục rỗng
                    articleCate.DisplayOrder = 1;
                else
                    articleCate.DisplayOrder = cates.Max(x => x.DisplayOrder) + 1;

                var user = (UserLogin)context.Session[Constant.ADMIN_SESSION];
                articleCate.CreatedBy = user.FirstName + user.LastName;
                articleCate.CreatedDate = DateTime.Now;
                db.Categories.Add(articleCate);
                db.SaveChanges();
                return articleCate.ID;
            }
            catch
            {
                return 0;
            }
        }
        public bool CheckAlias(string alias)
        {
            if (alias != null)
            {
                var menu = db.Categories.SingleOrDefault(x => x.Alias == alias);
                if (menu == null)
                    return false;
                return true;
            }
            else
                return false;
        }
        public string FixAlias(string alias)
        {
            bool flag = true;
            int i = 0;
            while (flag)
            {
                if (!CheckAlias(alias))
                    flag = false;
                else
                    i++;
            }
            if (i > 0)
                alias += i;
            return alias;
        }
        public bool Update(Category articleCate)
        {
            try
            {
                var model = db.Categories.Find(articleCate.ID);
                ConvertData convertData = new ConvertData();
                model.Alias = convertData.ConvertToAlias(articleCate.Name);
                model.Name = articleCate.Name;
                model.ModifiedDate = DateTime.Now;
                model.Status = articleCate.Status;
                var parent_old = model.ParentID;
                if (articleCate.ParentID == 0)
                {
                    model.ParentID = null;
                    model.Level = 1;
                }
                else
                {
                    using (var db = new FashionShopEntities())
                    {
                        var temp = db.Categories.Find(articleCate.ParentID);
                        model.Level = temp.Level + 1;
                        model.ParentID = temp.ID;
                    }
                }
                var user = (UserLogin)context.Session[Constant.ADMIN_SESSION];
                model.ModifiedBy = user.FirstName + user.LastName;
                model.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                if (articleCate.DisplayOrder != model.DisplayOrder && model.ParentID == parent_old) // đảm bảo vị trí thay đổi trong cùng 1 danh mục và không phải là vị trí cũ
                {
                    UpdateDisLayOrder(articleCate.DisplayOrder, model.DisplayOrder, model);
                }
                else
                {
                    var cate = db.Categories.Find(model.ID);
                    var cates = db.Categories.Where(x => x.ParentID == model.ParentID && x.ID != model.ID).ToList();
                    if (cates.Count() == 0) //trường hợp danh mục rỗng
                        cate.DisplayOrder = 1;
                    else
                        cate.DisplayOrder = cates.Max(x => x.DisplayOrder) + 1;
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateLevel(long cateID)
        {
            try
            {
                Category cate = new Category();
                using (var db = new FashionShopEntities())
                {
                    cate = db.Categories.Find(cateID);
                }
                foreach (var item in db.Categories.Where(x => x.ParentID == cate.ID))
                {

                    var cate2 = db.Menus.Find(item.ID);
                    cate2.Level = cate.Level + 1;
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void UpdateDisLayOrder(int ptNew, int ptOul, Category cate)
        {
            var articleCates = new List<Category>();
            if (ptNew < ptOul)
            {
                articleCates = db.Categories.Where(x => x.ParentID == cate.ParentID & x.DisplayOrder > ptNew & x.DisplayOrder < ptOul)
                    .OrderBy(x => x.DisplayOrder).ToList(); // ds menu cùng cha với vị trí đối tượng được chọn
                var pt = ptNew; // lưu vị trí muốn thay đổi
                foreach (var item in articleCates)
                {
                    if (pt + 1 == item.DisplayOrder) // xét xem vị trí tiếp đến có liền nhau hay không
                    {
                        var cate2 = db.Categories.Find(item.ID);
                        cate2.DisplayOrder += 1;
                        db.SaveChanges();
                        pt++;
                    }
                }
                var cateCurrent = db.Categories.Find(cate.ID);
                cateCurrent.DisplayOrder = ptNew + 1;
                db.SaveChanges();
            }
            else
            {
                articleCates = db.Categories.Where(x => x.ParentID == cate.ParentID & x.DisplayOrder > ptOul & x.DisplayOrder <= ptNew)
                    .OrderByDescending(x => x.DisplayOrder).ToList();
                var pt = ptNew;
                foreach (var item in articleCates)
                {
                    if (pt == item.DisplayOrder)
                    {
                        var cate2 = db.Categories.Find(item.ID);
                        cate2.DisplayOrder -= 1;
                        db.SaveChanges();
                        pt--;
                    }
                    else
                        break; // nếu có khoảng trống thì không cần đẩy lùi danh sách
                }
                var cateCurrent = db.Categories.Find(cate.ID);
                cateCurrent.DisplayOrder = ptNew;
                db.SaveChanges();
            }
        }
        public bool Delete(long? ID)
        {
            try
            {
                var articleCate = db.Categories.Find(ID);
                db.Categories.Remove(articleCate);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateStatus(long? ID)
        {
            try
            {
                var cate = db.Categories.Find(ID);
                if (cate.Status == true)
                    cate.Status = false;
                else
                    cate.Status = true;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}