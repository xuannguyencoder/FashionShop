using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FashionShop.Models.ViewModel
{
    public class ForgetViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}