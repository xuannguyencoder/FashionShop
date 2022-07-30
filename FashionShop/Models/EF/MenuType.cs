namespace FashionShop.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MenuType")]
    public partial class MenuType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MenuType()
        {
            Menus = new HashSet<Menu>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên menu")]
        [Required(ErrorMessage = "Vui lòng nhập tên menu")]
        public string Name { get; set; }
        [Display(Name = "Vị trí trên website")]
        public int PositionID { get; set; }
        [Display(Name = "Thứ tự sắp xếp")]
        public int DisplayOrder { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Menu> Menus { get; set; }

        public virtual Position Position { get; set; }
    }
}
