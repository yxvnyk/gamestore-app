using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddLegacyIdFieldToGenre : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "LegacyId",
            table: "Genres",
            type: "int",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111115"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222224"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222225"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444445"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444446"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
            column: "LegacyId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
            column: "LegacyId",
            value: null);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "LegacyId",
            table: "Genres");
    }
}
