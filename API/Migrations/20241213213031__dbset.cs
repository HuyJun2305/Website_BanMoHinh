using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class _dbset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderAddress_Orders_OrderId",
                table: "OrderAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderAddress",
                table: "OrderAddress");

            

            migrationBuilder.RenameTable(
                name: "OrderAddress",
                newName: "OrderAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_OrderAddress_OrderId",
                table: "OrderAddresses",
                newName: "IX_OrderAddresses_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderAddresses",
                table: "OrderAddresses",
                column: "Id");


            migrationBuilder.AddForeignKey(
                name: "FK_OrderAddresses_Orders_OrderId",
                table: "OrderAddresses",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderAddresses_Orders_OrderId",
                table: "OrderAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderAddresses",
                table: "OrderAddresses");

            

            migrationBuilder.RenameTable(
                name: "OrderAddresses",
                newName: "OrderAddress");

            migrationBuilder.RenameIndex(
                name: "IX_OrderAddresses_OrderId",
                table: "OrderAddress",
                newName: "IX_OrderAddress_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderAddress",
                table: "OrderAddress",
                column: "Id");

            

            migrationBuilder.AddForeignKey(
                name: "FK_OrderAddress_Orders_OrderId",
                table: "OrderAddress",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
