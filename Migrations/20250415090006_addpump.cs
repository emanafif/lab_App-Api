using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab_App.Migrations
{
    /// <inheritdoc />
    public partial class addpump : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Qpumpplus",
                table: "Labs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Qpumpreturn",
                table: "Labs",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qpumpplus",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "Qpumpreturn",
                table: "Labs");
        }
    }
}
