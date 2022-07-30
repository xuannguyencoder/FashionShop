using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class ColorModel
    {
        FashionShopEntities db = null;
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