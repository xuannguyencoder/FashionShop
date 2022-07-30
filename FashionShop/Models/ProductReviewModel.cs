using FashionShop.Models.Common;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class ProductReviewModel
    {
        FashionShopEntities db = null;
        HttpContext context = HttpContext.Current;
        public ProductReviewModel()
        {
            db = new FashionShopEntities();
        }

        public List<ProductReview> ListAll()
        {
            return db.ProductReviews.ToList();
        }
        public List<ProductReview> GetByProductID(long? ID)
        {
            return db.ProductReviews.Where(x => x.ProductID == ID).OrderBy(x => x.CreatedDate).ToList();
        }
        public long Insert(ProductReview model)
        {
            try
            {
                var user = (UserLogin)context.Session[Constant.CUSTOMER_SESSION];
                model.CreatedDate = DateTime.Now;
                model.CustomerID = user.UserID;
                db.ProductReviews.Add(model);
                db.SaveChanges();
                return model.ID;
            }
            catch
            {
                return 0;
            }
        }
    }
}