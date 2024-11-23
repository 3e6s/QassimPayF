using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QassimPay.Migrations
{
    /// <inheritdoc />
    public partial class MapUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "USER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_USER",
                table: "USER",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_User_ID",
                table: "Wallet",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_Sender_ID",
                table: "Transfer",
                column: "Sender_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Billing_W_ID",
                table: "Billing",
                column: "W_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_U_ID",
                table: "Address",
                column: "U_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_USER_U_ID",
                table: "Address",
                column: "U_ID",
                principalTable: "USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Billing_Wallet_W_ID",
                table: "Billing",
                column: "W_ID",
                principalTable: "Wallet",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_Wallet_Sender_ID",
                table: "Transfer",
                column: "Sender_ID",
                principalTable: "Wallet",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_USER_User_ID",
                table: "Wallet",
                column: "User_ID",
                principalTable: "USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_USER_U_ID",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Billing_Wallet_W_ID",
                table: "Billing");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_Wallet_Sender_ID",
                table: "Transfer");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_USER_User_ID",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_User_ID",
                table: "Wallet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USER",
                table: "USER");

            migrationBuilder.DropIndex(
                name: "IX_Transfer_Sender_ID",
                table: "Transfer");

            migrationBuilder.DropIndex(
                name: "IX_Billing_W_ID",
                table: "Billing");

            migrationBuilder.DropIndex(
                name: "IX_Address_U_ID",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "USER",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "ID");
        }
    }
}
