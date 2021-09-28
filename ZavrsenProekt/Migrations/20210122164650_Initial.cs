using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZavrsenProekt.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Turist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassportId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DatumPrijava = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TuristickiVodic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Obrazovanie = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuristickiVodic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Poseta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Znamenitost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Komentar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumPoseta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TuristickiVodicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poseta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poseta_TuristickiVodic_TuristickiVodicId",
                        column: x => x.TuristickiVodicId,
                        principalTable: "TuristickiVodic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VkluciSe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosetaId = table.Column<int>(type: "int", nullable: false),
                    TouristId = table.Column<int>(type: "int", nullable: false),
                    TuristId = table.Column<int>(type: "int", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PosetaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZnamenitostUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrojKartica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidnostData = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkluciSe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VkluciSe_Poseta_PosetaId",
                        column: x => x.PosetaId,
                        principalTable: "Poseta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VkluciSe_Turist_TuristId",
                        column: x => x.TuristId,
                        principalTable: "Turist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Poseta_TuristickiVodicId",
                table: "Poseta",
                column: "TuristickiVodicId");

            migrationBuilder.CreateIndex(
                name: "IX_VkluciSe_PosetaId",
                table: "VkluciSe",
                column: "PosetaId");

            migrationBuilder.CreateIndex(
                name: "IX_VkluciSe_TuristId",
                table: "VkluciSe",
                column: "TuristId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VkluciSe");

            migrationBuilder.DropTable(
                name: "Poseta");

            migrationBuilder.DropTable(
                name: "Turist");

            migrationBuilder.DropTable(
                name: "TuristickiVodic");
        }
    }
}
