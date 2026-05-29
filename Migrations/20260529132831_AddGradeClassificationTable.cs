using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikOcen.Migrations
{
    /// <inheritdoc />
    public partial class AddGradeClassificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeClassificationId",
                table: "StudentGrades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GradeClassifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeClassifications", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHis5W5GG23NFOSrKvIItXZTE+7spKZiP+Qw7ui0xMZs0qGJhFFOaLhYqCvyeschQA==");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_GradeClassificationId",
                table: "StudentGrades",
                column: "GradeClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_GradeClassifications_GradeClassificationId",
                table: "StudentGrades",
                column: "GradeClassificationId",
                principalTable: "GradeClassifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_GradeClassifications_GradeClassificationId",
                table: "StudentGrades");

            migrationBuilder.DropTable(
                name: "GradeClassifications");

            migrationBuilder.DropIndex(
                name: "IX_StudentGrades_GradeClassificationId",
                table: "StudentGrades");

            migrationBuilder.DropColumn(
                name: "GradeClassificationId",
                table: "StudentGrades");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC5Gh5ZYxwVXuITcDppk3s22/HDq0ndrJaoCmKplMbPcG+/Zdnm3JFZsw9tpe0CddQ==");
        }
    }
}
