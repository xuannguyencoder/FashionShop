using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models.ViewModel
{
    public class ProductImageViewModel
    {
        public ProductImageViewModel(){}
        public ProductImageViewModel(long ProductID, string ColorCode, string Image, int DisplayOrder)
        {
            this.ProductID = ProductID;
            this.ColorCode = ColorCode;
            this.Image = Image;
            this.DisplayOrder = DisplayOrder;
        }
        public long ProductID { get; set; }
        public string ColorCode { get; set; }
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}