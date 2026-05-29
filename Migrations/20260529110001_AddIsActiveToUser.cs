using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikOcen.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsActive", "PasswordHash" },
                values: new object[] { true, "AQAAAAIAAYagAAAAEC5Gh5ZYxwVXuITcDppk3s22/HDq0ndrJaoCmKplMbPcG+/Zdnm3JFZsw9tpe0CddQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAOOKmtjGDbKyZ+LFqSsDhhtn84RVZBu20XJUZgeBaVyDGtlVocxUgSMDTbgu9eJvw==");
        }
    }
}
