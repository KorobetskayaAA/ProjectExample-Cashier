using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CashierDB.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Barcode = table.Column<string>(type: "char(13)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Barcode);
                });

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Bill_BillStatus_Status",
                        column: x => x.Status,
                        principalTable: "BillStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "char(13)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Bill = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Bill_Bill",
                        column: x => x.Bill,
                        principalTable: "Bill",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_Product_Barcode",
                        column: x => x.Barcode,
                        principalTable: "Product",
                        principalColumn: "Barcode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BillStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "Открыт" });

            migrationBuilder.InsertData(
                table: "BillStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Закрыт" });

            migrationBuilder.InsertData(
                table: "BillStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Отмена" });

            migrationBuilder.CreateIndex(
                name: "IX_Bill_Status",
                table: "Bill",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Barcode",
                table: "Item",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Bill",
                table: "Item",
                column: "Bill");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "BillStatus");
        }
    }
}
