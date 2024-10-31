using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class Fixed_Dd_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Materials_MaterialId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Promotions_PromotionId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Sizes_SizeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductId",
                table: "Images");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("88c81da7-ce74-4c67-a20a-fe82d16585c4"), new Guid("46f36725-2255-4fa7-bb81-82c459769e29") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("8c857644-06c2-4b43-aebe-e4da93dba60e"), new Guid("aa51b02e-2c29-44d1-8cf4-233890bc5c74") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("38297047-64dd-4b9e-b516-d0b780852c04"), new Guid("f6b6a3b6-c37c-4b55-a278-2d505c8d8831") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("38297047-64dd-4b9e-b516-d0b780852c04"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("88c81da7-ce74-4c67-a20a-fe82d16585c4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8c857644-06c2-4b43-aebe-e4da93dba60e"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("46f36725-2255-4fa7-bb81-82c459769e29"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aa51b02e-2c29-44d1-8cf4-233890bc5c74"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f6b6a3b6-c37c-4b55-a278-2d505c8d8831"));

            migrationBuilder.AlterColumn<Guid>(
                name: "SizeId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PromotionId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6f7c70e9-73be-4a20-9584-482b195764f6"), "d465fca2-bc2b-4075-a325-72c761b2c0fa", "Customer", "CUSTOMER" },
                    { new Guid("9350f0d9-8a79-41d3-a5b1-583253051f9e"), "32e5ea4a-9cca-4b1c-a948-9365e114b970", "Staff", "STAFF" },
                    { new Guid("db614e5f-0870-40f9-95ba-8779ef4c3d60"), "c471806b-6277-439c-8320-594047730d42", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImgUrl", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("467abb61-0158-4505-9bb0-8e7cc0f1bef1"), 0, "62ffef8d-ccd7-4af5-9df5-9e9238e5a13f", "user1@gmail.com", false, "", false, null, "khoong", "USER1@GMAIL.COM", "USER1", "AQAAAAEAACcQAAAAEEhWzLzA2uSwTsUhrBB0TJXir1rnMZP3Rf/7pgpeG5v5zjdSl3FbaUpxbBJ46Q1jBw==", "chua co", false, "97356ef9-f74f-47a9-9def-fb2fc5f3741f", false, "user1" },
                    { new Guid("b7082ff2-b583-4e7b-a080-ea70f4148711"), 0, "a335604e-12df-48bf-8c90-6e66815c8eee", "staff@gmail.com", false, "", false, null, "khoong", "STAFF@GMAIL.COM", "STAFF", "AQAAAAEAACcQAAAAEALS5hD70h+BH+Lj32Gvqsrh3RQM+Nf3VvE5kBRsJDxLL4yeabfFNYcdVP+0/sjc0w==", "chua co", false, "44110947-82d8-4453-bb82-ccbd22820ae0", false, "staff" },
                    { new Guid("f258d77a-30d7-4595-941a-f68e953be916"), 0, "6e26575b-75ab-4959-9259-30e458429ea2", "admin@gmail.com", false, "", false, null, "khoong", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEOyfVjc1NfqIxyczge41WOmGgAMv7NfzvyYI3AZFxREbfuHzZ2xRJoXgMZVen6nNJA==", "chua co", false, "009b4f69-8625-4492-9d49-5d3bf8fb016a", false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("6f7c70e9-73be-4a20-9584-482b195764f6"), new Guid("467abb61-0158-4505-9bb0-8e7cc0f1bef1") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("9350f0d9-8a79-41d3-a5b1-583253051f9e"), new Guid("b7082ff2-b583-4e7b-a080-ea70f4148711") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("db614e5f-0870-40f9-95ba-8779ef4c3d60"), new Guid("f258d77a-30d7-4595-941a-f68e953be916") });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Materials_MaterialId",
                table: "Products",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Promotions_PromotionId",
                table: "Products",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Sizes_SizeId",
                table: "Products",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Materials_MaterialId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Promotions_PromotionId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Sizes_SizeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductId",
                table: "Images");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("6f7c70e9-73be-4a20-9584-482b195764f6"), new Guid("467abb61-0158-4505-9bb0-8e7cc0f1bef1") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("9350f0d9-8a79-41d3-a5b1-583253051f9e"), new Guid("b7082ff2-b583-4e7b-a080-ea70f4148711") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("db614e5f-0870-40f9-95ba-8779ef4c3d60"), new Guid("f258d77a-30d7-4595-941a-f68e953be916") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6f7c70e9-73be-4a20-9584-482b195764f6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9350f0d9-8a79-41d3-a5b1-583253051f9e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("db614e5f-0870-40f9-95ba-8779ef4c3d60"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("467abb61-0158-4505-9bb0-8e7cc0f1bef1"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b7082ff2-b583-4e7b-a080-ea70f4148711"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f258d77a-30d7-4595-941a-f68e953be916"));

            migrationBuilder.AlterColumn<Guid>(
                name: "SizeId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PromotionId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("38297047-64dd-4b9e-b516-d0b780852c04"), "66fd9a33-8144-47bd-a999-0dc372a0d52c", "Staff", "STAFF" },
                    { new Guid("88c81da7-ce74-4c67-a20a-fe82d16585c4"), "29ab5713-8fac-457d-974b-5ed0ddea848b", "Customer", "CUSTOMER" },
                    { new Guid("8c857644-06c2-4b43-aebe-e4da93dba60e"), "68c376a3-d67d-4816-9adf-333641c88080", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImgUrl", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("46f36725-2255-4fa7-bb81-82c459769e29"), 0, "8cfa625b-32f3-4983-b8c9-24b534aa5fdb", "user1@gmail.com", false, "", false, null, "khoong", "USER1@GMAIL.COM", "USER1", "AQAAAAEAACcQAAAAEA60/ZTDt998tW0dO1m/EXTUT3kl4AxicbenQ2hyCIS3U+4h4TdDUDlkEJKi0iW/Sg==", "chua co", false, "1dd14c31-f694-4f5b-9199-61871b55c400", false, "user1" },
                    { new Guid("aa51b02e-2c29-44d1-8cf4-233890bc5c74"), 0, "c0911bf9-2897-423c-8727-f18a4672154e", "admin@gmail.com", false, "", false, null, "khoong", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEETr3cLUFnpRQkG9ixxldYc2PRVz3ML7odOqrAaevY1GTpLZgyxmy62wb+1UHtuWgQ==", "chua co", false, "087a7c6e-93ac-43a2-a970-089e24b03515", false, "admin" },
                    { new Guid("f6b6a3b6-c37c-4b55-a278-2d505c8d8831"), 0, "a1da29b0-3dd0-47d1-942e-0041c9197a1c", "staff@gmail.com", false, "", false, null, "khoong", "STAFF@GMAIL.COM", "STAFF", "AQAAAAEAACcQAAAAEJJzzukqpRQzX3EpyF8au91DEPKHf21/hOX49rgGn1EjjUmft4cG2w9khIOavucPtw==", "chua co", false, "5bb507b4-90cc-4f36-afd9-f8e79f5bb40b", false, "staff" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("88c81da7-ce74-4c67-a20a-fe82d16585c4"), new Guid("46f36725-2255-4fa7-bb81-82c459769e29") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("8c857644-06c2-4b43-aebe-e4da93dba60e"), new Guid("aa51b02e-2c29-44d1-8cf4-233890bc5c74") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("38297047-64dd-4b9e-b516-d0b780852c04"), new Guid("f6b6a3b6-c37c-4b55-a278-2d505c8d8831") });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Materials_MaterialId",
                table: "Products",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Promotions_PromotionId",
                table: "Products",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Sizes_SizeId",
                table: "Products",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
