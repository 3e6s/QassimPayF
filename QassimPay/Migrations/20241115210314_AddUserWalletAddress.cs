using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QassimPay.Migrations
{
    /// <inheritdoc />
    public partial class AddUserWalletAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_USER",
                table: "USER");

            migrationBuilder.DeleteData(
                table: "USER",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "USER",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.RenameTable(
                name: "USER",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "Wallet_ID",
                table: "Wallet",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ADD_ID",
                table: "Address",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wallet",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "ID");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Address_U_ID",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wallet");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "USER");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Wallet",
                newName: "Wallet_ID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Address",
                newName: "ADD_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_USER",
                table: "USER",
                column: "ID");

            migrationBuilder.InsertData(
                table: "USER",
                columns: new[] { "ID", "Email", "First_name", "Last_name", "Monthly_income", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "john.doe@example.com", "John", "Doe", 5000.00m, "password123", "johndoe" },
                    { 2, "jane.smith@example.com", "Jane", "Smith", 6000.00m, "password456", "janesmith" }
                });
        }
    }
}
