using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsEshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexOnOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderNumber_CreatedAt",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId_CreatedAt",
                table: "Orders",
                columns: new[] { "ApplicationUserId", "CreatedAt" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ApplicationUserId_CreatedAt",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber_CreatedAt",
                table: "Orders",
                columns: new[] { "OrderNumber", "CreatedAt" },
                unique: true);
        }
    }
}
