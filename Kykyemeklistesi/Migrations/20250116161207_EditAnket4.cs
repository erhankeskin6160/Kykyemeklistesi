using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kykyemeklistesi.Migrations
{
    /// <inheritdoc />
    public partial class EditAnket4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ankets_YemekListesi_YemekId",
                table: "Ankets");

            migrationBuilder.DropIndex(
                name: "IX_Ankets_YemekId",
                table: "Ankets");

            migrationBuilder.AddColumn<int>(
                name: "AnketId",
                table: "YemekListesi",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_Ankets_YemekId",
                table: "Ankets",
                column: "YemekId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ankets_YemekListesi_YemekId",
                table: "Ankets",
                column: "YemekId",
                principalTable: "YemekListesi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
