using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triolingo.Core.Migrations
{
    public partial class updateExerciseFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Exercise",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Exercise");
        }
    }
}
