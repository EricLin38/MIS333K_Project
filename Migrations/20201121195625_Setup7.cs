using Microsoft.EntityFrameworkCore.Migrations;

namespace Team1_FinalProject.Migrations
{
    public partial class Setup7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "MatineePrice",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "SeniorDiscount",
                table: "Prices");

            migrationBuilder.RenameColumn(
                name: "WeekendPrice",
                table: "Prices",
                newName: "PriceType");

            migrationBuilder.AddColumn<string>(
                name: "PriceTitle",
                table: "Prices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceValue",
                table: "Prices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceTitle",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "PriceValue",
                table: "Prices");

            migrationBuilder.RenameColumn(
                name: "PriceType",
                table: "Prices",
                newName: "WeekendPrice");

            migrationBuilder.AddColumn<int>(
                name: "BasePrice",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountPrice",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatineePrice",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "SeniorDiscount",
                table: "Prices",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
