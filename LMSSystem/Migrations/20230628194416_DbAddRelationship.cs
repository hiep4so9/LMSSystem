using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSSystem.Migrations
{
    public partial class DbAddRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleID1",
                table: "User_Schedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID1",
                table: "User_Schedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignmentID",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Schedule_ScheduleID1",
                table: "User_Schedule",
                column: "ScheduleID1");

            migrationBuilder.CreateIndex(
                name: "IX_User_Schedule_UserID1",
                table: "User_Schedule",
                column: "UserID1");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_AssignmentID",
                table: "Classes",
                column: "AssignmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Assignments_AssignmentID",
                table: "Classes",
                column: "AssignmentID",
                principalTable: "Assignments",
                principalColumn: "AssignmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Schedule_Schedules_ScheduleID1",
                table: "User_Schedule",
                column: "ScheduleID1",
                principalTable: "Schedules",
                principalColumn: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Schedule_User_UserID1",
                table: "User_Schedule",
                column: "UserID1",
                principalTable: "User",
                principalColumn: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Assignments_AssignmentID",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Schedule_Schedules_ScheduleID1",
                table: "User_Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Schedule_User_UserID1",
                table: "User_Schedule");

            migrationBuilder.DropIndex(
                name: "IX_User_Schedule_ScheduleID1",
                table: "User_Schedule");

            migrationBuilder.DropIndex(
                name: "IX_User_Schedule_UserID1",
                table: "User_Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Classes_AssignmentID",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "ScheduleID1",
                table: "User_Schedule");

            migrationBuilder.DropColumn(
                name: "UserID1",
                table: "User_Schedule");

            migrationBuilder.DropColumn(
                name: "AssignmentID",
                table: "Classes");
        }
    }
}
