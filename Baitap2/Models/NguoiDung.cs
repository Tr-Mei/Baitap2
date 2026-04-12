using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // ===== Thông tin =====

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15, MinimumLength = 9, ErrorMessage = "SĐT phải từ 9-15 ký tự")]
        [RegularExpression(@"^(0|\+84)[0-9]{8,14}$", ErrorMessage = "SĐT phải đúng định dạng VN (0xxx hoặc +84xxx)")]
        public string DienThoai { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username không được để trống")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username từ 3-50 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username chỉ chứa chữ, số và dấu _")]
        public string Username { get; set; }

        // ⚠️ vẫn giữ plain text theo yêu cầu
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$",
            ErrorMessage = "Mật khẩu phải chứa ít nhất 1 chữ và 1 số")]
        public string MatKhau { get; set; }

        // ===== Vai trò =====
        [Required(ErrorMessage = "Vai trò không hợp lệ")]
        [EnumDataType(typeof(VaiTro), ErrorMessage = "Vai trò không tồn tại")]
        public VaiTro VaiTro { get; set; }

        // ===== Trạng thái =====
        [Editable(false)]
        public bool IsActive { get; set; } = true;

        [Editable(false)]
        public bool BiKhoa { get; set; } = false;

        // ===== Hệ thống =====
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Editable(false)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
