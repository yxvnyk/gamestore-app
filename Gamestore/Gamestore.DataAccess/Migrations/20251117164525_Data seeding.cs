using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gamestore.DataAccess.Migrations;

/// <inheritdoc />
public partial class Dataseeding : Migration
{
    private static readonly string[] Columns = ["Id", "Name", "ParentGenreId"];
    private static readonly string[] ColumnsValue = ["Id", "Type"];

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Genres",
            columns: Columns,
            values: new object[,]
            {
                { new Guid("11111111-1111-1111-1111-111111111111"), "Strategy", null },
                { new Guid("11111111-1111-1111-1111-111111111114"), "RPG", null },
                { new Guid("11111111-1111-1111-1111-111111111115"), "Sports", null },
                { new Guid("22222222-2222-2222-2222-222222222222"), "Races", null },
                { new Guid("33333333-3333-3333-3333-333333333333"), "Arcade", null },
                { new Guid("44444444-4444-4444-4444-444444444444"), "Action", null },
                { new Guid("55555555-5555-5555-5555-555555555555"), "Adventure", null },
                { new Guid("66666666-6666-6666-6666-666666666666"), "Puzzle & Skill", null },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: ColumnsValue,
            values: new object[,]
            {
                { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Mobile" },
                { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Browser" },
                { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Desktop" },
                { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Console" },
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: ["Id", "Name", "ParentGenreId"],
            values: new object[,]
            {
                { new Guid("11111111-1111-1111-1111-111111111112"), "RTS", new Guid("11111111-1111-1111-1111-111111111111") },
                { new Guid("11111111-1111-1111-1111-111111111113"), "TBS", new Guid("11111111-1111-1111-1111-111111111111") },
                { new Guid("22222222-2222-2222-2222-222222222223"), "Rally", new Guid("22222222-2222-2222-2222-222222222222") },
                { new Guid("22222222-2222-2222-2222-222222222224"), "Formula", new Guid("22222222-2222-2222-2222-222222222222") },
                { new Guid("22222222-2222-2222-2222-222222222225"), "Off-road", new Guid("22222222-2222-2222-2222-222222222222") },
                { new Guid("44444444-4444-4444-4444-444444444445"), "FPS", new Guid("44444444-4444-4444-4444-444444444444") },
                { new Guid("44444444-4444-4444-4444-444444444446"), "TPS", new Guid("44444444-4444-4444-4444-444444444444") },
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111112"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111113"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111114"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111115"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222223"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222224"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222225"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444445"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444446"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444444"));
    }
}
