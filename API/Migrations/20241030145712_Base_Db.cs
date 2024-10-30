using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class Base_Db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("c8878e30-5a84-46b6-a07d-742f152a885e"), new Guid("1de54bfd-4809-4b6f-8c1c-e6441b94d009") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("050c5628-2d9a-4791-80c7-c34336c09204"), new Guid("c8d9dc7d-e40e-41eb-b3cc-dd0ab380006b") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("ae4d1446-7827-4545-aa08-9018691f8d0b"), new Guid("cd274b42-fc29-418c-88ae-4233ea107a92") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("050c5628-2d9a-4791-80c7-c34336c09204"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ae4d1446-7827-4545-aa08-9018691f8d0b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c8878e30-5a84-46b6-a07d-742f152a885e"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1de54bfd-4809-4b6f-8c1c-e6441b94d009"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c8d9dc7d-e40e-41eb-b3cc-dd0ab380006b"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("cd274b42-fc29-418c-88ae-4233ea107a92"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("529f2ca9-a866-441f-a755-b885ab6a9c78"), "e8f43029-964b-4c79-8807-6eba7f6f2dda", "Customer", "CUSTOMER" },
                    { new Guid("d1b25175-b093-4f3e-8db1-c3835c2e2d54"), "6be576ba-a90c-4c4f-956c-37e4fb620306", "Staff", "STAFF" },
                    { new Guid("e5a9340c-6970-495d-81fa-1b8d752d59a4"), "75d8d7de-1a7d-44a5-a9bd-70d3893da178", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImgUrl", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("05c41b46-2643-4640-9c2a-0b408e450119"), 0, "3511474f-2a5a-4dc4-879c-bff88e04e1a9", "staff@gmail.com", false, "", false, null, "khoong", "STAFF@GMAIL.COM", "STAFF", "AQAAAAEAACcQAAAAEFDgU9+eCqaL3TKCKDtZ9v72u4XOvI13FDcWmVURHfXsMYkRiRSN9wh08dPgpmlPQQ==", "chua co", false, "cfd8f1a8-6745-49fd-b70f-55a9da5fd508", false, "staff" },
                    { new Guid("22238106-ccfd-4230-aa39-e434c3236ec4"), 0, "6e6be376-a259-43f3-84da-01b06b67c63c", "user1@gmail.com", false, "", false, null, "khoong", "USER1@GMAIL.COM", "USER1", "AQAAAAEAACcQAAAAEO1+VNa+1vOXFu7X9u5Q7abc6QQfAEU9o1YA8GGkk7XZoNG5z25M89lMoQFX3lRflA==", "chua co", false, "06a5a6ad-a824-497a-ad25-34c55de09702", false, "user1" },
                    { new Guid("828ded69-831f-43b0-8e89-dd535fbb9446"), 0, "7ac9d2b7-e4c6-4029-a4b7-57dc8467f30a", "admin@gmail.com", false, "", false, null, "khoong", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEJBUG9thsOR0TmGt6TdE6+VhaqGCnMkUBtMP7sGYsRZG03Qs9oz8eOJvjbK6MIIv0Q==", "chua co", false, "e353defd-1cbc-4ebb-8006-3bd5f7fa12af", false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d1b25175-b093-4f3e-8db1-c3835c2e2d54"), new Guid("05c41b46-2643-4640-9c2a-0b408e450119") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("529f2ca9-a866-441f-a755-b885ab6a9c78"), new Guid("22238106-ccfd-4230-aa39-e434c3236ec4") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("e5a9340c-6970-495d-81fa-1b8d752d59a4"), new Guid("828ded69-831f-43b0-8e89-dd535fbb9446") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d1b25175-b093-4f3e-8db1-c3835c2e2d54"), new Guid("05c41b46-2643-4640-9c2a-0b408e450119") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("529f2ca9-a866-441f-a755-b885ab6a9c78"), new Guid("22238106-ccfd-4230-aa39-e434c3236ec4") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("e5a9340c-6970-495d-81fa-1b8d752d59a4"), new Guid("828ded69-831f-43b0-8e89-dd535fbb9446") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("529f2ca9-a866-441f-a755-b885ab6a9c78"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d1b25175-b093-4f3e-8db1-c3835c2e2d54"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e5a9340c-6970-495d-81fa-1b8d752d59a4"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("05c41b46-2643-4640-9c2a-0b408e450119"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("22238106-ccfd-4230-aa39-e434c3236ec4"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("828ded69-831f-43b0-8e89-dd535fbb9446"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("050c5628-2d9a-4791-80c7-c34336c09204"), "ad7ae5ca-0a3d-4cd3-a3b8-a4dd77dc4654", "Admin", "ADMIN" },
                    { new Guid("ae4d1446-7827-4545-aa08-9018691f8d0b"), "418a31c5-45b9-4280-8fa5-ff5bdf8b02bf", "Customer", "CUSTOMER" },
                    { new Guid("c8878e30-5a84-46b6-a07d-742f152a885e"), "37b9ba17-e462-428b-a672-cee1b8d0dd19", "Staff", "STAFF" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImgUrl", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("1de54bfd-4809-4b6f-8c1c-e6441b94d009"), 0, "089ef9b5-7cdf-423b-a3a1-c91081ee1fd1", "staff@gmail.com", false, "", false, null, "khoong", "STAFF@GMAIL.COM", "STAFF", "AQAAAAEAACcQAAAAEJUQgLTP5D7W2IcuNzkNrfRsBERti4ROY6p8bCqqWtkJD2AdZ6TYuhsQwO+IfwAb4Q==", "chua co", false, "6fedfb74-f42c-4c6c-8ca0-2f05baba4b05", false, "staff" },
                    { new Guid("c8d9dc7d-e40e-41eb-b3cc-dd0ab380006b"), 0, "127a3721-2ede-4b28-8f90-e8ef117308dc", "admin@gmail.com", false, "", false, null, "khoong", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEM6C3YgtVry0RYZRUTL+ky89ZLVzE6WBo8zxwZ3zQ31kW2lJeg+r2QgCO3vhiFrYbA==", "chua co", false, "afca50f3-4f75-47e2-b431-4d60dbcccd8f", false, "admin" },
                    { new Guid("cd274b42-fc29-418c-88ae-4233ea107a92"), 0, "3e46b0f7-280e-4e93-bac7-9863aed6a2e0", "user1@gmail.com", false, "", false, null, "khoong", "USER1@GMAIL.COM", "USER1", "AQAAAAEAACcQAAAAEA9fuTmtsl/1cpynQe5R1qGxov3C9F4N4wBaFdA/qXIZBS4nHzVO58U5QNJ7ol+5hg==", "chua co", false, "2d4a3c74-0f93-404f-8520-099d0dd58600", false, "user1" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("c8878e30-5a84-46b6-a07d-742f152a885e"), new Guid("1de54bfd-4809-4b6f-8c1c-e6441b94d009") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("050c5628-2d9a-4791-80c7-c34336c09204"), new Guid("c8d9dc7d-e40e-41eb-b3cc-dd0ab380006b") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("ae4d1446-7827-4545-aa08-9018691f8d0b"), new Guid("cd274b42-fc29-418c-88ae-4233ea107a92") });
        }
    }
}
