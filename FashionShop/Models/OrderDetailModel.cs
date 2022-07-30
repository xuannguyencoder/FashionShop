using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class OrderDetailModel
    {
        FashionShopEntities db = null;
        public OrderDetailModel()
        {
            db = new FashionShopEntities();
        }
        public bool Insert(OrderDetail orderDetail)
        {
            try
            {
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<OrderDetail> ListByOrderID(long OrderID)
        {
            return db.OrderDetails.Where(x => x.OrderID == OrderID).ToList();
        }
        public List<OrderDetail> ListAll()
        {
            return db.OrderDetails.ToList();
        }
    }
}