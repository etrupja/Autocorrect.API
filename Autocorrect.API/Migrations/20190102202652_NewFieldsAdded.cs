using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Autocorrect.API.Migrations
{
    public partial class NewFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "SpecialWords",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRetreived",
                table: "SpecialWords",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "SpecialWords",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "SpecialWords");

            migrationBuilder.DropColumn(
                name: "DateRetreived",
                table: "SpecialWords");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "SpecialWords");
        }
    }
}
