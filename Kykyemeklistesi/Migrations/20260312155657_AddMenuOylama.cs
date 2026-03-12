using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kykyemeklistesi.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuOylama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuOylari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YemekId = table.Column<int>(type: "int", nullable: false),
                    IpAdresi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    OyTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuOylari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuOylari_YemekListesi_YemekId",
                        column: x => x.YemekId,
                        principalTable: "YemekListesi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuPaylasimlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GonderenAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sehir = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YurtAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosyaYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DosyaVerisi = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DosyaUzantisi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GonderimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPaylasimlar", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuOylari_YemekId",
                table: "MenuOylari",
                column: "YemekId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuOylari");

            migrationBuilder.DropTable(
                name: "MenuPaylasimlar");
        }
    }
}
