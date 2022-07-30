namespace FashionShop.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            Menu1 = new HashSet<Menu>();
        }

        public int ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Tên mục menu")]
        [Required(ErrorMessage = "Vui lòng nhập tên mục menu")]
        public string Name { get; set; }
        [Display(Name = "Thứ tự sắp xếp")]
        public int DisplayOrder { get; set; }
        [Display(Name = "Danh mục cha")]
        public int? ParentID { get; set; }

        [StringLength(250)]
        public string Alias { get; set; }
        public int Level { get; set; }

        [StringLength(50)]
        [Display(Name = "Loại hiển thị")]
        [Required(ErrorMessage = "Vui lòng chọn loại hiển thị")]
        public string Type { get; set; }

        [StringLength(250)]
        [Display(Name = "Đường dẫn")]
        public string Link { get; set; }
        [Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public bool Status { get; set; }
        [Display(Name = "Menu")]
        public int? MenuTypeID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Menu> Menu1 { get; set; }

        public virtual Menu Menu2 { get; set; }

        public virtual MenuType MenuType { get; set; }
    }
}
