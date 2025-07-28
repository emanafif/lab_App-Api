using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab_App.Migrations
{
    /// <inheritdoc />
    public partial class addpump2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Qpumpplus",
                table: "mainTableS",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Qpumpreturn",
                table: "mainTableS",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Qtimeras",
                table: "mainTableS",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qpumpplus",
                table: "mainTableS");

            migrationBuilder.DropColumn(
                name: "Qpumpreturn",
                table: "mainTableS");

            migrationBuilder.DropColumn(
                name: "Qtimeras",
                table: "mainTableS");
        }
    }
}
