using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baitap2.Models
{
    public class ThanhToan
    {
        public int Id { get; set; }

        // ===== Liên kết chuyến =====
        [Required(ErrorMessage = "ChuyenDiId không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "ChuyenDiId phải > 0")]
        [Editable(false)]
        public int ChuyenDiId { get; set; }

        // ===== Số tiền =====
        [Required(ErrorMessage = "Số tiền không được để trống")]
        [Range(1000, 100000000, ErrorMessage = "Số tiền phải từ 1.000 đến 100.000.000")]
        public decimal SoTien { get; set; }

        // ===== Trạng thái =====
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [RegularExpression("^(ChuaTT|DaTT)$", ErrorMessage = "Trạng thái không hợp lệ")]
        public string TrangThai { get; set; } = "ChuaTT";

        // ===== Hệ thống =====
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Editable(false)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
