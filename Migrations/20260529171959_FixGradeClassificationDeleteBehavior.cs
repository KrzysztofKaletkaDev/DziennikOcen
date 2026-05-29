using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikOcen.Migrations
{
    /// <inheritdoc />
    public partial class FixGradeClassificationDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_GradeClassifications_GradeClassificationId",
                table: "StudentGrades");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGCU9gDodQlRx3lDe3q1EMKvvVVOxJlWz84BydpGGvBcUB4VKtLQ1RuQEoMWboGEPg==");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_GradeClassifications_GradeClassificationId",
                table: "StudentGrades",
                column: "GradeClassificationId",
                principalTable: "GradeClassifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_GradeClassifications_GradeClassificationId",
                table: "StudentGrades");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENyqccLrYb58Awp8xYUysaEX+YMFffUtfyuUQsxOB9CD8VLk/Og82VO4zZyGIONB/w==");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_GradeClassifications_GradeClassificationId",
                table: "StudentGrades",
                column: "GradeClassificationId",
                principalTable: "GradeClassifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
