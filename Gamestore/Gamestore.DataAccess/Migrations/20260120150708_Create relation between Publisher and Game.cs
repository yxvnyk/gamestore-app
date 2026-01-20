using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DataAccess.Migrations;

/// <inheritdoc />
public partial class CreaterelationbetweenPublisherandGame : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "PublisherId",
            table: "Games",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Games_PublisherId",
            table: "Games",
            column: "PublisherId");

        migrationBuilder.AddForeignKey(
            name: "FK_Games_Publishers_PublisherId",
            table: "Games",
            column: "PublisherId",
            principalTable: "Publishers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Games_Publishers_PublisherId",
            table: "Games");

        migrationBuilder.DropIndex(
            name: "IX_Games_PublisherId",
            table: "Games");

        migrationBuilder.AlterColumn<Guid>(
            name: "PublisherId",
            table: "Games",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");
    }
}
