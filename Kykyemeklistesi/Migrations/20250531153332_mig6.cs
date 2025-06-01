using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kykyemeklistesi.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YemekListesi_Ankets_AnketId",
                table: "YemekListesi");

            migrationBuilder.DropIndex(
                name: "IX_YemekListesi_AnketId",
                table: "YemekListesi");

            migrationBuilder.DropColumn(
                name: "AnketId",
                table: "YemekListesi");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ısBegenme",
                table: "Ankets");

            migrationBuilder.AddColumn<int>(
                name: "AnketId",
                table: "YemekListesi",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_YemekListesi_AnketId",
                table: "YemekListesi",
                column: "AnketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_YemekListesi_Ankets_AnketId",
                table: "YemekListesi",
                column: "AnketId",
                principalTable: "Ankets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
