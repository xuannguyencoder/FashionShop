using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class PositionModel
    {
        FashionShopEntities db = null;
        public PositionModel()
        {
            db = new FashionShopEntities();
        }
        public List<Position> ListAll()
        {
            return db.Positions.ToList();

        }
    }
}