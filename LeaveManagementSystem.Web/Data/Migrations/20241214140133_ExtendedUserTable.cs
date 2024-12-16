using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6c3fb309-1468-4791-af73-fb73e3efd3a6",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a072b865-0a4e-4b5f-ab7f-82b3a54339b3", new DateOnly(1980, 12, 19), "Deafault", "Admin", "AQAAAAIAAYagAAAAENTW9VT1cYwHJMZgh/SLtJG0iejHqPwHZAGTVGLkJ7rOjBsEu1plmoLU5fA/ZRvaNA==", "cfa2544c-b0a4-41cd-af0e-6154140e0df0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6c3fb309-1468-4791-af73-fb73e3efd3a6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9bd8948f-3e80-4eeb-9f7f-cedc1eda0fef", "AQAAAAIAAYagAAAAEA8OUrN6/9JtpAtyOYQJ02J+WR4zZk4ono5Aq37+zldiF8jpoE9ifuQWhlM9ygPE+g==", "d46551ea-e7ab-42bb-b266-1fa8272c1046" });
        }
    }
}
