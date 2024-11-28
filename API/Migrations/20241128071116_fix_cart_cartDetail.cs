using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class fix_cart_cartDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CartDetails",
                newName: "TotalPrice");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "CartDetails",
                newName: "Price");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            
        }
    }
}
