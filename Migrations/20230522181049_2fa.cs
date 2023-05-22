using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IB_projekat.Migrations
{
    /// <inheritdoc />
    public partial class _2fa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "SmsVerificationCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "SmsVerificationCodes");
        }
    }
}
