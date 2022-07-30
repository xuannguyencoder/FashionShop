using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class ShipModel
    {
        FashionShopEntities db = null;
        public ShipModel()
        {
            db = new FashionShopEntities();
        }
        public long Insert(Ship ship)
        {
            try
            {
                db.Ships.Add(ship);
                db.SaveChanges();
                return ship.ID;
            }
            catch
            {
                return 0;
            }
        }
        public Ship GetByID(long? ID)
        {
            return db.Ships.Find(ID);
        }
        public bool Update(Ship ship)
        {
            try
            {
                var ship2 = db.Ships.Find(ship.ID);
                ship2.Name = ship.Name;
                ship2.Address = ship.Address;
                ship2.Note = ship.Note;
                ship2.Phone = ship.Phone;
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