namespace FashionShop.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductDetail")]
    public partial class ProductDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductDetail()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        public long ID { get; set; }
        public long ProductID { get; set; }
        [Display(Name = "Kích thước")]
        [Required(ErrorMessage = "Vui lòng chọn kích thước")]
        public int SizeID { get; set; }

        [StringLength(6)]
        [Display(Name = "Màu sắc")]
        [Required(ErrorMessage = "Vui lòng chọn màu sắc")]
        public string ColorCode { get; set; }
        [Display(Name = "Số lượng")]
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual Color Color { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual Product Product { get; set; }

        public virtual Size Size { get; set; }
    }
}
