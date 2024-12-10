using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class _fix_role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("617d2a53-59bf-4828-88b2-1617aac0f028"), new Guid("21735c02-cff2-45ce-96bc-76e837bd3782") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("617d2a53-59bf-4828-88b2-1617aac0f028"), new Guid("c39e6070-4a44-42a3-b822-d5ec876fa7e9") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("660852a8-a4a6-4329-aa3d-db14b5ee4abb"), new Guid("c39e6070-4a44-42a3-b822-d5ec876fa7e9") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("becc7f50-faa9-4ddd-914f-8ebeb53424f8"), new Guid("c39e6070-4a44-42a3-b822-d5ec876fa7e9") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("617d2a53-59bf-4828-88b2-1617aac0f028"), new Guid("e61c9d93-ece2-4e35-aed2-22a19ec2b1c6") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("660852a8-a4a6-4329-aa3d-db14b5ee4abb"), new Guid("e61c9d93-ece2-4e35-aed2-22a19ec2b1c6") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("617d2a53-59bf-4828-88b2-1617aac0f028"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("660852a8-a4a6-4329-aa3d-db14b5ee4abb"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("becc7f50-faa9-4ddd-914f-8ebeb53424f8"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("21735c02-cff2-45ce-96bc-76e837bd3782"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c39e6070-4a44-42a3-b822-d5ec876fa7e9"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e61c9d93-ece2-4e35-aed2-22a19ec2b1c6"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("27cd84d4-9d98-4049-8b34-09cbbdd02d57"), "c01e6780-b746-4893-a68d-7ff6a95f7532", "Staff", "STAFF" },
                    { new Guid("2f2f4628-2dbd-4cdd-bf59-b4a0aafdfd5e"), "3a11bc61-d3c8-456f-837d-e64d3c92cc82", "Customer", "CUSTOMER" },
                    { new Guid("51bef982-5f47-41ac-895c-587ba8516f06"), "52726267-c315-4fcc-b459-739b26bfc69a", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImgUrl", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("0d244281-e717-447e-965d-e25ac18944e0"), 0, "f030fa3b-002f-4e33-8df8-ae4f77964dc8", "staff@gmail.com", false, "", false, null, "khoong", "STAFF@GMAIL.COM", "STAFF", "AQAAAAEAACcQAAAAEPL+BPf66MaROmCce9hmvA8vVO5jvBQNoqFP1AswY7ZGcZpOl9T+TE49fh/8slVrHw==", "chua co", false, "ad7e3ec2-8a2c-4741-bb24-7920f3ec0dd1", false, "staff" },
                    { new Guid("843f6d29-8ef8-4c1b-91cd-c9a055842b26"), 0, "dbdda04d-96cc-4484-8e8a-f0f65d164a85", "admin@gmail.com", false, "", false, null, "khoong", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEEcHjXuPwdGy//xi3mZ1YFRL6TCtwRBW3EZRM1+eSApH2dz4pir0wA5RZ6zCSsJxWQ==", "chua co", false, "eab5374a-d8a3-448b-9fd2-f47c290ae92d", false, "admin" },
                    { new Guid("b54dbab1-c873-436e-8c53-6a9ba5e0da4c"), 0, "513e6d5f-1eb2-4c6b-971c-22c0850c970e", "user1@gmail.com", false, "", false, null, "khoong", "USER1@GMAIL.COM", "USER1", "AQAAAAEAACcQAAAAEJPbKbhILGCOm38sqhoc2aTS+xy2pJMPsK/PPMjXTbybD67IEuw+FlcczQW1Xnaq+g==", "chua co", false, "697b7fbd-266b-4a41-9304-12bced6d858e", false, "user1" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("27cd84d4-9d98-4049-8b34-09cbbdd02d57"), new Guid("0d244281-e717-447e-965d-e25ac18944e0") },
                    { new Guid("27cd84d4-9d98-4049-8b34-09cbbdd02d57"), new Guid("843f6d29-8ef8-4c1b-91cd-c9a055842b26") },
                    { new Guid("51bef982-5f47-41ac-895c-587ba8516f06"), new Guid("843f6d29-8ef8-4c1b-91cd-c9a055842b26") },
                    { new Guid("2f2f4628-2dbd-4cdd-bf59-b4a0aafdfd5e"), new Guid("b54dbab1-c873-436e-8c53-6a9ba5e0da4c") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("27cd84d4-9d98-4049-8b34-09cbbdd02d57"), new Guid("0d244281-e717-447e-965d-e25ac18944e0") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("27cd84d4-9d98-4049-8b34-09cbbdd02d57"), new Guid("843f6d29-8ef8-4c1b-91cd-c9a055842b26") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("51bef982-5f47-41ac-895c-587ba8516f06"), new Guid("843f6d29-8ef8-4c1b-91cd-c9a055842b26") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("2f2f4628-2dbd-4cdd-bf59-b4a0aafdfd5e"), new Guid("b54dbab1-c873-436e-8c53-6a9ba5e0da4c") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("27cd84d4-9d98-4049-8b34-09cbbdd02d57"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2f2f4628-2dbd-4cdd-bf59-b4a0aafdfd5e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("51bef982-5f47-41ac-895c-587ba8516f06"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0d244281-e717-447e-965d-e25ac18944e0"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("843f6d29-8ef8-4c1b-91cd-c9a055842b26"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b54dbab1-c873-436e-8c53-6a9ba5e0da4c"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("617d2a53-59bf-4828-88b2-1617aac0f028"), "d2a83c37-059e-40a8-b199-ce9bff72baae", "Customer", "CUSTOMER" },
                    { new Guid("660852a8-a4a6-4329-aa3d-db14b5ee4abb"), "88778c56-9ee6-42d1-91a7-97b9a8603b9d", "Staff", "STAFF" },
                    { new Guid("becc7f50-faa9-4ddd-914f-8ebeb53424f8"), "7e2fa601-9d59-4590-b97d-b2980b3aacbb", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImgUrl", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("21735c02-cff2-45ce-96bc-76e837bd3782"), 0, "06371ffc-f696-48bd-b1d9-c5576abe3f6c", "user1@gmail.com", false, "", false, null, "khoong", "USER1@GMAIL.COM", "USER1", "AQAAAAEAACcQAAAAEHR9yThWOGiiTTNSJZ526b7QEBpJGLyFQ+Hb9ZD4QLL2//u/L6atDj38StfodQOnZg==", "chua co", false, "59f3e7f6-cac7-424f-83f7-821c4d842265", false, "user1" },
                    { new Guid("c39e6070-4a44-42a3-b822-d5ec876fa7e9"), 0, "a70119dd-3bc2-4654-ab83-3e48de59a161", "admin@gmail.com", false, "", false, null, "khoong", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEAyeA9YwXAGMQP2QJ8w5Rb90/V1jMZPWfH2j07hdLV8A/dKoOjEwM6vi7ZR93lkO3w==", "chua co", false, "4a150839-1483-4111-a09f-8eea6594b697", false, "admin" },
                    { new Guid("e61c9d93-ece2-4e35-aed2-22a19ec2b1c6"), 0, "bbd7e0f0-1c05-42c6-8eb3-6b66cc615378", "staff@gmail.com", false, "", false, null, "khoong", "STAFF@GMAIL.COM", "STAFF", "AQAAAAEAACcQAAAAEFgSKQ9C3mubdFGFISldw2x5D4rJP/skiKV58JTMOlr5X4Yet9UlcUuDtCowu4WXnQ==", "chua co", false, "d0d6a5aa-3344-41dc-988f-8c8c25189c00", false, "staff" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("617d2a53-59bf-4828-88b2-1617aac0f028"), new Guid("21735c02-cff2-45ce-96bc-76e837bd3782") },
                    { new Guid("617d2a53-59bf-4828-88b2-1617aac0f028"), new Guid("c39e6070-4a44-42a3-b822-d5ec876fa7e9") },
                    { new Guid("660852a8-a4a6-4329-aa3d-db14b5ee4abb"), new Guid("c39e6070-4a44-42a3-b822-d5ec876fa7e9") },
                    { new Guid("becc7f50-faa9-4ddd-914f-8ebeb53424f8"), new Guid("c39e6070-4a44-42a3-b822-d5ec876fa7e9") },
                    { new Guid("617d2a53-59bf-4828-88b2-1617aac0f028"), new Guid("e61c9d93-ece2-4e35-aed2-22a19ec2b1c6") },
                    { new Guid("660852a8-a4a6-4329-aa3d-db14b5ee4abb"), new Guid("e61c9d93-ece2-4e35-aed2-22a19ec2b1c6") }
                });
        }
    }
}
