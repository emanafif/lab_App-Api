using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab_App.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserLabRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Labs_Labslab_id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Labslab_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Labslab_id",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_labId",
                table: "AspNetUsers",
                column: "labId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Labs_labId",
                table: "AspNetUsers",
                column: "labId",
                principalTable: "Labs",
                principalColumn: "lab_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Labs_labId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_labId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Labslab_id",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Labslab_id",
                table: "AspNetUsers",
                column: "Labslab_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Labs_Labslab_id",
                table: "AspNetUsers",
                column: "Labslab_id",
                principalTable: "Labs",
                principalColumn: "lab_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
