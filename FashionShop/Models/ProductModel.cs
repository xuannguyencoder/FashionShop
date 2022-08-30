using FashionShop.Models.Common;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class ProductModel
    {
        private FashionShopEntities db = null;
        private HttpContext context = HttpContext.Current;

        public ProductModel()
        {
            db = new FashionShopEntities();
        }

        public Product GetByID(long? ID)
        {
            return db.Products.Find(ID);
        }

        public List<Product> ListAll()
        {
            return db.Products.ToList();
        }

        public List<Product> ListAllByCateID(long CateID)
        {
            return db.Products.Where(x => x.ProductCategoryID == CateID).ToList();
        }

        public Product GetByAlias(string alias)
        {
            return db.Products.FirstOrDefault(x => x.Alias == alias);
        }

        public long Insert(Product product)
        {
            try
            {
                ConvertData convertData = new ConvertData();
                product.Alias = convertData.ConvertToAlias(product.Name);
                product.Alias = FixAlias(product.Alias, 0); //fixbug Alias lại nếu bị trùng khi thêm

                product.Price = product.Price.GetValueOrDefault(0);
                product.PromotionPrice = product.PromotionPrice.GetValueOrDefault(0);

                var user = (UserLogin)context.Session[Constant.ADMIN_SESSION];
                product.CreatedBy = user.FirstName + user.LastName;
                product.CreatedDate = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();
                return product.ID;
            }
            catch
            {
                return 0;
            }
        }

        public string FixAlias(string alias, long ProductID)
        {
            bool flag = true;
            int i = 1;
            string alias1 = alias;
            while (flag)
            {
                if (!CheckAlias(alias1, ProductID))
                    break;
                else
                {
                    alias1 = alias + i;
                }
                i++;
            }
            return alias1;
        }

        public bool CheckAlias(string alias, long ProductID)
        {
            if (alias != null)
            {
                var product = db.Products.SingleOrDefault(x => x.Alias == alias);
                if (product == null)
                    return false;
                else if (product.ID == ProductID)
                    return false;
                return true;
            }
            else
                return false;
        }

        public bool Update(Product product)
        {
            try
            {
                var model = db.Products.Find(product.ID);
                ConvertData convertData = new ConvertData();
                model.Alias = convertData.ConvertToAlias(product.Name);
                model.Alias = FixAlias(product.Alias, product.ID); //fixbug Alias lại nếu bị trùng khi thêm

                model.Name = product.Name;
                model.Detail = product.Detail;
                model.Description = product.Description;
                model.Price = product.Price;
                model.PromotionPrice = product.PromotionPrice;
                model.ProductCategoryID = product.ProductCategoryID;
                model.Status = product.Status;

                var user = (UserLogin)context.Session[Constant.ADMIN_SESSION];
                model.ModifiedBy = user.FirstName + user.LastName;
                model.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(long? ID)
        {
            try
            {
                var product = db.Products.Find(ID);
                db.Products.Remove(product);
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
                var product = db.Products.Find(ID);
                if (product.Status == true)
                    product.Status = false;
                else
                    product.Status = true;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Product> Search(string productName)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(productName))
            {
                model = model.Where(x => x.Name.Contains(productName));
            }
            return model.Where(x => x.Status == true && x.ProductCategory.Status == true).ToList();
        }
    }
}