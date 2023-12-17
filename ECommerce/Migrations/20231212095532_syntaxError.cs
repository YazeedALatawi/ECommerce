using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class syntaxError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptions_Products_productId",
                table: "ProductOptions");

            migrationBuilder.DropIndex(
                name: "IX_ProductOptions_productId",
                table: "ProductOptions");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "ProductOptions");

            migrationBuilder.AlterColumn<int>(
                name: "prdouctId",
                table: "ProductOptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptions_prdouctId",
                table: "ProductOptions",
                column: "prdouctId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOptions_Products_prdouctId",
                table: "ProductOptions",
                column: "prdouctId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptions_Products_prdouctId",
                table: "ProductOptions");

            migrationBuilder.DropIndex(
                name: "IX_ProductOptions_prdouctId",
                table: "ProductOptions");

            migrationBuilder.AlterColumn<int>(
                name: "prdouctId",
                table: "ProductOptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "ProductOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptions_productId",
                table: "ProductOptions",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOptions_Products_productId",
                table: "ProductOptions",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
