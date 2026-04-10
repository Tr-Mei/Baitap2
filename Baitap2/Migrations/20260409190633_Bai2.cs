using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baitap2.Migrations
{
    /// <inheritdoc />
    public partial class Bai2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChuyenDiId = table.Column<int>(type: "int", nullable: false),
                    NguoiDungId = table.Column<int>(type: "int", nullable: false),
                    SoSao = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiaDiems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViDo = table.Column<double>(type: "float", nullable: false),
                    KinhDo = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaDiems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChuyenDiId = table.Column<int>(type: "int", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaiXes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiDungId = table.Column<int>(type: "int", nullable: false),
                    Online = table.Column<bool>(type: "bit", nullable: false),
                    DanhGiaTB = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiXes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaiXes_NguoiDungs_NguoiDungId",
                        column: x => x.NguoiDungId,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChuyenDis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KhachId = table.Column<int>(type: "int", nullable: false),
                    TaiXeId = table.Column<int>(type: "int", nullable: true),
                    DiemDon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiemDen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoaiXe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThoiGianDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiaDuKien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuyenDis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChuyenDis_NguoiDungs_KhachId",
                        column: x => x.KhachId,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChuyenDis_TaiXes_TaiXeId",
                        column: x => x.TaiXeId,
                        principalTable: "TaiXes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenDis_KhachId",
                table: "ChuyenDis",
                column: "KhachId");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenDis_TaiXeId",
                table: "ChuyenDis",
                column: "TaiXeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaiXes_NguoiDungId",
                table: "TaiXes",
                column: "NguoiDungId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChuyenDis");

            migrationBuilder.DropTable(
                name: "DanhGias");

            migrationBuilder.DropTable(
                name: "DiaDiems");

            migrationBuilder.DropTable(
                name: "ThanhToans");

            migrationBuilder.DropTable(
                name: "TaiXes");

            migrationBuilder.DropTable(
                name: "NguoiDungs");
        }
    }
}
