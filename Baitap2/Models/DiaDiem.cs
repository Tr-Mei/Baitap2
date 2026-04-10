using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baitap2.Models
{
    public class DiaDiem
    {
        public int Id { get; set; }

        // ===== Tọa độ =====
        [Required(ErrorMessage = "Vĩ độ không được để trống")]
        [Range(-90, 90, ErrorMessage = "Vĩ độ phải từ -90 đến 90")]
        public double ViDo { get; set; }

        [Required(ErrorMessage = "Kinh độ không được để trống")]
        [Range(-180, 180, ErrorMessage = "Kinh độ phải từ -180 đến 180")]
        public double KinhDo { get; set; }

        // ===== Hệ thống =====
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Editable(false)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
