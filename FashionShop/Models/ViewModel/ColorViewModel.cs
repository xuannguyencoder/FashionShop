using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models.ViewModel
{
    public class ColorViewModel
    {
        public ColorViewModel(string Code, string Name)
        {
            this.Code = Code;
            this.Name = Name;
        }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}