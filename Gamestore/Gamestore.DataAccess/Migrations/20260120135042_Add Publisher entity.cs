using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddPublisherentity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "Discount",
            table: "Games",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<double>(
            name: "Price",
            table: "Games",
            type: "float",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<int>(
            name: "UnitInStock",
            table: "Games",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "Publishers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                HomePage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Publishers", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Publishers_CompanyName",
            table: "Publishers",
            column: "CompanyName",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Publishers");

        migrationBuilder.DropColumn(
            name: "Discount",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "Price",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "UnitInStock",
            table: "Games");
    }
}
