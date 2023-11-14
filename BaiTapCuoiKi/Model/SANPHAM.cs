namespace BaiTapCuoiKi.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SANPHAM")]
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            CHITIETHOADON = new HashSet<CHITIETHOADON>();
        }

        [Key]
        public int Sanpham_ID { get; set; }

        [StringLength(250)]
        public string Sanpham_ten { get; set; }

        [Column(TypeName = "ntext")]
        public string Sanpham_mota { get; set; }

        public decimal? Sanpham_gia { get; set; }

        public int? Sanpham_soluong { get; set; }

        [StringLength(2000)]
        public string Sanpham_anh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETHOADON> CHITIETHOADON { get; set; }
    }
}
