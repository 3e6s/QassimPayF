using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QassimPay.Migrations
{
    /// <inheritdoc />
    public partial class fixwallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Wallet",
                newName: "Wallet_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Wallet_ID",
                table: "Wallet",
                newName: "ID");
        }
    }
}
