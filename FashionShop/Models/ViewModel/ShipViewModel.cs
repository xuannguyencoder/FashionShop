using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FashionShop.Models.ViewModel
{
    public class ShipViewModel
    {
        public ShipViewModel() { }
        public ShipViewModel(string Firstname, string Lastname, string Address, string Phone)
        {
            Name = Firstname + " " + Lastname;
            this.Address = Address;
            this.Phone = Phone;
        }
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [StringLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        public string Note { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}