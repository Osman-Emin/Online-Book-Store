using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineBookStore.Migrations
{
    public partial class fixrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientBooks");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentReferenceNo",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "CartItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_BookId",
                table: "CartItems",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Books_BookId",
                table: "CartItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Books_BookId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_BookId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentReferenceNo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "CartItems");

            migrationBuilder.CreateTable(
                name: "ClientBooks",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ClientBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientBooks_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientBooks_BookId",
                table: "ClientBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientBooks_ClientId",
                table: "ClientBooks",
                column: "ClientId");
        }
    }
}
