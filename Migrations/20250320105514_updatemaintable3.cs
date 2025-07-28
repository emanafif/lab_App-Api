using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab_App.Migrations
{
    /// <inheritdoc />
    public partial class updatemaintable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mainTableS_Labs_labId",
                table: "mainTableS");

            migrationBuilder.RenameColumn(
                name: "labId",
                table: "mainTableS",
                newName: "lab_id");

            migrationBuilder.RenameIndex(
                name: "IX_mainTableS_labId",
                table: "mainTableS",
                newName: "IX_mainTableS_lab_id");

            migrationBuilder.AddForeignKey(
                name: "FK_mainTableS_Labs_lab_id",
                table: "mainTableS",
                column: "lab_id",
                principalTable: "Labs",
                principalColumn: "lab_id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mainTableS_Labs_lab_id",
                table: "mainTableS");

            migrationBuilder.RenameColumn(
                name: "lab_id",
                table: "mainTableS",
                newName: "labId");

            migrationBuilder.RenameIndex(
                name: "IX_mainTableS_lab_id",
                table: "mainTableS",
                newName: "IX_mainTableS_labId");

            migrationBuilder.AddForeignKey(
                name: "FK_mainTableS_Labs_labId",
                table: "mainTableS",
                column: "labId",
                principalTable: "Labs",
                principalColumn: "lab_id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
