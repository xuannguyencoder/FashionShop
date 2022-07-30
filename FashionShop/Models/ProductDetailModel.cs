using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class ProductDetailModel
    {
        FashionShopEntities db = null;
        HttpContext context = HttpContext.Current;
        public ProductDetailModel()
        {
            db = new FashionShopEntities();
        }
        public ProductDetail GetByID(long? ID)
        {
            return db.ProductDetails.Find(ID);
        }
        public ProductDetail GetByProperties(long? ProductID, int? SizeID, string ColorCode)
        {
            return db.ProductDetails.SingleOrDefault(
                x => x.ProductID == ProductID && x.SizeID == SizeID && x.ColorCode == ColorCode);
        }
        public List<ProductDetail> GetByProductID(long? ProductID)
        {
            return db.ProductDetails.Where(x => x.ProductID == ProductID).ToList();
        }
        public long Insert(ProductDetail productDetail)
        {
            try
            {
                productDetail.Quantity = productDetail.Quantity.GetValueOrDefault(0);
                productDetail.CreatedDate = DateTime.Now;

                db.ProductDetails.Add(productDetail);
                db.SaveChanges();
                return productDetail.ID;
            }
            catch
            {
                return 0;
            }
        }

        public bool UpdateQuantity(ProductDetail proDetail)
        {
            try
            {
                var model = db.ProductDetails.Find(proDetail.ID); 
                model.Quantity = proDetail.Quantity;
                model.ModifiedDate = DateTime.Now;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(long? ProductID, int? SizeID, string ColorCode)
        {
            try
            {
                var productDetail = GetByProperties(ProductID, SizeID, ColorCode);
                db.ProductDetails.Remove(productDetail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateQuantity(long ProductDetaiID, int quantity)
        {
            try
            {
                var productDetail = GetByID(ProductDetaiID);
                productDetail.Quantity = quantity;
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