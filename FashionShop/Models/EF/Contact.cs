namespace FashionShop.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        public int ID { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name ="Nội dung")]
        public string Content { get; set; }
        [StringLength(50)]
        [Display(Name = "Tọa độ x")]
        public string PointX { get; set; }
        [StringLength(50)]
        [Display(Name = "Tọa độ y")]
        public string PointY { get; set; }
        public bool Status { get; set; }
    }
}
