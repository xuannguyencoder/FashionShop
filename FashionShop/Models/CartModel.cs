using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionShop.Models.Common;
namespace FashionShop.Models
{
    public class CartModel
    {
        HttpContext context = HttpContext.Current;
        public List<CartItem> ListAll()
        {
            var sessionCart = context.Session[Constant.CartSession];
            var model = (List<CartItem>)sessionCart;
            if (model == null)
            {
                model = new List<CartItem>();
            }
            return model;

        }
        public void Update(List<CartItem> cart)
        {
            context.Session[Constant.CartSession] = cart;
        }
        public void DeleteAll()
        {
            context.Session[Constant.CartSession] = null;
        }
        public void Delete(long productDetailID)
        {
            var sessionCart = ListAll();
            sessionCart.RemoveAll(x => x.ProductDetail.ID ==productDetailID);
        }
        public decimal getCartTotal()
        {
            var model = ListAll();
            decimal cartTotal = 0;
            foreach (var item in model)
            {
                cartTotal += item.Total;
            }
            return cartTotal;
        }
    }
}