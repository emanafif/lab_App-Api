using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab_App.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyWithoutCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mainTableS_Labs_labId",
                table: "mainTableS");

            migrationBuilder.AlterColumn<int>(
                name: "labId",
                table: "mainTableS",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_mainTableS_Labs_labId",
                table: "mainTableS",
                column: "labId",
                principalTable: "Labs",
                principalColumn: "lab_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mainTableS_Labs_labId",
                table: "mainTableS");

            migrationBuilder.AlterColumn<int>(
                name: "labId",
                table: "mainTableS",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_mainTableS_Labs_labId",
                table: "mainTableS",
                column: "labId",
                principalTable: "Labs",
                principalColumn: "lab_id");
        }
    }
}
