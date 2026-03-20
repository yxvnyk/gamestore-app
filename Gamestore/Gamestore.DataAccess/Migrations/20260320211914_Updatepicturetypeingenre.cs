using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DataAccess.Migrations;

/// <inheritdoc />
public partial class Updatepicturetypeingenre : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Picture",
            table: "Genres",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(max)",
            oldNullable: true);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111115"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222224"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222225"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444445"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444446"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
            column: "Picture",
            value: null);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<byte[]>(
            name: "Picture",
            table: "Genres",
            type: "varbinary(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111115"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222224"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222225"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444445"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444446"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
            column: "Picture",
            value: null);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
            column: "Picture",
            value: null);
    }
}
