using FashionShop.Models.EF;
using System.Linq;

namespace FashionShop.Models
{
    public class CartItem
    {
        public ProductDetail ProductDetail { get; set; }
        public int Quantity { get; set; }

        public decimal Price
        {
            get
            {
                if (ProductDetail.Product.PromotionPrice > 0)
                {
                    return ProductDetail.Product.PromotionPrice.GetValueOrDefault(0);
                }
                else
                {
                    return ProductDetail.Product.Price.GetValueOrDefault(0);
                }
            }
        }

        public decimal Total
        {
            get
            {
                return Price * Quantity;
            }
        }

        public string ProductImage
        {
            get
            {
                string image = "";
                var productImage = ProductDetail.Product.ProductImages.OrderBy(x => x.DisplayOrder)
                    .FirstOrDefault(x => x.ProductID == ProductDetail.ProductID & x.ColorCode == ProductDetail.ColorCode);
                if (productImage != null)
                {
                    image = productImage.Image;
                }
                return image;
            }
        }
    }
}