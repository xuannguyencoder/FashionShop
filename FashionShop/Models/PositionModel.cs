using FashionShop.Models.EF;
using System.Collections.Generic;
using System.Linq;

namespace FashionShop.Models
{
    public class PositionModel
    {
        private FashionShopEntities db = null;

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