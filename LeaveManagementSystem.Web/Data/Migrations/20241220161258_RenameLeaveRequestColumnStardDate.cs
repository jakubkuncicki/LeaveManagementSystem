using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameLeaveRequestColumnStardDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StardDate",
                table: "LeaveRequests",
                newName: "StartDate");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6c3fb309-1468-4791-af73-fb73e3efd3a6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f248dcff-dbf0-423f-88fe-a1ee3b79f410", "AQAAAAIAAYagAAAAEMcwu1crAaRHJHdu2hyT5k4OyZY2D7ihoh9Mt0FPAZ3lO/EbK/rN+b8/di8s3xzkxw==", "a01661a0-82de-4cc7-9f17-ebfd76280738" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "LeaveRequests",
                newName: "StardDate");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6c3fb309-1468-4791-af73-fb73e3efd3a6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5af28d02-490a-46f7-a20f-c913740a3ef3", "AQAAAAIAAYagAAAAEIdXAz3E3MMk4qoluvqhjv6kSfqncBqO2y66nDK13l1lZiJB3/OsXmNbsSvQdHVCcQ==", "447cb6e9-0824-401e-837c-6ac8678819f1" });
        }
    }
}
