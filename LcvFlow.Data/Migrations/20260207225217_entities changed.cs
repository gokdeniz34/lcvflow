using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LcvFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class entitieschanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "TableNumber",
                table: "Guests");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalDataJson",
                table: "Guests",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FormConfigJson",
                table: "Events",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "InstructionSheetNote",
                table: "Events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalDataJson",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "FormConfigJson",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "InstructionSheetNote",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Guests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TableNumber",
                table: "Guests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
