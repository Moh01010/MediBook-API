using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediBook.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate_StartTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "DoctorDaysOff",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_Date_StartTime",
                table: "Appointments",
                columns: new[] { "DoctorId", "Date", "StartTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_Date_StartTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "DoctorDaysOff",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate_StartTime",
                table: "Appointments",
                columns: new[] { "DoctorId", "AppointmentDate", "StartTime" },
                unique: true);
        }
    }
}
