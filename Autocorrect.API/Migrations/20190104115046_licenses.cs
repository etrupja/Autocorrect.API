using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Autocorrect.API.Migrations
{
    public partial class licenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaxUtilization = table.Column<int>(nullable: false),
                    Utilized = table.Column<int>(nullable: false),
                    ExpiresOn = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Licenses");
        }
    }
}
