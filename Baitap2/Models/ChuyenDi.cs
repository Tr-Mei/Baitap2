using System.ComponentModel.DataAnnotations;

namespace Baitap2.Models
{
    public enum TrangThai
    {
        DangTimTaiXe,
        DaNhan,
        DaDen,
        DangDi,
        HoanThanh
    }
    public class ChuyenDi
    {
        public int Id { get; set; }

        public int KhachId { get; set; }
        public NguoiDung Khach { get; set; }

        public int? TaiXeId { get; set; }
        public TaiXe TaiXe { get; set; }

        public string DiemDon { get; set; }
        public string DiemDen { get; set; }

        public string LoaiXe { get; set; }
        public string? GhiChu { get; set; }

        public DateTime ThoiGianDat { get; set; }

        public decimal GiaDuKien { get; set; }

        public TrangThai TrangThai { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }


}
