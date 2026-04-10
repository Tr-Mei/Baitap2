using System.ComponentModel.DataAnnotations;

namespace Baitap2.Models
{
    public class ThanhToan
    {
        public int Id { get; set; }

        public int ChuyenDiId { get; set; }
        public decimal SoTien { get; set; }

        public string TrangThai { get; set; } // ChuaTT / DaTT
    }


}
