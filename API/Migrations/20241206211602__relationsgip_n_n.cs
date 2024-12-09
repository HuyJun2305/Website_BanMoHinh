using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class _relationsgip_n_n : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Products_ProductsId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_ProductsId",
                table: "Sizes");

           

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Sizes");

            migrationBuilder.CreateTable(
                name: "ProductSizes",
                columns: table => new
                {
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SizesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSizes", x => new { x.ProductsId, x.SizesId });
                    table.ForeignKey(
                        name: "FK_ProductSizes_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSizes_Sizes_SizesId",
                        column: x => x.SizesId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_SizesId",
                table: "ProductSizes",
                column: "SizesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSizes");

            

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "Sizes",
                type: "uniqueidentifier",
                nullable: true);

            

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_ProductsId",
                table: "Sizes",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Products_ProductsId",
                table: "Sizes",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
