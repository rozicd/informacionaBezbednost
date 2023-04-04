using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IB_projekat.Migrations
{
    /// <inheritdoc />
    public partial class Flags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Flags",
                table: "Requests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flags",
                table: "Requests");
        }
    }
}
