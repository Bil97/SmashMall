using Microsoft.EntityFrameworkCore.Migrations;

namespace SmashMall.Server.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "586c3783-e5d6-42f9-a8c0-aa35b1e398bc", "be7d6c49-4be9-4824-a088-e6d78662b420", "StandardUser", "STANDARDUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "59076949-79e0-4d11-b704-3c6c86483d52", "d26c7d01-a511-485d-a231-27633431fc92", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c9feeaa2-d0a3-4fab-8fed-bee9a7057134", "43ee1880-59e2-4954-8d94-7c8753ecea5e", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "586c3783-e5d6-42f9-a8c0-aa35b1e398bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59076949-79e0-4d11-b704-3c6c86483d52");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9feeaa2-d0a3-4fab-8fed-bee9a7057134");
        }
    }
}
