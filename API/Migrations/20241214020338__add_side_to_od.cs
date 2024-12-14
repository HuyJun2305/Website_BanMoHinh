using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class _add_side_to_od : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingFee",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductSizeProductId",
                table: "OrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductSizeSizeId",
                table: "OrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SizeId",
                table: "OrderDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductSizeProductId_ProductSizeSizeId",
                table: "OrderDetails",
                columns: new[] { "ProductSizeProductId", "ProductSizeSizeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductSizes_ProductSizeProductId_ProductSizeSizeId",
                table: "OrderDetails",
                columns: new[] { "ProductSizeProductId", "ProductSizeSizeId" },
                principalTable: "ProductSizes",
                principalColumns: new[] { "ProductId", "SizeId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductSizes_ProductSizeProductId_ProductSizeSizeId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductSizeProductId_ProductSizeSizeId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductSizeProductId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductSizeSizeId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingFee",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

           
        }
    }
}
