using System;
using System.ComponentModel.DataAnnotations;

namespace FashionShop.Models.Common
{
    [Serializable]
    public class UserCode
    {
        public long UserID { get; set; }

        [Display(Name = "Mã xác thực")]
        [Required(ErrorMessage = "Vui lòng nhập mã xác thực")]
        public string Code { get; set; }

        public string Email { get; set; }
        public bool Status { get; set; }
    }
}