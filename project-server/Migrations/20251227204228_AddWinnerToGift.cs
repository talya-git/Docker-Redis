using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class AddWinnerToGift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "Gifts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_WinnerId",
                table: "Gifts",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Customers_WinnerId",
                table: "Gifts",
                column: "WinnerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Customers_WinnerId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_WinnerId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Gifts");
        }
    }
}
