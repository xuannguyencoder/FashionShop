using System;

namespace FashionShop.Models.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
    }
}