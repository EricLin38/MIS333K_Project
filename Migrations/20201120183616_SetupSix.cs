using Microsoft.EntityFrameworkCore.Migrations;

namespace Team1_FinalProject.Migrations
{
    public partial class SetupSix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PopcornPoints",
                table: "Orders",
                newName: "PopcornPointsUsed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PopcornPointsUsed",
                table: "Orders",
                newName: "PopcornPoints");
        }
    }
}
