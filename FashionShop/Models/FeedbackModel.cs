using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FashionShop.Models
{
    public class FeedbackModel
    {
        private FashionShopEntities db = null;

        public FeedbackModel()
        {
            db = new FashionShopEntities();
        }

        public long Insert(Feedback feedback)
        {
            try
            {
                feedback.CreateDate = DateTime.Now;
                feedback.Status = false;
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return feedback.ID;
            }
            catch
            {
                return 0;
            }
        }

        public List<Feedback> ListAll()
        {
            return db.Feedbacks.OrderByDescending(x => x.CreateDate).ToList();
        }

        public Feedback GetByID(long? ID)
        {
            return db.Feedbacks.Find(ID);
        }

        public bool UpdateStatus(long ID)
        {
            try
            {
                var fb = db.Feedbacks.Find(ID);
                fb.Status = true;
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