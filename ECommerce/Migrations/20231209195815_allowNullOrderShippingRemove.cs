using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class allowNullOrderShippingRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shippings_shippingId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_shippingId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "shippingId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "shipping",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shipping",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "shippingId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_shippingId",
                table: "Orders",
                column: "shippingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shippings_shippingId",
                table: "Orders",
                column: "shippingId",
                principalTable: "Shippings",
                principalColumn: "Id");
        }
    }
}
