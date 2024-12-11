using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class _add_stock_productsize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Products_ProductId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_ProductId",
                table: "Sizes");

            

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Sizes");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "ProductSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "ProductSizes");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Sizes",
                type: "uniqueidentifier",
                nullable: true);

            

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_ProductId",
                table: "Sizes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Products_ProductId",
                table: "Sizes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
