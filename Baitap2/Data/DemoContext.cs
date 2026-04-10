using Microsoft.EntityFrameworkCore;
using Baitap2.Models;

namespace Baitap2.Data
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions<DemoContext> options) : base(options) { }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<TaiXe> TaiXes { get; set; }
        public DbSet<ChuyenDi> ChuyenDis { get; set; }
        public DbSet<DiaDiem> DiaDiems { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }

        // 👇 THÊM ĐOẠN NÀY
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChuyenDi>()
                .Property(x => x.GiaDuKien)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ThanhToan>()
                .Property(x => x.SoTien)
                .HasPrecision(18, 2);
        }
    }
}
