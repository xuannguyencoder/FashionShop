using FashionShop.Models.EF;
using System.Linq;

namespace FashionShop.Models
{
    public class ContactModel
    {
        private FashionShopEntities db = null;

        public ContactModel()
        {
            db = new FashionShopEntities();
        }

        public Contact GetByID(int? ID)
        {
            return db.Contacts.Find(ID);
        }

        public Contact GetActive()
        {
            return db.Contacts.FirstOrDefault(x => x.Status == true);
        }

        public bool Update(Contact contact)
        {
            try
            {
                var model = db.Contacts.Find(contact.ID);
                model.PointX = contact.PointX;
                model.PointY = contact.PointY;
                model.Content = contact.Content;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePoint(int ID, string pointX, string pointY)
        {
            try
            {
                var contact = db.Contacts.Find(ID);
                contact.PointX = pointX;
                contact.PointY = pointY;
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