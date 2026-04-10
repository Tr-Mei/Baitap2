using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baitap2.Models
{
    public class DanhGia
    {
        public int Id { get; set; }

        // ===== Liên kết =====
        [Required(ErrorMessage = "ChuyenDiId không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "ChuyenDiId phải > 0")]
        [Editable(false)]
        public int ChuyenDiId { get; set; }

        [Required(ErrorMessage = "NguoiDungId không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "NguoiDungId phải > 0")]
        [Editable(false)]
        public int NguoiDungId { get; set; }

        // ===== Nội dung đánh giá =====
        [Required(ErrorMessage = "Vui lòng chọn số sao")]
        [Range(1, 5, ErrorMessage = "Số sao từ 1 đến 5")]
        public int SoSao { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Nội dung từ 3-500 ký tự")]
        public string? NoiDung { get; set; }

        // ===== Hệ thống =====
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
