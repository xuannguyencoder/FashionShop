using FashionShop.Models.EF;
using System.Collections.Generic;
using System.Linq;

namespace FashionShop.Models
{
    public class PaymentModel
    {
        private FashionShopEntities db = null;

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
            return db.Payments.SingleOrDefault(x => x.ID == ID);
        }
    }
}