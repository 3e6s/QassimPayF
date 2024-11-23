﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QassimPay.Migrations
{
    /// <inheritdoc />
    public partial class changename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_Address_User_U_ID",
                table: "Address",
                column: "U_ID",
                principalTable: "User",
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
                name: "FK_Billing_Wallet_W_ID",
                table: "Billing");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_Wallet_Sender_ID",
                table: "Transfer");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_User_ID",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_User_ID",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Transfer_Sender_ID",
                table: "Transfer");

            migrationBuilder.DropIndex(
                name: "IX_Billing_W_ID",
                table: "Billing");

            migrationBuilder.DropIndex(
                name: "IX_Address_U_ID",
                table: "Address");
        }
    }
}
