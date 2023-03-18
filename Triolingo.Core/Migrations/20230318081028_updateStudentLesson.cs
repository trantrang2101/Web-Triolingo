using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triolingo.Core.Migrations
{
    public partial class updateStudentLesson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentLesson_Lesson_LessonId",
                table: "StudentLesson");

            migrationBuilder.DropIndex(
                name: "IX_StudentLesson_LessonId",
                table: "StudentLesson");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "StudentLesson");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLesson_LessionId",
                table: "StudentLesson",
                column: "LessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentLesson_Lesson_LessionId",
                table: "StudentLesson",
                column: "LessionId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentLesson_Lesson_LessionId",
                table: "StudentLesson");

            migrationBuilder.DropIndex(
                name: "IX_StudentLesson_LessionId",
                table: "StudentLesson");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "StudentLesson",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentLesson_LessonId",
                table: "StudentLesson",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentLesson_Lesson_LessonId",
                table: "StudentLesson",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
