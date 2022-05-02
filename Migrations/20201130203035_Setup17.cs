using Microsoft.EntityFrameworkCore.Migrations;

namespace Team1_FinalProject.Migrations
{
    public partial class Setup17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPriceID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DiscountPriceID",
                table: "Orders",
                column: "DiscountPriceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Prices_DiscountPriceID",
                table: "Orders",
                column: "DiscountPriceID",
                principalTable: "Prices",
                principalColumn: "PriceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Prices_DiscountPriceID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DiscountPriceID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountPriceID",
                table: "Orders");
        }
    }
}
