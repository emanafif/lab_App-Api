using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab_App.Migrations
{
    /// <inheritdoc />
    public partial class createlabdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Labslab_id",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "labId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Labs",
                columns: table => new
                {
                    lab_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lab_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labs", x => x.lab_id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Labs_Labslab_id",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Labs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Labslab_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Labslab_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "labId",
                table: "AspNetUsers");
        }
    }
}
