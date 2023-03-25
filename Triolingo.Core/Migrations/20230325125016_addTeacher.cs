using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triolingo.Core.Migrations
{
    public partial class addTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStudent",
                table: "StudentCourse",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStudent",
                table: "StudentCourse");
        }
    }
}
