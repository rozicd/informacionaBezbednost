using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IB_projekat.Migrations
{
    /// <inheritdoc />
    public partial class hashmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "hash",
                table: "ActivationTokens",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hash",
                table: "ActivationTokens");
        }
    }
}
