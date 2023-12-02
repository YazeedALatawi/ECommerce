using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class four : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_FinalCarts_FinalCartId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FinalCartId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FinalCartId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "FinalCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FinalCarts_productId",
                table: "FinalCarts",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalCarts_Products_productId",
                table: "FinalCarts",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalCarts_Products_productId",
                table: "FinalCarts");

            migrationBuilder.DropIndex(
                name: "IX_FinalCarts_productId",
                table: "FinalCarts");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "FinalCarts");

            migrationBuilder.AddColumn<int>(
                name: "FinalCartId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FinalCartId",
                table: "Products",
                column: "FinalCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_FinalCarts_FinalCartId",
                table: "Products",
                column: "FinalCartId",
                principalTable: "FinalCarts",
                principalColumn: "Id");
        }
    }
}
