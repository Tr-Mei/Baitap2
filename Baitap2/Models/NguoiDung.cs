using System.ComponentModel.DataAnnotations;


namespace Baitap2.Models
{
    public enum VaiTro
    {
        Khach,
        TaiXe,
        Admin
    }

    public class NguoiDung
    {
        public int Id { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }
        public string MatKhau { get; set; }

        public VaiTro VaiTro { get; set; }

        public bool IsActive { get; set; } = true;
    }


}
