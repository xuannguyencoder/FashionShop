using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models.ViewModel
{
    public class ProductViewModel
    {
        public ProductViewModel(long ID, string Name, string CateName)
        {
            this.ID = ID;
            this.Name = Name;
            this.CateName = CateName;
        }
        public long ID { get; set; }
        public string Name { get; set; }
        public string CateName { get; set; }

    }
}