using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baitap2.Models
{
    public enum TrangThai
    {
        DangTimTaiXe,
        DaNhan,
        DaDen,
        DangDi,
        HoanThanh,
        DaHuy
    }

    public class ChuyenDi
    {
        public int Id { get; set; }

        // ===== Khách =====
        [Required(ErrorMessage = "Khách không hợp lệ")]
        [Range(1, int.MaxValue, ErrorMessage = "KhachId phải > 0")]
        [Editable(false)]
        public int KhachId { get; set; }

        public NguoiDung Khach { get; set; }

        // ===== Tài xế (🔥 FIX FK nhưng giữ nguyên validation) =====
        [Range(1, int.MaxValue, ErrorMessage = "TaiXeId phải > 0")]
        public int? TaiXeId { get; set; }

        [ForeignKey("TaiXeId")]
        public TaiXe TaiXe { get; set; }

        // ===== Địa điểm =====
        [Required(ErrorMessage = "Điểm đón không được để trống")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Điểm đón phải từ 3-255 ký tự")]
        public string DiemDon { get; set; }

        [Required(ErrorMessage = "Điểm đến không được để trống")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Điểm đến phải từ 3-255 ký tự")]
        public string DiemDen { get; set; }

        // ===== Loại xe =====
        [Required(ErrorMessage = "Loại xe không được để trống")]
        [RegularExpression("^(XeMay|Oto4Cho|Oto7Cho)$", ErrorMessage = "Loại xe không hợp lệ")]
        public LoaiXe  LoaiXe { get; set; }

        [StringLength(500, ErrorMessage = "Ghi chú tối đa 500 ký tự")]
        public string? GhiChu { get; set; }

        // ===== Thời gian =====
        [Editable(false)]
        public DateTime ThoiGianDat { get; set; } = DateTime.Now;

        // ===== Giá =====
        [Range(1000, 10000000, ErrorMessage = "Giá phải từ 1.000 đến 10.000.000")]
        public decimal GiaDuKien { get; set; }

        // ===== Trạng thái =====
        [Required(ErrorMessage = "Trạng thái không hợp lệ")]
        [EnumDataType(typeof(TrangThai), ErrorMessage = "Trạng thái không tồn tại")]
        public TrangThai TrangThai { get; set; } = TrangThai.DangTimTaiXe;

        // ===== Hệ thống =====
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ===== Đánh giá =====
        [Range(1, 5, ErrorMessage = "Đánh giá từ 1 đến 5 sao")]
        public int? DanhGia { get; set; }

        [StringLength(500, ErrorMessage = "Nhận xét tối đa 500 ký tự")]
        public string? NhanXet { get; set; }
    }
}
