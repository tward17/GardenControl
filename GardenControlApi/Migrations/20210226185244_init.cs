using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GardenControlApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSetting",
                columns: table => new
                {
                    AppSettingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    CanBeUpdated = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetting", x => x.AppSettingId);
                });

            migrationBuilder.CreateTable(
                name: "ControlDevice",
                columns: table => new
                {
                    ControlDeviceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Alias = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    GPIOPinNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultState = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlDevice", x => x.ControlDeviceId);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    TriggerTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    TriggerTimeOfDay = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TriggerOffsetAmount = table.Column<int>(type: "INTEGER", nullable: true),
                    TriggerOffsetAmountTimeIntervalUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    IntervalAmount = table.Column<int>(type: "INTEGER", nullable: true),
                    IntervalAmountTimeIntervalUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    NextRunDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "Measurement",
                columns: table => new
                {
                    MeasurementId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ControlDeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    MeasurementValue = table.Column<double>(type: "REAL", nullable: false),
                    MeasurementUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    MeasurementDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurement", x => x.MeasurementId);
                    table.ForeignKey(
                        name: "FK_Measurement_ControlDevice_ControlDeviceId",
                        column: x => x.ControlDeviceId,
                        principalTable: "ControlDevice",
                        principalColumn: "ControlDeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleTask",
                columns: table => new
                {
                    ScheduleTaskId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false),
                    TaskActionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ControlDeviceId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleTask", x => x.ScheduleTaskId);
                    table.ForeignKey(
                        name: "FK_ScheduleTask_ControlDevice_ControlDeviceId",
                        column: x => x.ControlDeviceId,
                        principalTable: "ControlDevice",
                        principalColumn: "ControlDeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleTask_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_ControlDeviceId",
                table: "Measurement",
                column: "ControlDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTask_ControlDeviceId",
                table: "ScheduleTask",
                column: "ControlDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTask_ScheduleId",
                table: "ScheduleTask",
                column: "ScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSetting");

            migrationBuilder.DropTable(
                name: "Measurement");

            migrationBuilder.DropTable(
                name: "ScheduleTask");

            migrationBuilder.DropTable(
                name: "ControlDevice");

            migrationBuilder.DropTable(
                name: "Schedule");
        }
    }
}
