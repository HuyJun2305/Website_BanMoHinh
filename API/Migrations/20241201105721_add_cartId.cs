using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class add_cartId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<Guid>(
                name: "CartId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "AspNetUsers");

            
        }
    }
}
