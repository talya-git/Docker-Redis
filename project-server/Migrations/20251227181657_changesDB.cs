using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class changesDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customerDatails_Customers_CustomerId",
                table: "customerDatails");

            migrationBuilder.DropForeignKey(
                name: "FK_customerDatails_Gifts_GiftId",
                table: "customerDatails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customerDatails",
                table: "customerDatails");

            migrationBuilder.RenameTable(
                name: "customerDatails",
                newName: "CustomerDetails");

            migrationBuilder.RenameIndex(
                name: "IX_customerDatails_GiftId",
                table: "CustomerDetails",
                newName: "IX_CustomerDetails_GiftId");

            migrationBuilder.RenameIndex(
                name: "IX_customerDatails_CustomerId",
                table: "CustomerDetails",
                newName: "IX_CustomerDetails_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerDetails",
                table: "CustomerDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDetails_Customers_CustomerId",
                table: "CustomerDetails",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDetails_Gifts_GiftId",
                table: "CustomerDetails",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDetails_Customers_CustomerId",
                table: "CustomerDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDetails_Gifts_GiftId",
                table: "CustomerDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerDetails",
                table: "CustomerDetails");

            migrationBuilder.RenameTable(
                name: "CustomerDetails",
                newName: "customerDatails");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerDetails_GiftId",
                table: "customerDatails",
                newName: "IX_customerDatails_GiftId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerDetails_CustomerId",
                table: "customerDatails",
                newName: "IX_customerDatails_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customerDatails",
                table: "customerDatails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_customerDatails_Customers_CustomerId",
                table: "customerDatails",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customerDatails_Gifts_GiftId",
                table: "customerDatails",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
