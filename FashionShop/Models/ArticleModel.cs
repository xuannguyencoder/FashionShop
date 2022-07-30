using FashionShop.Models.Common;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class ArticleModel
    {
        FashionShopEntities db = null;
        HttpContext context = HttpContext.Current;
        public ArticleModel()
        {
            db = new FashionShopEntities();
        }
        public Content GetByID(long? ID)
        {
            return db.Contents.Find(ID);
        }
        public List<Content> ListAll()
        {
            return db.Contents.ToList();
        }
        public List<Content> ListContentTagByTagID(string tagID)
        {
            return db.ContentTags.Where(x => x.TagID == tagID).AsEnumerable().Select(x=> new Content()
            {
                Name = x.Content.Name,
                CreatedDate = x.Content.CreatedDate,
                CreatedBy = x.Content.CreatedBy,
                Alias = x.Content.Alias,
                Status = x.Content.Status,
                Image = x.Content.Image,
                Category = x.Content.Category,

            }).ToList();
        }
        public Tag GetTagByID(string TagID)
        {
            return db.Tags.Find(TagID);
        }
        public List<Content> ListAllByCateID(long CateID)
        {
            return db.Contents.Where(x => x.CategoryID == CateID).ToList();
        }
        public Content GetByAlias(string alias)
        {
            return db.Contents.FirstOrDefault(x => x.Alias == alias);
        }
        public long Insert(Content content)
        {
            try
            {
                ConvertData convertData = new ConvertData();
                content.Alias = convertData.ConvertToAlias(content.Name);
                content.Alias = FixAlias(content.Alias,0); //fixbug Alias lại nếu bị trùng khi thêm
                content.ViewCount = 0;
                content.CreatedDate = DateTime.Now;
                var user = (UserLogin)context.Session[Constant.ADMIN_SESSION];
                content.CreatedBy = user.FirstName + user.LastName;
                db.Contents.Add(content);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(content.Tags))
                {
                    string[] tags = content.Tags.Split(',');
                    foreach (string tag in tags)
                    {
                        var tagID = convertData.ConvertToAlias(tag);
                        var result = CheckTag(tagID);
                        if (!result)
                        {
                            InsertTag(tagID, tag);
                        }
                        InsertContentTag(content.ID, tagID);
                    }
                }

               
                return content.ID;
            }
            catch
            {
                return 0;
            }
        }
        public bool CheckTag(string ID)
        {
            return db.Tags.Count(x => x.ID == ID) > 0;
        }
        public void InsertTag(string ID, string Name)
        {
            Tag tag= new Tag();
            tag.ID = ID;
            tag.Name = Name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }
        public List<ContentTag> ListContentTag(long ID)
        {
            return db.ContentTags.Where(x => x.ContentID == ID).ToList();
        }
        public void InsertContentTag(long ContentID, string TagID)
        {
            ContentTag contentTag = new ContentTag();
            contentTag.ContentID = ContentID;
            contentTag.TagID = TagID;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }

        public string FixAlias(string alias, long ArticleID)
        {
            bool flag = true;
            int i = 1;
            string alies1 = alias;
            while (flag)
            {
                if (!CheckAlias(alies1, ArticleID))
                    break;
                else
                {
                    alies1 = alias + i;
                }
                i++;
            }
            return alies1;
        }
        public bool CheckAlias(string alias, long ArticleID)
        {
            if (alias != null)
            {
                var article = db.Contents.SingleOrDefault(x => x.Alias == alias);
                if (article == null)
                    return false;
                else if (article.ID == ArticleID)
                    return false;
                return true;
            }
            else
                return false;
        }
        public bool Update(Content content)
        {
            try
            {
                var model = db.Contents.Find(content.ID);
                ConvertData convertData = new ConvertData();
                model.Alias = convertData.ConvertToAlias(content.Name);
                model.Alias = FixAlias(content.Alias, content.ID); //fixbug Alias lại nếu bị trùng khi thêm
                if (content.Image != null)
                    model.Image = content.Image;
                model.Name = content.Name;

                model.CategoryID = content.CategoryID;
                model.Status = content.Status;

                var user = (UserLogin)context.Session[Constant.ADMIN_SESSION];
                model.ModifiedBy = user.FirstName + user.LastName;
                model.ModifiedDate = DateTime.Now;
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
                var content = db.Contents.Find(ID);
                RemoveAllContentTag(ID);
                db.Contents.Remove(content);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void RemoveAllContentTag(long? ContentID)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentID == ContentID));
            db.SaveChanges();
        }
        public bool UpdateStatus(long? ID)
        {
            try
            {
                var content = db.Contents.Find(ID);
                if (content.Status == true)
                    content.Status = false;
                else
                    content.Status = true;
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