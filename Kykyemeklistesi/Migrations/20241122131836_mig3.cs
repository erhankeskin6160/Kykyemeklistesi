using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kykyemeklistesi.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodList",
                table: "YemekListesi");

            migrationBuilder.AddColumn<string>(
                name: "AksamYemekListesi",
                table: "YemekListesi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SabahYemekListesi",
                table: "YemekListesi",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AksamYemekListesi",
                table: "YemekListesi");

            migrationBuilder.DropColumn(
                name: "SabahYemekListesi",
                table: "YemekListesi");

            migrationBuilder.AddColumn<string>(
                name: "FoodList",
                table: "YemekListesi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
