using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baitap2.Migrations
{
    /// <inheritdoc />
    public partial class BT2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BiKhoa",
                table: "NguoiDungs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiKhoa",
                table: "NguoiDungs");
        }
    }
}
