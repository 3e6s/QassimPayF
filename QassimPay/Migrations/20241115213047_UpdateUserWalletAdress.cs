using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QassimPay.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserWalletAdress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_U_ID",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_User_ID",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_User_ID",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Address_U_ID",
                table: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Wallet_User_ID",
                table: "Wallet",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_U_ID",
                table: "Address",
                column: "U_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_U_ID",
                table: "Address",
                column: "U_ID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_User_ID",
                table: "Wallet",
                column: "User_ID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
