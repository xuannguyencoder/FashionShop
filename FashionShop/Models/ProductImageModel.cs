using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class ProductImageModel
    {
        FashionShopEntities db = null;
        public ProductImageModel()
        {
            db = new FashionShopEntities();
        }
        public long Insert(ProductImage proImage)
        {
            try
            {
                var proImages = db.ProductImages.Where(x => x.ProductID == proImage.ProductID & x.ColorCode == proImage.ColorCode);
                if (proImages.Count() == 0) //trường hợp danh mục rỗng
                    proImage.DisplayOrder = 1;
                else
                    proImage.DisplayOrder = proImages.Max(x => x.DisplayOrder) + 1;
                db.ProductImages.Add(proImage);
                db.SaveChanges();
                return proImage.ID;
            }
            catch
            {
                return 0;
            }
        }
        public List<ProductImage> ListAll()
        {
            return db.ProductImages.ToList();
        }
        public List<ProductImage> ListByProductID(long? ProductID)
        {
            return db.ProductImages.Where(x => x.ProductID == ProductID).ToList();
        }
    }
}