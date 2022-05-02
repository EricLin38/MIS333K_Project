using Microsoft.EntityFrameworkCore.Migrations;

namespace Team1_FinalProject.Migrations
{
    public partial class SetupFour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderHistory",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderHistory",
                table: "Orders");
        }
    }
}
