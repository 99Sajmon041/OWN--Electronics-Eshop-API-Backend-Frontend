using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsEshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetUsers");
        }
    }
}
