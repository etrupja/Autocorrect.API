using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Autocorrect.API.Migrations
{
    public partial class SuggestedWords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuggestedWords",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    WrongWord = table.Column<string>(nullable: true),
                    RightWord = table.Column<string>(nullable: true),
                    WordStatus = table.Column<int>(nullable: false),
                    DateSuggested = table.Column<DateTime>(nullable: true),
                    DateAccepted = table.Column<DateTime>(nullable: true),
                    DateRetreived = table.Column<DateTime>(nullable: true),
                    DateRefused = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuggestedWords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuggestedWords");
        }
    }
}
