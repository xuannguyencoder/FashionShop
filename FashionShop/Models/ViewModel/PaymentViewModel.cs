using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FashionShop.Models.ViewModel
{
    public class PaymentViewModel
    {
        public PaymentViewModel(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }
        public int ID { get; set; }
        public string Name { get; set; }

    }
}