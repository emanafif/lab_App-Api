using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab_App.Migrations
{
    /// <inheritdoc />
    public partial class updatemaintable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "labId",
                table: "mainTableS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_mainTableS_labId",
                table: "mainTableS",
                column: "labId");

            migrationBuilder.AddForeignKey(
                name: "FK_mainTableS_Labs_labId",
                table: "mainTableS",
                column: "labId",
                principalTable: "Labs",
                principalColumn: "lab_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mainTableS_Labs_labId",
                table: "mainTableS");

            migrationBuilder.DropIndex(
                name: "IX_mainTableS_labId",
                table: "mainTableS");

            migrationBuilder.DropColumn(
                name: "labId",
                table: "mainTableS");
        }
    }
}
