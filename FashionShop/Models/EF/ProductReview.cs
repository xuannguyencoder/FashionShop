namespace FashionShop.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductReview")]
    public partial class ProductReview
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string Content { get; set; }

        public int? Rate { get; set; }

        public long CustomerID { get; set; }

        public long? ProductID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Product Product { get; set; }
    }
}
