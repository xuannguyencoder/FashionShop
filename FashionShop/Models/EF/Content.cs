namespace FashionShop.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Content")]
    public partial class Content
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Content()
        {
            ContentTags = new HashSet<ContentTag>();
        }

        public long ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Tên bài viết")]
        [Required(ErrorMessage = "Vui lòng nhập tên bài viết")]
        public string Name { get; set; }

        [StringLength(250)]
        public string Alias { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(250)]
        public string SeoTitle { get; set; }

        [StringLength(250)]
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Column("Content", TypeName = "ntext")]
        [Display(Name = "Nội dung")]
        public string Content1 { get; set; }

        [StringLength(500)]
        [Display(Name = "Các thẻ tag")]
        public string Tags { get; set; }
        [Display(Name = "Danh mục bài viết")]
        public long CategoryID { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
        public int? ViewCount { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContentTag> ContentTags { get; set; }
    }
}
