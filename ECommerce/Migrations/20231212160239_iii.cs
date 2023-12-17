using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class iii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_options_ProductOptions_productOptionsId",
                table: "options");

            migrationBuilder.AlterColumn<int>(
                name: "productOptionsId",
                table: "options",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_options_ProductOptions_productOptionsId",
                table: "options",
                column: "productOptionsId",
                principalTable: "ProductOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_options_ProductOptions_productOptionsId",
                table: "options");

            migrationBuilder.AlterColumn<int>(
                name: "productOptionsId",
                table: "options",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_options_ProductOptions_productOptionsId",
                table: "options",
                column: "productOptionsId",
                principalTable: "ProductOptions",
                principalColumn: "Id");
        }
    }
}
