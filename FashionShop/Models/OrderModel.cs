using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class OrderModel
    {
        FashionShopEntities db = null;
        public OrderModel()
        {
            db = new FashionShopEntities();
        }
        public long Insert(Order order)
        {
            try
            {
                order.CreatedDate = DateTime.Now;
                order.OrderStatus = 1;
                db.Orders.Add(order);
                db.SaveChanges();
                return order.ID;
            }
            catch
            {
                return 0;
            }
        }
        public Order GetByID(long ID)
        {
            return db.Orders.Find(ID);
        }
        public List<Order> ListALL()
        {
            return db.Orders.OrderByDescending(x=>x.CreatedDate).ToList();
        }
        public Order GetByID(long? ID)
        {
            return db.Orders.Find(ID);
        }
        public bool UpdateStatus(long? ID, int StatusID)
        {
            try
            {
                var order = db.Orders.Find(ID);
                order.OrderStatus = StatusID;
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