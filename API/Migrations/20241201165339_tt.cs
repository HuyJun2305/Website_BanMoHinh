using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class tt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "AspNetUsers");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<Guid>(
                name: "CartId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

           
        }
    }
}
