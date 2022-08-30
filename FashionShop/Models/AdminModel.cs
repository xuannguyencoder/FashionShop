using FashionShop.Models.Common;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FashionShop.Models
{
    public class AdminModel
    {
        private FashionShopEntities db = null;

        public AdminModel()
        {
            db = new FashionShopEntities();
        }

        public int Login(string Username, string Password)
        {
            Password = ConvertData.Encryptor(Password);
            var result = db.Admins.SingleOrDefault(x => x.Username == Username);
            if (result == null)
            {
                return 0; // tài khoản không tồn tại
            }
            else
            {
                if (result.Status == false)
                    return -1; // tài khoản bị khóa
                else if (result.Password == Password)
                    return 1; // đăng nhập thành công
                else
                    return -2; // sai mật khẩu
            }
        }

        public List<Admin> ListAll()
        {
            return db.Admins.ToList();
        }

        public Admin GetByUsername(string Username)
        {
            return db.Admins.SingleOrDefault(x => x.Username == Username);
        }

        public Admin GetByID(long? ID)
        {
            return db.Admins.Find(ID);
        }

        public Admin GetByEmail(string Email)
        {
            return db.Admins.SingleOrDefault(x => x.Email == Email);
        }

        public long Insert(Admin admin)
        {
            try
            {
                admin.Password = ConvertData.Encryptor(admin.Password);
                admin.CreatedDate = DateTime.Now;
                admin.EmailConfirmed = false;
                admin.Status = true;
                db.Admins.Add(admin);
                db.SaveChanges();
                return admin.ID;
            }
            catch
            {
                return 0;
            }
        }

        public long InsertAccountGoogle(Admin admin)
        {
            try
            {
                admin.CreatedDate = DateTime.Now;
                admin.Password = "Google";
                admin.Status = true;
                admin.EmailConfirmed = true;
                db.Admins.Add(admin);
                db.SaveChanges();
                return admin.ID;
            }
            catch
            {
                return 0;
            }
        }

        public bool CheckUsername(string Username)
        {
            var result = db.Admins.Count(x => x.Username == Username);
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool CheckUserNameByID(long ID, string Username) //update user
        {
            var result = db.Admins.SingleOrDefault(x => x.Username == Username);
            if (result.ID == ID || result == null) // check username tồn tại. Và nếu user name trùng chính nó hoặc rỗng thì cho phép
                return true;
            else
                return false;
        }

        public bool UpdatePassword(Admin model)
        {
            try
            {
                var user = db.Admins.Find(model.ID);
                user.Password = ConvertData.Encryptor(model.Password);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Customer customer)
        {
            try
            {
                var cus = db.Admins.Find(customer.ID);
                cus.Username = customer.Username;
                cus.FirstName = customer.FirstName;
                cus.LastName = customer.LastName;
                cus.Status = customer.Status;
                cus.Address = customer.Address;
                cus.Phone = customer.Phone;
                cus.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMyAccout(Customer user)
        {
            try
            {
                var temp_user = db.Admins.Find(user.ID);
                temp_user.FirstName = user.FirstName;
                temp_user.LastName = user.LastName;
                temp_user.Address = user.Address;
                temp_user.Phone = user.Phone;
                if (user.Avatar != null)
                {
                    temp_user.Avatar = user.Avatar;
                }
                temp_user.ModifiedDate = DateTime.Now;
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
                var user = db.Admins.Find(ID);
                if (user.Status == true)
                    user.Status = false;
                else
                    user.Status = true;
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
                var user = db.Admins.Find(ID);
                db.Admins.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateEmailConfirm(long ID)
        {
            try
            {
                var user = db.Admins.Find(ID);
                user.EmailConfirmed = true;
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