using FashionShop.Models.EF;
using System.Collections.Generic;
using System.Linq;

namespace FashionShop.Models
{
    public class ColorModel
    {
        private FashionShopEntities db = null;

        public ColorModel()
        {
            db = new FashionShopEntities();
        }

        public List<EF.Color> ListAll()
        {
            return db.Colors.ToList();
        }

        public EF.Color GetByID(long ID)
        {
            return db.Colors.Find(ID);
        }
    }
}