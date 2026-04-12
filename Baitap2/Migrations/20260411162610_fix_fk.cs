using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baitap2.Migrations
{
    /// <inheritdoc />
    public partial class fix_fk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuyenDis_TaiXes_TaiXeId",
                table: "ChuyenDis");

            migrationBuilder.AddForeignKey(
                name: "FK_ChuyenDis_NguoiDungs_TaiXeId",
                table: "ChuyenDis",
                column: "TaiXeId",
                principalTable: "NguoiDungs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuyenDis_NguoiDungs_TaiXeId",
                table: "ChuyenDis");

            migrationBuilder.AddForeignKey(
                name: "FK_ChuyenDis_TaiXes_TaiXeId",
                table: "ChuyenDis",
                column: "TaiXeId",
                principalTable: "TaiXes",
                principalColumn: "Id");
        }
    }
}
