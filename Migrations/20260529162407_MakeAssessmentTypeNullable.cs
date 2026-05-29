using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikOcen.Migrations
{
    /// <inheritdoc />
    public partial class MakeAssessmentTypeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AssessmentTypeId",
                table: "StudentGrades",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENyqccLrYb58Awp8xYUysaEX+YMFffUtfyuUQsxOB9CD8VLk/Og82VO4zZyGIONB/w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AssessmentTypeId",
                table: "StudentGrades",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHis5W5GG23NFOSrKvIItXZTE+7spKZiP+Qw7ui0xMZs0qGJhFFOaLhYqCvyeschQA==");
        }
    }
}
