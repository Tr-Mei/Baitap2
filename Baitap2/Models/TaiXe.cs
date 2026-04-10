using System.ComponentModel.DataAnnotations;

namespace Baitap2.Models
{
    public class TaiXe
    {
        public int Id { get; set; }

        public int NguoiDungId { get; set; }
        public NguoiDung NguoiDung { get; set; }

        public bool Online { get; set; }
        public double DanhGiaTB { get; set; }
    }


}
