using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a4cb174-c3fe-4a25-be05-387b93c2de03", null, "Employee", "EMPLOYEE" },
                    { "31ba5abb-b764-49b4-8c90-b9969f7a5eda", null, "Administrator", "ADMINISTRATOR" },
                    { "990f4fe3-e2a5-44eb-8585-9b176fd92b7c", null, "Supervisor", "SUPERVISOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6c3fb309-1468-4791-af73-fb73e3efd3a6", 0, "9bd8948f-3e80-4eeb-9f7f-cedc1eda0fef", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEA8OUrN6/9JtpAtyOYQJ02J+WR4zZk4ono5Aq37+zldiF8jpoE9ifuQWhlM9ygPE+g==", null, false, "d46551ea-e7ab-42bb-b266-1fa8272c1046", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "31ba5abb-b764-49b4-8c90-b9969f7a5eda", "6c3fb309-1468-4791-af73-fb73e3efd3a6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a4cb174-c3fe-4a25-be05-387b93c2de03");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "990f4fe3-e2a5-44eb-8585-9b176fd92b7c");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "31ba5abb-b764-49b4-8c90-b9969f7a5eda", "6c3fb309-1468-4791-af73-fb73e3efd3a6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31ba5abb-b764-49b4-8c90-b9969f7a5eda");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6c3fb309-1468-4791-af73-fb73e3efd3a6");
        }
    }
}
