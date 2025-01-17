using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kykyemeklistesi.Migrations
{
    /// <inheritdoc />
    public partial class EditAnket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ısBegenme",
                table: "Ankets");

            migrationBuilder.AddColumn<int>(
                name: "BegenmeSayisi",
                table: "Ankets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BegenmemeSayisi",
                table: "Ankets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BegenmeSayisi",
                table: "Ankets");

            migrationBuilder.DropColumn(
                name: "BegenmemeSayisi",
                table: "Ankets");

            migrationBuilder.AddColumn<bool>(
                name: "ısBegenme",
                table: "Ankets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
