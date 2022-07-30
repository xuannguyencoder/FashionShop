using FashionShop.Models.Common;
using FashionShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models
{
    public class CustomerModel
    {
        FashionShopEntities db = null;
        public CustomerModel()
        {
            db = new FashionShopEntities();
        }
        public int Login(string Username, string Password)
        {
            var result = db.Customers.SingleOrDefault(x => x.Username == Username);
            if (result == null)
            {
                return 0; // tài khoản không tồn tại
            }
            else
            {
                if (result.Type == "Shop")
                {
                    if (result.Status == false)
                        return -1; // tài khoản bị khóa
                    else if (result.Password == Password)
                        return 1; // đăng nhập thành công
                    else
                        return -2; // sai mật khẩu
                }
                else
                    return 0;
            }
        }
        public List<Customer> ListAll()
        {
            return db.Customers.ToList();
        }
        public Customer GetByUsername(string Username)
        {
            return db.Customers.SingleOrDefault(x => x.Username == Username);
        }
        public Customer GetByID(long? ID)
        {
            return db.Customers.Find(ID);
        }
        public Customer GetByEmail(string Email)
        {
            return db.Customers.SingleOrDefault(x => x.Email == Email);
        }
        public long Insert(Customer customer)
        {
            try
            {
                customer.CreatedDate = DateTime.Now;
                customer.Password = ConvertData.Encryptor(customer.Password);
                customer.EmailConfirmed = false;
                customer.Status = true;
                customer.Type = "Shop";
                db.Customers.Add(customer);
                db.SaveChanges();
                return customer.ID;
            }
            catch
            {
                return 0;
            }
        }
        public long InsertAccountGoogle(Customer customer)
        {
            try
            {
                customer.CreatedDate = DateTime.Now;
                customer.Type = "Google";
                string password =  System.Web.Security.Membership.GeneratePassword(32,0);
                customer.Password = password;
                customer.Status = true;
                customer.EmailConfirmed = true;
                db.Customers.Add(customer);
                db.SaveChanges();
                return customer.ID;
            }
            catch
            {
                return 0;
            }
        }
        public bool CheckUsername(string Username)
        {
            var result = db.Customers.SingleOrDefault(x => x.Username == Username);
            if (result !=null)
                return true;
            else
                return false;
        }
        public bool CheckUsername(long ID, string Username) //update user
        {
            var result = db.Customers.SingleOrDefault(x => x.Username == Username);
            if (result.ID == ID || result == null) // check username tồn tại. Và nếu user name trùng chính nó hoặc rỗng thì cho phép
                return true;
            else
                return false;
        }
        public bool UpdatePassword(Customer customer)
        {
            try
            {
                var user = db.Customers.Find(customer.ID);
                user.Password = ConvertData.Encryptor(customer.Password);
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
                var cus = db.Customers.Find(customer.ID);
                cus.Username = customer.Username;
                cus.FirstName = customer.FirstName;
                cus.LastName = customer.LastName;
                cus.Status = customer.Status;
                cus.Address = customer.Address;
                cus.Phone = customer.Phone;
                cus.Email = customer.Email;
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
                var temp_user = db.Customers.Find(user.ID);
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
                var user = db.Customers.Find(ID);
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
                var user = db.Customers.Find(ID);
                db.Customers.Remove(user);
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
                var user = db.Customers.Find(ID);
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