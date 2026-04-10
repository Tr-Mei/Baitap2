using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baitap2.Models
{
    public class TaiXe
    {
        public int Id { get; set; }

        // ===== Liên kết user =====
        [Required(ErrorMessage = "NguoiDungId không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "NguoiDungId phải hợp lệ (>0)")]
        [Editable(false)] // không cho client đổi sang user khác
        public int NguoiDungId { get; set; }

        public NguoiDung NguoiDung { get; set; }

        // ===== Trạng thái =====
        public bool Online { get; set; } = false;

        // ===== Đánh giá =====
        [Range(0, 5, ErrorMessage = "Điểm phải từ 0 đến 5")]
        [Editable(false)] // không cho client tự sửa điểm
        public double DanhGiaTB { get; set; } = 5.0;

        // ===== Hệ thống =====
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Editable(false)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
