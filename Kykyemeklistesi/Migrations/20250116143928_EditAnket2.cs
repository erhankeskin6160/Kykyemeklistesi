using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kykyemeklistesi.Migrations
{
    /// <inheritdoc />
    public partial class EditAnket2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ankets_YemekId",
                table: "Ankets");

            migrationBuilder.CreateIndex(
                name: "IX_Ankets_YemekId",
                table: "Ankets",
                column: "YemekId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ankets_YemekId",
                table: "Ankets");

            migrationBuilder.CreateIndex(
                name: "IX_Ankets_YemekId",
                table: "Ankets",
                column: "YemekId");
        }
    }
}
