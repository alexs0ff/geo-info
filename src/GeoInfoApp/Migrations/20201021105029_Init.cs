using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GeoInfoApp.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeoInfoHistoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateTimeUtc = table.Column<DateTime>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    CurrentTemperatureCelsius = table.Column<float>(nullable: false),
                    TimeZone = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoInfoHistoryItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoInfoHistoryItems");
        }
    }
}
