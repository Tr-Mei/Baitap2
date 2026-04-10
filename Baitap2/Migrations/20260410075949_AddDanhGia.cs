using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baitap2.Migrations
{
    /// <inheritdoc />
    public partial class AddDanhGia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DanhGia",
                table: "ChuyenDis",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NhanXet",
                table: "ChuyenDis",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DanhGia",
                table: "ChuyenDis");

            migrationBuilder.DropColumn(
                name: "NhanXet",
                table: "ChuyenDis");
        }
    }
}
