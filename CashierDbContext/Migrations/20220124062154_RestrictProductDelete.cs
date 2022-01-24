using Microsoft.EntityFrameworkCore.Migrations;

namespace CashierDB.Migrations
{
    public partial class RestrictProductDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Product_Barcode",
                table: "Item");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Product_Barcode",
                table: "Item",
                column: "Barcode",
                principalTable: "Product",
                principalColumn: "Barcode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Product_Barcode",
                table: "Item");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Product_Barcode",
                table: "Item",
                column: "Barcode",
                principalTable: "Product",
                principalColumn: "Barcode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
