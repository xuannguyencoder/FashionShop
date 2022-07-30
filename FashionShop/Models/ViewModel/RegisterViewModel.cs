using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FashionShop.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Key]
        public long ID { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa là 50 ký tự")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu xác nhận")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ")]
        public string LastName { get; set; }
        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string FirstName { get; set; }
        [Display(Name = "Địa chỉ")]
        [StringLength(250, ErrorMessage = "Độ dài tối đa là 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(10, ErrorMessage = "Độ dài tối đa là 10 số")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        public string Email { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
    }
}