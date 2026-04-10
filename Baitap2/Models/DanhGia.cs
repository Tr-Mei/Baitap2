using System.ComponentModel.DataAnnotations;

namespace Baitap2.Models
{
    public class DanhGia
    {
        public int Id { get; set; }

        public int ChuyenDiId { get; set; }
        public int NguoiDungId { get; set; }

        public int SoSao { get; set; }
        public string? NoiDung { get; set; }
    }


}
