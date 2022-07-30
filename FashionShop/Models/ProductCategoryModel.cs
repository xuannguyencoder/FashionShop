using FashionShop.Models.Common;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Models
{
    public class ProductCategoryModel
    {
        FashionShopEntities db = null;
        HttpContext context = HttpContext.Current;
        public ProductCategoryModel()
        {
            db = new FashionShopEntities();
        }
        public ProductCategory GetByID(long? ID)
        {
            return db.ProductCategories.Find(ID);
        }
        public int GetLevelByID(long? ID)
        {
            return db.ProductCategories.Find(ID).Level;
        }
        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.ToList();
        }
        public List<ProductCategory> ListOrderByDisplayOrder()
        {
            return db.ProductCategories.OrderBy(x => x.DisplayOrder).ToList();
        }
        public List<SelectListItem> DDLProductCate()
        {
            var productCateList = new SelectList(new List<SelectListItem>()).ToList();
            foreach (var item in db.ProductCategories.OrderByDescending(x => x.DisplayOrder).ToList().OrderBy(x => x.Level).ToList())
            {
                string name = "";
                for (int i = 1; i <= item.Level; i++)
                {
                    name += " - ";
                }
                if (item.Level == 1)
                {
                    productCateList.Insert(0, new SelectListItem { Text = name + item.Name, Value = item.ID.ToString() });
                }
                else
                {
                    var cate = db.ProductCategories.Find(item.ParentID);
                    var index = productCateList.FindIndex(x => x.Value == cate.ID.ToString()) + 1;
                    productCateList.Insert(index, new SelectListItem { Text = name+item.Name , Value = item.ID.ToString() });
                }
            }
            return productCateList;
        }

        public List<ProductCategory> ListAllByFormat()
        {
            List<ProductCategory> productCates = new List<ProductCategory>();
            foreach (var item in db.ProductCategories.OrderByDescending(x => x.DisplayOrder).ToList().OrderBy(x => x.Level).ToList())
            {
                string name = "";
                for (int i = 1; i <= item.Level; i++)
                {
                    name += " - ";
                }
                var item2 = item;
                item2.Name = name +item.Name;
                if (item.Level == 1)
                {
                    productCates.Insert(0, item2);
                }
                else
                {
                    var cate = db.ProductCategories.Find(item.ParentID);
                    var index = productCates.FindIndex(x => x.ID == cate.ID) + 1;
                    productCates.Insert(index, item);
                }
            }
            return productCates;
        }

        public List<ProductCategory> ListAllOrderByLevel()
        {
            return db.ProductCategories.OrderBy(x=>x.Level).ToList();
        }
        public List<ProductCategory> ListAllByLevel(int Level)
        {
            return db.ProductCategories.Where(x => x.Level == Level).ToList();
        }

        public long Insert(ProductCategory productCate)
        {
            try
            {
                if (productCate.ParentID == 0)
                {
                    productCate.ParentID = null;
                    productCate.Level = 1;
                }
                else
                {
                    var temp = db.ProductCategories.Find(productCate.ParentID);
                    productCate.Level = temp.Level + 1;
                }
                ConvertData convertData = new ConvertData();
                productCate.Alias = convertData.ConvertToAlias(productCate.Name);
                productCate.Alias = FixAlias(productCate.Alias); //fixbug Alias lại nếu bị trùng khi thêm
                //display order
                var cates = db.ProductCategories.Where(x => x.ParentID == productCate.ParentID).ToList();
                if (cates.Count() == 0) //trường hợp danh mục rỗng
                    productCate.DisplayOrder = 1;
                else
                    productCate.DisplayOrder = cates.Max(x => x.DisplayOrder) + 1;
                var user = (UserLogin)context.Session[Constant.ADMIN_SESSION];
                productCate.CreatedBy = user.FirstName + user.LastName;
                productCate.CreatedDate = DateTime.Now;
                db.ProductCategories.Add(productCate);

                db.SaveChanges();
                return productCate.ID;
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
                var menu = db.ProductCategories.SingleOrDefault(x => x.Alias == alias);
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
        public bool Update(ProductCategory productCate)
        {
            try
            {
                var model = db.ProductCategories.Find(productCate.ID);
                ConvertData convertData = new ConvertData();
                model.Alias = convertData.ConvertToAlias(productCate.Name);
                model.Name = productCate.Name;
                model.Status = productCate.Status;
                var parent_old = model.ParentID;
                if (productCate.ParentID == 0)
                {
                    model.ParentID = null;
                    model.Level = 1;
                }
                else
                {
                    using (var db = new FashionShopEntities())
                    {
                        var temp = db.ProductCategories.Find(productCate.ParentID);
                        model.Level = temp.Level + 1;
                        model.ParentID = temp.ID;
                    }
                }
                var user = (UserLogin)context.Session[Constant.ADMIN_SESSION];
                model.ModifiedBy = user.FirstName + user.LastName;
                model.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                if (productCate.DisplayOrder != model.DisplayOrder && model.ParentID == parent_old) // đảm bảo vị trí thay đổi trong cùng 1 danh mục và không phải là vị trí cũ
                {
                    UpdateDisLayOrder(productCate.DisplayOrder, model.DisplayOrder, model);
                }
                else
                {
                    var cate = db.ProductCategories.Find(model.ID);
                    var cates = db.ProductCategories.Where(x => x.ParentID == model.ParentID && x.ID != model.ID).ToList();
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
                ProductCategory cate = new ProductCategory();
                using (var db = new FashionShopEntities())
                {
                    cate = db.ProductCategories.Find(cateID);
                }
                foreach (var item in db.ProductCategories.Where(x => x.ParentID == cate.ID))
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
        private void UpdateDisLayOrder(int ptNew, int ptOul, ProductCategory cate)
        {
            var productCates = new List<ProductCategory>();
            if (ptNew < ptOul)
            {
                productCates = db.ProductCategories.Where(x => x.ParentID == cate.ParentID & x.DisplayOrder > ptNew & x.DisplayOrder < ptOul)
                    .OrderBy(x => x.DisplayOrder).ToList(); // ds menu cùng cha với vị trí đối tượng được chọn
                var pt = ptNew; // lưu vị trí muốn thay đổi
                foreach (var item in productCates)
                {
                    if (pt + 1 == item.DisplayOrder) // xét xem vị trí tiếp đến có liền nhau hay không
                    {
                        var cate2 = db.ProductCategories.Find(item.ID);
                        cate2.DisplayOrder += 1;
                        db.SaveChanges();
                        pt++;
                    }
                }
                var cateCurrent = db.ProductCategories.Find(cate.ID);
                cateCurrent.DisplayOrder = ptNew + 1;
                db.SaveChanges();
            }
            else
            {
                productCates = db.ProductCategories.Where(x => x.ParentID == cate.ParentID & x.DisplayOrder > ptOul & x.DisplayOrder <= ptNew)
                    .OrderByDescending(x => x.DisplayOrder).ToList();
                var pt = ptNew;
                foreach (var item in productCates)
                {
                    if (pt == item.DisplayOrder)
                    {
                        var cate2 = db.ProductCategories.Find(item.ID);
                        cate2.DisplayOrder -= 1;
                        db.SaveChanges();
                        pt--;
                    }
                    else
                        break; // nếu có khoảng trống thì không cần đẩy lùi danh sách
                }
                var cateCurrent = db.ProductCategories.Find(cate.ID);
                cateCurrent.DisplayOrder = ptNew;
                db.SaveChanges();
            }
        }
        public bool Delete(long? ID)
        {
            try
            {
                var productCate = db.ProductCategories.Find(ID);
                db.ProductCategories.Remove(productCate);
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
                var cate = db.ProductCategories.Find(ID);
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