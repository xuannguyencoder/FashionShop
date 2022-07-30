using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models.ViewModel
{
    public class SizeViewModel
    {
        public SizeViewModel(int ID, string Code)
        {
            this.ID = ID;
            this.Code = Code;
        }
        public int ID { get; set; }
        public string Code { get; set; }
    }
}