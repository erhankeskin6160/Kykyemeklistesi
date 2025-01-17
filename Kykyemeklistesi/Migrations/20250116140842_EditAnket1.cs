using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kykyemeklistesi.Migrations
{
    /// <inheritdoc />
    public partial class EditAnket1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ankets_YemekId",
                table: "Ankets",
                column: "YemekId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ankets_YemekListesi_YemekId",
                table: "Ankets",
                column: "YemekId",
                principalTable: "YemekListesi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ankets_YemekListesi_YemekId",
                table: "Ankets");

            migrationBuilder.DropIndex(
                name: "IX_Ankets_YemekId",
                table: "Ankets");
        }
    }
}
