using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            [Key]
            public int MaThanhVien { get; set; }

            [StringLength(255)]
            public string TaiKhoan { get; set; }

            [StringLength(255)]
            public string MatKhau { get; set; }

            [StringLength(255)]
            public string Hoten { get; set; }

            [StringLength(255)]
            public string DiaChi { get; set; }

            [StringLength(255)]
            public string Email { get; set; }

            [StringLength(12)]
            public string SoDienThoai { get; set; }

            public string CauHoi { get; set; }

            public string CauTraLoi { get; set; }

            public int? MaLoaiTv { get; set; }
        }
    }
}