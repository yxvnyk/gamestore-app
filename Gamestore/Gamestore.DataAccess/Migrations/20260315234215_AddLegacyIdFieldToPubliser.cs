using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddLegacyIdFieldToPubliser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Publishers",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "LegacyId",
            table: "Publishers",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Genres",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Games",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111115"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222224"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222225"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444445"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444446"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
            column: "IsDeleted",
            value: false);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
            column: "IsDeleted",
            value: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "LegacyId",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Genres");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Games");
    }
}
