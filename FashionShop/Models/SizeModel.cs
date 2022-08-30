using FashionShop.Models.EF;
using System.Collections.Generic;
using System.Linq;

namespace FashionShop.Models
{
    public class SizeModel
    {
        private FashionShopEntities db = null;

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