using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class _111p2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("03aa8a22-8287-41a2-a33f-24249583dde3"), new Guid("2ede68c8-39a1-4d9a-88fb-8eead2fc98e4") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("632effdb-7e6d-406a-8b09-08564c4e8458"), new Guid("aa4615f6-70c3-48cb-a96c-2213189df6d0") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("385c5b59-a55c-4911-98cf-3a7955aab731"), new Guid("fc9d01bf-1a75-43f8-8eaa-3b6915dce99a") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("03aa8a22-8287-41a2-a33f-24249583dde3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("385c5b59-a55c-4911-98cf-3a7955aab731"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("632effdb-7e6d-406a-8b09-08564c4e8458"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("2ede68c8-39a1-4d9a-88fb-8eead2fc98e4"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aa4615f6-70c3-48cb-a96c-2213189df6d0"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fc9d01bf-1a75-43f8-8eaa-3b6915dce99a"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("205ee03c-ef11-4f4d-bc33-fba701a1d514"), "c36e7dd6-6a6d-47dd-b16e-c2a11ba4de12", "Admin", "ADMIN" },
                    { new Guid("4d1b8b0e-40e9-4dfb-8263-b5e12765ae94"), "e0b7c679-4cb1-44cc-aef9-d65c0de33212", "Customer", "CUSTOMER" },
                    { new Guid("5036a282-0685-4a06-9c62-fa9325c2fd16"), "63a8d3d9-4837-4734-88d9-0072fd2e87ac", "Staff", "STAFF" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImgUrl", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("ba35c184-fa16-49c2-9248-373038918d47"), 0, "e963a60c-2a9a-49fe-b3df-ee8e656dd251", "staff@gmail.com", false, "", false, null, "khoong", "STAFF@GMAIL.COM", "STAFF", "AQAAAAEAACcQAAAAELuqqRsJy0flf1sqMiwK2ZarmcP5EvXwYyGi+uYi2j86Yon74Q6rruq6NtFQ/ngVdg==", "chua co", false, "0430c914-e126-4530-9f89-640d79e19a17", false, "staff" },
                    { new Guid("cb9a8b8d-dd30-4367-bf53-ba5d71254901"), 0, "1b54286d-679c-4d55-9097-3fb637d8f454", "admin@gmail.com", false, "", false, null, "khoong", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEHJwzx+2cAkbuHtcBFDNMzjAuzZ5Vw+WbSTDx6sXYM3RIsXMHGY+7NjQoC6OgTt8OA==", "chua co", false, "19f03ca7-bfdd-4e71-9104-02a8256567fb", false, "admin" },
                    { new Guid("fd41dbe1-06b9-4420-9c2c-909028775f96"), 0, "80080073-7f73-4cb0-bb2b-a57797b567dc", "user1@gmail.com", false, "", false, null, "khoong", "USER1@GMAIL.COM", "USER1", "AQAAAAEAACcQAAAAELJ1Ftj1D72mTW6AJzikbJV1RVoUnTPTMBo5DFJZaEeqmaZCQhtz7V/GItMDZuVeOA==", "chua co", false, "d4417ba9-5ffc-4af2-b537-d2785ea1054e", false, "user1" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("5036a282-0685-4a06-9c62-fa9325c2fd16"), new Guid("ba35c184-fa16-49c2-9248-373038918d47") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("205ee03c-ef11-4f4d-bc33-fba701a1d514"), new Guid("cb9a8b8d-dd30-4367-bf53-ba5d71254901") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("4d1b8b0e-40e9-4dfb-8263-b5e12765ae94"), new Guid("fd41dbe1-06b9-4420-9c2c-909028775f96") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("5036a282-0685-4a06-9c62-fa9325c2fd16"), new Guid("ba35c184-fa16-49c2-9248-373038918d47") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("205ee03c-ef11-4f4d-bc33-fba701a1d514"), new Guid("cb9a8b8d-dd30-4367-bf53-ba5d71254901") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("4d1b8b0e-40e9-4dfb-8263-b5e12765ae94"), new Guid("fd41dbe1-06b9-4420-9c2c-909028775f96") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("205ee03c-ef11-4f4d-bc33-fba701a1d514"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4d1b8b0e-40e9-4dfb-8263-b5e12765ae94"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5036a282-0685-4a06-9c62-fa9325c2fd16"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ba35c184-fa16-49c2-9248-373038918d47"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("cb9a8b8d-dd30-4367-bf53-ba5d71254901"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fd41dbe1-06b9-4420-9c2c-909028775f96"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("03aa8a22-8287-41a2-a33f-24249583dde3"), "8920384d-d7f8-4fd5-aac0-ad781396027d", "Admin", "ADMIN" },
                    { new Guid("385c5b59-a55c-4911-98cf-3a7955aab731"), "b1ed1234-4f9e-4d60-a567-61e42e28a30f", "Staff", "STAFF" },
                    { new Guid("632effdb-7e6d-406a-8b09-08564c4e8458"), "983a423d-7343-48e8-a2e2-6b1fdd6c54c9", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "ImgUrl", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("2ede68c8-39a1-4d9a-88fb-8eead2fc98e4"), 0, "ffe00d37-f8b4-4b27-a122-d9f258178879", "admin@gmail.com", false, "", false, null, "khoong", "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEGQxpcCIXTL6eI6729H1ARrzTyogPy2jyTZphhdkGApCHuadxU9kUZ2RsxunjRexvw==", "chua co", false, "5f4bb2ba-8201-4b03-9aae-ab0bb5aa3030", false, "admin" },
                    { new Guid("aa4615f6-70c3-48cb-a96c-2213189df6d0"), 0, "4b5311a9-3369-4e2e-b3b4-92c9789f5e1c", "user1@gmail.com", false, "", false, null, "khoong", "USER1@GMAIL.COM", "USER1", "AQAAAAEAACcQAAAAEM9c07pxfHhyzY7VlsuMHqG/cZ7HNSy61/kvllqkk7rWTrI07EUBrXXRztjHfVgefg==", "chua co", false, "432b377d-8cee-4eca-ae97-c0d2bb93a2b5", false, "user1" },
                    { new Guid("fc9d01bf-1a75-43f8-8eaa-3b6915dce99a"), 0, "8bb3434b-5d9e-4358-8d5e-d73fce203f80", "staff@gmail.com", false, "", false, null, "khoong", "STAFF@GMAIL.COM", "STAFF", "AQAAAAEAACcQAAAAEL8BJx4iJRskWCgk9BgkcWdElcquXN8Lh8THvTx5QhyxxtoVthqUeH8ZdwdARWBm+w==", "chua co", false, "3e2b1ac0-ebee-4f48-bba7-ccc71965db3a", false, "staff" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("03aa8a22-8287-41a2-a33f-24249583dde3"), new Guid("2ede68c8-39a1-4d9a-88fb-8eead2fc98e4") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("632effdb-7e6d-406a-8b09-08564c4e8458"), new Guid("aa4615f6-70c3-48cb-a96c-2213189df6d0") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("385c5b59-a55c-4911-98cf-3a7955aab731"), new Guid("fc9d01bf-1a75-43f8-8eaa-3b6915dce99a") });
        }
    }
}
