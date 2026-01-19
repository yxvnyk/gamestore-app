using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DataAccess.Migrations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class Uniquefields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Platforms_Type",
            table: "Platforms",
            column: "Type",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Genres_Name",
            table: "Genres",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Games_Key",
            table: "Games",
            column: "Key",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Platforms_Type",
            table: "Platforms");

        migrationBuilder.DropIndex(
            name: "IX_Genres_Name",
            table: "Genres");

        migrationBuilder.DropIndex(
            name: "IX_Games_Key",
            table: "Games");
    }
}
