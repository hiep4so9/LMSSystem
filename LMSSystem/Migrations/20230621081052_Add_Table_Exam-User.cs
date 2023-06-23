using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSSystem.Migrations
{
    public partial class Add_Table_ExamUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Classes_ClassID",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "ClassID",
                table: "Exams",
                newName: "CourseID");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_ClassID",
                table: "Exams",
                newName: "IX_Exams_CourseID");

            migrationBuilder.CreateTable(
                name: "Exam_User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ExamID = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GradedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam_User", x => new { x.UserID, x.ExamID });
                    table.ForeignKey(
                        name: "FK_Exam_User_Exams_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exams",
                        principalColumn: "ExamID");
                    table.ForeignKey(
                        name: "FK_Exam_User_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exam_User_ExamID",
                table: "Exam_User",
                column: "ExamID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Courses_CourseID",
                table: "Exams",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Courses_CourseID",
                table: "Exams");

            migrationBuilder.DropTable(
                name: "Exam_User");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Exams",
                newName: "ClassID");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_CourseID",
                table: "Exams",
                newName: "IX_Exams_ClassID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Classes_ClassID",
                table: "Exams",
                column: "ClassID",
                principalTable: "Classes",
                principalColumn: "ClassID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
