using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class addCartProductsOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartProductsOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainOptionID = table.Column<int>(type: "int", nullable: false),
                    MainOptionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubOptionID = table.Column<int>(type: "int", nullable: false),
                    SubOptionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CartProductsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartProductsOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartProductsOptions_FinalCarts_CartProductsID",
                        column: x => x.CartProductsID,
                        principalTable: "FinalCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartProductsOptions_CartProductsID",
                table: "CartProductsOptions",
                column: "CartProductsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartProductsOptions");
        }
    }
}
