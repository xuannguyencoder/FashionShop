using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class SizeModel
    {
        FashionShopEntities db = null;
        public SizeModel()
        {
            db = new FashionShopEntities();
        }
        public List<Size> ListAll()
        {
            return db.Sizes.ToList();
        }
        public Size GetByID(long ID)
        {
            return db.Sizes.Find(ID);
        }
    }
}