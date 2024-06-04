using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Changebasketpropertyemailuniqueandunitprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "BasketItems");

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "BasketItems",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "BasketItems");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "BasketItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
