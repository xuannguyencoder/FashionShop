namespace FashionShop.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductImage")]
    public partial class ProductImage
    {
        public long ID { get; set; }

        public long ProductID { get; set; }

        [Required]
        [StringLength(6)]
        public string ColorCode { get; set; }

        public int DisplayOrder { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        public virtual Color Color { get; set; }

        public virtual Product Product { get; set; }
    }
}
