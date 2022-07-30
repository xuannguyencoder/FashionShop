using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class PaymentModel
    {
        FashionShopEntities db = null;
        public PaymentModel()
        {
            db = new FashionShopEntities();
        }
        public List<Payment> ListAll()
        {
            return db.Payments.ToList();
        }
        public Payment GetByID(int ID)
        {
            return db.Payments.SingleOrDefault(x=>x.ID== ID);
        }
    }
}