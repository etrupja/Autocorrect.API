using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Autocorrect.API.Migrations
{
    public partial class licensefile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "LicenseFile",
                table: "Licenses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseFile",
                table: "Licenses");
        }
    }
}
