using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models.Common
{
    [Serializable]
    public class UserCode
    {
        public long UserID { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
    }
}