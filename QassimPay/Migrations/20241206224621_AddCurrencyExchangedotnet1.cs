using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QassimPay.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyExchangedotnet1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyExchange",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrencyFrom = table.Column<string>(type: "text", nullable: false),
                    CurrencyTo = table.Column<string>(type: "text", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchange", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    monthly_income = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Street_Adress = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Postal_Code = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    U_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Address_User_U_ID",
                        column: x => x.U_ID,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Wallet_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    User_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Wallet_ID);
                    table.ForeignKey(
                        name: "FK_Wallet_User_User_ID",
                        column: x => x.User_ID,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Billing",
                columns: table => new
                {
                    Billing_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Billing_number = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    W_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billing", x => x.Billing_ID);
                    table.ForeignKey(
                        name: "FK_Billing_Wallet_W_ID",
                        column: x => x.W_ID,
                        principalTable: "Wallet",
                        principalColumn: "Wallet_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transfer",
                columns: table => new
                {
                    Receipt_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AmountM = table.Column<decimal>(type: "numeric", nullable: false),
                    Reciver = table.Column<int>(type: "integer", nullable: false),
                    T_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Sender_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfer", x => x.Receipt_ID);
                    table.ForeignKey(
                        name: "FK_Transfer_Wallet_Sender_ID",
                        column: x => x.Sender_ID,
                        principalTable: "Wallet",
                        principalColumn: "Wallet_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CurrencyExchange",
                columns: new[] { "ID", "CurrencyFrom", "CurrencyTo", "ExchangeRate" },
                values: new object[,]
                {
                    { 1, "USD", "SAR", 3.75m },
                    { 2, "SAR", "USD", 0.27m },
                    { 3, "USD", "AED", 3.67m },
                    { 4, "AED", "USD", 0.27m },
                    { 5, "USD", "EGP", 30.96m },
                    { 6, "EGP", "USD", 0.032m },
                    { 7, "USD", "QAR", 3.64m },
                    { 8, "QAR", "USD", 0.27m },
                    { 9, "USD", "KWD", 0.31m },
                    { 10, "KWD", "USD", 3.24m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_U_ID",
                table: "Address",
                column: "U_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Billing_W_ID",
                table: "Billing",
                column: "W_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_Sender_ID",
                table: "Transfer",
                column: "Sender_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_User_ID",
                table: "Wallet",
                column: "User_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Billing");

            migrationBuilder.DropTable(
                name: "CurrencyExchange");

            migrationBuilder.DropTable(
                name: "Transfer");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
