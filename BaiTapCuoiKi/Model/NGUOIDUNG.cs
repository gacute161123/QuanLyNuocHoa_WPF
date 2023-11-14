namespace BaiTapCuoiKi.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NGUOIDUNG")]
    public partial class NGUOIDUNG
    {
        [Key]
        public int IDNguoiDung { get; set; }

        [StringLength(250)]
        public string TenNguoiDung { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(250)]
        public string MatKhau { get; set; }

        public string NgayDangKy { get; set; }

        [StringLength(250)]
        public string VaiTro { get; set; }
    }
}
