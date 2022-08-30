using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class SlideModel
    {
        private FashionShopEntities db = null;
        private HttpContext context = HttpContext.Current;

        public SlideModel()
        {
            db = new FashionShopEntities();
        }

        public Slide GetByID(long? ID)
        {
            return db.Slides.Find(ID);
        }

        public List<Slide> ListAll()
        {
            return db.Slides.ToList();
        }

        public long Insert(Slide slide)
        {
            try
            {
                slide.CreatedDate = DateTime.Now;

                db.Slides.Add(slide);
                db.SaveChanges();
                return slide.ID;
            }
            catch
            {
                return 0;
            }
        }

        public bool Update(Slide slide)
        {
            try
            {
                var tempSlide = db.Slides.Find(slide.ID);
                tempSlide.Name = slide.Name;
                if (tempSlide.Image != null)
                    tempSlide.Image = slide.Image;
                tempSlide.Status = slide.Status;
                tempSlide.ModifiedDate = DateTime.Now;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(long? ID)
        {
            try
            {
                var slide = db.Slides.Find(ID);
                db.Slides.Remove(slide);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateStatus(long? ID)
        {
            try
            {
                var slide = db.Slides.Find(ID);
                if (slide.Status == true)
                    slide.Status = false;
                else
                    slide.Status = true;
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