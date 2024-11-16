using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QassimPay.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WALLET",
                table: "WALLET");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TRANSFER",
                table: "TRANSFER");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BILLING",
                table: "BILLING");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ADRESS",
                table: "ADRESS");

            migrationBuilder.RenameTable(
                name: "WALLET",
                newName: "Wallet");

            migrationBuilder.RenameTable(
                name: "TRANSFER",
                newName: "Transfer");

            migrationBuilder.RenameTable(
                name: "BILLING",
                newName: "Billing");

            migrationBuilder.RenameTable(
                name: "ADRESS",
                newName: "Address");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallet",
                table: "Wallet",
                column: "Wallet_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfer",
                table: "Transfer",
                column: "Receipt_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Billing",
                table: "Billing",
                column: "Billing_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "ADD_ID");

            migrationBuilder.InsertData(
                table: "USER",
                columns: new[] { "ID", "Email", "First_name", "Last_name", "Monthly_income", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "john.doe@example.com", "John", "Doe", 5000.00m, "password123", "johndoe" },
                    { 2, "jane.smith@example.com", "Jane", "Smith", 6000.00m, "password456", "janesmith" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallet",
                table: "Wallet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfer",
                table: "Transfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Billing",
                table: "Billing");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.DeleteData(
                table: "USER",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "USER",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.RenameTable(
                name: "Wallet",
                newName: "WALLET");

            migrationBuilder.RenameTable(
                name: "Transfer",
                newName: "TRANSFER");

            migrationBuilder.RenameTable(
                name: "Billing",
                newName: "BILLING");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "ADRESS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WALLET",
                table: "WALLET",
                column: "Wallet_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TRANSFER",
                table: "TRANSFER",
                column: "Receipt_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BILLING",
                table: "BILLING",
                column: "Billing_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ADRESS",
                table: "ADRESS",
                column: "ADD_ID");
        }
    }
}
