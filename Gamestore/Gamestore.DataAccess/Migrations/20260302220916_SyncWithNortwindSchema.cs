using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DataAccess.Migrations;

/// <inheritdoc />
public partial class SyncWithNortwindSchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "UnitInStock",
            table: "Games",
            newName: "UnitsOnOrder");

        migrationBuilder.AddColumn<string>(
            name: "Address",
            table: "Publishers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "City",
            table: "Publishers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "ContactName",
            table: "Publishers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "ContactTitle",
            table: "Publishers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Country",
            table: "Publishers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Fax",
            table: "Publishers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Phone",
            table: "Publishers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "PostalCode",
            table: "Publishers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Region",
            table: "Publishers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "EmployeeId",
            table: "Orders",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "Freight",
            table: "Orders",
            type: "decimal(18,2)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "LegacyOrderId",
            table: "Orders",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "RequiredDate",
            table: "Orders",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ShipAddress",
            table: "Orders",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ShipCity",
            table: "Orders",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ShipCountry",
            table: "Orders",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ShipName",
            table: "Orders",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ShipPostalCode",
            table: "Orders",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ShipRegion",
            table: "Orders",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "ShipVia",
            table: "Orders",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ShippedDate",
            table: "Orders",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "Genres",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<byte[]>(
            name: "Picture",
            table: "Genres",
            type: "varbinary(max)",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "Discontinued",
            table: "Games",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "QuantityPerUnit",
            table: "Games",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<int>(
            name: "ReorderLevel",
            table: "Games",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "UnitsInStock",
            table: "Games",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111115"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222224"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222225"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444445"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("44444444-4444-4444-4444-444444444446"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
            columns: ["Description", "Picture"],
            values: [null, null]);

        migrationBuilder.UpdateData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
            columns: ["Description", "Picture"],
            values: [null, null]);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Address",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "City",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "ContactName",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "ContactTitle",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "Country",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "Fax",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "Phone",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "PostalCode",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "Region",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "EmployeeId",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "Freight",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "LegacyOrderId",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "RequiredDate",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "ShipAddress",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "ShipCity",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "ShipCountry",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "ShipName",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "ShipPostalCode",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "ShipRegion",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "ShipVia",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "ShippedDate",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "Description",
            table: "Genres");

        migrationBuilder.DropColumn(
            name: "Picture",
            table: "Genres");

        migrationBuilder.DropColumn(
            name: "Discontinued",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "QuantityPerUnit",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "ReorderLevel",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "UnitsInStock",
            table: "Games");

        migrationBuilder.RenameColumn(
            name: "UnitsOnOrder",
            table: "Games",
            newName: "UnitInStock");
    }
}
