using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kykyemeklistesi.Migrations
{
    /// <inheritdoc />
    public partial class AddAdEarnings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdKazanclari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Miktar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReklamAgi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TiklamaSayisi = table.Column<int>(type: "int", nullable: false),
                    Goruntuleme = table.Column<int>(type: "int", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdKazanclari", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdKazanclari");
        }
    }
}
