using Microsoft.EntityFrameworkCore.Migrations;

namespace ZavrsenProekt.Migrations
{
    public partial class PosetaTuristTuristickiVodic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VkluciSe_Turist_TuristId",
                table: "VkluciSe");

            migrationBuilder.DropIndex(
                name: "IX_VkluciSe_TuristId",
                table: "VkluciSe");

            migrationBuilder.DropColumn(
                name: "TuristId",
                table: "VkluciSe");

            migrationBuilder.CreateIndex(
                name: "IX_VkluciSe_TouristId",
                table: "VkluciSe",
                column: "TouristId");

            migrationBuilder.AddForeignKey(
                name: "FK_VkluciSe_Turist_TouristId",
                table: "VkluciSe",
                column: "TouristId",
                principalTable: "Turist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VkluciSe_Turist_TouristId",
                table: "VkluciSe");

            migrationBuilder.DropIndex(
                name: "IX_VkluciSe_TouristId",
                table: "VkluciSe");

            migrationBuilder.AddColumn<int>(
                name: "TuristId",
                table: "VkluciSe",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VkluciSe_TuristId",
                table: "VkluciSe",
                column: "TuristId");

            migrationBuilder.AddForeignKey(
                name: "FK_VkluciSe_Turist_TuristId",
                table: "VkluciSe",
                column: "TuristId",
                principalTable: "Turist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
