using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Team1_FinalProject.Migrations
{
    public partial class Setup2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "SeatClaim",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PriceID",
                table: "Showings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Tagline",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleInitial",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Showings_PriceID",
                table: "Showings",
                column: "PriceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Showings_Prices_PriceID",
                table: "Showings",
                column: "PriceID",
                principalTable: "Prices",
                principalColumn: "PriceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showings_Prices_PriceID",
                table: "Showings");

            migrationBuilder.DropIndex(
                name: "IX_Showings_PriceID",
                table: "Showings");

            migrationBuilder.DropColumn(
                name: "SeatClaim",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PriceID",
                table: "Showings");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Tagline",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MiddleInitial",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
