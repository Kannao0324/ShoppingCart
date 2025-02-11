using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCartSW.Migrations
{
    /// <inheritdoc />
    public partial class Addproductfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "Email", "Password" },
                values: new object[] { 1, "admin@test.com", "$2a$11$cCTk5FxpTNZR1SIfyIX6pupn0OqxiYFoA4i..hKBpc69DqpU6RQ96" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");
        }
    }
}
