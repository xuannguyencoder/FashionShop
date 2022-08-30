namespace FashionShop.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Feedback")]
    public partial class Feedback
    {
        public long ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage ="Vui lòng nhập họ tên")]
        public string Name { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Content { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        public bool Status { get; set; }
    }
}
